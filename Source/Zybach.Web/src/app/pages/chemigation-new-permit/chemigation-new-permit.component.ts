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
import { ChemigationPermitNewDto } from 'src/app/shared/generated/model/chemigation-permit-new-dto';
import { ChemigationPermitStatusDto } from 'src/app/shared/generated/model/chemigation-permit-status-dto';
import { UserDto } from 'src/app/shared/generated/model/user-dto';
import { ChemigationPermitAnnualRecordApplicatorUpsertDto } from 'src/app/shared/generated/model/chemigation-permit-annual-record-applicator-upsert-dto';
import { ChemigationInjectionUnitTypeEnum } from 'src/app/shared/models/enums/chemigation-injection-unit-type.enum';
import { Observable, of } from 'rxjs';
import { debounceTime, distinctUntilChanged, tap, switchMap, catchError } from 'rxjs/operators';
import { WellService } from 'src/app/services/well.service';
import { CountyDto } from 'src/app/shared/generated/model/county-dto';

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
  public counties: Array<CountyDto>;

  public model: ChemigationPermitNewDto;
  public defaultState: string = 'NE';

  public isLoadingSubmit: boolean = false;
  public isAnnualRecordFormValidCheck: boolean;
  public isApplicatorsFormValidCheck: boolean;
  public searchFailed : boolean = false;

  constructor(
    private cdr: ChangeDetectorRef,
    private router: Router,
    private wellService: WellService,
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
      // default to portable injection unit type
      ChemigationInjectionUnitTypeID: ChemigationInjectionUnitTypeEnum.Portable,
      PivotName: null,
      RecordYear: new Date().getFullYear(),
      TownshipRangeSection: null,
      ApplicantCompany: null,
      ApplicantFirstName: null,
      ApplicantLastName: null,
      ApplicantMailingAddress: null,
      ApplicantCity: null,
      // default to Nebraska
      ApplicantState: this.defaultState,
      ApplicantZipCode: null,
      ApplicantPhone: null,
      ApplicantMobilePhone: null,
      ApplicantEmail: null,
      DateReceived: null,
      DatePaid: null,
      AnnualNotes: null,
    };
    this.model.ChemigationPermitAnnualRecord.Applicators = new Array<ChemigationPermitAnnualRecordApplicatorUpsertDto>();

    this.watchUserChangeSubscription = this.authenticationService.currentUserSetObservable.subscribe(currentUser => {
      this.currentUser = currentUser;
      if (!this.authenticationService.isUserAnAdministrator(this.currentUser)) {
        this.router.navigateByUrl("/not-found")
          .then();
        return;
      }
      this.chemigationPermitService.getChemigationPermitStatuses().subscribe(permitStatuses => {
        this.permitStatuses = permitStatuses;
      });
      this.chemigationPermitService.getCounties().subscribe(counties => {
        this.counties = counties;
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

  public isApplicatorsFormValid(formValid: any): void {
    this.isApplicatorsFormValidCheck = formValid;
  }

  public isFormValid(addChemigationPermitAnnualRecordForm: any): boolean {
    return this.isLoadingSubmit || !this.isAnnualRecordFormValidCheck || !this.isApplicatorsFormValidCheck
     || !addChemigationPermitAnnualRecordForm.form.valid;
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

  
  searchApi = (text$: Observable<string>) => {
    return text$.pipe(      
        debounceTime(200), 
        distinctUntilChanged(),
        tap(() => this.searchFailed = false),
        switchMap(searchText => searchText.length > 2 ? this.wellService.searchByWellRegistrationID(searchText) : ([])), 
        catchError(() => {
          this.searchFailed = true;
          return of([]);
        })     
      );                 
  }
}