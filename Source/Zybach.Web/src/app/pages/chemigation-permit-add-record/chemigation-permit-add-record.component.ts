import { ChangeDetectorRef, Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbDateAdapter, NgbDateNativeUTCAdapter } from '@ng-bootstrap/ng-bootstrap';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { ChemigationPermitService } from 'src/app/services/chemigation-permit.service';
import { ChemigationPermitAnnualRecordUpsertComponent } from 'src/app/shared/components/chemigation-permit-annual-record-upsert/chemigation-permit-annual-record-upsert.component';
import { UserDetailedDto } from 'src/app/shared/models';
import { Alert } from 'src/app/shared/models/alert';
import { ChemigationPermitAnnualRecordUpsertDto } from 'src/app/shared/models/chemigation-permit-annual-record-upsert-dto';
import { AlertContext } from 'src/app/shared/models/enums/alert-context.enum';
import { ChemigationPermitAnnualRecordStatusEnum } from 'src/app/shared/models/enums/chemigation-permit-annual-record-status.enum';
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
  @ViewChild('annualRecordForm') private chemigationPermitAnnualRecordUpsertComponent : ChemigationPermitAnnualRecordUpsertComponent;
  
  private watchUserChangeSubscription: any;
  private currentUser: UserDetailedDto;
  
  public chemigationPermitNumber: number;
  public chemigationPermit: ChemigationPermitDto;

  public permitStatuses: Array<ChemigationPermitStatusDto>;
  public model: ChemigationPermitAnnualRecordUpsertDto;
  public newRecordYear: number;
  
  public isLoadingSubmit: boolean = false;
  public isAnnualRecordFormValidCheck: boolean;

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
  
  private isAnnualRecordFormValid(formValid: any) : void {
    this.isAnnualRecordFormValidCheck = formValid;
  }

  public isFormValid(editChemigationPermitAnnualRecordForm: any) : boolean{
    return this.isLoadingSubmit || !this.isAnnualRecordFormValidCheck || !editChemigationPermitAnnualRecordForm.form.valid;
  }

  onSubmit(addChemigationPermitAnnualRecordForm: HTMLFormElement): void {
    this.isLoadingSubmit = true;
  
    this.chemigationPermitService.createChemigationPermitAnnualRecord(this.model)
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



