import { ChangeDetectorRef, Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { forkJoin } from 'rxjs';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { ChemigationPermitService } from 'src/app/services/chemigation-permit.service';
import { ChemigationPermitAnnualRecordUpsertComponent } from 'src/app/shared/components/chemigation-permit-annual-record-upsert/chemigation-permit-annual-record-upsert.component';
import { ChemigationPermitAnnualRecordChemicalFormulationSimpleDto } from 'src/app/shared/generated/model/chemigation-permit-annual-record-chemical-formulation-simple-dto';
import { ChemigationPermitAnnualRecordDetailedDto } from 'src/app/shared/generated/model/chemigation-permit-annual-record-detailed-dto';
import { ChemigationPermitAnnualRecordDto } from 'src/app/shared/generated/model/chemigation-permit-annual-record-dto';
import { ChemigationPermitAnnualRecordUpsertDto } from 'src/app/shared/generated/model/chemigation-permit-annual-record-upsert-dto';
import { ChemigationPermitDto } from 'src/app/shared/generated/model/chemigation-permit-dto';
import { ChemigationPermitStatusDto } from 'src/app/shared/generated/model/chemigation-permit-status-dto';
import { UserDto } from 'src/app/shared/generated/model/user-dto';
import { Alert } from 'src/app/shared/models/alert';
import { AlertContext } from 'src/app/shared/models/enums/alert-context.enum';
import { AlertService } from 'src/app/shared/services/alert.service';

@Component({
  selector: 'zybach-chemigation-permit-edit-record',
  templateUrl: './chemigation-permit-edit-record.component.html',
  styleUrls: ['./chemigation-permit-edit-record.component.scss']
})

export class ChemigationPermitEditRecordComponent implements OnInit, OnDestroy {
  @ViewChild('annualRecordForm') private chemigationPermitAnnualRecordUpsertComponent : ChemigationPermitAnnualRecordUpsertComponent;

  private watchUserChangeSubscription: any;
  private currentUser: UserDto;
  
  public chemigationPermitNumber: number;
  public chemigationPermit: ChemigationPermitDto;
  public permitStatuses: Array<ChemigationPermitStatusDto>;
  
  public chemigationPermitAnnualRecordID: number;
  public model: ChemigationPermitAnnualRecordUpsertDto;
  public recordYear: number;
  
  public isLoadingSubmit: boolean = false;
  public isAnnualRecordFormValidCheck: boolean;
  public isChemicalFormulationsFormValidCheck: boolean;

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
        annualRecord: this.chemigationPermitService.getAnnualRecordByPermitNumberAndRecordYear(this.chemigationPermitNumber, this.recordYear),
        chemicalFormulations: this.chemigationPermitService.getChemicalFormulationsByPermitNumberAndRecordYear(this.chemigationPermitNumber, this.recordYear),
      }).subscribe(({ annualRecord, chemicalFormulations }) => {
        this.initializeModel(annualRecord, chemicalFormulations);
        this.cdr.detectChanges();
      });

    
    });
  }

  private initializeModel(annualRecord: ChemigationPermitAnnualRecordDetailedDto, chemicalFormulations: Array<ChemigationPermitAnnualRecordChemicalFormulationSimpleDto>) {
    this.chemigationPermit = annualRecord.ChemigationPermit;
    this.chemigationPermitAnnualRecordID = annualRecord.ChemigationPermitAnnualRecordID
    var chemigationPermitAnnualRecordUpsertDto = new ChemigationPermitAnnualRecordUpsertDto();
    chemigationPermitAnnualRecordUpsertDto.ChemigationPermitAnnualRecordStatusID = annualRecord.ChemigationPermitAnnualRecordStatusID;
    chemigationPermitAnnualRecordUpsertDto.ChemigationInjectionUnitTypeID = annualRecord.ChemigationInjectionUnitTypeID;
    chemigationPermitAnnualRecordUpsertDto.RecordYear = annualRecord.RecordYear;
    chemigationPermitAnnualRecordUpsertDto.PivotName = annualRecord.PivotName;
    chemigationPermitAnnualRecordUpsertDto.ApplicantFirstName = annualRecord.ApplicantFirstName;
    chemigationPermitAnnualRecordUpsertDto.ApplicantLastName = annualRecord.ApplicantLastName;
    chemigationPermitAnnualRecordUpsertDto.ApplicantMailingAddress = annualRecord.ApplicantMailingAddress;
    chemigationPermitAnnualRecordUpsertDto.ApplicantCity = annualRecord.ApplicantCity;
    chemigationPermitAnnualRecordUpsertDto.ApplicantState = annualRecord.ApplicantState;
    chemigationPermitAnnualRecordUpsertDto.ApplicantZipCode = annualRecord.ApplicantZipCode;
    chemigationPermitAnnualRecordUpsertDto.ApplicantPhone = annualRecord.ApplicantPhone;
    chemigationPermitAnnualRecordUpsertDto.ApplicantMobilePhone = annualRecord.ApplicantMobilePhone;
    chemigationPermitAnnualRecordUpsertDto.ApplicantEmail = annualRecord.ApplicantEmail;
    chemigationPermitAnnualRecordUpsertDto.DateReceived = annualRecord.DateReceived;
    chemigationPermitAnnualRecordUpsertDto.DatePaid = annualRecord.DatePaid;
    chemigationPermitAnnualRecordUpsertDto.ChemicalFormulations = chemicalFormulations;
    this.model = chemigationPermitAnnualRecordUpsertDto;
  }

  ngOnDestroy() {
    this.watchUserChangeSubscription.unsubscribe();
    this.authenticationService.dispose();
    this.cdr.detach();
  }

  public isAnnualRecordFormValid(formValid: any) : void {
    this.isAnnualRecordFormValidCheck = formValid;
  }

  public isChemicalFormulationsFormValid(formValid: any): void {
    this.isChemicalFormulationsFormValidCheck = formValid;
  }

  public isFormValid(editChemigationPermitAnnualRecordForm: any) : boolean{
    return this.isLoadingSubmit || !this.isAnnualRecordFormValidCheck || !this.isChemicalFormulationsFormValidCheck || !editChemigationPermitAnnualRecordForm.form.valid;
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