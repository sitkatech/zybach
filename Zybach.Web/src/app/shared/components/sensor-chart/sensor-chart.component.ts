import { Component, OnInit, Input, ChangeDetectorRef } from '@angular/core';
import { SensorMeasurementDto } from '../../generated/model/sensor-measurement-dto';
import { AsyncParser } from 'json2csv';
import moment from 'moment';
import { default as vegaEmbed } from 'vega-embed';
import * as vega from 'vega';
import { NgbDateAdapter } from '@ng-bootstrap/ng-bootstrap';
import { NgbDateAdapterFromString } from 'src/app/shared/components/ngb-date-adapter-from-string';
import { SensorChartDataDto } from '../../generated/model/sensor-chart-data-dto';

@Component({
  selector: 'zybach-sensor-chart',
  templateUrl: './sensor-chart.component.html',
  styleUrls: ['./sensor-chart.component.scss'],
  providers: [{ provide: NgbDateAdapter, useClass: NgbDateAdapterFromString }]
})
export class SensorChartComponent {
  _sensorChartDataDto: SensorChartDataDto;
  timeSeries: Array<SensorMeasurementDto> = new Array<SensorMeasurementDto>();
  chartID: string = "sensorChart";
  private chartSpec: any;
  @Input()
  get sensorChartData(): SensorChartDataDto {
    return this._sensorChartDataDto;
  }

  set sensorChartData(val: SensorChartDataDto) {
    this._sensorChartDataDto = val;
    if(val)
    {
      this.chartSpec = JSON.parse(this._sensorChartDataDto.ChartSpec);
      console.log(this.chartSpec);
      this.timeSeries = this._sensorChartDataDto.SensorMeasurements;
      const currentYear = new Date().getFullYear();
      console.log(this.timeSeries);
  
      this.startDate = new Date(currentYear, 0, 1).toISOString();
      this.endDate = new Date().toISOString();
      this.noTimeSeriesData = true;
      if(this.timeSeries?.length > 0)
      {
        this.noTimeSeriesData = false;
        this.buildChart();
      }
      this.setRangeMax(this.timeSeries);
    }
  }

  vegaView: any;
  rangeMax: number;
  noTimeSeriesData: boolean = false;
  startDate: any;
  endDate: any;

  constructor(
    private cdr: ChangeDetectorRef,
  ) { }

  // Begin section: chart
  buildChart() {
    var self = this;
    vegaEmbed(`#${this.chartID}`, this.chartSpec, {
      actions: false, tooltip: true, renderer: "svg"
    }).then(function (res) {
      self.vegaView = res.view;
      self.filterChart(new Date(self.startDate), new Date(self.endDate));
    });
  }

  private addDays(date: Date, days: number): Date {
    var result = new Date(date);
    result.setDate(result.getDate() + days);
    return result;
  }

  private addMonths(date: Date, months: number): Date {
    var result = new Date(date);
    result.setMonth(result.getMonth() + months);
    return result;
  }

  private addYears(date: Date, years: number): Date {
    var result = new Date(date);
    result.setFullYear(result.getFullYear() + years);
    return result;
  }

  public fullDateRange(): void {
    this.startDate = this.sensorChartData.FirstReadingDate;
    this.endDate = this.sensorChartData.LastReadingDate ?? new Date().toISOString();
    this.filterChart(new Date(this.startDate), new Date(this.endDate));
  }

  public lastThirtyDays(): void {
    this.endDate = new Date().toISOString();
    this.startDate = this.addDays(new Date(this.endDate), -30).toISOString();
    this.filterChart(new Date(this.startDate), new Date(this.endDate));
  }

  public lastSixMonths(): void {
    this.endDate = new Date().toISOString();
    this.startDate = this.addMonths(new Date(this.endDate), -6).toISOString();
    this.filterChart(new Date(this.startDate), new Date(this.endDate));
  }

  public lastOneYear(): void {
    this.endDate = new Date().toISOString();
    this.startDate = this.addYears(new Date(this.endDate), -1).toISOString();
    this.filterChart(new Date(this.startDate), new Date(this.endDate));
  }

  onStartDateChanged(event) {
    const startDate = new Date(event);
    const endDate = new Date(this.endDate);
    this.filterChart(startDate, endDate);
  }

  onEndDateChanged(event) {
    const endDate = new Date(event);
    const startDate = new Date(this.startDate);
    this.filterChart(startDate, endDate);
  }

  filterChart(startDate: Date, endDate: Date) {
    const filteredTimeSeries = this.timeSeries.filter(x => {
      const asDate = new Date(x.MeasurementDate);
      return asDate.getTime() >= startDate.getTime() && asDate.getTime() <= endDate.getTime();
    });

    this.setRangeMax(filteredTimeSeries);

    var changeSet = vega.changeset().remove(() => true).insert(filteredTimeSeries);
    this.vegaView.change('TimeSeries', changeSet).run();
    this.resizeWindow();
  }

  setRangeMax(timeSeries: any) {
    if (timeSeries && timeSeries.length > 0) {
      const measurementValueMax = timeSeries.sort((a, b) => b.MeasurementValue - a.MeasurementValue)[0].MeasurementValue;
      if (measurementValueMax !== 0) {
        this.rangeMax = measurementValueMax * 1.05;
      } else {
        this.rangeMax = 10000;
      }
    }
    else {
      this.rangeMax = 10000;
    }
  }

  public resizeWindow() {
    this.cdr.detectChanges();
    window.dispatchEvent(new Event('resize'));
  }

  async exportChartData(){
    const pivoted = new Map();
    for (const point of this.timeSeries){
      let pivotRow = pivoted.get(point.MeasurementDate);
      if (pivotRow){
        pivotRow[point.DataSourceName] = point.MeasurementValue;
      } else{
        pivotRow = {"Date": point.MeasurementDate};
        pivotRow[point.DataSourceName] = point.MeasurementValue;
        pivoted.set(point.MeasurementDate, pivotRow);
      }
    }

    const dataSources = [...new Set(this.timeSeries.map(item => item.DataSourceName))];
    const fields = ['Date', ...dataSources];

    const pivotedAndSorted = Array.from(pivoted.values())
      .map(x => {
        let csvRow = {
          "Date": moment(x.Date).format('M/D/yyyy')
        };
        dataSources.forEach(element => {
          csvRow[element] = x[element]
        });
        return csvRow;
      })
      .sort((a,b) => new Date(a.Date).getTime() - new Date(b.Date).getTime());


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

    var exportedFilename = `sensorChartData.csv`;

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

}
