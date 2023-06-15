import { ChangeDetectorRef, Component, OnInit, ViewChild } from '@angular/core';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { SensorSimpleDto } from 'src/app/shared/generated/model/sensor-simple-dto';
import { WellWaterLevelMapSummaryDto } from 'src/app/shared/generated/model/well-water-level-map-summary-dto';
import { CustomRichTextTypeEnum } from 'src/app/shared/generated/enum/custom-rich-text-type-enum';
import { WellWaterLevelMapComponent } from '../well-water-level-map/well-water-level-map.component';
import { forkJoin } from 'rxjs';
import { MapDataService } from 'src/app/shared/generated/api/map-data.service';
import { WellService } from 'src/app/shared/generated/api/well.service';
import { SensorChartDataDto } from 'src/app/shared/generated/model/sensor-chart-data-dto';
import { default as vegaEmbed } from 'vega-embed';
import { WellGroupService } from 'src/app/shared/generated/api/well-group.service';
import { WellGroupSimpleDto } from 'src/app/shared/generated/model/well-group-simple-dto';
import { WellGroupDto } from 'src/app/shared/generated/model/well-group-dto';
import { WaterLevelInspectionsChartDataDto } from 'src/app/shared/generated/model/water-level-inspections-chart-data-dto';
import { AsyncParser } from 'json2csv';
import moment from 'moment';

@Component({
  selector: 'zybach-water-level-explorer',
  templateUrl: './water-level-explorer.component.html',
  styleUrls: ['./water-level-explorer.component.scss']
})
export class WaterLevelExplorerComponent implements OnInit {
  @ViewChild("wellMap") wellMap: WellWaterLevelMapComponent;

  public wellGroups: WellGroupDto[];

  public selectedWellGroup: WellGroupDto;
  public wellsGeoJson: any;
  
  public richTextTypeID: number = CustomRichTextTypeEnum.WaterLevelExplorerMap;
  public disclaimerRichTextTypeID: number = CustomRichTextTypeEnum.WaterLevelExplorerMapDisclaimer;

  public sensorChartData: SensorChartDataDto;
  public waterLevelInspectionChartData: WaterLevelInspectionsChartDataDto;
  public hasWaterLevelInspectionChartData: boolean;
  public waterLevelInspectionChartID = "waterLevelInspectionChart";
  public isDisplayingWaterLevelInspectionChart = false;
  
  constructor(
    private authenticationService: AuthenticationService,
    private cdr: ChangeDetectorRef,
    private mapDataService: MapDataService,
    private wellService: WellService,
    private wellGroupService: WellGroupService
  ) { }

  ngOnInit(): void {

    this.wellGroupService.wellGroupsGet().subscribe(wellGroups => {
      this.wellGroups = wellGroups;

      var wellsGeoJson = {
        type: "FeatureCollection",
        features: 
          wellGroups.filter(x => x.PrimaryWell.Location).map(x => {
            const geoJsonPoint = x.PrimaryWell.Location;
            geoJsonPoint.properties = {
              wellID: x.PrimaryWell.WellID,
              wellRegistrationID: x.PrimaryWell.WellRegistrationID,
              sensors: []
            }
            return geoJsonPoint;
          })
      };
      this.wellsGeoJson = wellsGeoJson;
    });
  }

  public onMapSelection(wellID: number) {
    this.selectedWellGroup = this.wellGroups.find(x => x.PrimaryWell.WellID == wellID);
    if (!this.selectedWellGroup) return;

    forkJoin({
      sensorChartData: this.wellGroupService.wellGroupsWellGroupIDWaterLevelSensorsChartSpecGet(this.selectedWellGroup.WellGroupID),
      waterLevelInspectionChartData: this.wellGroupService.wellGroupsWellGroupIDWaterLevelInspectionChartSpecGet(this.selectedWellGroup.WellGroupID)
    }).subscribe(({sensorChartData, waterLevelInspectionChartData}) => {
      this.sensorChartData = sensorChartData;
      this.waterLevelInspectionChartData = waterLevelInspectionChartData;
      this.buildWaterLevelChart();

      this.cdr.detectChanges();
    });
  }

  private buildWaterLevelChart() {
    if (this.waterLevelInspectionChartData == null || this.waterLevelInspectionChartData == undefined || 
      this.waterLevelInspectionChartData.WaterLevelInspections?.length == 0) {
      this.hasWaterLevelInspectionChartData = false;
      return;
    }

    this.hasWaterLevelInspectionChartData = true;
    if (!this.isDisplayingWaterLevelInspectionChart) return;

    const waterLevelInspectionChartVegaSpec = JSON.parse(this.waterLevelInspectionChartData.ChartSpec);
    vegaEmbed(`#${this.waterLevelInspectionChartID}`, waterLevelInspectionChartVegaSpec, {
      actions: false, tooltip: true, renderer: "svg"
    }).catch(console.error);
  }

  public isAuthenticated(): boolean {
    return this.authenticationService.isAuthenticated();
  }

  public updateChart(selectedTab: string) {
    if (selectedTab.split('-')[2] == "1") {
      // water level inspection chart tab
      this.isDisplayingWaterLevelInspectionChart = true;
      this.buildWaterLevelChart();

    } else {
      // sensor chart tab
      this.isDisplayingWaterLevelInspectionChart = false;

      const tempSensorChartData = Object.assign({}, this.sensorChartData);
      this.sensorChartData = null;
      this.sensorChartData = tempSensorChartData;
    }
  }

  async exportWaterLevelInspectionChartData() {
    var dataForDownload = this.waterLevelInspectionChartData.WaterLevelInspections?.map(x => new Object({
      Date: moment(x.InspectionDate).format('M/D/yyyy'),
      Well: x.WellRegistrationID,
      Measurement: x.Measurement
    }));

    var fields = ["Date", "Well", "Measurement"];

    const opts = { fields };

    const asyncParser = new AsyncParser(opts);

    let csv: string = await new Promise((resolve,reject)=>{
      let csv = '';
      asyncParser.processor
        .on('data', chunk => (csv += chunk.toString()))
        .on('end', () => {
          resolve(csv);
        })
        .on('error', err => {
          console.error(err);
          reject(err);
        });
      
      asyncParser.input.push(JSON.stringify(dataForDownload));
      asyncParser.input.push(null);
    });

    var exportedFilename = `water-level-inspections_${this.selectedWellGroup?.PrimaryWell.WellRegistrationID}.csv`;

    var blob = new Blob([csv], { type: 'text/csv;charset=utf-8;' });
    var link = document.createElement("a");

    var url = URL.createObjectURL(blob);
    link.setAttribute("href", url);
    link.setAttribute("download", exportedFilename);
    link.style.visibility = 'hidden';
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);

    return dataForDownload;
  }

  // async exportChartData(){
  //   const pivoted = new Map();
  //   for (const point of this.timeSeries){
  //     let pivotRow = pivoted.get(point.MeasurementDate);
  //     if (pivotRow){
  //       pivotRow[point.DataSourceName] = point.MeasurementValue;
  //     } else{
  //       pivotRow = {"Date": point.MeasurementDate};
  //       pivotRow[point.DataSourceName] = point.MeasurementValue;
  //       pivoted.set(point.MeasurementDate, pivotRow);
  //     }
  //   }

  //   const dataSources = [...new Set(this.timeSeries.map(item => item.DataSourceName))];
  //   const fields = ['Date', ...dataSources];

  //   const pivotedAndSorted = Array.from(pivoted.values())
  //     .map(x => {
  //       let csvRow = {
  //         "Date": moment(x.Date).format('M/D/yyyy')
  //       };
  //       dataSources.forEach(element => {
  //         csvRow[element] = x[element]
  //       });
  //       return csvRow;
  //     })
  //     .sort((a,b) => new Date(a.Date).getTime() - new Date(b.Date).getTime());


  //   const opts = { fields };

  //   const asyncParser = new AsyncParser(opts);

  //   let csv: string = await new Promise((resolve,reject)=>{
  //     let csv = '';
  //     asyncParser.processor
  //       .on('data', chunk => (csv += chunk.toString()))
  //       .on('end', () => {
  //         resolve(csv);
  //       })
  //       .on('error', err => {
  //         console.error(err);
  //         reject(err);
  //       });
      
  //     asyncParser.input.push(JSON.stringify(pivotedAndSorted));
  //     asyncParser.input.push(null);
  //   });

  //   var exportedFilename = `sensorChartData.csv`;

  //   var blob = new Blob([csv], { type: 'text/csv;charset=utf-8;' });
  //   var link = document.createElement("a");

  //   var url = URL.createObjectURL(blob);
  //   link.setAttribute("href", url);
  //   link.setAttribute("download", exportedFilename);
  //   link.style.visibility = 'hidden';
  //   document.body.appendChild(link);
  //   link.click();
  //   document.body.removeChild(link);

  //   return pivotedAndSorted;
  // }
}
