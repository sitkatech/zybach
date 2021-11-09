import { ChangeDetectorRef, Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { ChemigationInspectionService } from 'src/app/services/chemigation-inspection.service';
import { UserDetailedDto } from 'src/app/shared/models';
import { Alert } from 'src/app/shared/models/alert';
import { ChemigationPermitUpsertDto } from 'src/app/shared/models/chemigation-permit-upsert-dto';
import { AlertContext } from 'src/app/shared/models/enums/alert-context.enum';
import { AlertService } from 'src/app/shared/services/alert.service';

@Component({
  selector: 'zybach-chemigation-new-permit',
  templateUrl: './chemigation-new-permit.component.html',
  styleUrls: ['./chemigation-new-permit.component.scss']
})

export class ChemigationNewPermitComponent implements OnInit, OnDestroy {
  private watchUserChangeSubscription: any;
  private currentUser: UserDetailedDto;
  
  public model: ChemigationPermitUpsertDto;
  
  public isLoadingSubmit: boolean = false;

  constructor(
    private cdr: ChangeDetectorRef,
    private router: Router, 
    private chemigationPermitService: ChemigationInspectionService,
    private authenticationService: AuthenticationService, 
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
          this.alertService.pushAlert(new Alert("Chemigation Permit '" + response.ChemigationPermitNumber + "' successfully created.", AlertContext.Success));
        });
      },
        error => {
          this.isLoadingSubmit = false;
          this.cdr.detectChanges();
        }
      );
  }

}