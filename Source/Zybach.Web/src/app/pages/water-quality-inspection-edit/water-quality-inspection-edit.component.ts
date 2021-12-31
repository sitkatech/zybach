import { ChangeDetectorRef, Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { WaterQualityInspectionService } from 'src/app/services/water-quality-inspection.service';
import { WaterQualityInspectionUpsertComponent } from 'src/app/shared/components/water-quality-inspection-upsert/water-quality-inspection-upsert.component';
import { UserDto } from 'src/app/shared/generated/model/user-dto';
import { WaterQualityInspectionSimpleDto } from 'src/app/shared/generated/model/water-quality-inspection-simple-dto';
import { WaterQualityInspectionUpsertDto } from 'src/app/shared/generated/model/water-quality-inspection-upsert-dto';
import { Alert } from 'src/app/shared/models/alert';
import { AlertContext } from 'src/app/shared/models/enums/alert-context.enum';
import { AlertService } from 'src/app/shared/services/alert.service';

@Component({
  selector: 'zybach-water-quality-inspection-edit',
  templateUrl: './water-quality-inspection-edit.component.html',
  styleUrls: ['./water-quality-inspection-edit.component.scss']
})
export class WaterQualityInspectionEditComponent implements OnInit {
  @ViewChild('inspectionUpsertForm') private waterQualityInspectionUpsertComponent : WaterQualityInspectionUpsertComponent;

  public watchUserChangeSubscription: any;
  public currentUser: UserDto;

  public waterQualityInspectionID: number;
  public waterQualityInspection: WaterQualityInspectionUpsertDto;

  public isLoadingSubmit: boolean;
  public isInspectionUpsertFormValidCheck: boolean;

  constructor(
    private waterQualityInspectionService: WaterQualityInspectionService,
    private authenticationService: AuthenticationService,
    private route: ActivatedRoute,
    private router: Router,
    private cdr: ChangeDetectorRef,
    private alertService: AlertService
  ) { }

  ngOnInit(): void {
    this.watchUserChangeSubscription = this.authenticationService.currentUserSetObservable.subscribe(currentUser => {
      this.currentUser = currentUser;
      this.waterQualityInspectionID = parseInt(this.route.snapshot.paramMap.get("id"));

      this.waterQualityInspectionService.getByID(this.waterQualityInspectionID).subscribe(waterQualityInspectionSimpleDto => {
        this.initializeInspectionModel(waterQualityInspectionSimpleDto);
      });
      this.cdr.detectChanges();
    });
  }
  
  private initializeInspectionModel(waterQualityInspectionSimpleDto: WaterQualityInspectionSimpleDto) : void {
    var waterQualityInspectionUpsertDto = new WaterQualityInspectionUpsertDto();

    waterQualityInspectionUpsertDto.WellRegistrationID = waterQualityInspectionSimpleDto.Well.WellRegistrationID;
    waterQualityInspectionUpsertDto.WaterQualityInspectionTypeID = waterQualityInspectionSimpleDto.WaterQualityInspectionTypeID;
    waterQualityInspectionUpsertDto.InspectionDate = waterQualityInspectionSimpleDto.InspectionDate;
    waterQualityInspectionUpsertDto.InspectorUserID = waterQualityInspectionSimpleDto.InspectorUserID;
    waterQualityInspectionUpsertDto.Temperature = waterQualityInspectionSimpleDto.Temperature;
    waterQualityInspectionUpsertDto.PH = waterQualityInspectionSimpleDto.PH;
    waterQualityInspectionUpsertDto.Conductivity = waterQualityInspectionSimpleDto.Conductivity;
    waterQualityInspectionUpsertDto.FieldAlkilinity = waterQualityInspectionSimpleDto.FieldAlkilinity;
    waterQualityInspectionUpsertDto.FieldNitrates = waterQualityInspectionSimpleDto.FieldNitrates;
    waterQualityInspectionUpsertDto.LabNitrates = waterQualityInspectionSimpleDto.LabNitrates;
    waterQualityInspectionUpsertDto.Salinity = waterQualityInspectionSimpleDto.Salinity;
    waterQualityInspectionUpsertDto.MV = waterQualityInspectionSimpleDto.MV;
    waterQualityInspectionUpsertDto.Sodium = waterQualityInspectionSimpleDto.Sodium;
    waterQualityInspectionUpsertDto.Calcium = waterQualityInspectionSimpleDto.Calcium;
    waterQualityInspectionUpsertDto.Magnesium = waterQualityInspectionSimpleDto.Magnesium;
    waterQualityInspectionUpsertDto.Potassium = waterQualityInspectionSimpleDto.Potassium;
    waterQualityInspectionUpsertDto.HydrogenCarbonate = waterQualityInspectionSimpleDto.HydrogenCarbonate;
    waterQualityInspectionUpsertDto.CalciumCarbonate = waterQualityInspectionSimpleDto.CalciumCarbonate;
    waterQualityInspectionUpsertDto.Sulfate = waterQualityInspectionSimpleDto.Sulfate;
    waterQualityInspectionUpsertDto.Chloride = waterQualityInspectionSimpleDto.Chloride;
    waterQualityInspectionUpsertDto.SiliconDioxide = waterQualityInspectionSimpleDto.SiliconDioxide;
    waterQualityInspectionUpsertDto.CropTypeID = waterQualityInspectionSimpleDto.CropTypeID;
    waterQualityInspectionUpsertDto.PreWaterLevel = waterQualityInspectionSimpleDto.PreWaterLevel;
    waterQualityInspectionUpsertDto.PostWaterLevel = waterQualityInspectionSimpleDto.PostWaterLevel;
    waterQualityInspectionUpsertDto.InspectionNotes = waterQualityInspectionSimpleDto.InspectionNotes;
  
    this.waterQualityInspection = waterQualityInspectionUpsertDto;
  }

  ngOnDestroy() {
    this.watchUserChangeSubscription.unsubscribe();
    this.authenticationService.dispose();
    this.cdr.detach();
  }

  public isInspectionUpsertFormValid(formValid: any): void {
    this.isInspectionUpsertFormValidCheck = formValid;
  }

  public isFormValid(editWaterQualityInspectionForm: any) : boolean{
    return this.isLoadingSubmit || !this.isInspectionUpsertFormValidCheck || !editWaterQualityInspectionForm.form.valid;
  }

  
  public onSubmit(editWaterQualityInspectionForm: HTMLFormElement): void {
      this.isLoadingSubmit = true;
    
      this.waterQualityInspectionService.updateWaterQualityInspection(this.waterQualityInspectionID, this.waterQualityInspection)
        .subscribe(response => {
          this.isLoadingSubmit = false;
          editWaterQualityInspectionForm.reset();
          this.router.navigateByUrl("/water-quality-inspections/" + this.waterQualityInspectionID).then(() => {
            this.alertService.pushAlert(new Alert(`Water Quality Inspection Record updated.`, AlertContext.Success));
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

