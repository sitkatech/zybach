import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import moment from 'moment';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { WellService } from 'src/app/services/well.service';
import { UserDetailedDto } from 'src/app/shared/models';
import { WellWithSensorSummaryDto } from 'src/app/shared/models/well-with-sensor-summary-dto';
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
import { BoundingBoxDto } from 'src/app/shared/models/bounding-box-dto';
import { InstallationDto } from 'src/app/shared/models/installation-dto';
import { AngularMyDatePickerDirective, IAngularMyDpOptions } from 'angular-mydatepicker';
import { doPerf } from '@microsoft/applicationinsights-web';
import { DecimalPipe } from '@angular/common';
import { forkJoin } from 'rxjs';
import { environment } from 'src/environments/environment';

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

  currentUser: UserDetailedDto;
  chartSubscription: any;
  well: WellWithSensorSummaryDto;
  installations: InstallationDto[] = [];
  rawResults: string;
  timeSeries: any[];
  vegaView: any;
  rangeMax: number;
  wellRegistrationID: string;
  tooltipFields: any;
  noTimeSeriesData: boolean = false; sensors: any;
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

  constructor(
    private wellService: WellService,
    private authenticationService: AuthenticationService,
    private route: ActivatedRoute,
    private cdr: ChangeDetectorRef,
    private decimalPipe: DecimalPipe
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

    this.watchUserChangeSubscription = this.authenticationService.currentUserSetObservable.subscribe(currentUser => {
      this.currentUser = currentUser;
      this.wellRegistrationID = this.route.snapshot.paramMap.get("wellRegistrationID");
      this.getWellDetails();
      this.getInstallationDetails();
      this.getChartDataAndBuildChart();
    })
  }

  ngOnDestroy() {
    this.watchUserChangeSubscription.unsubscribe();
    this.chartSubscription.unsubscribe();
  }

  getWellDetails(){
    this.wellService.getWellDetails(this.wellRegistrationID).subscribe(well=>{
      this.well = well;
      
      this.cdr.detectChanges();
      this.addWellToMap();
    })
  }

  getInstallationDetails(){
    this.wellService.getInstallationDetails(this.wellRegistrationID).subscribe(installations => {
      this.installations = installations;

      for (const installation of installations) {

        this.getPhotoRecords(installation)
      }
    });
  }

  getDataSourcesLabel() {
    let plural = true;
    let sensorCount = this.getSensors().length;
    if ((sensorCount == 0 && this.well.hasElectricalData) || (sensorCount == 1 && !this.well.hasElectricalData)) {
      plural = false;
    }

    return `Data Source${plural ? "s": ""}: `
  }

  getPhotoRecords(installation){
    installation.photoDataUrls = [];
    installation.noPhotoAvailable = false;
    const photos = installation.photos;

    const photoObservables = photos.map(
      photo => this.wellService.getPhoto(this.wellRegistrationID, installation.installationCanonicalName, photo)
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
          installation.photoDataUrls.push({path: reader.result});
        };
      }

      installation.noPhotoAvailable = !foundPhoto;
    });
  }
  
  wellInGeoOptixUrl(): string {
    return `${environment.geoOptixWebUrl}/program/main/(inner:site)?projectCName=water-data-program&siteCName=${this.wellRegistrationID}`;
  }

  getSensors() {
    return this.well.sensors;
  }

  getLastReadingDate() {
    if (!this.well.lastReadingDate) {
      return ""
    }
    const time = moment(this.well.lastReadingDate)
    const timepiece = time.format('h:mm a');
    return time.format('M/D/yyyy ') + timepiece;
  }

  getInstallationDate(installation) {
    if (!installation.date) {
      return ""
    }
    const time = moment(installation.date)
    const timepiece = time.format('h:mm a');
    return time.format('M/D/yyyy ') + timepiece;
  }

  getAnnualPumpedVolume(year, dataSource){
    const annualPumpedVolume = this.well.annualPumpedVolume.find(x=> 
      x.year === year && x.dataSource === dataSource
    )

    if (!annualPumpedVolume || ! annualPumpedVolume.gallons){
      return "-"
    }
    

    if (this.unitsShown == "gal") {
      const value = this.decimalPipe.transform(annualPumpedVolume.gallons , "1.0-0")
      return `${value} ${this.unitsShown}`; 
    }

    const irrigatedAcresPerYear = this.well.irrigatedAcresPerYear.find(x => x.Year === year);

    if (!irrigatedAcresPerYear || (irrigatedAcresPerYear.Acres == null || irrigatedAcresPerYear.Acres == undefined)) {
      return "-";
    }

    const value = this.decimalPipe.transform((annualPumpedVolume.gallons / 27154) / irrigatedAcresPerYear.Acres , "1.1-1")
    return `${value} ${this.unitsShown}`;
  }

  displayIrrigatedAcres(): boolean {
    if (!this.well) {
      return false;
    }

    return this.well.irrigatedAcresPerYear.length > 0 && !this.well.irrigatedAcresPerYear.every(x => x.Acres == null || x.Acres == undefined);
  }

  public toggleUnitsShown(units : string): void {
    this.unitsShown = units;
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
    this.boundingBox = new BoundingBoxDto();
    this.boundingBox.Left = -122.65840077734131;
    this.boundingBox.Bottom = 44.800395454281436;
    this.boundingBox.Right = -121.65139301718362;
    this.boundingBox.Top = 45.528908149000124;

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
    const sensorTypes = this.well.sensors.map(x => x.sensorType);
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
    const wellLayer = new GeoJSON(this.well.location, {
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
      if (!response.timeSeries) {
        this.noTimeSeriesData = true;
        this.timeSeries = [];
        return;
      }

      this.sensors = response.sensors;
      this.timeSeries = response.timeSeries;
      
      console.log(this.timeSeries);

      this.cdr.detectChanges();

      const gallonsMax = this.timeSeries.sort((a, b) => b.gallons - a.gallons)[0].gallons;
      if (gallonsMax !== 0) {
        this.rangeMax = gallonsMax * 1.05;
      } else {
        this.rangeMax = 10000;
      }

      this.tooltipFields = response.sensors.map(x => ({ "field": x.sensorType, "type": "ordinal" }));
      const sensorTypes = response.sensors.map(x=>x.sensorType);

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
      let pivotRow = pivoted.get(point.time);
      if (pivotRow){
        pivotRow[point.dataSource] = point.gallons;
      } else{
        pivotRow = {"Date": point.time};
        pivotRow[point.dataSource] = point.gallons;
        pivoted.set(point.time, pivotRow);
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
    
    console.log(pivotedAndSorted);

    const fields = ['Date'];

    const sensorTypes = this.well.sensors.map(x=>x.sensorType);

    if (sensorTypes.includes("Flow Meter")){
      fields.push('FlowmeterGallons');
    }
    
    if (this.well.hasElectricalData){
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
    console.log(startDate, endDate);
    
    this.filterChart(startDate, endDate);
  }

  useFullDateRange(){
    const startDate = new Date(this.well.firstReadingDate) 
    const endDate = new Date(this.well.lastReadingDate)
    console.log(startDate,endDate);

    this.initDateRange(startDate, endDate);

    this.filterChart(startDate, endDate);
  }

  filterChart(startDate: Date, endDate: Date){
    const filteredTimeSeries = this.timeSeries.filter(x=>{
      const asDate =  new Date(x.time)
      return asDate.getTime() >= startDate.getTime() && asDate.getTime() <= endDate.getTime()
    });

    var changeSet = vega.changeset().remove(x => true).insert(filteredTimeSeries);
    console.log(filteredTimeSeries)
    this.vegaView.change('timeSeries', changeSet).run();
  }

  getVegaSpec(): any {
    return {
      "$schema": "https://vega.github.io/schema/vega-lite/v4.json",
      "description": "A charmt",
      "width": "container",
      "height": "container",
      "data": { "name": "timeSeries" },
      "encoding": {
        "x": {
          "field": "time",
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
              "field": "gallons",
              "type": "quantitative",
              "axis": {
                "title": "Gallons"
              },
              "scale": {
                "domain": [0, this.rangeMax]
              }
            },
            "color": {
              "field": "dataSource",
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
          "transform": [{ "pivot": "dataSource", "value": "gallonsString", "groupby": ["time"], "op": "max" }],
          "mark": "rule",
          "encoding": {
            "opacity": {
              "condition": { "value": 0.3, "selection": "hover" },
              "value": 0
            },
            "tooltip": [
              { "field": "time", "type": "temporal", "title": "Date" },
              ...this.tooltipFields
            ]
          },
          "selection": {
            "hover": {
              "type": "single",
              "fields": ["time"],
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
