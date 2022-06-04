import { ChangeDetectorRef, Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { forkJoin } from 'rxjs';
import { AuthenticationService } from 'src/app/services/authentication.service';
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
import { AlertService } from 'src/app/shared/services/alert.service';
import { ChemigationPermitAnnualRecordService } from 'src/app/shared/generated/api/chemigation-permit-annual-record.service';

@Component({
  selector: 'zybach-chemigation-permit-edit-record',
  templateUrl: './chemigation-permit-edit-record.component.html',
  styleUrls: ['./chemigation-permit-edit-record.component.scss']
})

export class ChemigationPermitEditRecordComponent implements OnInit, OnDestroy {
  @ViewChild('annualRecordForm') private chemigationPermitAnnualRecordUpsertComponent : ChemigationPermitAnnualRecordUpsertComponent;
  
  private currentUser: UserDto;
  
  public chemigationPermitNumber: number;
  public chemigationPermit: ChemigationPermitDto;
  public permitStatuses: Array<ChemigationPermitStatusDto>;
  
  public chemigationPermitAnnualRecordID: number;
  public model: ChemigationPermitAnnualRecordUpsertDto;
  public recordYear: number;
  
  public isLoadingSubmit: boolean = false;
  
  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private authenticationService: AuthenticationService,
    private chemigationPermitAnnualRecordService: ChemigationPermitAnnualRecordService,
    private cdr: ChangeDetectorRef,
    private alertService: AlertService
  ) { }

  ngOnInit(): void {
    this.authenticationService.getCurrentUser().subscribe(currentUser => {
      this.currentUser = currentUser;

      this.chemigationPermitNumber = parseInt(this.route.snapshot.paramMap.get("permit-number"));
      this.recordYear = parseInt(this.route.snapshot.paramMap.get("record-year"));

      forkJoin({
        annualRecord: this.chemigationPermitAnnualRecordService.chemigationPermitsChemigationPermitNumberRecordYearGet(this.chemigationPermitNumber, this.recordYear),
      }).subscribe(({ annualRecord }) => {
        this.initializeModel(annualRecord);
        this.cdr.detectChanges();
      });

    
    });
  }

  private initializeModel(annualRecord: ChemigationPermitAnnualRecordDetailedDto) {
    this.chemigationPermit = annualRecord.ChemigationPermit;
    this.chemigationPermitAnnualRecordID = annualRecord.ChemigationPermitAnnualRecordID
    var chemigationPermitAnnualRecordUpsertDto = new ChemigationPermitAnnualRecordUpsertDto();
    chemigationPermitAnnualRecordUpsertDto.ChemigationPermitAnnualRecordStatusID = annualRecord.ChemigationPermitAnnualRecordStatusID;
    chemigationPermitAnnualRecordUpsertDto.ChemigationPermitAnnualRecordFeeTypeID = annualRecord.ChemigationPermitAnnualRecordFeeTypeID;
    chemigationPermitAnnualRecordUpsertDto.ChemigationInjectionUnitTypeID = annualRecord.ChemigationInjectionUnitTypeID;
    chemigationPermitAnnualRecordUpsertDto.RecordYear = annualRecord.RecordYear;
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
    chemigationPermitAnnualRecordUpsertDto.DateReceived = annualRecord.DateReceived;
    chemigationPermitAnnualRecordUpsertDto.DatePaid = annualRecord.DatePaid;
    chemigationPermitAnnualRecordUpsertDto.DateApproved = annualRecord.DateApproved;
    chemigationPermitAnnualRecordUpsertDto.AnnualNotes = annualRecord.AnnualNotes;
    chemigationPermitAnnualRecordUpsertDto.TownshipRangeSection = annualRecord.TownshipRangeSection;
    
    const chemicalFormulations = new Array<ChemigationPermitAnnualRecordChemicalFormulationUpsertDto>();
    annualRecord.ChemicalFormulations.map(x => {
      const chemicalFormulation = new ChemigationPermitAnnualRecordChemicalFormulationUpsertDto();
      chemicalFormulation.ChemigationPermitAnnualRecordChemicalFormulationID = x.ChemigationPermitAnnualRecordChemicalFormulationID;
      chemicalFormulation.ChemigationPermitAnnualRecordID = x.ChemigationPermitAnnualRecordID;
      chemicalFormulation.ChemicalFormulationID = x.ChemicalFormulationID;
      chemicalFormulation.ChemicalUnitID = x.ChemicalUnitID;
      chemicalFormulation.TotalApplied = x.TotalApplied;
      chemicalFormulation.AcresTreated = x.AcresTreated;
      chemicalFormulations.push(chemicalFormulation);
    });
    chemigationPermitAnnualRecordUpsertDto.ChemicalFormulations = chemicalFormulations;

    const applicators = new Array<ChemigationPermitAnnualRecordApplicatorUpsertDto>();
    annualRecord.Applicators.map(x => {
      const applicator = new ChemigationPermitAnnualRecordApplicatorUpsertDto();
      applicator.ChemigationPermitAnnualRecordApplicatorID = x.ChemigationPermitAnnualRecordApplicatorID;
      applicator.ChemigationPermitAnnualRecordID = x.ChemigationPermitAnnualRecordID;
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

  onSubmit(editChemigationPermitAnnualRecordForm: HTMLFormElement): void {
    this.isLoadingSubmit = true;
    this.alertService.clearAlerts();
  
    this.chemigationPermitAnnualRecordService.chemigationPermitAnnualRecordsChemigationPermitAnnualRecordIDPut(this.chemigationPermitAnnualRecordID, this.model)
      .subscribe(response => {
        this.isLoadingSubmit = false;
        editChemigationPermitAnnualRecordForm.reset();
        this.router.navigateByUrl("/chemigation-permits/" + this.chemigationPermit.ChemigationPermitNumber).then(() => {
          this.alertService.pushAlert(new Alert(`Annual Record updated for ${this.recordYear}.`, AlertContext.Success));
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