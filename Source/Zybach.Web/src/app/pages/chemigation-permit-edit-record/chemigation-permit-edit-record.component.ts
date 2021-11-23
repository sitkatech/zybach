import { ChangeDetectorRef, Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbDate, NgbDateAdapter, NgbDateNativeAdapter } from '@ng-bootstrap/ng-bootstrap';
import { forkJoin } from 'rxjs';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { ChemigationPermitService } from 'src/app/services/chemigation-permit.service';
import { ChemigationPermitAnnualRecordUpsertComponent } from 'src/app/shared/components/chemigation-permit-annual-record-upsert/chemigation-permit-annual-record-upsert.component';
import { UserDetailedDto } from 'src/app/shared/models';
import { Alert } from 'src/app/shared/models/alert';
import { ChemigationPermitAnnualRecordUpsertDto } from 'src/app/shared/models/chemigation-permit-annual-record-upsert-dto';
import { AlertContext } from 'src/app/shared/models/enums/alert-context.enum';
import { ChemigationPermitAnnualRecordDto } from 'src/app/shared/models/generated/chemigation-permit-annual-record-dto';
import { ChemigationPermitDto } from 'src/app/shared/models/generated/chemigation-permit-dto';
import { ChemigationPermitStatusDto } from 'src/app/shared/models/generated/chemigation-permit-status-dto';
import { AlertService } from 'src/app/shared/services/alert.service';

@Component({
  selector: 'zybach-chemigation-permit-edit-record',
  templateUrl: './chemigation-permit-edit-record.component.html',
  styleUrls: ['./chemigation-permit-edit-record.component.scss'],
  providers: [{provide: NgbDateAdapter, useClass: NgbDateNativeAdapter}]
})

export class ChemigationPermitEditRecordComponent implements OnInit, OnDestroy {
  @ViewChild('cparUpsert') private chemigationPermitAnnualRecordUpsertComponent : ChemigationPermitAnnualRecordUpsertComponent;

  private watchUserChangeSubscription: any;
  private currentUser: UserDetailedDto;
  
  public chemigationPermitNumber: number;
  public chemigationPermit: ChemigationPermitDto;
  public permitStatuses: Array<ChemigationPermitStatusDto>;
  
  public chemigationPermitAnnualRecordID: number;
  public model: ChemigationPermitAnnualRecordUpsertDto;
  public recordYear: number;
  
  public isLoadingSubmit: boolean = false;
  public isCPARFormValidCheck: boolean;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private authenticationService: AuthenticationService,
    private chemigationPermitService: ChemigationPermitService,
    private cdr: ChangeDetectorRef,
    private alertService: AlertService
  ) { }

  ngOnInit(): void {
    this.watchUserChangeSubscription = this.authenticationService.currentUserSetObservable.subscribe(currentUser => {
      this.currentUser = currentUser;
  
      if (!this.authenticationService.isUserAnAdministrator(this.currentUser)) {
        this.router.navigateByUrl("/not-found")
          .then();
        return;
      }

      this.chemigationPermitNumber = parseInt(this.route.snapshot.paramMap.get("permit-number"));
      this.recordYear = parseInt(this.route.snapshot.paramMap.get("record-year"));

      forkJoin({
        annualRecord: this.chemigationPermitService.getAnnualRecordByPermitNumberAndRecordYear(this.chemigationPermitNumber, this.recordYear)
      }).subscribe(({ annualRecord }) => {
        this.initializeModel(annualRecord);
        this.cdr.detectChanges();
      });
      this.chemigationPermitAnnualRecordUpsertComponent?.validateCPARForm();
    
    });
  }

  private initializeModel(annualRecord: ChemigationPermitAnnualRecordDto) {
    this.chemigationPermit = annualRecord.ChemigationPermit;
    this.chemigationPermitAnnualRecordID = annualRecord.ChemigationPermitAnnualRecordID
    var chemigationPermitAnnualRecordUpsertDto = new ChemigationPermitAnnualRecordUpsertDto(annualRecord, annualRecord.RecordYear, annualRecord.ChemigationPermitAnnualRecordStatus.ChemigationPermitAnnualRecordStatusID);
    this.model = chemigationPermitAnnualRecordUpsertDto;
  }

  ngOnDestroy() {
    this.watchUserChangeSubscription.unsubscribe();
    this.authenticationService.dispose();
    this.cdr.detach();
  }

  private isCPARFormValid(formValid: any) {
    this.isCPARFormValidCheck = formValid;
  }

  onSubmit(editChemigationPermitAnnualRecordForm: HTMLFormElement): void {
    this.isLoadingSubmit = true;
  
    this.chemigationPermitService.updateChemigationPermitAnnualRecord(this.chemigationPermitAnnualRecordID, this.model)
      .subscribe(response => {
        this.isLoadingSubmit = false;
        editChemigationPermitAnnualRecordForm.reset();
        this.router.navigateByUrl("/chemigation-permits/" + response.ChemigationPermit.ChemigationPermitNumber).then(() => {
          this.alertService.pushAlert(new Alert(`Annual Record updated for ${response.RecordYear}.`, AlertContext.Success));
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



