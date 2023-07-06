import { DecimalPipe } from '@angular/common';
import { ChangeDetectorRef, Component, Input, OnInit } from '@angular/core';
import { WellService } from 'src/app/shared/generated/api/well.service';
import { WellDetailDto } from '../../generated/model/well-detail-dto';
import { SensorStatusService } from 'src/app/shared/generated/api/sensor-status.service';
import { SensorSimpleDto } from '../../generated/model/sensor-simple-dto';
import { SensorChartDataDto } from '../../generated/model/sensor-chart-data-dto';
import { ConfirmService } from 'src/app/services/confirm.service';
import { AlertService } from '../../services/alert.service';
import { Alert } from '../../models/alert';
import { AlertContext } from '../../models/enums/alert-context.enum';
import { Router } from '@angular/router';

@Component({
  selector: 'zybach-well-water-data-tab',
  templateUrl: './well-water-data-tab.component.html',
  styleUrls: ['./well-water-data-tab.component.scss']
})
export class WellWaterDataTabComponent implements OnInit {
  @Input() well: WellDetailDto;

  sensorsWithStatus: SensorSimpleDto[];
  public years: number[];
  public unitsShown: string = "gal";
  public sensorChartData: SensorChartDataDto;

  public isLoadingSubmit: boolean = false;
  public math = Math;

  constructor(
    private wellService: WellService,
    private sensorService: SensorStatusService,
    private cdr: ChangeDetectorRef,
    private decimalPipe: DecimalPipe,
    private confirmService: ConfirmService,
    private alertService: AlertService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.years = []
    const currentDate = new Date();
    const currentYear = currentDate.getFullYear();
    for (var year = currentYear; year >= 2016; year--){
      this.years.push(year);
    }

    this.getSensorsWithAgeMessages();
    this.wellService.wellsWellIDFlowMeterSensorsGet(this.well.WellID).subscribe(sensorChartData =>
      {
        this.sensorChartData = sensorChartData;
      }
    );
  }
  
  getSensorsWithAgeMessages(){
    this.sensorService.sensorStatusWellIDGet(this.well.WellID).subscribe(wellWithSensorMessageAge => {
      this.sensorsWithStatus = wellWithSensorMessageAge.Sensors;
    });
  }

  getAnnualPumpedVolume(year, dataSource){
    const annualPumpedVolume = this.well.AnnualPumpedVolume.find(x=> 
      x.Year === year && x.DataSource === dataSource
    )

    if (!annualPumpedVolume || ! annualPumpedVolume.Gallons){
      return "-"
    }
    

    if (this.unitsShown == "gal") {
      const value = this.decimalPipe.transform(annualPumpedVolume.Gallons , "1.0-0")
      return `${value} ${this.unitsShown}`; 
    }

    const irrigatedAcresPerYear = this.well.IrrigatedAcresPerYear.find(x => x.Year === year);

    if (!irrigatedAcresPerYear || (irrigatedAcresPerYear.Acres == null || irrigatedAcresPerYear.Acres == undefined)) {
      return "-";
    }

    const value = this.decimalPipe.transform((annualPumpedVolume.Gallons / 27154) / irrigatedAcresPerYear.Acres , "1.1-1")
    return `${value} ${this.unitsShown}`;
  }

  
  displayIrrigatedAcres(): boolean {
    if (!this.well || ! this.well.IrrigatedAcresPerYear) {
      return false;
    }

    return this.well.IrrigatedAcresPerYear.length > 0 && !this.well.IrrigatedAcresPerYear.every(x => x.Acres == null || x.Acres == undefined);
  }

  public toggleUnitsShown(units : string): void {
    this.unitsShown = units;
  }

  syncWithAgHub()
  {
    this.confirmService.confirm({ modalSize: "md", buttonClassYes: "btn-zybach", buttonTextYes: "Yes", buttonTextNo: "No", title: "Sync Pumped Volume from AgHub", message: "Are you sure you want to sync all pumped volume data for this well?" }).then(confirmed => {
      if (confirmed) {
        this.isLoadingSubmit = true;
        this.wellService.wellsWellIDSyncPumpedVolumeFromAgHubPost(this.well.WellID).subscribe(() => {
          this.isLoadingSubmit = false;
          this.router.navigateByUrl("/wells/" + this.well.WellID).then(() => {
            this.alertService.pushAlert(new Alert(`Pumped volume synced.`, AlertContext.Success));
          });          
        }, error => {
            this.isLoadingSubmit = false;
            this.cdr.detectChanges();
          }
        );
      }
    });
  }
}
