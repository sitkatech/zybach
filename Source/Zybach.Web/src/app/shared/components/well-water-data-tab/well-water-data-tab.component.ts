import { DecimalPipe } from '@angular/common';
import { ChangeDetectorRef, Component, Input, OnInit, ViewChild } from '@angular/core';
import { AngularMyDatePickerDirective, IAngularMyDpOptions } from 'angular-mydatepicker';
import { AsyncParser } from 'json2csv';
import moment from 'moment';
import { WellService } from 'src/app/services/well.service';
import { default as vegaEmbed } from 'vega-embed';
import * as vega from 'vega';
import { WellDetailDto } from '../../generated/model/well-detail-dto';
import { SensorStatusService } from 'src/app/services/sensor-status.service';
import { SensorMessageAgeDto } from '../../generated/model/sensor-message-age-dto';

@Component({
  selector: 'zybach-well-water-data-tab',
  templateUrl: './well-water-data-tab.component.html',
  styleUrls: ['./well-water-data-tab.component.scss']
})
export class WellWaterDataTabComponent implements OnInit {
  @Input() well: WellDetailDto;

  @ViewChild('dp') mydp: AngularMyDatePickerDirective;

  sensorsWithStatus: SensorMessageAgeDto[];
  public years: number[];
  myDpOptions: IAngularMyDpOptions = {
    dateRange: true,
    dateFormat: 'mm/dd/yyyy'
    // other options are here...
  };
  public chartID: string = "wellChart";
  public dateRange: any;
  public unitsShown: string = "gal";
  legendNames: any[];
  legendColors: any[];
  tooltipFields: any;
  noTimeSeriesData: boolean = false;
  rawResults: string;
  timeSeries: any[];
  vegaView: any;
  rangeMax: number;
  chartSubscription: any;
  sensors: any;

  public isLoadingSubmit: boolean = false;

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
    for (var year = currentYear; year >= 2019; year--){
      this.years.push(year);
    }

    const startDate = new Date(currentYear, 0, 1)
    this.initDateRange(startDate, currentDate);
    this.getSensorsWithAgeMessages();
    this.getChartDataAndBuildChart();
  }
  
  initDateRange(startDate: Date, endDate: Date) {
    this.dateRange = {
      isRange: true, 
      singleDate: null, 
      dateRange: {
        beginDate: {
          year: startDate.getFullYear(), month: startDate.getMonth() + 1, day: startDate.getDate()
        },
        endDate: {
          year: endDate.getFullYear(), month: endDate.getMonth() + 1, day: endDate.getDate()
        }
      }
    };
  }

  getSensorsWithAgeMessages(){
    this.sensorService.getSensorStatusForWell(this.well.WellID).subscribe(wellWithSensorMessageAge => {
      this.sensorsWithStatus = wellWithSensorMessageAge.Sensors;

      for (var sensor of this.sensorsWithStatus){
        sensor.MessageAge = Math.floor(sensor.MessageAge / 3600)
      }
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

  // Begin section: chart

  getChartDataAndBuildChart() {

    this.chartSubscription = this.wellService.getChartData(this.well.WellID).subscribe(response => {
      if (!response.TimeSeries || response.TimeSeries.length == 0) {
        this.noTimeSeriesData = true;
        this.timeSeries = [];
        return;
      }

      this.sensors = response.Sensors;
      this.timeSeries = response.TimeSeries;
      
      this.cdr.detectChanges();

      this.setRangeMax(this.timeSeries);

      this.tooltipFields = response.Sensors.map(x => ({ "field": x.SensorType, "type": "ordinal" }));
      const sensorTypes = response.Sensors.map(x=>x.SensorType);

      this.legendNames = [];
      this.legendColors = [];

      if( sensorTypes.includes("Flow Meter")){
        this.legendNames.push("Flow Meter");
        this.legendColors.push("#13B5EA");
      }

      if( sensorTypes.includes("Continuity Meter")){
        this.legendNames.push("Continuity Meter");
        this.legendColors.push("#4AAA42");
      }

      if( sensorTypes.includes("Electrical Usage")){
        this.legendNames.push("Electrical Usage");
        this.legendColors.push("#0076C0");
      }

      this.buildChart();
    });
  }

  async exportChartData(){
    const pivoted = new Map();
    for (const point of this.timeSeries){
      let pivotRow = pivoted.get(point.Time);
      if (pivotRow){
        pivotRow[point.DataSource] = point.Gallons;
      } else{
        pivotRow = {"Date": point.Time};
        pivotRow[point.DataSource] = point.Gallons;
        pivoted.set(point.Time, pivotRow);
      }
    }

    const pivotedAndSorted = Array.from(pivoted.values())
      .map(x=> ({
        "Date": moment(x.Date).format('M/D/yyyy'),
        "FlowmeterGallons": x["Flow Meter"],
        "ElectricalUsageGallons": x["Electrical Usage"],
        "ContinuityDeviceGallons": x["Continuity Meter"]
      }))
      .sort((a,b) => new Date(a.Date).getTime() - new Date(b.Date).getTime());

    const fields = ['Date'];

    const sensorTypes = this.well.Sensors.map(x=>x.SensorType);

    if (sensorTypes.includes("Flow Meter")){
      fields.push('FlowmeterGallons');
    }
    
    if (this.well.HasElectricalData){
      fields.push('ElectricalUsageGallons');
    }
    
    if (sensorTypes.includes("Continuity Meter")){
      fields.push('ContinuityDeviceGallons');
    }

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
      
      asyncParser.input.push(JSON.stringify(pivotedAndSorted));
      asyncParser.input.push(null);
    });

    var exportedFilename = `${this.well.WellRegistrationID}_WaterUsage.csv`;

    var blob = new Blob([csv], { type: 'text/csv;charset=utf-8;' });
    var link = document.createElement("a");

    var url = URL.createObjectURL(blob);
    link.setAttribute("href", url);
    link.setAttribute("download", exportedFilename);
    link.style.visibility = 'hidden';
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);

    return pivotedAndSorted;
  }

  buildChart() {
    var self = this;
    vegaEmbed(`#${this.chartID}`, this.getVegaSpec(), {
      actions: false, tooltip: true, renderer: "svg"
    }).then(function (res) {
      self.vegaView = res.view;

      setTimeout(() => self.filterChart(new Date(2021, 0, 1), new Date()), 200);
    });
  }

  onDateChanged(event){
    const startDate = event.dateRange.beginJsDate;
    const endDate = event.dateRange.endJsDate;

    this.filterChart(startDate, endDate);
  }

  useFullDateRange(){
    const startDate = new Date(this.well.FirstReadingDate) 
    const endDate = new Date(this.well.LastReadingDate)

    this.initDateRange(startDate, endDate);

    this.filterChart(startDate, endDate);
  }

  filterChart(startDate: Date, endDate: Date){
    const filteredTimeSeries = this.timeSeries.filter(x=>{
      const asDate =  new Date(x.Time)
      return asDate.getTime() >= startDate.getTime() && asDate.getTime() <= endDate.getTime()
    });

    this.setRangeMax(filteredTimeSeries);

    var changeSet = vega.changeset().remove(x => true).insert(filteredTimeSeries);
    this.vegaView.change('TimeSeries', changeSet).runAsync();
    window.dispatchEvent(new Event('resize'));
  }

  setRangeMax(timeSeries: any){
    if(timeSeries && timeSeries.length > 0)
    {
      const gallonsMax = timeSeries.sort((a, b) => b.Gallons - a.Gallons)[0].Gallons;
      if (gallonsMax !== 0) {
        this.rangeMax = gallonsMax * 1.05;
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
          "field": "Time",
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
              "field": "Gallons",
              "type": "quantitative",
              "axis": {
                "title": "Gallons"
              }
              // ,
              // "scale": {
              //   "domain": [0, this.rangeMax]
              // }
            },
            "color": {
              "field": "DataSource",
              "type": "nominal",
              "axis": {
                "title": "Data Source"
              },
              "scale": {
                // "domain": ["Flow Meter", "Continuity Meter", "Electrical Usage"],
                // "range": ["#13B5EA", "#4AAA42", "#0076C0"],
                "domain": this.legendNames,
                "range": this.legendColors
              }
            }
          },
          "layer": [
            { "mark": "line" },
            { "transform": [{ "filter": { "selection": "hover" } }], "mark": "point" }
          ]
        },
        {
          "transform": [{ "pivot": "DataSource", "value": "GallonsString", "groupby": ["Time"], "op": "max" }],
          "mark": "rule",
          "encoding": {
            "opacity": {
              "condition": { "value": 0.3, "selection": "hover" },
              "value": 0
            },
            "tooltip": [
              { "field": "Time", "type": "temporal", "title": "Date" },
              ...this.tooltipFields
            ]
          },
          "selection": {
            "hover": {
              "type": "single",
              "fields": ["Time"],
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

}
