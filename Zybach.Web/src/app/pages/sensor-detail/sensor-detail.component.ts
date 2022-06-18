import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { default as vegaEmbed } from 'vega-embed';
import * as vega from 'vega';
import { AsyncParser } from 'json2csv';
import moment from 'moment';
import { forkJoin } from 'rxjs';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { SensorStatusService } from 'src/app/shared/generated/api/sensor-status.service';
import { SensorService } from 'src/app/shared/generated/api/sensor.service';
import { UtilityFunctionsService } from 'src/app/services/utility-functions.service';
import { WellService } from 'src/app/shared/generated/api/well.service';
import { InstallationRecordDto } from 'src/app/shared/generated/model/installation-record-dto';
import { SensorSimpleDto } from 'src/app/shared/generated/model/sensor-simple-dto';
import { SensorSummaryDto } from 'src/app/shared/generated/model/sensor-summary-dto';
import { UserDto } from 'src/app/shared/generated/model/user-dto';
import { AlertService } from 'src/app/shared/services/alert.service';
import { environment } from 'src/environments/environment';
import { IAngularMyDpOptions } from 'angular-mydatepicker';
import { SensorTypeEnum } from 'src/app/shared/generated/enum/sensor-type-enum';
import { SensorAnomalyService } from 'src/app/shared/generated/api/sensor-anomaly.service';
import { Alert } from 'src/app/shared/models/alert';
import { AlertContext } from 'src/app/shared/models/enums/alert-context.enum';
import { NgbDateAdapter } from '@ng-bootstrap/ng-bootstrap';
import { NgbDateAdapterFromString } from 'src/app/shared/components/ngb-date-adapter-from-string';
import { SensorAnomalyUpsertDto } from 'src/app/shared/generated/model/sensor-anomaly-upsert-dto';

@Component({
  selector: 'zybach-sensor-detail',
  templateUrl: './sensor-detail.component.html',
  styleUrls: ['./sensor-detail.component.scss'],
  providers: [{provide: NgbDateAdapter, useClass: NgbDateAdapterFromString}]
})
export class SensorDetailComponent implements OnInit {
  public sensorID: number;
  public wellID: number;
  
  public currentUser: UserDto;
  public sensor: SensorSimpleDto;

  public installations: InstallationRecordDto[] = [];
  public installationPhotos: any[]; 

  public isLoadingSubmit: boolean = false;
  public isDisplayingSensorAnomalyPanel = false;
  public sensorAnomalyModel: SensorAnomalyUpsertDto;

  public chartID: string = "sensorChart";
  chartColor: string;
  chartSubscription: any;
  timeSeries: any[];
  vegaView: any;
  rangeMax: number;
  tooltipFields: any;
  noTimeSeriesData: boolean = false;
  myDpOptions: IAngularMyDpOptions = {
    dateRange: true,
    dateFormat: 'mm/dd/yyyy'
    // other options are here...
  };
  // For example initialize to specific date (09.10.2018 - 19.10.2018). It is also possible
  // to set initial date range value using the selDateRange attribute.
  dateRange: any;
  public unitsShown: string = "gal";

  constructor(
    private authenticationService: AuthenticationService,
    private sensorService: SensorService,
    private sensorStatusService: SensorStatusService,
    private wellService: WellService,
    private route: ActivatedRoute,
    private router: Router,
    private cdr: ChangeDetectorRef,
    private utilityFunctionsService: UtilityFunctionsService,
    private alertService: AlertService,
    private sensorAnomalyService: SensorAnomalyService
  ) { }

  ngOnInit(): void {
    this.sensorID = parseInt(this.route.snapshot.paramMap.get("id"));
    this.authenticationService.getCurrentUser().subscribe(currentUser => {
      this.currentUser = currentUser;

      this.getSensorDetails();
    });
  }
  
  private getSensorDetails() {
    forkJoin({
      sensor: this.sensorService.sensorsSensorIDGet(this.sensorID), 
      chartSpec: this.sensorService.sensorsSensorIDChartSpecGet(this.sensorID)
    })
    .subscribe(({sensor, chartSpec}) => {
      this.sensor = sensor;
      // convert to hours
      this.sensor.MessageAge = Math.floor(this.sensor.MessageAge / 3600);

      this.wellID = this.sensor.WellID;
      if(this.sensor.SensorTypeID === SensorTypeEnum.PumpMonitor){
        this.chartColor = "#4AAA42";
      }
      else{
        this.chartColor = "#13B5EA";
      }

      this.getInstallationDetails();
      this.initDateRange(new Date(this.sensor.FirstReadingDate), new Date(this.sensor.LastReadingDate));
      this.getChartDataAndBuildChart(chartSpec);
    });
  }

  private getInstallationDetails() {
    this.wellService.wellsWellIDInstallationGet(this.wellID).subscribe(installations => {
      this.installations = installations.filter(x => x.SensorSerialNumber == this.sensor.SensorName);
      for (const installation of installations) {
        this.installationPhotos = this.getPhotoRecords(installation);
      }
    });
  }

  private getPhotoRecords(installation: InstallationRecordDto) : any[]{
    const installationPhotoDataUrls = [];
    const photos = installation.Photos;

    const photoObservables = photos.map(
      photo => this.wellService.wellsWellIDInstallationInstallationCanonicalNamePhotoPhotoCanonicalNameGet(this.wellID, installation.InstallationCanonicalName, photo)
    );

    let foundPhoto = false;

    forkJoin(photoObservables).subscribe((blobs: any[]) => {
      for (const blob of blobs){
        if (!blob){
          // we're ignoring errors that come from the GO request by sending 204 in their place, 
          // so skip through this iteration if the current blob is null/undefined
          continue; 
        }

        foundPhoto = true;

        const reader = new FileReader();
        reader.readAsDataURL(blob);
        reader.onloadend = () => {
          // result includes identifier 'data:image/png;base64,' plus the base64 data
          installationPhotoDataUrls.push({path: reader.result});
        };
      }
    });

    return installationPhotoDataUrls;
  }

  public getInstallationDate(installation: InstallationRecordDto) {
    if (!installation.Date) {
      return ""
    }
    const time = moment(installation.Date)
    const timepiece = time.format('h:mm a');
    return time.format('M/D/yyyy ') + timepiece;
  }
  
  public toggleIsActive(isActive : boolean): void {
    this.isLoadingSubmit = true;
    var sensorSummaryDto = new SensorSummaryDto();
    sensorSummaryDto.SensorName = this.sensor.SensorName;
    sensorSummaryDto.SensorID = this.sensor.SensorID;
    sensorSummaryDto.IsActive = isActive
    this.sensorStatusService.sensorStatusEnableDisablePut(sensorSummaryDto)
      .subscribe(response => {
        this.isLoadingSubmit = false;
        this.sensor.IsActive = isActive;
        // this.alertService.pushAlert(new Alert(`Sensor '${sensorName}' now ${isActive ? "enabled" : "disabled"}`, AlertContext.Success));
      }
        ,
        error => {
          this.isLoadingSubmit = false;
          this.cdr.detectChanges();
        }
      );
  }

  public wellInGeoOptixUrl(): string {
    return `${environment.geoOptixWebUrl}/program/main/(inner:site)?projectCName=water-data-program&siteCName=${this.sensor.WellRegistrationID}`;
  }

  public displaySensorAnomalyPanel() {
    this.isDisplayingSensorAnomalyPanel = true;
    
    if (this.sensorAnomalyModel) {
      this.sensorAnomalyModel = null;
    }

    this.sensorAnomalyModel = new SensorAnomalyUpsertDto();
    this.sensorAnomalyModel.SensorID = this.sensorID;

    this.resizeWindow();
  }

  public closeSensorAnomalyPanel() {
    this.isDisplayingSensorAnomalyPanel = false;
    this.resizeWindow();
  }

  public submitSensorAnomaly() {
    this.isLoadingSubmit = true;
    
    this.sensorAnomalyService.sensorAnomaliesNewPost(this.sensorAnomalyModel).subscribe(() => {
      this.isLoadingSubmit = false;
      this.sensorAnomalyModel = new SensorAnomalyUpsertDto();
      this.sensorAnomalyModel.SensorID = this.sensorID;
      
      this.alertService.pushAlert(new Alert('Sensor anomaly report successfully created.', AlertContext.Success));
      window.scroll(0, 0);

      this.getSensorDetails();
    }, error => {
      this.isLoadingSubmit = false;
      window.scroll(0, 0);
    });
  }

  public resizeWindow() {
    this.cdr.detectChanges();
    window.dispatchEvent(new Event('resize'));
  }

  // Begin section: chart

  getChartDataAndBuildChart(chartSpec: any) {
      if (this.sensor.SensorMeasurements.length === 0) {
        this.noTimeSeriesData = true;
        this.timeSeries = [];
        return;
      }

      this.timeSeries = this.sensor.SensorMeasurements;      
      this.cdr.detectChanges();
      this.setRangeMax(this.timeSeries);
      this.tooltipFields = [{ "field": this.sensor.SensorTypeName, "type": "ordinal" }];
      this.buildChart(chartSpec);
  }

  async exportChartData(){
    const pivoted = new Map();
    for (const point of this.timeSeries){
      let pivotRow = pivoted.get(point.MeasurementDate);
      if (pivotRow){
        pivotRow["Reading"] = point.MeasurementValue;
      } else{
        pivotRow = {"Date": point.MeasurementDate};
        pivotRow["Reading"] = point.MeasurementValue;
        pivoted.set(point.MeasurementDate, pivotRow);
      }
    }

    const pivotedAndSorted = Array.from(pivoted.values())
      .map(x=> ({
        "Date": moment(x.Date).format('M/D/yyyy'),
        "Reading": x["Reading"]
      }))
      .sort((a,b) => new Date(a.Date).getTime() - new Date(b.Date).getTime());

    const fields = ['Date', 'Reading'];

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

    var exportedFilename = `${this.sensor.SensorName}_DataReadings.csv`;
    

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

  buildChart(chartSpec: any) {
    var self = this;
    console.log(chartSpec);
    vegaEmbed(`#${this.chartID}`, chartSpec, {
      actions: false, tooltip: true, renderer: "svg"
    }).then(function (res) {
      self.vegaView = res.view;

      self.filterChart(new Date(2021, 0, 1), new Date());
    });
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

  onDateChanged(event){
    const startDate = event.dateRange.beginJsDate;
    const endDate = event.dateRange.endJsDate;

    this.filterChart(startDate, endDate);
  }

  useFullDateRange(){
    const startDate = new Date(this.sensor.FirstReadingDate) 
    const endDate = new Date(this.sensor.LastReadingDate)

    this.initDateRange(startDate, endDate);

    this.filterChart(startDate, endDate);
  }

  filterChart(startDate: Date, endDate: Date){
    const filteredTimeSeries = this.timeSeries.filter(x=>{
      const asDate =  new Date(x.MeasurementDate);
      return asDate.getTime() >= startDate.getTime() && asDate.getTime() <= endDate.getTime();
    });

    this.setRangeMax(filteredTimeSeries);
    console.log(filteredTimeSeries);
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
  // End section: chart
}
