import { ChangeDetectorRef, Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { WaterLevelInspectionService } from 'src/app/shared/generated/api/water-level-inspection.service';
import { WaterLevelInspectionUpsertComponent } from 'src/app/shared/components/water-level-inspection-upsert/water-level-inspection-upsert.component';
import { UserDto } from 'src/app/shared/generated/model/user-dto';
import { WaterLevelInspectionSimpleDto } from 'src/app/shared/generated/model/water-level-inspection-simple-dto';
import { WaterLevelInspectionUpsertDto } from 'src/app/shared/generated/model/water-level-inspection-upsert-dto';
import { Alert } from 'src/app/shared/models/alert';
import { AlertContext } from 'src/app/shared/models/enums/alert-context.enum';
import { AlertService } from 'src/app/shared/services/alert.service';


@Component({
  selector: 'zybach-water-level-inspection-edit',
  templateUrl: './water-level-inspection-edit.component.html',
  styleUrls: ['./water-level-inspection-edit.component.scss']
})
export class WaterLevelInspectionEditComponent implements OnInit, OnDestroy {
  @ViewChild('inspectionUpsertForm') private waterLevelInspectionUpsertComponent : WaterLevelInspectionUpsertComponent;

  public currentUser: UserDto;

  public waterLevelInspectionID: number;
  public waterLevelInspection: WaterLevelInspectionUpsertDto;

  public isLoadingSubmit: boolean;

  constructor(
    private waterLevelInspectionService: WaterLevelInspectionService,
    private authenticationService: AuthenticationService,
    private route: ActivatedRoute,
    private router: Router,
    private cdr: ChangeDetectorRef,
    private alertService: AlertService
  ) { }

  ngOnInit(): void {
    this.authenticationService.getCurrentUser().subscribe(currentUser => {
      this.currentUser = currentUser;
      this.waterLevelInspectionID = parseInt(this.route.snapshot.paramMap.get("id"));

      this.waterLevelInspectionService.waterLevelInspectionsWaterLevelInspectionIDGet(this.waterLevelInspectionID).subscribe(waterLevelInspectionSimpleDto => {
        this.initializeInspectionModel(waterLevelInspectionSimpleDto);
      });
      this.cdr.detectChanges();
    });
  }

  private initializeInspectionModel(waterLevelInspectionSimpleDto: WaterLevelInspectionSimpleDto) : void {
    var waterLevelInspectionUpsertDto = new WaterLevelInspectionUpsertDto();

    waterLevelInspectionUpsertDto.WellRegistrationID = waterLevelInspectionSimpleDto.Well.WellRegistrationID;
    waterLevelInspectionUpsertDto.InspectionDate = waterLevelInspectionSimpleDto.InspectionDate;
    waterLevelInspectionUpsertDto.InspectorUserID = waterLevelInspectionSimpleDto.InspectorUserID;
    waterLevelInspectionUpsertDto.HasBrokenTape = waterLevelInspectionSimpleDto.HasBrokenTape;
    waterLevelInspectionUpsertDto.HasOil = waterLevelInspectionSimpleDto.HasOil;
    waterLevelInspectionUpsertDto.Measurement = waterLevelInspectionSimpleDto.Measurement;
    waterLevelInspectionUpsertDto.MeasuringEquipment = waterLevelInspectionSimpleDto.MeasuringEquipment;
    waterLevelInspectionUpsertDto.InspectionNotes = waterLevelInspectionSimpleDto.InspectionNotes;
    waterLevelInspectionUpsertDto.InspectionNickname = waterLevelInspectionSimpleDto.InspectionNickname;
  
    this.waterLevelInspection = waterLevelInspectionUpsertDto;
  }

  public onSubmit(editWaterLevelInspectionForm: HTMLFormElement): void {
    this.isLoadingSubmit = true;
    this.alertService.clearAlerts();
  
    this.waterLevelInspectionService.waterLevelInspectionsWaterLevelInspectionIDPut(this.waterLevelInspectionID, this.waterLevelInspection)
      .subscribe(response => {
        this.isLoadingSubmit = false;
        editWaterLevelInspectionForm.reset();
        this.router.navigateByUrl("/water-level-inspections/" + this.waterLevelInspectionID).then(() => {
          this.alertService.pushAlert(new Alert(`Water Level Inspection Record updated.`, AlertContext.Success));
        });
      }
        ,
        error => {
          this.isLoadingSubmit = false;
          this.cdr.detectChanges();
        }
      );
  }

  ngOnDestroy() {
    this.cdr.detach();
  }
}





  



