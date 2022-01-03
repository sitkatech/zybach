import { ChangeDetectorRef, Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { forkJoin } from 'rxjs';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { ChemigationInspectionService } from 'src/app/services/chemigation-inspection.service';
import { ChemigationPermitService } from 'src/app/services/chemigation-permit.service';
import { ChemigationInspectionUpsertComponent } from 'src/app/shared/components/chemigation-inspection-upsert/chemigation-inspection-upsert.component';
import { ChemigationInspectionSimpleDto } from 'src/app/shared/generated/model/chemigation-inspection-simple-dto';
import { ChemigationInspectionUpsertDto } from 'src/app/shared/generated/model/chemigation-inspection-upsert-dto';
import { UserDto } from 'src/app/shared/generated/model/user-dto';
import { Alert } from 'src/app/shared/models/alert';
import { AlertContext } from 'src/app/shared/models/enums/alert-context.enum';
import { AlertService } from 'src/app/shared/services/alert.service';

@Component({
  selector: 'zybach-chemigation-inspection-edit',
  templateUrl: './chemigation-inspection-edit.component.html',
  styleUrls: ['./chemigation-inspection-edit.component.scss']
})

export class ChemigationInspectionEditComponent implements OnInit {
  @ViewChild('inspectionUpsertForm') private chemigationInspectionUpsertComponent : ChemigationInspectionUpsertComponent;

  public watchUserChangeSubscription: any;
  public currentUser: UserDto;

  public chemigationPermitNumber: string;
  public chemigationInspectionID: number;

  public inspection: ChemigationInspectionSimpleDto;
  public isLoadingSubmit: boolean;
  
  constructor(
    private chemigationPermitService: ChemigationPermitService,
    private chemigationInspectionService: ChemigationInspectionService,
    private authenticationService: AuthenticationService,
    private route: ActivatedRoute,
    private router: Router,
    private cdr: ChangeDetectorRef,
    private alertService: AlertService
  ) { }

  ngOnInit(): void {
    this.watchUserChangeSubscription = this.authenticationService.currentUserSetObservable.subscribe(currentUser => {
      this.currentUser = currentUser;
      this.chemigationInspectionID = parseInt(this.route.snapshot.paramMap.get("inspection-id"));

      forkJoin({
        chemigationInspection: this.chemigationInspectionService.getChemigationInspectionByID(this.chemigationInspectionID)
      }).subscribe(({ chemigationInspection }) => {
        this.chemigationPermitNumber = chemigationInspection.ChemigationPermitNumberDisplay;
        this.initializeInspectionModel(chemigationInspection)
        this.cdr.detectChanges();
      });
      
    })
  }

  private initializeInspectionModel(chemigationInspection: ChemigationInspectionSimpleDto) : void {
    var chemigationInspectionUpsertDto = new ChemigationInspectionUpsertDto();
    chemigationInspectionUpsertDto.ChemigationPermitAnnualRecordID = chemigationInspection.ChemigationPermitAnnualRecordID;
    chemigationInspectionUpsertDto.ChemigationInspectionStatusID = chemigationInspection.ChemigationInspectionStatusID;
    chemigationInspectionUpsertDto.ChemigationInspectionTypeID = chemigationInspection.ChemigationInspectionTypeID;
    chemigationInspectionUpsertDto.InspectionDate = chemigationInspection.InspectionDate;
    chemigationInspectionUpsertDto.ChemigationInspectionFailureReasonID = chemigationInspection.ChemigationInspectionFailureReasonID;
    chemigationInspectionUpsertDto.TillageID = chemigationInspection.TillageID;
    chemigationInspectionUpsertDto.CropTypeID = chemigationInspection.CropTypeID;
    chemigationInspectionUpsertDto.InspectorUserID =chemigationInspection.InspectorUserID;
    chemigationInspectionUpsertDto.ChemigationMainlineCheckValveID = chemigationInspection.ChemigationMainlineCheckValveID;
    chemigationInspectionUpsertDto.ChemigationLowPressureValveID = chemigationInspection.ChemigationLowPressureValveID;
    chemigationInspectionUpsertDto.ChemigationInjectionValveID = chemigationInspection.ChemigationInjectionValveID;
    chemigationInspectionUpsertDto.ChemigationInterlockTypeID = chemigationInspection.ChemigationInterlockTypeID;
    chemigationInspectionUpsertDto.HasVacuumReliefValve = chemigationInspection.HasVacuumReliefValve;
    chemigationInspectionUpsertDto.HasInspectionPort = chemigationInspection.HasInspectionPort;
    chemigationInspectionUpsertDto.InspectionNotes = chemigationInspection.InspectionNotes;

    this.inspection = chemigationInspectionUpsertDto;
  }

  public onSubmit(updateChemigationInspectionForm: HTMLFormElement): void {
    this.isLoadingSubmit = true;
  
    this.chemigationInspectionService.updateChemigationInspectionByID(this.chemigationInspectionID, this.inspection)
      .subscribe(response => {
        this.isLoadingSubmit = false;
        updateChemigationInspectionForm.reset();
        this.router.navigateByUrl("/chemigation-permits/" + this.chemigationPermitNumber).then(() => {
          this.alertService.pushAlert(new Alert(`Chemigation Inspection Record updated.`, AlertContext.Success));
        });
      }
        ,
        error => {
          this.isLoadingSubmit = false;
          this.cdr.detectChanges();
        }
      );
  }

  ngOnDestroy() {
    this.watchUserChangeSubscription.unsubscribe();
    this.authenticationService.dispose();
    this.cdr.detach();
  }
}



