import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import moment from 'moment';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { WellService } from 'src/app/services/well.service';
import { default as vegaEmbed } from 'vega-embed';
import * as vega from 'vega';
import { AsyncParser } from 'json2csv';

import {
  GeoJSON,
  marker,
  map,
  Map as LeafletMap,
  MapOptions,
  tileLayer,
  icon} from 'leaflet';
import 'leaflet.icon.glyph';
import 'leaflet.fullscreen';
import {GestureHandling} from 'leaflet-gesture-handling';
import { AngularMyDatePickerDirective, IAngularMyDpOptions } from 'angular-mydatepicker';
import { DecimalPipe } from '@angular/common';
import { forkJoin } from 'rxjs';
import { environment } from 'src/environments/environment';
import { AlertService } from 'src/app/shared/services/alert.service';
import { Alert } from 'src/app/shared/models/alert';
import { DefaultBoundingBox } from 'src/app/shared/models/default-bounding-box';
import { SensorStatusService } from 'src/app/services/sensor-status.service';
import { SensorMessageAgeDto } from 'src/app/shared/generated/model/sensor-message-age-dto';
import { SensorSummaryDto } from 'src/app/shared/generated/model/sensor-summary-dto';
import { UserDto } from 'src/app/shared/generated/model/user-dto';
import { WellDetailDto } from 'src/app/shared/generated/model/well-detail-dto';
import { ChemigationPermitAnnualRecordDetailedDto } from 'src/app/shared/generated/model/chemigation-permit-annual-record-detailed-dto';
import { InstallationRecordDto } from 'src/app/shared/generated/model/installation-record-dto';

@Component({
  selector: 'zybach-well-detail',
  templateUrl: './well-detail.component.html',
  styleUrls: ['./well-detail.component.scss']
})
export class WellDetailComponent implements OnInit, OnDestroy, AfterViewInit {
  @ViewChild('dp') mydp: AngularMyDatePickerDirective;
  
  public chartID: string = "wellChart";


  public watchUserChangeSubscription: any;

  maxZoom = 17;

  currentUser: UserDto;
  chartSubscription: any;
  well: WellDetailDto;
  sensorsWithStatus: SensorMessageAgeDto[];
  installations: InstallationRecordDto[] = [];
  installationPhotos: Map<string, any[]>; 
  rawResults: string;
  timeSeries: any[];
  vegaView: any;
  rangeMax: number;
  wellRegistrationID: string;
  tooltipFields: any;
  noTimeSeriesData: boolean = false;
  sensors: any;
  boundingBox: any;
  tileLayers: any;
  map: LeafletMap;
  mapID = "wellLocation";
  photoDataUrl: string | ArrayBuffer;
  years: number[];
  legendNames: any[];
  legendColors: any[];
  
  myDpOptions: IAngularMyDpOptions = {
    dateRange: true,
    dateFormat: 'mm/dd/yyyy'
    // other options are here...
  };
  // For example initialize to specific date (09.10.2018 - 19.10.2018). It is also possible
  // to set initial date range value using the selDateRange attribute.
  dateRange: any;
  public unitsShown: string = "gal";

  public isInAgHubOrGeoOptix: boolean;

  public isLoadingSubmit: boolean = false;

  public chemigationPermitAnnualRecords: Array<ChemigationPermitAnnualRecordDetailedDto>;

  constructor(
    private wellService: WellService,
    private sensorService: SensorStatusService,
    private authenticationService: AuthenticationService,
    private route: ActivatedRoute,
    private cdr: ChangeDetectorRef,
    private decimalPipe: DecimalPipe,
    private alertService: AlertService
  ) { }

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

  ngOnInit(): void {
    this.years = []
    const currentDate = new Date();
    const currentYear = currentDate.getFullYear();
    for (var year = currentYear; year >= 2019; year--){
      this.years.push(year);
    }

    const startDate = new Date(currentYear, 0, 1)
    this.initDateRange(startDate, currentDate);

    this.initMapConstants();

    this.authenticationService.getCurrentUser().subscribe(currentUser => {
      this.currentUser = currentUser;
      this.wellRegistrationID = this.route.snapshot.paramMap.get("wellRegistrationID");
      this.getWellDetails();
      this.getInstallationDetails();
      this.getSenorsWithAgeMessages();
      this.getChartDataAndBuildChart();
      this.getChemigationPermits();
    })
  }

  ngOnDestroy() {
    
    this.chartSubscription.unsubscribe();
  }

  getWellDetails(){
    this.wellService.getWellDetails(this.wellRegistrationID).subscribe((well: WellDetailDto)=>{
      this.well = well;
      this.isInAgHubOrGeoOptix = this.well.InAgHub || this.well.InGeoOptix;
      this.cdr.detectChanges();
      
      if (well.Location != null && well.Location != undefined) {
        this.addWellToMap();
      }
      else {
        this.alertService.pushAlert(new Alert(`No location was provided for ${well.WellRegistrationID}.`))
      }
    })
  }

  getWellIrrigatedAcresPerYear(){
    return this.well.IrrigatedAcresPerYear.sort((a, b) => a.Year < b.Year ? 1 : a.Year === b.Year ? 0 : -1);
  }

  getInstallationDetails(){
    this.wellService.getInstallationDetails(this.wellRegistrationID).subscribe(installations => {
      this.installations = installations;
      this.installationPhotos = new Map();
      for (const installation of installations) {

        const installationPhotoDataUrls = this.getPhotoRecords(installation);
        this.installationPhotos.set(installation.InstallationCanonicalName, installationPhotoDataUrls);
      }
    });
  }

  getSenorsWithAgeMessages(){
    this.sensorService.getSensorStatusForWell(this.wellRegistrationID).subscribe(wellWithSensorMessageAge => {
      this.sensorsWithStatus = wellWithSensorMessageAge.Sensors;

      for (var sensor of this.sensorsWithStatus){
        sensor.MessageAge = Math.floor(sensor.MessageAge / 3600)
      }
    });
  }

  getChemigationPermits(){
    this.wellService.getChemigationPermts(this.wellRegistrationID).subscribe(chemigationPermitAnnualRecords => {
      this.chemigationPermitAnnualRecords = chemigationPermitAnnualRecords;
    });
  }

  getDataSourcesLabel() {
    let plural = true;
    let sensorCount = this.getSensorTypes().size;
    if ((sensorCount == 0 && this.well.HasElectricalData) || (sensorCount == 1 && !this.well.HasElectricalData)) {
      plural = false;
    }

    return `Data Source${plural ? "s": ""}: `
  }

  getPhotoRecords(installation: InstallationRecordDto) : any[]{
    const installationPhotoDataUrls = [];
    const photos = installation.Photos;

    const photoObservables = photos.map(
      photo => this.wellService.getPhoto(this.wellRegistrationID, installation.InstallationCanonicalName, photo)
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
  
  wellInGeoOptixUrl(): string {
    return `${environment.geoOptixWebUrl}/program/main/(inner:site)?projectCName=water-data-program&siteCName=${this.wellRegistrationID}`;
  }

  getSensorTypes() {
    return new Set(this.well.Sensors.map(sensor => {return sensor.SensorType}));
  }

  getLastReadingDate() {
    if (!this.well.LastReadingDate) {
      return ""
    }
    const time = moment(this.well.LastReadingDate)
    //const timepiece = time.format('h:mm a');
    return time.format('M/D/yyyy');// + timepiece;
  }

  getInstallationDate(installation: InstallationRecordDto) {
    if (!installation.Date) {
      return ""
    }
    const time = moment(installation.Date)
    const timepiece = time.format('h:mm a');
    return time.format('M/D/yyyy ') + timepiece;
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

  getWellOwner() {
    let contactParts:string[] = [];
    if(this.well.OwnerName && this.well.OwnerName.length > 0)
    {
      contactParts.push(this.well.OwnerName);
    }
    if(this.well.OwnerAddress && this.well.OwnerAddress.length > 0)
    {
      contactParts.push(this.well.OwnerAddress);
    }
    let cityStateZipParts:string[] = [];
    if(this.well.OwnerCity && this.well.OwnerCity.length > 0)
    {
      cityStateZipParts.push(this.well.OwnerCity);
    }
    let stateZipParts:string[] = [];
    if(this.well.OwnerState && this.well.OwnerState.length > 0)
    {
      stateZipParts.push(this.well.OwnerState);
    }
    if(this.well.OwnerZipCode && this.well.OwnerZipCode.length > 0)
    {
      stateZipParts.push(this.well.OwnerZipCode);
    }
    if(stateZipParts.length > 0)
    {
      cityStateZipParts.push(stateZipParts.join(" "));
    }
    if(cityStateZipParts.length > 0)
    {
      contactParts.push(cityStateZipParts.join(", "));
    }
    return contactParts.join("<br>");
  }

  getWellAdditionalContact() {
    let contactParts:string[] = [];
    if(this.well.AdditionalContactName && this.well.AdditionalContactName.length > 0)
    {
      contactParts.push(this.well.AdditionalContactName);
    }
    if(this.well.AdditionalContactAddress && this.well.AdditionalContactAddress.length > 0)
    {
      contactParts.push(this.well.AdditionalContactAddress);
    }
    let cityStateZipParts:string[] = [];
    if(this.well.AdditionalContactCity && this.well.AdditionalContactCity.length > 0)
    {
      cityStateZipParts.push(this.well.AdditionalContactCity);
    }
    let stateZipParts:string[] = [];
    if(this.well.AdditionalContactState && this.well.AdditionalContactState.length > 0)
    {
      stateZipParts.push(this.well.AdditionalContactState);
    }
    if(this.well.AdditionalContactZipCode && this.well.AdditionalContactZipCode.length > 0)
    {
      stateZipParts.push(this.well.AdditionalContactZipCode);
    }
    if(stateZipParts.length > 0)
    {
      cityStateZipParts.push(stateZipParts.join(" "));
    }
    if(cityStateZipParts.length > 0)
    {
      contactParts.push(cityStateZipParts.join(", "));
    }
    return contactParts.join("<br>");
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

  public toggleIsActive(sensorName : string, isActive : boolean): void{
    var sensor = this.sensorsWithStatus.find(x => x.SensorName === sensorName);
    if(sensor) {
      this.isLoadingSubmit = true;
      var sensorSummaryDto = new SensorSummaryDto();
      sensorSummaryDto.SensorName = sensorName;
      sensorSummaryDto.IsActive = isActive
      this.sensorService.updateSensorIsActive(sensorSummaryDto)
        .subscribe(response => {
          this.isLoadingSubmit = false;
          sensor.IsActive = isActive;
          // this.alertService.pushAlert(new Alert(`Sensor '${sensorName}' now ${isActive ? "enabled" : "disabled"}`, AlertContext.Success));
        }
          ,
          error => {
            this.isLoadingSubmit = false;
            this.cdr.detectChanges();
          }
        );
    }
  }

  // Begin section: location map

  public ngAfterViewInit(): void {
    LeafletMap.addInitHook("addHandler", "gestureHandling", GestureHandling);
    const mapOptions: MapOptions = {
      maxZoom: this.maxZoom,
      layers: [
        this.tileLayers["Aerial"],
      ],
      gestureHandling: true,
      fullscreenControl: true
    } as MapOptions;
    this.map = map(this.mapID, mapOptions);

    this.map.fitBounds([[this.boundingBox.Bottom, this.boundingBox.Left], [this.boundingBox.Top, this.boundingBox.Right]], null);

  }

  public initMapConstants() {
    this.boundingBox = DefaultBoundingBox;

    this.tileLayers = Object.assign({}, {
      "Aerial": tileLayer('https://services.arcgisonline.com/ArcGIS/rest/services/World_Imagery/MapServer/tile/{z}/{y}/{x}', {
        attribution: 'Aerial',
      }),
      "Street": tileLayer('https://services.arcgisonline.com/ArcGIS/rest/services/World_Street_Map/MapServer/tile/{z}/{y}/{x}', {
        attribution: 'Aerial',
      }),
      "Terrain": tileLayer('https://server.arcgisonline.com/ArcGIS/rest/services/World_Topo_Map/MapServer/tile/{z}/{y}/{x}', {
        attribution: 'Terrain',
      }),
    }, this.tileLayers);
  }

  public addWellToMap() {
    const sensorTypes = this.well.Sensors.map(x => x.SensorType);
    let mapIcon;

    if (sensorTypes.includes("Flow Meter")) {
      mapIcon = icon.glyph({
        prefix: "fas",
        glyph: "tint",
        iconUrl: "/assets/main/flowMeterMarker.png"
      });
    } else if (sensorTypes.includes("Continuity Meter")) {
      mapIcon = icon.glyph({
        prefix: "fas",
        glyph: "tint",
        iconUrl: "/assets/main/continuityMeterMarker.png"
      });
    } else if (sensorTypes.includes("Electrical Usage")) {
      mapIcon = icon.glyph({
        prefix: "fas",
        glyph: "tint",
        iconUrl: "/assets/main/electricalDataMarker.png"
      });
    } else {
      mapIcon = icon.glyph({
        prefix: "fas",
        glyph: "tint",
        iconUrl: "/assets/main/noDataSourceMarker.png"
      });
    }
    const wellLayer = new GeoJSON(this.well.Location, {
      pointToLayer: function (feature, latlng) {
        return marker(latlng, {icon: mapIcon});
      }
    });
    wellLayer.addTo(this.map);
    
    let target = (this.map as any)._getBoundsCenterZoom(wellLayer.getBounds(), null);
    this.map.setView(target.center, 16, null);
  }

  // End section: location map

  // Begin section: chart

  getChartDataAndBuildChart() {

    this.chartSubscription = this.wellService.getChartData(this.wellRegistrationID).subscribe(response => {
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

    var exportedFilename = `${this.wellRegistrationID}_WaterUsage.csv`;
    

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

      self.filterChart(new Date(2021, 0, 1), new Date());
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
    this.vegaView.change('TimeSeries', changeSet).run();
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
      "$schema": "https://vega.github.io/schema/vega-lite/v4.json",
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
