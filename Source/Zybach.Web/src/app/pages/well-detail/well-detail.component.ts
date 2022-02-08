import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
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
import { DatePipe, DecimalPipe } from '@angular/common';
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
import { InstallationRecordDto } from 'src/app/shared/generated/model/installation-record-dto';
import { AgGridAngular } from 'ag-grid-angular';
import { WaterLevelInspectionSummaryDto } from 'src/app/shared/generated/model/water-level-inspection-summary-dto';
import { WaterQualityInspectionSummaryDto } from 'src/app/shared/generated/model/water-quality-inspection-summary-dto';
import { ColDef } from 'ag-grid-community';
import { CustomDropdownFilterComponent } from 'src/app/shared/components/custom-dropdown-filter/custom-dropdown-filter.component';
import { LinkRendererComponent } from 'src/app/shared/components/ag-grid/link-renderer/link-renderer.component';
import { UtilityFunctionsService } from 'src/app/services/utility-functions.service';
import { ChemigationPermitDetailedDto } from 'src/app/shared/generated/model/chemigation-permit-detailed-dto';

@Component({
  selector: 'zybach-well-detail',
  templateUrl: './well-detail.component.html',
  styleUrls: ['./well-detail.component.scss']
})
export class WellDetailComponent implements OnInit, OnDestroy, AfterViewInit {
  @ViewChild('dp') mydp: AngularMyDatePickerDirective;
  @ViewChild('wellLocation') mapContainer;  
  @ViewChild('waterLevelInspectionsGrid') waterLevelInspectionsGrid: AgGridAngular;
  @ViewChild('waterQualityInspectionsGrid') waterQualityInspectionsGrid: AgGridAngular;
  
  public chartID: string = "wellChart";

  public waterLevelInspectionColumnDefs: any[];
  public waterQualityInspectionColumnDefs: any[];
  public defaultColDef: ColDef;

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
  wellID: number;
  wellRegistrationID: string;
  tooltipFields: any;
  noTimeSeriesData: boolean = false;
  sensors: any;
  boundingBox: any;
  tileLayers: any;
  map: LeafletMap;
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

  public chemigationPermits: Array<ChemigationPermitDetailedDto>;

  public waterLevelInspections: Array<WaterLevelInspectionSummaryDto>;
  public waterQualityInspections: Array<WaterQualityInspectionSummaryDto>;

  constructor(
    private wellService: WellService,
    private sensorService: SensorStatusService,
    private authenticationService: AuthenticationService,
    private route: ActivatedRoute,
    private router: Router,
    private cdr: ChangeDetectorRef,
    private datePipe: DatePipe,
    private decimalPipe: DecimalPipe,
    private utilityFunctionsService: UtilityFunctionsService,
    private alertService: AlertService
  ) {
    // force route reload whenever params change;
    this.router.routeReuseStrategy.shouldReuseRoute = () => false;
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

    this.initializeWaterLevelInspectionsGrid();
    this.initializeWaterQualityInspectionsGrid();

    this.wellID = parseInt(this.route.snapshot.paramMap.get("id"));
    this.authenticationService.getCurrentUser().subscribe(currentUser => {
      this.currentUser = currentUser;
      this.getWellDetails();
      this.getSensorsWithAgeMessages();
      this.getChartDataAndBuildChart();
      this.getChemigationPermits();
      this.getInspections();
    })
  }

  private initializeWaterLevelInspectionsGrid(): void {
    let datePipe = this.datePipe;
    this.waterLevelInspectionColumnDefs = [
      {
        headerName: "Inspection Date", 
        valueGetter: function (params: any) {
          return { LinkValue: params.data.WaterLevelInspectionID, LinkDisplay: datePipe.transform(params.data.InspectionDate, "M/dd/yyyy h:mm a") };
        },
        cellRendererFramework: LinkRendererComponent,
        cellRendererParams: { inRouterLink: "/water-level-inspections/" },
        comparator: function (id1: any, id2: any) {
          const date1 = Date.parse(id1.LinkDisplay);
          const date2 = Date.parse(id2.LinkDisplay);
          if (date1 < date2) {
            return -1;
          }
          return (date1 > date2)  ?  1 : 0;
        },
        filter: 'agDateColumnFilter',
        filterParams: {
          filterOptions: ['inRange'],
          comparator: this.dateFilterComparator
        }, 
        width: 140,
        resizable: true,
        sortable: true
      },
      { 
        headerName: 'Measurement', 
        field: 'Measurement',
        filter: 'agNumberColumnFilter',
        type: 'rightAligned',
        width: 120,
        resizable: true,
        sortable: true
      },
      { 
        headerName: 'Measuring Equipment', field: 'MeasuringEquipment', 
        filterFramework: CustomDropdownFilterComponent,
        filterParams: {
          field: 'MeasuringEquipment'
        }, 
        width: 170,
        resizable: true,
        sortable: true 
      }
    ];
  }

  private initializeWaterQualityInspectionsGrid(): void {
    let datePipe = this.datePipe;
    this.waterQualityInspectionColumnDefs = [
      {
        headerName: "Inspection Date", 
        valueGetter: function (params: any) {
          return { LinkValue: params.data.WaterQualityInspectionID, LinkDisplay: datePipe.transform(params.data.InspectionDate, "M/dd/yyyy") };
        },
        cellRendererFramework: LinkRendererComponent,
        cellRendererParams: { inRouterLink: "/water-quality-inspections/" },
        comparator: function (id1: any, id2: any) {
          const date1 = Date.parse(id1.LinkDisplay);
          const date2 = Date.parse(id2.LinkDisplay);
          if (date1 < date2) {
            return -1;
          }
          return (date1 > date2)  ?  1 : 0;
        },
        filter: 'agDateColumnFilter',
        filterParams: {
          filterOptions: ['inRange'],
          comparator: this.waterQualityDateFilterComparator
        }, 
        width: 130,
        resizable: true,
        sortable: true
      },
      { 
        headerName: 'Inspection Type', field: 'InspectionType', 
        filterFramework: CustomDropdownFilterComponent,
        filterParams: {
          field: 'InspectionType'
        }, 
        width: 170,
        resizable: true,
        sortable: true 
      },
      { 
        headerName: 'Notes', 
        field: 'InspectionNotes',
        width: 180,
        filter: true,
        resizable: true,
        sortable: true
      },
    ];
  }

  // using a separate comparator for the LinkDisplay on WQI grid
  private waterQualityDateFilterComparator(filterLocalDate, cellValue) {
    const cellDate = Date.parse(cellValue.LinkDisplay);
    const filterLocalDateAtMidnight = filterLocalDate.getTime();
    if (cellDate == filterLocalDateAtMidnight) {
      return 0;
    }
    return (cellDate < filterLocalDateAtMidnight) ? -1 : 1;
  }

  private dateFilterComparator(filterLocalDate, cellValue) {
    const cellDate = Date.parse(cellValue);
    const filterLocalDateAtMidnight = filterLocalDate.getTime();
    if (cellDate == filterLocalDateAtMidnight) {
      return 0;
    }
    return (cellDate < filterLocalDateAtMidnight) ? -1 : 1;
  }

  getInspections() {
    forkJoin({
      waterLevelInspections: this.wellService.listWaterLevelInspectionsByWellID(this.wellID),
      waterQualityInspections: this.wellService.listWaterQualityInspectionsByWellID(this.wellID)
    }).subscribe(({ waterLevelInspections, waterQualityInspections }) => {
      this.waterLevelInspections = waterLevelInspections;
      this.waterQualityInspections = waterQualityInspections;

      this.waterLevelInspectionsGrid ? this.waterLevelInspectionsGrid.api.setRowData(waterLevelInspections) : null;
      this.waterQualityInspectionsGrid ? this.waterQualityInspectionsGrid.api.setRowData(waterQualityInspections) : null;

      this.waterLevelInspectionsGrid.api.sizeColumnsToFit();
      this.waterQualityInspectionsGrid.api.sizeColumnsToFit();

      this.cdr.detectChanges();
    });
  }

  public exportWLIToCsv() {
    this.utilityFunctionsService.exportGridToCsv(this.waterLevelInspectionsGrid, `${this.wellRegistrationID}-water-level-inspections.csv`, null);
  }

  public exportWQIToCsv() {
    this.utilityFunctionsService.exportGridToCsv(this.waterQualityInspectionsGrid, `${this.wellRegistrationID}-water-quality-inspections.csv`, null);
  }

  ngOnDestroy() {    
    this.chartSubscription.unsubscribe();
  }

  getWellDetails(){
    this.wellService.getWellDetails(this.wellID).subscribe((well: WellDetailDto)=>{
      this.well = well;
      this.wellRegistrationID = well.WellRegistrationID;
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

  getSensorsWithAgeMessages(){
    this.sensorService.getSensorStatusForWell(this.wellID).subscribe(wellWithSensorMessageAge => {
      this.sensorsWithStatus = wellWithSensorMessageAge.Sensors;

      for (var sensor of this.sensorsWithStatus){
        sensor.MessageAge = Math.floor(sensor.MessageAge / 3600)
      }
    });
  }

  getChemigationPermits(){
    this.wellService.getChemigationPermits(this.wellID).subscribe(chemigationPermits => {
      this.chemigationPermits = chemigationPermits;
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
    this.map = map(this.mapContainer.nativeElement, mapOptions);

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

    this.chartSubscription = this.wellService.getChartData(this.wellID).subscribe(response => {
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
