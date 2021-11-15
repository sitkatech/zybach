import { ChangeDetectorRef, Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbDateAdapter, NgbDateNativeUTCAdapter } from '@ng-bootstrap/ng-bootstrap';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { ChemigationPermitService } from 'src/app/services/chemigation-permit.service';
import { UserDetailedDto } from 'src/app/shared/models';
import { Alert } from 'src/app/shared/models/alert';
import { ChemigationPermitUpsertDto } from 'src/app/shared/models/chemigation-permit-upsert-dto';
import { AlertContext } from 'src/app/shared/models/enums/alert-context.enum';
import { ChemigationPermitDto } from 'src/app/shared/models/generated/chemigation-permit-dto';
import { ChemigationPermitStatusDto } from 'src/app/shared/models/generated/chemigation-permit-status-dto';
import { AlertService } from 'src/app/shared/services/alert.service';

@Component({
  selector: 'zybach-chemigation-permit-edit',
  templateUrl: './chemigation-permit-edit.component.html',
  styleUrls: ['./chemigation-permit-edit.component.scss'],
  providers: [{provide: NgbDateAdapter, useClass: NgbDateNativeUTCAdapter}]
})
export class ChemigationPermitEditComponent implements OnInit, OnDestroy {

  private watchUserChangeSubscription: any;
  private currentUser: UserDetailedDto;
  
  public chemigationPermitNumber: number;
  public chemigationPermit: ChemigationPermitDto;
  public permitStatuses: Array<ChemigationPermitStatusDto>;
  public model: ChemigationPermitUpsertDto;
  
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
    this.model = new ChemigationPermitUpsertDto();

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

      this.chemigationPermitNumber = parseInt(this.route.snapshot.paramMap.get("permit-number"));
      this.chemigationPermitService.getChemigationPermitByPermitNumber(this.chemigationPermitNumber).subscribe(chemigationPermit => {
        this.chemigationPermit = chemigationPermit;
        this.model.ChemigationPermitNumber = this.chemigationPermit.ChemigationPermitNumber;
        this.model.ChemigationPermitStatusID = this.chemigationPermit.ChemigationPermitStatus.ChemigationPermitStatusID;
        // this.model.DateReceived = this.chemigationPermit.DateReceived;
        this.model.TownshipRangeSection = this.chemigationPermit.TownshipRangeSection;
        this.cdr.detectChanges();
      });

    });
  }
  
  ngOnDestroy() {
    this.watchUserChangeSubscription.unsubscribe();
    this.authenticationService.dispose();
    this.cdr.detach();
  }

  onSubmit(editChemigationPermitForm: HTMLFormElement): void {
    this.isLoadingSubmit = true;
  
    this.chemigationPermitService.updateChemigationPermitByID(this.chemigationPermit.ChemigationPermitID, this.model)
      .subscribe(response => {
        this.isLoadingSubmit = false;
        editChemigationPermitForm.reset();
        this.router.navigateByUrl("/chemigation-permits/" + response.ChemigationPermitNumber).then(() => {
          this.alertService.pushAlert(new Alert(`Chemigation Permit ${response.ChemigationPermitNumber} was successfully updated.`, AlertContext.Success));
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





