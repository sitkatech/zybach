import { ApplicationRef, Component, EventEmitter, OnInit, ViewChild, ElementRef, ChangeDetectorRef } from '@angular/core';
import { environment } from "src/environments/environment";
import * as L from 'leaflet';
import { GestureHandling } from "leaflet-gesture-handling";
import 'leaflet.snogylop';
import 'leaflet.fullscreen';
import * as esri from 'esri-leaflet'
import { CustomCompileService } from '../../shared/services/custom-compile.service';
import { MapExplorerService } from 'src/app/services/map-explorer/map-explorer.service';
import { Feature } from 'geojson';
import { WellService } from 'src/app/services/well.service';
import { SiteDto, WellDto, StationDto } from 'src/app/shared/models/geooptix/site-dto';
import { AlertService } from 'src/app/shared/services/alert.service';
import { Alert } from 'src/app/shared/models/alert';
import { AlertContext } from 'src/app/shared/models/enums/alert-context.enum';
import { ArcService, remapWellFeaturePropertiesFromArc } from 'src/app/services/arc.service';
import { forkJoin } from 'rxjs';
import * as moment from 'moment';
import { LinkRendererComponent } from 'src/app/shared/components/ag-grid/link-renderer/link-renderer.component';

@Component({
  selector: 'zybach-map-explorer',
  templateUrl: './map-explorer.component.html',
  styleUrls: ['./map-explorer.component.scss']
})
export class MapExplorerComponent implements OnInit {
  @ViewChild("mapDiv") mapElement: ElementRef;

  public defaultMapZoom = 9;
  public afterSetControl = new EventEmitter();
  public afterLoadMap = new EventEmitter();
  public onMapMoveEnd = new EventEmitter();

  public component: any;

  public mapID = "NeighborhoodExplorerMap";
  public mapHeight = window.innerHeight + "px";
  public map: L.Map;
  public featureLayer: any;
  public layerControl: L.Control.Layers;
  public tileLayers: { [key: string]: any } = {};
  public overlayLayers: { [key: string]: any } = {};
  public maskLayer: any;

  public columnDefs: any[];

  public wmsParams: any;
  public stormshedLayer: L.Layers;
  public backboneDetailLayer: L.Layers;
  public traceLayer: L.Layers;
  public currentSearchLayer: L.Layers;
  public currentMask: L.Layers;
  public clickMarker: L.Marker;
  public traceActive: boolean = false;
  public showInstructions: boolean = true;
  public searchActive: boolean = false;
  public searchAddress: string;
  public activeSearchNotFound: boolean = false;

  public wells: WellDto[];
  public certAcresLayer: any;
  public wellsLayer: any;
  public activeWell: any;
  gridApi: any;
  selectedFeatureLayer: any;
  wellRegistrationCodes: string[];

  constructor(
    private appRef: ApplicationRef,
    private compileService: CustomCompileService,
    private mapExplorerService: MapExplorerService,
    private siteService: WellService,
    private arcService: ArcService,
    private alertService: AlertService,
    private cdr: ChangeDetectorRef
  ) {
  }

  public ngOnInit(): void {
    this.makeColumnDefs();
    this.makeStaticMapLayers();

    forkJoin(
      this.siteService.getSites(),
      this.siteService.getStations()
    ).subscribe(([sites, stations]) => {
      this.wells = sites;
      this.wellRegistrationCodes = sites.map(x => x.CanonicalName)
      this.initializeMap();
      this.prepareWells(stations);

    }, () => {
      this.alertService.pushAlert(new Alert("There was an error communicating with GeoOptix for well data", AlertContext.Danger, true));
    });

    this.compileService.configure(this.appRef);
  }

  public prepareWells(stations: StationDto[]): void {
    this.wells.forEach(well => {
      const sensor = stations.find(station => station.Site.CanonicalName == well.CanonicalName)
      well.Sensor = sensor;
    });

    this.wells.filter(x => x.Sensor).forEach(site => {
      this.siteService.getDataFile(site.Sensor).subscribe(dataFile => {
        if (dataFile.UpdateDate) {
          site.LastReading = new Date(dataFile.UpdateDate);
          this.gridApi.refreshCells();
        }
      }, error => { /*expect 404s*/ });
    });
  }

  public nednrUrl() {
    return `${environment.nednrInventoryBaseUrl}${this.activeWell.RegistrationNumber}`;
  }

  onGridReady(params) {
    this.gridApi = params.api;
  }

  public selectWellByFeatureFromArc(feature: Feature) {
    this.activeWell = remapWellFeaturePropertiesFromArc(feature);
    this.selectFeature(feature);
  }

  public selectWellFallback(site: SiteDto) {
    this.activeWell = { arcError: true, RegistrationNumber: site.CanonicalName };
    this.selectFeature(site.Location);
  }

  public hasGeoOptixDetails(wellRegistrationNumber: string): boolean {
    return this.wells.find(well => well.CanonicalName === wellRegistrationNumber) !== undefined;
  }

  public onSelectionChanged(event) {
    const selectedNode = this.gridApi.getSelectedNodes()[0];
    if (!selectedNode) {
      // event was fired automatically when we updated the grid after a map click
      return;
    }

    const site: SiteDto = selectedNode.data;
    console.log(site);
    // we need to grab the properties from the GIS layer.
    const cName: string = site.CanonicalName;

    this.arcService.getWellFromArcByRegistrationNumber(cName).subscribe(arcFeature => {
      if (arcFeature) {
        this.selectWellByFeatureFromArc(arcFeature)
      } else {
        this.selectWellFallback(site);
      }
    });
  }

  public setGridSelection(feature: Feature): void {
    this.gridApi.deselectAll();
    this.gridApi.forEachNode(node => {
      const asSite = node.data as SiteDto;
      return asSite.CanonicalName === feature.properties.RegistrationNumber;
    })
  }

  public selectFeature(feature: Feature) {

    var geojsonMarkerOptions = {
      radius: 8,
      fillColor: "#ffff00",
      color: "#000",
      weight: 1,
      opacity: 1,
      fillOpacity: 0.8
    };
    this.clearLayer(this.selectedFeatureLayer);
    this.selectedFeatureLayer = L.geoJSON(feature, {
      pointToLayer: function (feature, latlng) {
        return L.circleMarker(latlng, geojsonMarkerOptions);
      }
    });

    this.selectedFeatureLayer.addTo(this.map);

    let target = this.map._getBoundsCenterZoom(this.selectedFeatureLayer.getBounds(), null);
    this.map.setView(target.center, 16, null);
  }

  //fitBounds will use it's default zoom level over what is sent in
  //if it determines that its max zoom is further away. This can make the 
  //map zoom out to inappropriate levels sometimes, and then setZoom 
  //won't be honored because it's in the middle of a zoom. So we'll manipulate
  //it a bit.
  public defaultFitBounds(): void {
    let target = this.map._getBoundsCenterZoom(this.maskLayer.getBounds(), null);
    this.map.setView(target.center, this.defaultMapZoom, null);
  }

  public clearSelection() {
    this.activeWell = null;
    this.clearLayer(this.selectedFeatureLayer);
  }

  public clearLayer(layer: L.Layer): void {
    if (layer) {
      this.map.removeLayer(layer);
      layer = null;
    }
  }

  private makeColumnDefs() {
    this.columnDefs = [
      {
        headerName: 'Well Name', valueGetter: function (params: any) {
          return { LinkValue: params.data.CanonicalName, LinkDisplay: params.data.CanonicalName };
        }, cellRendererFramework: LinkRendererComponent,
        cellRendererParams: { inRouterLink: "/wells/" },
        filterValueGetter: function (params: any) {
          return params.data.FullName;
        },
        comparator: function (id1: any, id2: any) {
          let link1 = id1.LinkDisplay;
          let link2 = id2.LinkDisplay;
          if (link1 < link2) {
            return -1;
          }
          if (link1 > link2) {
            return 1;
          }
          return 0;
        },
        sortable: true, filter: true, width: 170
      },
      {
        headerName: "Sensor Name",
        field: "Sensor.CanonicalName",
        sortable: true, filter: true, width: 120
      },
      {
        headerName: "Last Reading",
        field: "LastReading",
        sortable: true, filter: true, width: 170,
        valueFormatter: function (params) {
          if (params.value) {
            const time = moment(params.value)
            const timepiece = time.format('h:mm a');
            return time.format('M/D/yyyy ') + timepiece;
          }
          else {
            return null;
          }
        },
      }
    ];
  }

  private makeStaticMapLayers() {
    this.tileLayers = Object.assign({}, {
      "Aerial": L.tileLayer('https://services.arcgisonline.com/ArcGIS/rest/services/World_Imagery/MapServer/tile/{z}/{y}/{x}', {
        attribution: 'Aerial',
        maxNativeZoom: 16,
        maxZoom: 22
      }),
      "Street": L.tileLayer('https://services.arcgisonline.com/ArcGIS/rest/services/World_Street_Map/MapServer/tile/{z}/{y}/{x}', {
        attribution: 'Street',
        maxNativeZoom: 16,
        maxZoom: 22
      }),
      "Terrain": L.tileLayer('https://server.arcgisonline.com/ArcGIS/rest/services/World_Topo_Map/MapServer/tile/{z}/{y}/{x}', {
        attribution: 'Terrain',
        maxNativeZoom: 16,
        maxZoom: 22
      }),
      "Hillshade": L.tileLayer('https://wtb.maptiles.arcgis.com/arcgis/rest/services/World_Topo_Base/MapServer/tile/{z}/{y}/{x}', {
        attribution: 'Hillshade',
        maxNativeZoom: 15,
        maxZoom: 22
      })
    }, this.tileLayers);
  }

  private initializeMap(): void {
    this.mapExplorerService.getMask().subscribe(maskString => {
      this.maskLayer = L.geoJSON(maskString, {
        invert: true,
        style: function () {
          return {
            fillColor: "#323232",
            fill: true,
            fillOpacity: 0.4,
            color: "#3388ff",
            weight: 5,
            stroke: true
          };
        }
      });

      L.Map.addInitHook("addHandler", "gestureHandling", GestureHandling);

      const mapOptions: L.MapOptions = {
        minZoom: 6,
        maxZoom: 22,
        layers: [
          this.tileLayers["Street"]
        ],
        gestureHandling: true

      } as L.MapOptions;

      this.map = L.map(this.mapID, mapOptions);

      this.initializePanes();
      this.setControl();

      this.initializeTpnrdLayers();
      this.initializeMapEvents();

      this.maskLayer.addTo(this.map);
      this.defaultFitBounds();
    });
  }

  private initializeTpnrdLayers() {

    var wellMarkerOptions = {
      radius: 4,
      fillColor: "#666",
      color: "#000",
      weight: 1,
      opacity: 1,
      fillOpacity: 0.4
    };

    var meteredWellMarkerOptions = {
      radius: 8,
      fillColor: "#0076c0",
      color: "#000",
      weight: 1,
      opacity: 1,
      fillOpacity: 0.8

    }

    this.certAcresLayer = esri.featureLayer({ url: `${environment.certAcresLayerUrl}?token=${environment.arcToken}` });
    const wellRegistrationCodes = this.wellRegistrationCodes;
    this.wellsLayer = esri.featureLayer({
      url: `${environment.wellsLayerUrl}?token=${environment.arcToken}`,
      pointToLayer: function (feature, latlng) {
        // if well is in the list from geooptix, symbolize more prominently
        if (wellRegistrationCodes.includes(feature.properties.RegistrationNumber)) {
          return L.circleMarker(latlng, meteredWellMarkerOptions);
        }
        return L.circleMarker(latlng, wellMarkerOptions);
      }
    });

    this.layerControl.addOverlay(this.wellsLayer, "Wells");
    this.layerControl.addOverlay(this.certAcresLayer, "Cert Acres");

    this.wellsLayer.addTo(this.map);
  }

  private initializePanes(): void {
    let zybachOverlayPane = this.map.createPane("zybachOverlayPane");
    zybachOverlayPane.style.zIndex = 10000;
    this.map.getPane("markerPane").style.zIndex = 10001;
    this.map.getPane("popupPane").style.zIndex = 10002;
  }

  private setControl(): void {
    this.layerControl = new L.Control.Layers(this.tileLayers, this.overlayLayers)
      .addTo(this.map);
    this.map.zoomControl.setPosition('topright');

    this.afterSetControl.emit(this.layerControl);
  }

  private initializeMapEvents(): void {
    this.map.on('load', (event: L.LeafletEvent) => {
      this.afterLoadMap.emit(event);
    });

    this.map.on("moveend", (event: L.LeafletEvent) => {
      this.onMapMoveEnd.emit(event);
    });

    // this.map.on('click', (event: L.LeafletEvent) => {
    //   this.clearSelection();
    // })

    // this.wellsLayer.on("click", (event: L.LeafletEvent) => {
    //   this.selectWellByFeatureFromArc(event.layer.feature)
    //   this.setGridSelection(event.layer.feature);
    //   L.DomEvent.stop(event);
    // });
  }
}
