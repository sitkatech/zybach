import { ChangeDetectorRef, Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { ChemigationPermitService } from 'src/app/services/chemigation-permit.service';
import { ChemigationPermitAnnualRecordUpsertComponent } from 'src/app/shared/components/chemigation-permit-annual-record-upsert/chemigation-permit-annual-record-upsert.component';
import { ChemigationPermitAnnualRecordChemicalFormulationDto } from 'src/app/shared/generated/model/chemigation-permit-annual-record-chemical-formulation-dto';
import { ChemigationPermitAnnualRecordChemicalFormulationSimpleDto } from 'src/app/shared/generated/model/chemigation-permit-annual-record-chemical-formulation-simple-dto';
import { ChemigationPermitAnnualRecordUpsertDto } from 'src/app/shared/generated/model/chemigation-permit-annual-record-upsert-dto';
import { ChemigationPermitDto } from 'src/app/shared/generated/model/chemigation-permit-dto';
import { ChemigationPermitStatusDto } from 'src/app/shared/generated/model/chemigation-permit-status-dto';
import { UserDto } from 'src/app/shared/generated/model/user-dto';
import { Alert } from 'src/app/shared/models/alert';
import { AlertContext } from 'src/app/shared/models/enums/alert-context.enum';
import { ChemigationPermitAnnualRecordStatusEnum } from 'src/app/shared/models/enums/chemigation-permit-annual-record-status.enum';
import { AlertService } from 'src/app/shared/services/alert.service';
import { X } from 'vega-lite/build/src/channel';

@Component({
  selector: 'zybach-chemigation-permit-add-record',
  templateUrl: './chemigation-permit-add-record.component.html',
  styleUrls: ['./chemigation-permit-add-record.component.scss'],
})
export class ChemigationPermitAddRecordComponent implements OnInit, OnDestroy {
  @ViewChild('annualRecordForm') private chemigationPermitAnnualRecordUpsertComponent : ChemigationPermitAnnualRecordUpsertComponent;
  
  private watchUserChangeSubscription: any;
  private currentUser: UserDto;
  
  public chemigationPermitNumber: number;
  public chemigationPermit: ChemigationPermitDto;

  public permitStatuses: Array<ChemigationPermitStatusDto>;
  public model: ChemigationPermitAnnualRecordUpsertDto;
  public newRecordYear: number;
  
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
      this.newRecordYear = new Date().getFullYear();
  
      if (!this.authenticationService.isUserAnAdministrator(this.currentUser)) {
        this.router.navigateByUrl("/not-found")
          .then();
        return;
      }

      this.chemigationPermitNumber = parseInt(this.route.snapshot.paramMap.get("permit-number"));

      this.chemigationPermitService.getLatestAnnualRecordByPermitNumber(this.chemigationPermitNumber).subscribe(annualRecord => {
        this.chemigationPermit = annualRecord.ChemigationPermit;
        var chemigationPermitAnnualRecordUpsertDto = new ChemigationPermitAnnualRecordUpsertDto();
        chemigationPermitAnnualRecordUpsertDto.ChemigationPermitAnnualRecordStatusID = ChemigationPermitAnnualRecordStatusEnum.PendingPayment;
        chemigationPermitAnnualRecordUpsertDto.ChemigationInjectionUnitTypeID = annualRecord.ChemigationInjectionUnitTypeID;
        chemigationPermitAnnualRecordUpsertDto.RecordYear = this.newRecordYear;
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
        const chemicalFormulations = new Array<ChemigationPermitAnnualRecordChemicalFormulationSimpleDto>();
        annualRecord.ChemicalFormulations.forEach(x => chemicalFormulations.push(x));
        chemigationPermitAnnualRecordUpsertDto.ChemicalFormulations = chemicalFormulations;
        
        this.model = chemigationPermitAnnualRecordUpsertDto;
        this.cdr.detectChanges();
      });
  
    });
  }

  ngOnDestroy() {
    this.watchUserChangeSubscription.unsubscribe();
    this.authenticationService.dispose();
    this.cdr.detach();
  }
  
  private isAnnualRecordFormValid(formValid: any) : void {
    this.isAnnualRecordFormValidCheck = formValid;
  }

  public isChemicalFormulationsFormValid(formValid: any): void {
    this.isChemicalFormulationsFormValidCheck = formValid;
  }

  public isFormValid(editChemigationPermitAnnualRecordForm: any) : boolean{
    return this.isLoadingSubmit || !this.isAnnualRecordFormValidCheck || !this.isChemicalFormulationsFormValidCheck || !editChemigationPermitAnnualRecordForm.form.valid;
  }

  onSubmit(addChemigationPermitAnnualRecordForm: HTMLFormElement): void {
    this.isLoadingSubmit = true;
  
    this.chemigationPermitService.createChemigationPermitAnnualRecord(this.chemigationPermit.ChemigationPermitID, this.model)
      .subscribe(response => {
        this.isLoadingSubmit = false;
        addChemigationPermitAnnualRecordForm.reset();
        this.router.navigateByUrl("/chemigation-permits/" + response.ChemigationPermit.ChemigationPermitNumber).then(() => {
          this.alertService.pushAlert(new Alert(`Annual Record added for ${response.RecordYear}.`, AlertContext.Success));
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



