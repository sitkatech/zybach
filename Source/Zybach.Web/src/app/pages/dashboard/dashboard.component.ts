import { AfterViewInit, ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { ManagerDashboardService } from 'src/app/services/manager-dashboard.service';
import {
  Control, FitBoundsOptions,
  GeoJSON,
  map,
  Map,
  MapOptions,
  tileLayer,
  geoJSON,
  LeafletEvent,
  DomUtil
} from 'leaflet';
import 'leaflet.snogylop';
import 'leaflet.icon.glyph';
import 'leaflet.fullscreen';
import { GestureHandling } from 'leaflet-gesture-handling'
import { BoundingBoxDto } from 'src/app/shared/models/bounding-box-dto';
import { forkJoin } from 'rxjs';
import { DefaultBoundingBox } from 'src/app/shared/models/default-bounding-box';
import { DistrictStatisticsDto } from 'src/app/shared/generated/model/district-statistics-dto';
import { StreamFlowZoneDto } from 'src/app/shared/generated/model/stream-flow-zone-dto';
import { StreamFlowZonePumpingDepthDto } from 'src/app/shared/generated/model/stream-flow-zone-pumping-depth-dto';

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

  public pumpingDepthsByYear: { Year: number, StreamFlowZonePumpingDepths: StreamFlowZonePumpingDepthDto[] }[]
  allYearsPumpingDepths: StreamFlowZonePumpingDepthDto[];

  constructor(
    private managerDashboardService: ManagerDashboardService,
    public cdr: ChangeDetectorRef
  ) { }

  ngOnInit(): void {
    this.currentYear = new Date().getFullYear();
    this.yearToDisplay = new Date().getFullYear();

    this.managerDashboardService.getDistrictStatistics().subscribe(stats => {
      this.districtStatistics = stats;
      this.loadingDistrictStatistics = false;
    });
  }

  public updateAnnualData(): void {
    // the "district statistics" panel will show the same information for all years as for the current year
    // (unless we in the future get some way to indicate that a well/sensor has been decommissioned)
    const yearForStatistics = this.allYearsSelected ? this.currentYear : this.yearToDisplay;

    if (this.pumpingDepthsByYear) {
      this.displayStreamFlowZones();
    }
  }

  public getAcreage(streamFlowZone: StreamFlowZoneDto): number {
    return (streamFlowZone.StreamFlowZoneArea * 0.000247105) || 0;
  }

  public getSelectedYearPumpingDepths(): StreamFlowZonePumpingDepthDto[] {
    let selectedYearPumpingDepths : StreamFlowZonePumpingDepthDto[]
    if (!this.allYearsSelected) {
      selectedYearPumpingDepths = this.pumpingDepthsByYear.find(x => x.Year === this.yearToDisplay).StreamFlowZonePumpingDepths;
    } else {
      selectedYearPumpingDepths = this.allYearsPumpingDepths;
    }

    return selectedYearPumpingDepths;
  }


  public getPumpingDepth(streamFlowZone: StreamFlowZoneDto): number {
    let selectedYearPumpingDepths = this.getSelectedYearPumpingDepths();
    return selectedYearPumpingDepths.find(x => x.StreamFlowZoneID === streamFlowZone.StreamFlowZoneID).PumpingDepth;
  }

  public getTotalIrrigatedAcres(streamFlowZone: StreamFlowZoneDto) : number{
    let selectedYearPumpingDepths = this.getSelectedYearPumpingDepths();

    return selectedYearPumpingDepths.find(x => x.StreamFlowZoneID === streamFlowZone.StreamFlowZoneID).TotalIrrigatedAcres;
  }

  
  public getTotalPumpedVolume(streamFlowZone: StreamFlowZoneDto) : number{
    let selectedYearPumpingDepths = this.getSelectedYearPumpingDepths();

    return selectedYearPumpingDepths.find(x => x.StreamFlowZoneID === streamFlowZone.StreamFlowZoneID).TotalPumpedVolume;
  }

  public ngAfterViewInit(): void {
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

    Map.addInitHook("addHandler", "gestureHandling", GestureHandling);
    const mapOptions: MapOptions = {
      maxZoom: this.maxZoom,
      layers: [
        this.tileLayers["Aerial"],
      ],
      fullscreenControl: true,
      gestureHandling: true
    } as MapOptions;
    this.map = map(this.mapID, mapOptions);

    this.map.fitBounds([[this.boundingBox.Bottom, this.boundingBox.Left], [this.boundingBox.Top, this.boundingBox.Right]], this.defaultFitBoundsOptions);

    this.setControl();

    this.getAndDisplayStreamflowZones();
  }

  public setControl(): void {
    this.layerControl = new Control.Layers(this.tileLayers, this.overlayLayers, { collapsed: false })
      .addTo(this.map);

    const PumpingDepthLegend = Control.extend({
      onAdd: function(map) {
        var legendElement = DomUtil.create("div", "legend-control");
        legendElement.style.borderRadius = "5px";
        legendElement.style.backgroundColor = "white";
        legendElement.style.cursor = "default";
        legendElement.style.padding = "6px";

        legendElement.innerHTML += "<span style='height: 10px; width: 10px; background-color: #C2523C; margin-right: 30px; display: inline-block'></span> 0-3\"<br/>"
        legendElement.innerHTML += "<span style='height: 10px; width: 10px; background-color: #DB7A25; margin-right: 30px; display: inline-block'></span> 3-6\"<br/>"
        legendElement.innerHTML += "<span style='height: 10px; width: 10px; background-color: #F0B411; margin-right: 30px; display: inline-block'></span> 6-9\"<br/>"
        legendElement.innerHTML += "<span style='height: 10px; width: 10px; background-color: #FCF003; margin-right: 30px; display: inline-block'></span> 9-12\"<br/>"
        legendElement.innerHTML += "<span style='height: 10px; width: 10px; background-color: #7BED00; margin-right: 30px; display: inline-block'></span> 12-15\"<br/>"
        legendElement.innerHTML += "<span style='height: 10px; width: 10px; background-color: #06D41B; margin-right: 30px; display: inline-block'></span> 15-18\"<br/>"
        legendElement.innerHTML += "<span style='height: 10px; width: 10px; background-color: #1BA87C; margin-right: 30px; display: inline-block'></span> 18-21\"<br/>"
        legendElement.innerHTML += "<span style='height: 10px; width: 10px; background-color: #18758C; margin-right: 30px; display: inline-block'></span> 21-24\"<br/>"
        legendElement.innerHTML += "<span style='height: 10px; width: 10px; background-color: #0B2C7A; margin-right: 30px; display: inline-block'></span> 24\"+"

        return legendElement;
      }
    });

    

    new PumpingDepthLegend().addTo(this.map);
  }

  public getAndDisplayStreamflowZones() {
    forkJoin([
      this.managerDashboardService.getStreamflowZones(),
      this.managerDashboardService.getStreamFlowZonePumpingDepths()
    ]).subscribe(([zones, pumpingDepthsByYear]) => {
      this.streamFlowZones = zones;
      this.pumpingDepthsByYear = pumpingDepthsByYear;

      const allPumpingDepths: StreamFlowZonePumpingDepthDto[] = [].concat.apply([], this.pumpingDepthsByYear.map(x => x.StreamFlowZonePumpingDepths))
      this.allYearsPumpingDepths = this.streamFlowZones.map(zone => {
        const filteredPumpingDepths = allPumpingDepths.filter(depth => depth.StreamFlowZoneID === zone.StreamFlowZoneID);

        const allYearsPumpingDepth = filteredPumpingDepths.map(depth => depth.PumpingDepth).reduce((x, y) => x + y, 0);
        const allYearsTotalPumpedVolume = filteredPumpingDepths.map(depth => depth.TotalPumpedVolume).reduce((x, y) => x + y, 0);
        const allYearsTotalIrrigatedAcres = filteredPumpingDepths.map(depth => depth.TotalIrrigatedAcres).reduce((x, y) => x + y, 0) / filteredPumpingDepths.length;

        return { StreamFlowZoneID: zone.StreamFlowZoneID, PumpingDepth: allYearsPumpingDepth, TotalIrrigatedAcres: allYearsTotalIrrigatedAcres, TotalPumpedVolume: allYearsTotalPumpedVolume }
      });

      console.log(this.pumpingDepthsByYear);

      this.displayStreamFlowZones();
    })
  }

  displayStreamFlowZones() {
    if (this.streamFlowZoneLayer) {
      this.map.removeLayer(this.streamFlowZoneLayer);
      this.streamFlowZoneLayer = null;
    }

    let pumpingDepths: StreamFlowZonePumpingDepthDto[];

    if (!this.allYearsSelected) {
      pumpingDepths = this.pumpingDepthsByYear.find(x => x.Year === this.yearToDisplay).StreamFlowZonePumpingDepths;
    }
    else {
      pumpingDepths = this.allYearsPumpingDepths;
    }

   
    const featureCollection = {type: "FeatureCollection", features: this.streamFlowZones.map(x=>x.StreamFlowZoneFeature)};

    this.streamFlowZoneLayer = geoJSON(featureCollection as any, {
      style: (feature) => {
        const fillColor = this.getFillColor(feature.properties.FeatureID, pumpingDepths);
        const fillOpacity = fillColor ? .55 : 0;
        return {
          fillColor: fillColor,
          fill: true,
          fillOpacity: fillOpacity,
          color: "#3388ff",
          weight: 2,
          stroke: true
        };
      }
    });

    this.streamFlowZoneLayer.addTo(this.map);
    this.map.fitBounds(this.streamFlowZoneLayer.getBounds());

    this.streamFlowZoneLayer.on("click", (event: LeafletEvent) => {
      const selectedFeatureID = event.propagatedFrom.feature.properties.FeatureID
      this.selectedStreamflowZone = this.streamFlowZones.find(x => x.StreamFlowZoneID === selectedFeatureID );
      this.streamFlowZoneLayer.eachLayer(function(layer) {
        if (layer.feature.properties.FeatureID == selectedFeatureID) {
          layer.setStyle({color: "#00FFFF"});
          layer.bringToFront();
          return;
        }
        layer.setStyle({color: "#3388ff"});
      })

      this.cdr.detectChanges();
      this.map.invalidateSize();
    })

  }
  getFillColor(streamFlowZoneID: number, pumpingDepths: StreamFlowZonePumpingDepthDto[]) {

    const pumpingDepth = pumpingDepths.find(x => x.StreamFlowZoneID === streamFlowZoneID).PumpingDepth;

    if (pumpingDepth === 0) {
      return null;
    } else if (pumpingDepth < 3) {
      return "#C2523C";
    } else if (pumpingDepth < 6) {
      return "#DB7A25";
    } else if (pumpingDepth < 9) {
      return "#F0B411"
    } else if (pumpingDepth < 12) {
      return "#FCF003"
    } else if (pumpingDepth < 15) {
      return "#7BED00"
    } else if (pumpingDepth < 18) {
      return "#06D41B"
    } else if (pumpingDepth < 21) {
      return "#1BA87C"
    } else if (pumpingDepth < 24) {
      return "#18758C"
    } else {
      return "#0B2C7A"
    }
  }

}
