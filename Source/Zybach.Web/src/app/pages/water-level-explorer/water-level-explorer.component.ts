import { DatePipe } from '@angular/common';
import { Component, OnInit, ViewChild } from '@angular/core';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { SensorService } from 'src/app/services/sensor.service';
import { WellService } from 'src/app/services/well.service';
import { SensorSimpleDto } from 'src/app/shared/generated/model/sensor-simple-dto';
import { UserDto } from 'src/app/shared/generated/model/user-dto';
import { WellWaterLevelMapSummaryDto } from 'src/app/shared/generated/model/well-water-level-map-summary-dto';
import { CustomRichTextType } from 'src/app/shared/models/enums/custom-rich-text-type.enum';
import { WellWaterLevelMapComponent } from '../well-water-level-map/well-water-level-map.component';

@Component({
  selector: 'zybach-water-level-explorer',
  templateUrl: './water-level-explorer.component.html',
  styleUrls: ['./water-level-explorer.component.scss']
})
export class WaterLevelExplorerComponent implements OnInit {
  @ViewChild("wellMap") wellMap: WellWaterLevelMapComponent;

  public currentUser: UserDto;
  public wells: WellWaterLevelMapSummaryDto[];
  public selectedWell: WellWaterLevelMapSummaryDto;
  public sensors: SensorSimpleDto[];
  public wellsGeoJson: any;
  
  public richTextTypeID : number = CustomRichTextType.WaterLevelExplorerMap;
  public disclaimerRichTextTypeID : number = CustomRichTextType.WaterLevelExplorerMapDisclaimer;

  public isDisplayingWaterLevelPanel : boolean = true;
  
  constructor(
    private authenticationService: AuthenticationService,
    private datePipe: DatePipe,
    private sensorService: SensorService,
    private wellService: WellService
  ) { }

  ngOnInit(): void {
    this.authenticationService.getCurrentUser().subscribe(currentUser => {
      this.currentUser = currentUser;
      this.wellService.getWellsWithPressureSensorMapData().subscribe(wells => {
        this.wells = wells;
        this.wellsGeoJson =
        {
          type: "FeatureCollection",
          features:
            wells.filter(x => x.Location != null && x.Location != undefined).map(x => {
              const geoJsonPoint = x.Location;
              geoJsonPoint.properties = {
                wellID: x.WellID,
                wellRegistrationID: x.WellRegistrationID,
                sensors: x.Sensors || []
              };
              return geoJsonPoint;
            })
        }
      });
    });
  }

  public onMapSelection(wellID: number) {
    this.selectedWell = this.wells.find(x => x.WellID === wellID);
    this.sensorService.listSensorsByWellID(wellID).subscribe(sensors => {
      this.sensors = sensors;
    });
    //this.getChartDataAndBuildChart();
  }

  
  // Begin section: chart

//   getChartDataAndBuildChart() {
//     if (this.sensor.WellSensorMeasurements.length === 0) {
//       this.noTimeSeriesData = true;
//       this.timeSeries = [];
//       return;
//     }

//     this.timeSeries = this.sensor.WellSensorMeasurements;      
//     this.cdr.detectChanges();
//     this.setRangeMax(this.timeSeries);
//     this.tooltipFields = [{ "field": this.sensor.SensorTypeName, "type": "ordinal" }];
//     this.buildChart();
// }

// async exportChartData(){
//   const pivoted = new Map();
//   for (const point of this.timeSeries){
//     let pivotRow = pivoted.get(point.MeasurementDate);
//     if (pivotRow){
//       pivotRow["Reading"] = point.MeasurementValue;
//     } else{
//       pivotRow = {"Date": point.MeasurementDate};
//       pivotRow["Reading"] = point.MeasurementValue;
//       pivoted.set(point.MeasurementDate, pivotRow);
//     }
//   }

//   const pivotedAndSorted = Array.from(pivoted.values())
//     .map(x=> ({
//       "Date": moment(x.Date).format('M/D/yyyy'),
//       "Reading": x["Reading"]
//     }))
//     .sort((a,b) => new Date(a.Date).getTime() - new Date(b.Date).getTime());

//   const fields = ['Date', 'Reading'];

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

//   var exportedFilename = `${this.sensor.SensorName}_DataReadings.csv`;
  

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

// buildChart() {
//   var self = this;
//   vegaEmbed(`#${this.chartID}`, this.getVegaSpec(), {
//     actions: false, tooltip: true, renderer: "svg"
//   }).then(function (res) {
//     self.vegaView = res.view;

//     self.filterChart(new Date(2021, 0, 1), new Date());
//   });
// }

// initDateRange(startDate: Date, endDate: Date) {
//   this.dateRange = {
//     isRange: true, 
//     singleDate: null, 
//     dateRange: {
//       beginDate: {
//         year: startDate.getFullYear(), month: startDate.getMonth() + 1, day: startDate.getDate()
//       },
//       endDate: {
//         year: endDate.getFullYear(), month: endDate.getMonth() + 1, day: endDate.getDate()
//       }
//     }
//   };
// }

// onDateChanged(event){
//   const startDate = event.dateRange.beginJsDate;
//   const endDate = event.dateRange.endJsDate;

//   this.filterChart(startDate, endDate);
// }

// useFullDateRange(){
//   const startDate = new Date(this.sensor.FirstReadingDate) 
//   const endDate = new Date(this.sensor.LastReadingDate)

//   this.initDateRange(startDate, endDate);

//   this.filterChart(startDate, endDate);
// }

// filterChart(startDate: Date, endDate: Date){
//   const filteredTimeSeries = this.timeSeries.filter(x=>{
//     const asDate =  new Date(x.MeasurementDate);
//     return asDate.getTime() >= startDate.getTime() && asDate.getTime() <= endDate.getTime();
//   });

//   this.setRangeMax(filteredTimeSeries);

//   var changeSet = vega.changeset().remove(x => true).insert(filteredTimeSeries);
//   this.vegaView.change('TimeSeries', changeSet).run();
//   this.resizeWindow();
// }

// setRangeMax(timeSeries: any){
//   if(timeSeries && timeSeries.length > 0)
//   {
//     const measurementValueMax = timeSeries.sort((a, b) => b.MeasurementValue - a.MeasurementValue)[0].MeasurementValue;
//     if (measurementValueMax !== 0) {
//       this.rangeMax = measurementValueMax * 1.05;
//     } else {
//       this.rangeMax = 10000;
//     }
//   }
//   else
//   {
//     this.rangeMax = 10000;
//   }
// }

// getVegaSpec(): any {
//   return {
//     "$schema": "https://vega.github.io/schema/vega-lite/v4.json",
//     "description": "A chart",
//     "width": "container",
//     "height": "container",
//     "data": { "name": "TimeSeries" },
//     "encoding": {
//       "x": {
//         "field": "MeasurementDate",
//         "timeUnit": "yearmonthdate",
//         "type": "temporal",
//         "axis": {
//           "title": "Date"
//         }
//       }
//     },

//     "layer": [
//       {
//         "encoding": {
//           "y": {
//             "field": "MeasurementValue",
//             "type": "quantitative",
//             "axis": {
//               "title": this.sensor.SensorTypeID === SensorTypeEnum.WellPressure ? "Depth to Groundwater" : "Gallons"
//             }
//           },
//           "color": {
//             "field": "MeasurementType.MeasurementTypeDisplayName",
//             "type": "nominal",
//             "axis": {
//               "title": "Data Source"
//             },
//             "scale": {
//               "domain": [this.sensor.SensorTypeName],
//               "range": [this.chartColor],
//             }
//           }
//         },
//         "layer": [
//           { "mark": "line" },
//           { "transform": [{ "filter": { "selection": "hover" } }], "mark": "point" }
//         ]
//       },
//       {
//         "transform": [{ "pivot": "MeasurementType.MeasurementTypeDisplayName", "value": "MeasurementValueString", "groupby": ["MeasurementDate"], "op": "max" }],
//         "mark": "rule",
//         "encoding": {
//           "opacity": {
//             "condition": { "value": 0.3, "selection": "hover" },
//             "value": 0
//           },
//           "tooltip": [
//             { "field": "MeasurementDate", "type": "temporal", "title": "Date" },
//             ...this.tooltipFields
//           ]
//         },
//         "selection": {
//           "hover": {
//             "type": "single",
//             "fields": ["MeasurementDate"],
//             "nearest": true,
//             "on": "mouseover",
//             "empty": "none",
//             "clear": "mouseout"
//           }
//         }
//       }
//     ]
//   }
// }
// End section: chart

}
