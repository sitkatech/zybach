import { ChangeDetectorRef, Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { WaterLevelInspectionService } from 'src/app/services/water-level-inspection.service';
import { WaterLevelInspectionUpsertComponent } from 'src/app/shared/components/water-level-inspection-upsert/water-level-inspection-upsert.component';
import { UserDto } from 'src/app/shared/generated/model/user-dto';
import { WaterLevelInspectionUpsertDto } from 'src/app/shared/generated/model/water-level-inspection-upsert-dto';
import { Alert } from 'src/app/shared/models/alert';
import { AlertContext } from 'src/app/shared/models/enums/alert-context.enum';
import { AlertService } from 'src/app/shared/services/alert.service';

@Component({
  selector: 'zybach-water-level-inspection-new',
  templateUrl: './water-level-inspection-new.component.html',
  styleUrls: ['./water-level-inspection-new.component.scss']
})
export class WaterLevelInspectionNewComponent implements OnInit, OnDestroy {
  @ViewChild('inspectionUpsertForm') private waterLevelInspectionUpsertComponent : WaterLevelInspectionUpsertComponent;

  public currentUser: UserDto;

  public inspection: WaterLevelInspectionUpsertDto;
  public isLoadingSubmit: boolean;

  constructor(
    private waterLevelInspectionService: WaterLevelInspectionService,
    private authenticationService: AuthenticationService,
    private router: Router,
    private cdr: ChangeDetectorRef,
    private alertService: AlertService
  ) { }

  ngOnInit(): void {
    this.authenticationService.getCurrentUser().subscribe(currentUser => {
      this.currentUser = currentUser;
      this.initializeInspectionModel();
      this.cdr.detectChanges();
    });
  }

  private initializeInspectionModel() : void {
    var waterLevelInspectionUpsertDto = new WaterLevelInspectionUpsertDto();
    waterLevelInspectionUpsertDto.WellRegistrationID = null;
    waterLevelInspectionUpsertDto.HasBrokenTape = false;
    waterLevelInspectionUpsertDto.HasOil = false;
    waterLevelInspectionUpsertDto.InspectionDate = null;
    waterLevelInspectionUpsertDto.InspectorUserID = null;
    waterLevelInspectionUpsertDto.Measurement = null;

    waterLevelInspectionUpsertDto.InspectionNotes = null;
    waterLevelInspectionUpsertDto.InspectionNickname = null;

    this.inspection = waterLevelInspectionUpsertDto;
  }

  ngOnDestroy() {
    this.cdr.detach();
  }

  public onSubmit(addWaterLevelInspectionForm: HTMLFormElement): void {
    this.isLoadingSubmit = true;
    this.alertService.clearAlerts();

    this.waterLevelInspectionService.createWaterLevelInspection(this.inspection)
      .subscribe(response => {
        this.isLoadingSubmit = false;
        addWaterLevelInspectionForm.reset();
        this.router.navigateByUrl("/water-level-inspections/" + response.WaterLevelInspectionID).then(() => {
          this.alertService.pushAlert(new Alert(`Water Level Inspection Record added.`, AlertContext.Success));
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





 
  
