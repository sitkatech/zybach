import { ChangeDetectorRef, Component, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { WaterQualityInspectionService } from 'src/app/services/water-quality-inspection.service';
import { WaterQualityInspectionUpsertComponent } from 'src/app/shared/components/water-quality-inspection-upsert/water-quality-inspection-upsert.component';
import { UserDto } from 'src/app/shared/generated/model/user-dto';
import { WaterQualityInspectionUpsertDto } from 'src/app/shared/generated/model/water-quality-inspection-upsert-dto';
import { Alert } from 'src/app/shared/models/alert';
import { AlertContext } from 'src/app/shared/models/enums/alert-context.enum';
import { AlertService } from 'src/app/shared/services/alert.service';

@Component({
  selector: 'zybach-water-quality-inspection-new',
  templateUrl: './water-quality-inspection-new.component.html',
  styleUrls: ['./water-quality-inspection-new.component.scss']
})
export class WaterQualityInspectionNewComponent implements OnInit {
  @ViewChild('inspectionUpsertForm') private waterQualityInspectionUpsertComponent : WaterQualityInspectionUpsertComponent;

  public watchUserChangeSubscription: any;
  public currentUser: UserDto;

  public inspection: WaterQualityInspectionUpsertDto;
  public isLoadingSubmit: boolean;
  
  constructor(
    private waterQualityInspectionService: WaterQualityInspectionService,
    private authenticationService: AuthenticationService,
    private router: Router,
    private cdr: ChangeDetectorRef,
    private alertService: AlertService
  ) { }

  ngOnInit(): void {
    this.watchUserChangeSubscription = this.authenticationService.currentUserSetObservable.subscribe(currentUser => {
      this.currentUser = currentUser;
      this.initializeInspectionModel();
      this.cdr.detectChanges();
    });
  }

  private initializeInspectionModel() : void {
    var waterQualityInspectionUpsertDto = new WaterQualityInspectionUpsertDto();
    waterQualityInspectionUpsertDto.WellRegistrationID = null;
    waterQualityInspectionUpsertDto.WaterQualityInspectionTypeID = null;
    waterQualityInspectionUpsertDto.InspectionDate = null;
    waterQualityInspectionUpsertDto.InspectorUserID = null;
    waterQualityInspectionUpsertDto.Temperature = null;
    waterQualityInspectionUpsertDto.PH = null;
    waterQualityInspectionUpsertDto.Conductivity = null;
    waterQualityInspectionUpsertDto.FieldAlkilinity = null;
    waterQualityInspectionUpsertDto.FieldNitrates = null;
    waterQualityInspectionUpsertDto.LabNitrates = null;
    waterQualityInspectionUpsertDto.Salinity = null;
    waterQualityInspectionUpsertDto.MV = null;
    waterQualityInspectionUpsertDto.Sodium = null;
    waterQualityInspectionUpsertDto.Calcium = null;
    waterQualityInspectionUpsertDto.Magnesium = null;
    waterQualityInspectionUpsertDto.Potassium = null;
    waterQualityInspectionUpsertDto.HydrogenCarbonate = null;
    waterQualityInspectionUpsertDto.CalciumCarbonate = null;
    waterQualityInspectionUpsertDto.Sulfate = null;
    waterQualityInspectionUpsertDto.Chloride = null;
    waterQualityInspectionUpsertDto.SiliconDioxide = null;
    waterQualityInspectionUpsertDto.CropTypeID = null;
    waterQualityInspectionUpsertDto.PreWaterLevel = null;
    waterQualityInspectionUpsertDto.PostWaterLevel = null;
    waterQualityInspectionUpsertDto.InspectionNotes = null;

    this.inspection = waterQualityInspectionUpsertDto;
  }

  ngOnDestroy() {
    this.watchUserChangeSubscription.unsubscribe();
    this.authenticationService.dispose();
    this.cdr.detach();
  }

  public onSubmit(addWaterQualityInspectionForm: HTMLFormElement): void {
    this.isLoadingSubmit = true;
  
    this.waterQualityInspectionService.createWaterQualityInspection(this.inspection)
      .subscribe(response => {
        this.isLoadingSubmit = false;
        addWaterQualityInspectionForm.reset();
        this.router.navigateByUrl("/water-quality-inspections").then(() => {
          this.alertService.pushAlert(new Alert(`Water Quality Inspection Record added.`, AlertContext.Success));
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
