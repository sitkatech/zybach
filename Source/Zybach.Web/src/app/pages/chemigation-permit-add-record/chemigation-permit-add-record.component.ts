import { ChangeDetectorRef, Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { ChemigationPermitService } from 'src/app/services/chemigation-permit.service';
import { ChemigationPermitAnnualRecordUpsertComponent } from 'src/app/shared/components/chemigation-permit-annual-record-upsert/chemigation-permit-annual-record-upsert.component';
import { ChemigationPermitAnnualRecordApplicatorUpsertDto } from 'src/app/shared/generated/model/chemigation-permit-annual-record-applicator-upsert-dto';
import { ChemigationPermitAnnualRecordChemicalFormulationUpsertDto } from 'src/app/shared/generated/model/chemigation-permit-annual-record-chemical-formulation-upsert-dto';
import { ChemigationPermitAnnualRecordDetailedDto } from 'src/app/shared/generated/model/chemigation-permit-annual-record-detailed-dto';
import { ChemigationPermitAnnualRecordUpsertDto } from 'src/app/shared/generated/model/chemigation-permit-annual-record-upsert-dto';
import { ChemigationPermitDto } from 'src/app/shared/generated/model/chemigation-permit-dto';
import { ChemigationPermitStatusDto } from 'src/app/shared/generated/model/chemigation-permit-status-dto';
import { UserDto } from 'src/app/shared/generated/model/user-dto';
import { Alert } from 'src/app/shared/models/alert';
import { AlertContext } from 'src/app/shared/models/enums/alert-context.enum';
import { ChemigationPermitAnnualRecordFeeTypeEnum } from 'src/app/shared/models/enums/chemigation-permit-annual-record-fee-type.enum';
import { ChemigationPermitAnnualRecordStatusEnum } from 'src/app/shared/models/enums/chemigation-permit-annual-record-status.enum';
import { AlertService } from 'src/app/shared/services/alert.service';

@Component({
  selector: 'zybach-chemigation-permit-add-record',
  templateUrl: './chemigation-permit-add-record.component.html',
  styleUrls: ['./chemigation-permit-add-record.component.scss'],
})
export class ChemigationPermitAddRecordComponent implements OnInit, OnDestroy {
  @ViewChild('annualRecordForm') private chemigationPermitAnnualRecordUpsertComponent : ChemigationPermitAnnualRecordUpsertComponent;
  
  private currentUser: UserDto;
  
  public chemigationPermitNumber: number;
  public chemigationPermit: ChemigationPermitDto;

  public permitStatuses: Array<ChemigationPermitStatusDto>;
  public model: ChemigationPermitAnnualRecordUpsertDto;
  public newRecordYear: number;
  
  public isLoadingSubmit: boolean = false;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private authenticationService: AuthenticationService,
    private chemigationPermitService: ChemigationPermitService,
    private cdr: ChangeDetectorRef,
    private alertService: AlertService
  ) { }

  ngOnInit(): void {
    this.authenticationService.getCurrentUser().subscribe(currentUser => {
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
        this.initializeModel(annualRecord);        
        this.cdr.detectChanges();
      });
  
    });
  }

  private initializeModel(annualRecord: ChemigationPermitAnnualRecordDetailedDto) : void {
    var chemigationPermitAnnualRecordUpsertDto = new ChemigationPermitAnnualRecordUpsertDto();
    chemigationPermitAnnualRecordUpsertDto.ChemigationPermitAnnualRecordStatusID = ChemigationPermitAnnualRecordStatusEnum.PendingPayment;
    chemigationPermitAnnualRecordUpsertDto.ChemigationPermitAnnualRecordFeeTypeID = ChemigationPermitAnnualRecordFeeTypeEnum.Renewal;
    chemigationPermitAnnualRecordUpsertDto.ChemigationInjectionUnitTypeID = annualRecord.ChemigationInjectionUnitTypeID;
    chemigationPermitAnnualRecordUpsertDto.RecordYear = this.newRecordYear;
    chemigationPermitAnnualRecordUpsertDto.PivotName = annualRecord.PivotName;
    chemigationPermitAnnualRecordUpsertDto.ApplicantCompany = annualRecord.ApplicantCompany;
    chemigationPermitAnnualRecordUpsertDto.ApplicantFirstName = annualRecord.ApplicantFirstName;
    chemigationPermitAnnualRecordUpsertDto.ApplicantLastName = annualRecord.ApplicantLastName;
    chemigationPermitAnnualRecordUpsertDto.ApplicantMailingAddress = annualRecord.ApplicantMailingAddress;
    chemigationPermitAnnualRecordUpsertDto.ApplicantCity = annualRecord.ApplicantCity;
    chemigationPermitAnnualRecordUpsertDto.ApplicantState = annualRecord.ApplicantState;
    chemigationPermitAnnualRecordUpsertDto.ApplicantZipCode = annualRecord.ApplicantZipCode;
    chemigationPermitAnnualRecordUpsertDto.ApplicantPhone = annualRecord.ApplicantPhone;
    chemigationPermitAnnualRecordUpsertDto.ApplicantMobilePhone = annualRecord.ApplicantMobilePhone;
    chemigationPermitAnnualRecordUpsertDto.ApplicantEmail = annualRecord.ApplicantEmail;
    chemigationPermitAnnualRecordUpsertDto.DateReceived = null;
    chemigationPermitAnnualRecordUpsertDto.DatePaid = null;
    chemigationPermitAnnualRecordUpsertDto.DateApproved = null;
    chemigationPermitAnnualRecordUpsertDto.AnnualNotes = null;
    chemigationPermitAnnualRecordUpsertDto.TownshipRangeSection = annualRecord.TownshipRangeSection;

    const chemicalFormulations = new Array<ChemigationPermitAnnualRecordChemicalFormulationUpsertDto>();
    chemigationPermitAnnualRecordUpsertDto.ChemicalFormulations = chemicalFormulations;

    const applicators = new Array<ChemigationPermitAnnualRecordApplicatorUpsertDto>();
    annualRecord.Applicators.map(x => {
      const applicator = new ChemigationPermitAnnualRecordApplicatorUpsertDto();
      applicator.ApplicatorName = x.ApplicatorName;
      applicator.CertificationNumber = x.CertificationNumber;
      applicator.ExpirationYear = x.ExpirationYear;
      applicator.HomePhone = x.HomePhone;
      applicator.MobilePhone = x.MobilePhone;
      applicators.push(applicator);
    });
    chemigationPermitAnnualRecordUpsertDto.Applicators = applicators;

    this.model = chemigationPermitAnnualRecordUpsertDto;
  }

  ngOnDestroy() {
    
    
    this.cdr.detach();
  }

  onSubmit(addChemigationPermitAnnualRecordForm: HTMLFormElement): void {
    this.isLoadingSubmit = true;
    this.alertService.clearAlerts();
  
    this.chemigationPermitService.createChemigationPermitAnnualRecord(this.chemigationPermit.ChemigationPermitID, this.model)
      .subscribe(response => {
        this.isLoadingSubmit = false;
        addChemigationPermitAnnualRecordForm.reset();
        this.router.navigateByUrl("/chemigation-permits/" + this.chemigationPermit.ChemigationPermitNumber).then(() => {
          this.alertService.pushAlert(new Alert(`Annual Record added for ${this.model.RecordYear}.`, AlertContext.Success));
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



