import { DecimalPipe } from '@angular/common';
import { ChangeDetectorRef, Component, Input, OnInit } from '@angular/core';
import { WellService } from 'src/app/shared/generated/api/well.service';
import { WellDetailDto } from '../../generated/model/well-detail-dto';
import { SensorStatusService } from 'src/app/shared/generated/api/sensor-status.service';
import { SensorMessageAgeDto } from '../../generated/model/sensor-message-age-dto';
import { SensorChartDataDto } from '../../generated/model/sensor-chart-data-dto';

@Component({
  selector: 'zybach-well-water-data-tab',
  templateUrl: './well-water-data-tab.component.html',
  styleUrls: ['./well-water-data-tab.component.scss']
})
export class WellWaterDataTabComponent implements OnInit {
  @Input() well: WellDetailDto;

  sensorsWithStatus: SensorMessageAgeDto[];
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

      // for (var sensor of this.sensorsWithStatus){
      //   sensor.MessageAge = Math.floor(sensor.MessageAge / 60)
      // }
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
}
