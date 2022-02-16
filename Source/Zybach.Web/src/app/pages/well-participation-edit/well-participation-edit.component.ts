import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { forkJoin } from 'rxjs';
import { WaterQualityInspectionService } from 'src/app/services/water-quality-inspection.service';
import { WellService } from 'src/app/services/well.service';
import { WaterQualityInspectionTypeDto } from 'src/app/shared/generated/model/water-quality-inspection-type-dto';
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

  public wellID: number;
  public wellRegistrationID: string;
  public wellParticipationInfo: WellParticipationInfoDto;

  public wellUses: Array<WellUseDto>;
  public wellParticipations: Array<WellParticipationDto>;
  public waterQualityInspectionTypes: Array<WaterQualityInspectionTypeDto>;

  public isLoadingSubmit: boolean;

  constructor(
    private waterQualityInspectionService: WaterQualityInspectionService,
    private wellService: WellService,
    private route: ActivatedRoute,
    private router: Router,
    private cdr: ChangeDetectorRef,
    private alertService: AlertService
  ) { }

  ngOnInit(): void {
    this.wellID = parseInt(this.route.snapshot.paramMap.get("id"));
    forkJoin({
      wellParticipationInfo: this.wellService.getWellParticipationDetails(this.wellID),
      wellUses: this.wellService.getWellUses(),
      wellParticipations: this.wellService.getWellParticipations(),
      waterQualityInspectionTypes: this.waterQualityInspectionService.getWaterQualityInspectionTypes()
    }).subscribe(({ wellParticipationInfo, wellUses, wellParticipations, waterQualityInspectionTypes }) => {
      this.wellParticipationInfo = wellParticipationInfo;
      this.wellRegistrationID = wellParticipationInfo.WellRegistrationID; 
      this.wellUses = wellUses;
      this.wellParticipations = wellParticipations;
      this.waterQualityInspectionTypes = waterQualityInspectionTypes;

      this.cdr.detectChanges();
    });

  }

  public onSubmit(editWellParticipationForm: HTMLFormElement): void {
    this.isLoadingSubmit = true;
    
    this.wellService.updateWellParticipationDetails(this.wellID, this.wellParticipationInfo)
      .subscribe(response => {
        this.isLoadingSubmit = false;
        editWellParticipationForm.reset();
        this.router.navigateByUrl("/wells/" + this.wellID).then(() => {
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
