import { ChangeDetectorRef, Component, Input, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { WaterQualityInspectionService } from 'src/app/services/water-quality-inspection.service';
import { WellService } from 'src/app/services/well.service';
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
  public isInspectionUpsertFormValidCheck: boolean;
  public wellID: number;
  
  constructor(
    private waterQualityInspectionService: WaterQualityInspectionService,
    private wellService: WellService,
    private authenticationService: AuthenticationService,
    private route: ActivatedRoute,
    private router: Router,
    private cdr: ChangeDetectorRef,
    private alertService: AlertService
  ) { }

  ngOnInit(): void {
    this.authenticationService.getCurrentUser().subscribe(currentUser => {
      this.currentUser = currentUser;
      this.wellID = parseInt(this.route.snapshot.paramMap.get("id"));

      if (this.wellID > 0) {
        this.wellService.getWellSimpleDto(this.wellID).subscribe(well => {
          this.initializeInspectionModel(well.WellRegistrationID);
        });
      } else {
        this.initializeInspectionModel(null);
      }

      this.cdr.detectChanges();
    });
  }

  private initializeInspectionModel(wellRegistrationID: string) : void {
    var waterQualityInspectionUpsertDto = new WaterQualityInspectionUpsertDto();
    waterQualityInspectionUpsertDto.WellRegistrationID = wellRegistrationID;
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
    waterQualityInspectionUpsertDto.InspectionNickname = null;

    this.inspection = waterQualityInspectionUpsertDto;
  }

  ngOnDestroy() {
    this.cdr.detach();
  }

  public isInspectionUpsertFormValid(formValid: any): void {
    this.isInspectionUpsertFormValidCheck = formValid;
  }

  public isFormValid(addWaterQualityInspectionForm: any) : boolean{
    return this.isLoadingSubmit || !this.isInspectionUpsertFormValidCheck || !addWaterQualityInspectionForm.form.valid;
  }

  public onSubmit(addWaterQualityInspectionForm: HTMLFormElement): void {
    this.isLoadingSubmit = true;
  
    this.waterQualityInspectionService.createWaterQualityInspection(this.inspection)
      .subscribe(response => {
        this.isLoadingSubmit = false;
        addWaterQualityInspectionForm.reset();
        this.router.navigateByUrl("/water-quality-inspections/" + response.WaterQualityInspectionID).then(() => {
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
