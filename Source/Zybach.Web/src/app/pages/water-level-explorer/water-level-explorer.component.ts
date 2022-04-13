import { DatePipe } from '@angular/common';
import { ChangeDetectorRef, Component, OnInit, ViewChild } from '@angular/core';
import { IAngularMyDpOptions } from 'angular-mydatepicker';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { SensorService } from 'src/app/services/sensor.service';
import { WellService } from 'src/app/services/well.service';
import { SensorSimpleDto } from 'src/app/shared/generated/model/sensor-simple-dto';
import { UserDto } from 'src/app/shared/generated/model/user-dto';
import { WellWaterLevelMapSummaryDto } from 'src/app/shared/generated/model/well-water-level-map-summary-dto';
import { CustomRichTextType } from 'src/app/shared/models/enums/custom-rich-text-type.enum';
import { WellWaterLevelMapComponent } from '../well-water-level-map/well-water-level-map.component';
import { default as vegaEmbed } from 'vega-embed';
import * as vega from 'vega';
import { NgbDateAdapter } from '@ng-bootstrap/ng-bootstrap';
import { NgbDateAdapterFromString } from 'src/app/shared/components/ngb-date-adapter-from-string';
@Component({
  selector: 'zybach-water-level-explorer',
  templateUrl: './water-level-explorer.component.html',
  styleUrls: ['./water-level-explorer.component.scss'],
  providers: [{provide: NgbDateAdapter, useClass: NgbDateAdapterFromString}]
})
export class WaterLevelExplorerComponent implements OnInit {
  @ViewChild("wellMap") wellMap: WellWaterLevelMapComponent;

  public currentUser: UserDto;
  public wells: WellWaterLevelMapSummaryDto[];
  public selectedWell: WellWaterLevelMapSummaryDto;
  public selectedSensors: SensorSimpleDto[];
  public wellsGeoJson: any;
  
  public richTextTypeID : number = CustomRichTextType.WaterLevelExplorerMap;
  public disclaimerRichTextTypeID : number = CustomRichTextType.WaterLevelExplorerMapDisclaimer;

  public chartID: string = "sensorChart";
  chartColor: string = "#13B5EA";
  chartSubscription: any;
  timeSeries: any[];
  vegaView: any;
  rangeMax: number;
  tooltipFields: any;
  noTimeSeriesData: boolean = false;
  // myDpOptions: IAngularMyDpOptions = {
  //   dateRange: true,
  //   dateFormat: 'mm/dd/yyyy'
  // };
  // dateRange: any;
  startDate: any;
  endDate: any;
  public unitsShown: string = "gal";
  
  constructor(
    private authenticationService: AuthenticationService,
    private cdr: ChangeDetectorRef,
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
      this.selectedSensors = sensors;
      this.startDate = this.selectedSensors[0].FirstReadingDate;
      this.endDate = this.selectedWell.LastReadingDate ?? Date.now();
      //this.cdr.detectChanges();
      this.getChartDataAndBuildChart();
    });
  }

  // Begin section: chart

  getChartDataAndBuildChart() {
    this.selectedSensors.forEach(sensor => {
      if (sensor.WellSensorMeasurements.length === 0) {
        this.noTimeSeriesData = true;
        this.timeSeries = [];
        return;
      }
      else {
        this.timeSeries = sensor.WellSensorMeasurements;      

        this.cdr.detectChanges();
        this.setRangeMax(this.timeSeries);
        this.tooltipFields = [{ "field": sensor.SensorTypeName, "type": "ordinal" }];
        this.buildChart();

      }
    });
  }

  buildChart() {
    var self = this;
    vegaEmbed(`#${this.chartID}`, this.getVegaSpec(), {
      actions: false, tooltip: true, renderer: "svg"
    }).then(function (res) {
      self.vegaView = res.view;

      self.filterChart(new Date(2021, 0, 1), new Date());
    });
  }

  onStartDateChanged(event){
    const startDate = new Date(event);
    const endDate = new Date(this.endDate);
    this.filterChart(startDate, endDate);
  }

  onEndDateChanged(event){
    const endDate = new Date(event);
    const startDate = new Date(this.startDate);
    this.filterChart(startDate, endDate);
  }

  filterChart(startDate: Date, endDate: Date){
    const filteredTimeSeries = this.timeSeries.filter(x=>{
      const asDate = new Date(x.MeasurementDate);
      return asDate.getTime() >= startDate.getTime() && asDate.getTime() <= endDate.getTime();
    });

    this.setRangeMax(filteredTimeSeries);

    var changeSet = vega.changeset().remove(x => true).insert(filteredTimeSeries);
    this.vegaView.change('TimeSeries', changeSet).run();
    this.resizeWindow();
  }

  setRangeMax(timeSeries: any){
    if(timeSeries && timeSeries.length > 0)
    {
      const measurementValueMax = timeSeries.sort((a, b) => b.MeasurementValue - a.MeasurementValue)[0].MeasurementValue;
      if (measurementValueMax !== 0) {
        this.rangeMax = measurementValueMax * 1.05;
      } else {
        this.rangeMax = 10000;
      }
    }
    else
    {
      this.rangeMax = 10000;
    }
  }

  getVegaSpec(): any {
    return {
      "$schema": "https://vega.github.io/schema/vega-lite/v5.1.json",
      "description": "A chart",
      "width": "container",
      "height": "container",
      "data": { "name": "TimeSeries" },
      "encoding": {
        "x": {
          "field": "MeasurementDate",
          "timeUnit": "yearmonthdate",
          "type": "temporal",
          "axis": {
            "title": "Date"
          }
        }
      },

      "layer": [
        {
          "encoding": {
            "y": {
              "field": "MeasurementValue",
              "type": "quantitative",
              "axis": {
                "title": "Depth to Groundwater (ft)"
              }
            },
            "color": {
              "field": "MeasurementType.MeasurementTypeDisplayName",
              "type": "nominal",
              "axis": {
                "title": "Data Source"
              },
              "scale": {
                "domain": ["Well Pressure"],
                "range": [this.chartColor],
              }
            }
          },
          "layer": [
            { "mark": "line" },
            { "transform": [{ "filter": { "selection": "hover" } }], "mark": "point" }
          ]
        },
        {
          "transform": [{ "pivot": "MeasurementType.MeasurementTypeDisplayName", "value": "MeasurementValueString", "groupby": ["MeasurementDate"], "op": "max" }],
          "mark": "rule",
          "encoding": {
            "opacity": {
              "condition": { "value": 0.3, "selection": "hover" },
              "value": 0
            },
            "tooltip": [
              { "field": "MeasurementDate", "type": "temporal", "title": "Date" },
              ...this.tooltipFields
            ]
          },
          "selection": {
            "hover": {
              "type": "single",
              "fields": ["MeasurementDate"],
              "nearest": true,
              "on": "mouseover",
              "empty": "none",
              "clear": "mouseout"
            }
          }
        }
      ]
    }
  }
  // End section: chart

  public resizeWindow() {
    this.cdr.detectChanges();
    window.dispatchEvent(new Event('resize'));
  }

}
