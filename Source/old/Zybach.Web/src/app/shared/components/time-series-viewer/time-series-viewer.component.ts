import { ChangeDetectionStrategy, ChangeDetectorRef, Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { PluginLoaderService } from 'src/app/shared/services';
import { getChartDefinition } from './chart-definition';


@Component({
  selector: 'go-time-series-viewer',
  templateUrl: './time-series-viewer.component.html',
  styleUrls: ['./time-series-viewer.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class TimeSeriesViewerComponent implements OnInit {

  @Input() timeSeriesData: any[];
  @Input() timeSeriesTitle: string;
  @Output() close: EventEmitter<void>;

  isEChartsLoaded: boolean;
  chartData: any;
  chartInstance: any;

  constructor(
    private cdr: ChangeDetectorRef,
    private pluginLoaderService: PluginLoaderService,
  ) {
    this.close = new EventEmitter();
  }

  ngOnInit() {
    this.refresh();
  }

  refresh() {
    this.pluginLoaderService.load('echarts')
    .subscribe((pluginLoaded: boolean) => {
      this.isEChartsLoaded = pluginLoaded;

      this.chartData = this.parseTimeSeriesData(this.timeSeriesData);
      this.cdr.markForCheck();
    });
  }

  onChartInit(chartInstance: any) {
    this.chartInstance = chartInstance;
  }

  private parseTimeSeriesData(timeSeriesData: any[]) {
    let chartData;

    if (timeSeriesData && timeSeriesData.length) {
      chartData = getChartDefinition();
      let timeKey = 'time';
      for (const key of Object.keys(timeSeriesData[0])) {
        if (key === 'Time' || key === 'time') {
          timeKey = key;
          chartData.xAxis.data.push(new Date(timeSeriesData[0][key]));
        } else if (key !== 'Time' && key !== 'time' && key !== 'No') {
          chartData.legend.data.push(key);
          
          // only display series by default if it's the water below etc series.
          // this could be configurated in a more robust implementation, but this gets us where we need to be right now.
          if (key !== 'waterBelowGroundLevel'){
            chartData.legend.selected[key] = false;
          } else{
            chartData.legend.selected[key] = true;
          }

          chartData.series.push({
            name: key,
            type: 'line',
            data: [],
          });
        }
      }

      for (const item of timeSeriesData) {
        for (const key of Object.keys(item)) {
          if (key !== 'Time' && key !== 'time' && key !== 'No') {
            const serie = chartData.series.find((s: any) => {
              return s.name === key;
            });
            serie.data.push({
              value: [new Date(item[timeKey]), item[key]],
            });
          }
        }
      }

    }

    return chartData;
  }

}
