import { ChangeDetectorRef, Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { ChemigationPermitService } from 'src/app/services/chemigation-permit.service';
import { ChemigationPermitDto } from 'src/app/shared/generated/model/chemigation-permit-dto';
import { ChemigationPermitStatusDto } from 'src/app/shared/generated/model/chemigation-permit-status-dto';
import { ChemigationPermitUpsertDto } from 'src/app/shared/generated/model/chemigation-permit-upsert-dto';
import { UserDto } from 'src/app/shared/generated/model/user-dto';
import { Alert } from 'src/app/shared/models/alert';
import { AlertContext } from 'src/app/shared/models/enums/alert-context.enum';
import { AlertService } from 'src/app/shared/services/alert.service';

@Component({
  selector: 'zybach-chemigation-permit-edit',
  templateUrl: './chemigation-permit-edit.component.html',
  styleUrls: ['./chemigation-permit-edit.component.scss']
})
export class ChemigationPermitEditComponent implements OnInit, OnDestroy {

  
  private currentUser: UserDto;
  
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

    this.authenticationService.getCurrentUser().subscribe(currentUser => {
      this.currentUser = currentUser;
  
      if (!this.authenticationService.isUserAnAdministrator(this.currentUser)) {
        this.router.navigateByUrl("/not-found")
          .then();
        return;
      }

      this.chemigationPermitService.getChemigationPermitStatuses().subscribe(permitStatuses => {
        this.permitStatuses = permitStatuses;
      });

      this.chemigationPermitNumber = parseInt(this.route.snapshot.paramMap.get("permit-number"));
      this.chemigationPermitService.getChemigationPermitByPermitNumber(this.chemigationPermitNumber).subscribe(chemigationPermit => {
        this.chemigationPermit = chemigationPermit;
        this.model.ChemigationPermitStatusID = this.chemigationPermit.ChemigationPermitStatus.ChemigationPermitStatusID;
        this.model.CountyID = this.chemigationPermit.County.CountyID;
        this.cdr.detectChanges();
      });

    });
  }
  
  ngOnDestroy() {
    
       this.cdr.detach();
  }

  onSubmit(editChemigationPermitForm: HTMLFormElement): void {
    this.isLoadingSubmit = true;
  
    this.chemigationPermitService.updateChemigationPermitByID(this.chemigationPermit.ChemigationPermitID, this.model)
      .subscribe(response => {
        this.isLoadingSubmit = false;
        editChemigationPermitForm.reset();
        this.router.navigateByUrl("/chemigation-permits/" + this.chemigationPermitNumber).then(() => {
          this.alertService.pushAlert(new Alert(`Chemigation Permit ${this.chemigationPermitNumber} was successfully updated.`, AlertContext.Success));
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