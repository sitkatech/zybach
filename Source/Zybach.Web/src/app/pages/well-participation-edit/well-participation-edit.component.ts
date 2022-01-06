import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { forkJoin } from 'rxjs';
import { WellService } from 'src/app/services/well.service';
import { CountyDto } from 'src/app/shared/generated/model/county-dto';
import { WellParticipationDto } from 'src/app/shared/generated/model/well-participation-dto';
import { WellParticipationInfoDto } from 'src/app/shared/generated/model/well-participation-info-dto';
import { WellUseDto } from 'src/app/shared/generated/model/well-use-dto';
import { Alert } from 'src/app/shared/models/alert';
import { AlertContext } from 'src/app/shared/models/enums/alert-context.enum';
import { AlertService } from 'src/app/shared/services/alert.service';

@Component({
  selector: 'zybach-well-participation-edit',
  templateUrl: './well-participation-edit.component.html',
  styleUrls: ['./well-participation-edit.component.scss']
})
export class WellParticipationEditComponent implements OnInit {

  public wellRegistrationID: string;
  public wellParticipationInfo: WellParticipationInfoDto;

  public wellUses: Array<WellUseDto>;
  public wellParticipations: Array<WellParticipationDto>;

  public isLoadingSubmit: boolean;

  constructor(
    private wellService: WellService,
    private route: ActivatedRoute,
    private router: Router,
    private cdr: ChangeDetectorRef,
    private alertService: AlertService
  ) { }

  ngOnInit(): void {
    this.wellRegistrationID = this.route.snapshot.paramMap.get("wellRegistrationID");
    
    forkJoin({
      wellParticipationInfo: this.wellService.getWellParticipationDetails(this.wellRegistrationID),
      wellUses: this.wellService.getWellUses(),
      wellParticipations: this.wellService.getWellParticipations()
    }).subscribe(({ wellParticipationInfo, wellUses, wellParticipations }) => {
      this.wellParticipationInfo = wellParticipationInfo;
      this.wellUses = wellUses;
      this.wellParticipations = wellParticipations;

      this.cdr.detectChanges();
    });

  }

  public onSubmit(editWellParticipationForm: HTMLFormElement): void {
    this.isLoadingSubmit = true;
  
    this.wellService.updateWellParticipationDetails(this.wellRegistrationID, this.wellParticipationInfo)
      .subscribe(response => {
        this.isLoadingSubmit = false;
        editWellParticipationForm.reset();
        this.router.navigateByUrl("/wells/" + this.wellRegistrationID).then(() => {
          this.alertService.pushAlert(new Alert(`Well participation details updated.`, AlertContext.Success));
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
