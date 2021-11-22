import { ChangeDetectorRef, Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { UserDetailedDto } from 'src/app/shared/models';
import { Alert } from 'src/app/shared/models/alert';
import { AlertContext } from 'src/app/shared/models/enums/alert-context.enum';
import { ChemigationPermitStatusDto } from 'src/app/shared/models/generated/chemigation-permit-status-dto';
import { AlertService } from 'src/app/shared/services/alert.service';
import { NgbDateAdapter, NgbDateNativeUTCAdapter } from '@ng-bootstrap/ng-bootstrap';
import { ChemigationPermitService } from 'src/app/services/chemigation-permit.service';
import { ChemigationPermitNewDto } from 'src/app/shared/models/chemigation-permit-new-dto';
import { ChemigationPermitStatusEnum } from 'src/app/shared/models/enums/chemigation-permit-status.enum'
import { ChemigationCountyDto } from 'src/app/shared/models/generated/chemigation-county-dto';
import { ChemigationInjectionUnitTypeDto } from 'src/app/shared/models/generated/chemigation-injection-unit-type-dto';
import { runInThisContext } from 'vm';
import { ChemigationPermitAnnualRecordUpsertDto } from 'src/app/shared/models/chemigation-permit-annual-record-upsert-dto';
import { ChemigationPermitAnnualRecordStatusEnum } from 'src/app/shared/models/enums/chemigation-permit-annual-record-status.enum';
@Component({
  selector: 'zybach-chemigation-new-permit',
  templateUrl: './chemigation-new-permit.component.html',
  styleUrls: ['./chemigation-new-permit.component.scss'],
  providers: [{provide: NgbDateAdapter, useClass: NgbDateNativeUTCAdapter}]
})

export class ChemigationNewPermitComponent implements OnInit, OnDestroy {
  private watchUserChangeSubscription: any;
  private currentUser: UserDetailedDto;
  
  public permitStatuses: Array<ChemigationPermitStatusDto>;
  public chemigationCounties: Array<ChemigationCountyDto>;
  public injectionUnitTypes: Array<ChemigationInjectionUnitTypeDto>;
  public model: ChemigationPermitNewDto;
  
  public isLoadingSubmit: boolean = false;

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
    this.model.ChemigationPermitAnnualRecord = new ChemigationPermitAnnualRecordUpsertDto(null, null, null); 
    // {
    //   ChemigationPermitID : null,
    //   ChemigationPermitAnnualRecordStatusID: ChemigationPermitAnnualRecordStatusEnum.PendingPayment,
    //   ChemigationInjectionUnitTypeID : null,
    //   PivotName : null,
    //   RecordYear: new Date().getFullYear(),
    //   ApplicantFirstName : null,
    //   ApplicantLastName : null,
    //   ApplicantMailingAddress : null,
    //   ApplicantCity : null,
    //   ApplicantState : null,
    //   ApplicantZipCode : null,
    //   ApplicantPhone : null,
    //   ApplicantMobilePhone : null,
    //   ApplicantEmail : null,
    //   DateReceived : null,
    //   DatePaid : null,  
    // };

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
      this.chemigationPermitService.getAllChemigationInjectionUnitTypes().subscribe(injectionUnitTypes => {
        this.injectionUnitTypes = injectionUnitTypes;
      });

    });

  }

  ngOnDestroy() {
    this.watchUserChangeSubscription.unsubscribe();
    this.authenticationService.dispose();
    this.cdr.detach();
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