import { ChangeDetectorRef, Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { Alert } from 'src/app/shared/models/alert';
import { AlertContext } from 'src/app/shared/models/enums/alert-context.enum';
import { AlertService } from 'src/app/shared/services/alert.service';
import { ChemigationPermitService } from 'src/app/services/chemigation-permit.service';
import { ChemigationPermitStatusEnum } from 'src/app/shared/models/enums/chemigation-permit-status.enum'
import { ChemigationPermitAnnualRecordUpsertComponent } from 'src/app/shared/components/chemigation-permit-annual-record-upsert/chemigation-permit-annual-record-upsert.component';
import { ChemigationPermitAnnualRecordStatusEnum } from 'src/app/shared/models/enums/chemigation-permit-annual-record-status.enum';
import { ChemigationCountyDto } from 'src/app/shared/generated/model/chemigation-county-dto';
import { ChemigationPermitNewDto } from 'src/app/shared/generated/model/chemigation-permit-new-dto';
import { ChemigationPermitStatusDto } from 'src/app/shared/generated/model/chemigation-permit-status-dto';
import { UserDto } from 'src/app/shared/generated/model/user-dto';

@Component({
  selector: 'zybach-chemigation-new-permit',
  templateUrl: './chemigation-new-permit.component.html',
  styleUrls: ['./chemigation-new-permit.component.scss'],
})

export class ChemigationNewPermitComponent implements OnInit, OnDestroy {
  @ViewChild('annualRecordForm') private chemigationPermitAnnualRecordUpsertComponent: ChemigationPermitAnnualRecordUpsertComponent;

  private watchUserChangeSubscription: any;
  private currentUser: UserDto;

  public permitStatuses: Array<ChemigationPermitStatusDto>;
  public chemigationCounties: Array<ChemigationCountyDto>;

  public model: ChemigationPermitNewDto;

  public isLoadingSubmit: boolean = false;
  public isAnnualRecordFormValidCheck: boolean;

  constructor(
    private cdr: ChangeDetectorRef,
    private router: Router,
    private chemigationPermitService: ChemigationPermitService,
    private authenticationService: AuthenticationService,
    private alertService: AlertService
  ) { }

  ngOnInit(): void {
    this.model = new ChemigationPermitNewDto();
    // default to active for new permits
    this.model.ChemigationPermitStatusID = ChemigationPermitStatusEnum.Active;
    this.model.ChemigationPermitAnnualRecord =
    {
      ChemigationPermitAnnualRecordStatusID: ChemigationPermitAnnualRecordStatusEnum.PendingPayment,
      ChemigationInjectionUnitTypeID: null,
      PivotName: null,
      RecordYear: new Date().getFullYear(),
      ApplicantFirstName: null,
      ApplicantLastName: null,
      ApplicantMailingAddress: null,
      ApplicantCity: null,
      ApplicantState: null,
      ApplicantZipCode: null,
      ApplicantPhone: null,
      ApplicantMobilePhone: null,
      ApplicantEmail: null,
      DateReceived: null,
      DatePaid: null,
    };

    this.watchUserChangeSubscription = this.authenticationService.currentUserSetObservable.subscribe(currentUser => {
      this.currentUser = currentUser;
      if (!this.authenticationService.isUserAnAdministrator(this.currentUser)) {
        this.router.navigateByUrl("/not-found")
          .then();
        return;
      }
      this.chemigationPermitService.getAllChemigationPermitStatuses().subscribe(permitStatuses => {
        this.permitStatuses = permitStatuses;
      });
      this.chemigationPermitService.getAllChemigationCounties().subscribe(chemigationCounties => {
        this.chemigationCounties = chemigationCounties;
      });

    });

  }

  ngOnDestroy() {
    this.watchUserChangeSubscription.unsubscribe();
    this.authenticationService.dispose();
    this.cdr.detach();
  }

  public isAnnualRecordFormValid(formValid: any): void {
    this.isAnnualRecordFormValidCheck = formValid;
  }

  public isFormValid(addChemigationPermitAnnualRecordForm: any): boolean {
    return this.isLoadingSubmit || !this.isAnnualRecordFormValidCheck || !addChemigationPermitAnnualRecordForm.form.valid;
  }

  public onSubmit(newChemigationPermitForm: HTMLFormElement): void {
    this.isLoadingSubmit = true;
    this.chemigationPermitService.createNewChemigationPermit(this.model)
      .subscribe(response => {
        this.isLoadingSubmit = false;
        newChemigationPermitForm.reset();
        this.router.navigateByUrl("/chemigation-permits/" + response.ChemigationPermitNumber).then(() => {
          this.alertService.pushAlert(new Alert("Chemigation Permit " + response.ChemigationPermitNumber + " successfully created.", AlertContext.Success));
        });
      },
        error => {
          this.isLoadingSubmit = false;
          this.cdr.detectChanges();
        }
      );
  }

}