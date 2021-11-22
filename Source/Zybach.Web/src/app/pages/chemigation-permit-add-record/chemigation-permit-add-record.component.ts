import { ChangeDetectorRef, Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbDateAdapter, NgbDateNativeUTCAdapter } from '@ng-bootstrap/ng-bootstrap';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { ChemigationPermitService } from 'src/app/services/chemigation-permit.service';
import { UserDetailedDto } from 'src/app/shared/models';
import { Alert } from 'src/app/shared/models/alert';
import { ChemigationPermitAnnualRecordUpsertDto } from 'src/app/shared/models/chemigation-permit-annual-record-upsert-dto';
import { AlertContext } from 'src/app/shared/models/enums/alert-context.enum';
import { ChemigationPermitAnnualRecordStatusEnum } from 'src/app/shared/models/enums/chemigation-permit-annual-record-status.enum';
import { ChemigationInjectionUnitTypeDto } from 'src/app/shared/models/generated/chemigation-injection-unit-type-dto';
import { ChemigationPermitAnnualRecordDto } from 'src/app/shared/models/generated/chemigation-permit-annual-record-dto';
import { ChemigationPermitAnnualRecordStatusDto } from 'src/app/shared/models/generated/chemigation-permit-annual-record-status-dto';
import { ChemigationPermitDto } from 'src/app/shared/models/generated/chemigation-permit-dto';
import { ChemigationPermitStatusDto } from 'src/app/shared/models/generated/chemigation-permit-status-dto';
import { AlertService } from 'src/app/shared/services/alert.service';

@Component({
  selector: 'zybach-chemigation-permit-add-record',
  templateUrl: './chemigation-permit-add-record.component.html',
  styleUrls: ['./chemigation-permit-add-record.component.scss'],
  providers: [{provide: NgbDateAdapter, useClass: NgbDateNativeUTCAdapter}]
})
export class ChemigationPermitAddRecordComponent implements OnInit, OnDestroy {
  private watchUserChangeSubscription: any;
  private currentUser: UserDetailedDto;
  
  public chemigationPermitNumber: number;
  public chemigationPermit: ChemigationPermitDto;

  public permitStatuses: Array<ChemigationPermitStatusDto>;
  public injectionUnitTypes: Array<ChemigationInjectionUnitTypeDto>;
  public annualRecordStatuses: Array<ChemigationPermitAnnualRecordStatusDto>;
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
    this.watchUserChangeSubscription = this.authenticationService.currentUserSetObservable.subscribe(currentUser => {
      this.currentUser = currentUser;
      this.newRecordYear = new Date().getFullYear();
  
      if (!this.authenticationService.isUserAnAdministrator(this.currentUser)) {
        this.router.navigateByUrl("/not-found")
          .then();
        return;
      }

      this.chemigationPermitService.getAllChemigationPermitStatuses().subscribe(permitStatuses => {
        this.permitStatuses = permitStatuses;
      });

      this.chemigationPermitService.getAnnualRecordStatusTypes().subscribe(annualRecordStatuses => {
        this.annualRecordStatuses = annualRecordStatuses;
      });
      
      this.chemigationPermitService.getAllChemigationInjectionUnitTypes().subscribe(injectionUnitTypes => {
        this.injectionUnitTypes = injectionUnitTypes;
      });
      
      this.chemigationPermitNumber = parseInt(this.route.snapshot.paramMap.get("permit-number"));

      this.chemigationPermitService.getLatestAnnualRecordByPermitNumber(this.chemigationPermitNumber).subscribe(annualRecord => {
        this.chemigationPermit = annualRecord.ChemigationPermit;
        var chemigationPermitAnnualRecordUpsertDto = new ChemigationPermitAnnualRecordUpsertDto(annualRecord, this.newRecordYear, ChemigationPermitAnnualRecordStatusEnum.PendingPayment);
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

  onSubmit(editChemigationPermitAnnualRecordForm: HTMLFormElement): void {
    this.isLoadingSubmit = true;
  
    this.chemigationPermitService.createChemigationPermitAnnualRecord(this.model)
      .subscribe(response => {
        this.isLoadingSubmit = false;
        editChemigationPermitAnnualRecordForm.reset();
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



