import { ChangeDetectorRef, Component, OnInit, ViewChild } from '@angular/core';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { CustomRichTextTypeEnum } from 'src/app/shared/generated/enum/custom-rich-text-type-enum';
import { WellWaterLevelMapComponent } from '../well-water-level-map/well-water-level-map.component';
import { forkJoin } from 'rxjs';
import { SensorChartDataDto } from 'src/app/shared/generated/model/sensor-chart-data-dto';
import { default as vegaEmbed } from 'vega-embed';
import * as vega from 'vega';
import { WellGroupService } from 'src/app/shared/generated/api/well-group.service';
import { WellGroupDto } from 'src/app/shared/generated/model/well-group-dto';
import { WaterLevelInspectionsChartDataDto } from 'src/app/shared/generated/model/water-level-inspections-chart-data-dto';
import { AsyncParser } from 'json2csv';
import moment from 'moment';
import { SensorMeasurementDto } from 'src/app/shared/generated/model/sensor-measurement-dto';
import { NgbDateAdapter } from '@ng-bootstrap/ng-bootstrap';
import { NgbDateAdapterFromString } from 'src/app/shared/components/ngb-date-adapter-from-string';
import { WaterLevelInspectionSummaryDto } from 'src/app/shared/generated/model/water-level-inspection-summary-dto';

@Component({
  selector: 'zybach-water-level-explorer',
  templateUrl: './water-level-explorer.component.html',
  styleUrls: ['./water-level-explorer.component.scss'],
  providers: [{ provide: NgbDateAdapter, useClass: NgbDateAdapterFromString }]
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
  public timeSeries: WaterLevelInspectionSummaryDto[] = new Array<WaterLevelInspectionSummaryDto>();
  public hasWaterLevelInspectionChartData: boolean;
  public waterLevelInspectionChartID = "waterLevelInspectionChart";
  public isDisplayingWaterLevelInspectionChart = false;

  public vegaView: any;
  public startDate: string;
  public endDate: string;
  
  constructor(
    private authenticationService: AuthenticationService,
    private cdr: ChangeDetectorRef,
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
      this.timeSeries = waterLevelInspectionChartData.WaterLevelInspections;
      this.startDate = new Date(waterLevelInspectionChartData.FirstInspectionDate).toISOString();
      this.endDate = new Date().toISOString();

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
      actions: { export: true, source: false, compiled: false, editor: false }, tooltip: true, renderer: "svg"
    })
    .then(res => this.vegaView = res.view)
    .catch(console.error);
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

  onStartDateChanged(event) {
    const startDate = new Date(event);
    const endDate = new Date(this.endDate);
    this.filterChart(startDate, endDate);
  }

  onEndDateChanged(event) {
    const startDate = new Date(this.startDate);
    const endDate = new Date(event);
    this.filterChart(startDate, endDate);
  }

  setFullRange() {
    const startDate = new Date(this.waterLevelInspectionChartData.FirstInspectionDate);
    const endDate = new Date();
    this.filterChart(startDate, endDate);

    this.startDate = startDate.toISOString();
    this.endDate = endDate.toISOString();
 }

  filterChart(startDate: Date, endDate: Date) {
    const filteredTimeSeries = this.timeSeries.filter(x => {
      const inspectionDate = new Date(x.InspectionDate);
      return inspectionDate >= startDate && inspectionDate <= endDate;
    });

    var changeSet = vega.changeset().remove(() => true).insert(filteredTimeSeries);
    this.vegaView.change('TimeSeries', changeSet).run();
    this.resizeWindow();
  }

  public resizeWindow() {
    this.cdr.detectChanges();
    window.dispatchEvent(new Event('resize'));
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
}
