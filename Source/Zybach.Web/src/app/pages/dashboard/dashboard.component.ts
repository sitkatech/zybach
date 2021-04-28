import { AfterViewInit, ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { ManagerDashboardService } from 'src/app/services/manager-dashboard.service';
import { DistrictStatisticsDto } from 'src/app/shared/models/district-statistics-dto';
import { StreamFlowZoneDto } from 'src/app/shared/models/stream-flow-zone-dto';
import {
  Control, FitBoundsOptions,
  GeoJSON,
  map,
  Map,
  MapOptions,
  tileLayer,
  geoJSON,
  LeafletEvent} from 'leaflet';
import 'leaflet.snogylop';
import 'leaflet.icon.glyph';
import 'leaflet.fullscreen';
import {GestureHandling} from 'leaflet-gesture-handling'
import { BoundingBoxDto } from 'src/app/shared/models/bounding-box-dto';
import { TwinPlatteBoundaryGeoJson } from '../well-explorer/tpnrd-boundary';

@Component({
  selector: 'zybach-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit, AfterViewInit {
  
  public maxZoom: number = 17;
  public map: Map;
  public boundingBox: BoundingBoxDto;
  public defaultFitBoundsOptions?: FitBoundsOptions = null;
  public mapID: string = "dashboardMap";
  public tileLayers: { [key: string]: any } = {};
  public overlayLayers: { [key: string]: any } = {};
  public layerControl: Control.Layers;
  public tpnrdBoundaryLayer: GeoJSON<any>;
  public streamFlowZones: StreamFlowZoneDto[];
  public streamFlowZoneLayer: any;
  public selectedStreamflowZone: StreamFlowZoneDto;

  public allYearsSelected: boolean = false;
  public yearToDisplay: number;
  public currentYear: number;
  
  public districtStatistics: DistrictStatisticsDto;
  public loadingDistrictStatistics: boolean = true;

  constructor(
    private managerDashboardService: ManagerDashboardService,
    public cdr: ChangeDetectorRef
  ) { }

  ngOnInit(): void {
    this.currentYear = new Date().getFullYear();
    this.yearToDisplay = new Date().getFullYear();
  }

  public updateAnnualData(): void {
    this.loadingDistrictStatistics = true;

    // the "district statistics" panel will show the same information for all years as for the current year
    // (unless we in the future get some way to indicate that a well/sensor has been decommissioned)
    const yearForStatistics = this.allYearsSelected ? this.currentYear : this.yearToDisplay;

    this.managerDashboardService.getDistrictStatistics(yearForStatistics).subscribe(stats =>{
      this.districtStatistics = stats;
      this.loadingDistrictStatistics = false;
    });
  }

  public getAcreage(streamFlowZone: StreamFlowZoneDto): number {
    return streamFlowZone.properties.Area * 0.000247105;
  }

  
  public ngAfterViewInit(): void {
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

    Map.addInitHook("addHandler", "gestureHandling", GestureHandling);
    const mapOptions: MapOptions = {
      maxZoom: this.maxZoom,
      layers: [
        this.tileLayers["Aerial"],
      ],
      fullscreenControl: true,
      gestureHandling:true
    } as MapOptions;
    this.map = map(this.mapID, mapOptions);

    this.map.fitBounds([[this.boundingBox.Bottom, this.boundingBox.Left], [this.boundingBox.Top, this.boundingBox.Right]], this.defaultFitBoundsOptions);
    
    // this.tpnrdBoundaryLayer = geoJSON(TwinPlatteBoundaryGeoJson as any, {
    //   invert: true,
    //   style: function () {
    //     return {
    //       fillColor: "#323232",
    //       fill: true,
    //       fillOpacity: 0.4,
    //       color: "#3388ff",
    //       weight: 3,
    //       stroke: true
    //     };
    //   }
    // } as any)

    // this.tpnrdBoundaryLayer.addTo(this.map);

    // this.map.fitBounds(this.tpnrdBoundaryLayer.getBounds());

    // this.overlayLayers = {
    //   "District Boundary": this.tpnrdBoundaryLayer
    // };

    this.setControl();

    this.getAndDisplayStreamflowZones();
  }

  public setControl(): void {
    this.layerControl = new Control.Layers(this.tileLayers, this.overlayLayers, { collapsed: false })
      .addTo(this.map);
  }

  public getAndDisplayStreamflowZones(){
    this.managerDashboardService.getStreamflowZones().subscribe(x=>{
      this.streamFlowZones = x;
      this.streamFlowZoneLayer = geoJSON(this.streamFlowZones as any, {
        style: function(){
          return {
            fillColor: "#323232",
            fill: true,
            fillOpacity: 0.4,
            color: "#3388ff",
            weight: 2,
            stroke: true
          };
        }
      });
      
      this.streamFlowZoneLayer.addTo(this.map);
      this.map.fitBounds(this.streamFlowZoneLayer.getBounds());


      this.streamFlowZoneLayer.on("click", (event: LeafletEvent) =>{
        this.selectedStreamflowZone = event.propagatedFrom.feature;

        this.cdr.detectChanges();
        this.map.invalidateSize();
      })
    });
  }

}
