import { ChangeDetectorRef, Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { forkJoin } from 'rxjs';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { ChemigationInspectionService } from 'src/app/services/chemigation-inspection.service';
import { ChemigationPermitService } from 'src/app/services/chemigation-permit.service';
import { ChemigationInspectionUpsertComponent } from 'src/app/shared/components/chemigation-inspection-upsert/chemigation-inspection-upsert.component';
import { ChemigationInspectionUpsertDto } from 'src/app/shared/generated/model/chemigation-inspection-upsert-dto';
import { ChemigationPermitAnnualRecordDetailedDto } from 'src/app/shared/generated/model/chemigation-permit-annual-record-detailed-dto';
import { UserDto } from 'src/app/shared/generated/model/user-dto';
import { Alert } from 'src/app/shared/models/alert';
import { AlertContext } from 'src/app/shared/models/enums/alert-context.enum';
import { ChemigationInspectionStatusEnum } from 'src/app/shared/models/enums/chemigation-inspection-status';
import { AlertService } from 'src/app/shared/services/alert.service';

@Component({
  selector: 'zybach-chemigation-inspection-new',
  templateUrl: './chemigation-inspection-new.component.html',
  styleUrls: ['./chemigation-inspection-new.component.scss']
})
export class ChemigationInspectionNewComponent implements OnInit {
  @ViewChild('inspectionUpsertForm') private chemigationInspectionUpsertComponent : ChemigationInspectionUpsertComponent;

  public watchUserChangeSubscription: any;
  public currentUser: UserDto;

  public annualRecord: ChemigationPermitAnnualRecordDetailedDto;
  public chemigationPermitNumber: number;
  public recordYear: number;

  public inspection: ChemigationInspectionUpsertDto;
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
      this.chemigationPermitNumber = parseInt(this.route.snapshot.paramMap.get("permit-number"));
      this.recordYear = parseInt(this.route.snapshot.paramMap.get("record-year"));
      
      this.chemigationPermitService.getAnnualRecordByPermitNumberAndRecordYear(this.chemigationPermitNumber, this.recordYear).subscribe(annualRecord => {
        this.annualRecord = annualRecord;
        this.initializeInspectionModel(this.annualRecord.ChemigationPermitAnnualRecordID)
        this.cdr.detectChanges();
      });
      
    })
  }

  private initializeInspectionModel(annualRecordID: number) : void {
    var chemigationInspectionUpsertDto = new ChemigationInspectionUpsertDto();
    chemigationInspectionUpsertDto.ChemigationPermitAnnualRecordID = annualRecordID;
    chemigationInspectionUpsertDto.ChemigationInspectionStatusID = ChemigationInspectionStatusEnum.Pending;
    chemigationInspectionUpsertDto.ChemigationInspectionTypeID = null;
    chemigationInspectionUpsertDto.InspectionDate = null;
    chemigationInspectionUpsertDto.ChemigationInspectionFailureReasonID = null;
    chemigationInspectionUpsertDto.TillageID = null;
    chemigationInspectionUpsertDto.CropTypeID = null;
    chemigationInspectionUpsertDto.InspectorUserID = null;
    chemigationInspectionUpsertDto.ChemigationMainlineCheckValveID = null;
    chemigationInspectionUpsertDto.ChemigationLowPressureValveID = null;
    chemigationInspectionUpsertDto.ChemigationInjectionValveID = null;
    chemigationInspectionUpsertDto.HasVacuumReliefValve = false;
    chemigationInspectionUpsertDto.HasInspectionPort = false;
    chemigationInspectionUpsertDto.InspectionNotes = null;

    this.inspection = chemigationInspectionUpsertDto;
  }

  ngOnDestroy() {
    this.watchUserChangeSubscription.unsubscribe();
    this.authenticationService.dispose();
    this.cdr.detach();
  }

  public onSubmit(addChemigationInspectionForm: HTMLFormElement): void {
    this.isLoadingSubmit = true;
  
    this.chemigationInspectionService.createChemigationInspectionByAnnualRecordID(this.annualRecord.ChemigationPermitAnnualRecordID, this.inspection)
      .subscribe(response => {
        this.isLoadingSubmit = false;
        addChemigationInspectionForm.reset();
        this.router.navigateByUrl("/chemigation-permits/" + this.annualRecord.ChemigationPermit.ChemigationPermitNumber).then(() => {
          this.alertService.pushAlert(new Alert(`Chemigation Inspection Record added.`, AlertContext.Success));
        });
      }
        ,
        error => {
          this.isLoadingSubmit = false;
          this.cdr.detectChanges();
        }
      );
  }
}
