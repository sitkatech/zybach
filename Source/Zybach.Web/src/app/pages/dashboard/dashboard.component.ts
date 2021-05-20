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
  LeafletEvent
} from 'leaflet';
import 'leaflet.snogylop';
import 'leaflet.icon.glyph';
import 'leaflet.fullscreen';
import { GestureHandling } from 'leaflet-gesture-handling'
import { BoundingBoxDto } from 'src/app/shared/models/bounding-box-dto';
import { forkJoin } from 'rxjs';
import { streamFlowZonePumpingDepthDto } from 'src/app/shared/models/stream-flow-zone-pumping-depth-dto';
import { X } from 'vega-lite/build/src/channel';

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

  public pumpingDepthsByYear: { year: number, streamFlowZonePumpingDepths: streamFlowZonePumpingDepthDto[] }[]
  allYearsPumpingDepths: streamFlowZonePumpingDepthDto[];

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

    if (this.pumpingDepthsByYear) {
      this.displayStreamFlowZones();
    }
    this.managerDashboardService.getDistrictStatistics(yearForStatistics).subscribe(stats => {
      this.districtStatistics = stats;
      this.loadingDistrictStatistics = false;
    });
  }

  public getAcreage(streamFlowZone: StreamFlowZoneDto): number {
    return streamFlowZone.properties.Area * 0.000247105;
  }

  public getPumpingDepth(streamFlowZone: StreamFlowZoneDto): number {
    let selectedYearPumpingDepths;
    if (!this.allYearsSelected) {
      selectedYearPumpingDepths = this.pumpingDepthsByYear.find(x => x.year === this.yearToDisplay).streamFlowZonePumpingDepths;
    } else {
      selectedYearPumpingDepths = this.allYearsPumpingDepths;
    }

    return selectedYearPumpingDepths.find(x => x.streamFlowZoneFeatureID === streamFlowZone.properties.FeatureID).pumpingDepth;
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
  }

  public getAndDisplayStreamflowZones() {
    forkJoin([
      this.managerDashboardService.getStreamflowZones(),
      this.managerDashboardService.getStreamFlowZonePumpingDepths()
    ]).subscribe(([zones, pumpingDepthsByYear]) => {
      this.streamFlowZones = zones;
      this.pumpingDepthsByYear = pumpingDepthsByYear;

      const allPumpingDepths: streamFlowZonePumpingDepthDto[] = [].concat.apply([], this.pumpingDepthsByYear.map(x => x.streamFlowZonePumpingDepths))
      this.allYearsPumpingDepths = this.streamFlowZones.map(zone => {
        const allYearsPumpingDepth = allPumpingDepths.filter(depth => depth.streamFlowZoneFeatureID === zone.properties.FeatureID).map(depth => depth.pumpingDepth).reduce((x, y) => x + y, 0);
        return { streamFlowZoneFeatureID: zone.properties.FeatureID, pumpingDepth: allYearsPumpingDepth }
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

    let pumpingDepths: streamFlowZonePumpingDepthDto[];

    if (!this.allYearsSelected) {
      pumpingDepths = this.pumpingDepthsByYear.find(x => x.year === this.yearToDisplay).streamFlowZonePumpingDepths;
    }
    else {
      pumpingDepths = this.allYearsPumpingDepths;
    }

    const maxPumpingDepth = pumpingDepths.map(x => x.pumpingDepth).sort()[pumpingDepths.length - 1];

    this.streamFlowZoneLayer = geoJSON(this.streamFlowZones as any, {
      style: function (feature) {
        const pumpingDepth = pumpingDepths.find(x => x.streamFlowZoneFeatureID === feature.properties.FeatureID).pumpingDepth;
        const opacity = .75 * pumpingDepth / maxPumpingDepth;
        return {
          fillColor: "#0022b8",
          fill: true,
          fillOpacity: opacity,
          color: "#3388ff",
          weight: 2,
          stroke: true
        };
      }
    });

    this.streamFlowZoneLayer.addTo(this.map);
    this.map.fitBounds(this.streamFlowZoneLayer.getBounds());

    this.streamFlowZoneLayer.on("click", (event: LeafletEvent) => {
      this.selectedStreamflowZone = event.propagatedFrom.feature;

      this.cdr.detectChanges();
      this.map.invalidateSize();
    })

  }

}
