import { AfterViewInit, ApplicationRef, ChangeDetectionStrategy, Component, EventEmitter, OnInit, ViewChild, ElementRef } from '@angular/core';
import { environment } from "src/environments/environment";
import * as L from 'leaflet';
import { GestureHandling } from "leaflet-gesture-handling";
import 'leaflet.snogylop';
import 'leaflet.fullscreen';
import * as esri from 'esri-leaflet'
import { CustomCompileService } from '../../shared/services/custom-compile.service';
import { MapExplorerService } from 'src/app/services/map-explorer/map-explorer.service';
import { FeatureCollection, Feature } from 'geojson';
import { WellService } from 'src/app/services/well.service';
import { SiteDto } from 'src/app/shared/models/geooptix/site-dto';
import { AlertService } from 'src/app/shared/services/alert.service';
import { Alert } from 'src/app/shared/models/alert';
import { AlertContext } from 'src/app/shared/models/enums/alert-context.enum';
import { LinkRendererComponent } from 'src/app/shared/components/ag-grid/link-renderer/link-renderer.component';
import { ArcService } from 'src/app/services/arc.service';

declare var $: any;

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

  public wells: SiteDto[];
  public certAcresLayer: any;
  public wellsLayer: any;
  public activeWell: any;
  gridApi: any;
  selectedFeatureLayer: any;

  constructor(
    private appRef: ApplicationRef,
    private compileService: CustomCompileService,
    private mapExplorerService: MapExplorerService,
    private siteService: WellService,
    private arcService: ArcService,
    private alertService: AlertService
  ) {
  }

  public ngOnInit(): void {
    this.makeColumnDefs();
    this.makeStaticMapLayers();

    this.siteService.getSites().subscribe(x => {
      this.wells = x;
    }, error => {
      this.alertService.pushAlert(new Alert("There was an error communicating with GeoOptix for well data", AlertContext.Danger, true));
    });

    this.compileService.configure(this.appRef);
  }

  public ngAfterViewInit(): void {
    this.initializeMap();
  }

  private makeColumnDefs() {
    this.columnDefs = [
      {
        headerName: 'Well Name',
        // valueGetter: function (params: any) {
        //   return { LinkValue: params.data.CanonicalName, LinkDisplay: params.data.CanonicalName };
        // }, cellRendererFramework: LinkRendererComponent,
        // cellRendererParams: { inRouterLink: "/wells/" },
        // filterValueGetter: function (params: any) {
        //   return params.data.FullName;
        // },
        // comparator: function (id1: any, id2: any) {
        //   let link1 = id1.LinkDisplay;
        //   let link2 = id2.LinkDisplay;
        //   if (link1 < link2) {
        //     return -1;
        //   }
        //   if (link1 > link2) {
        //     return 1;
        //   }
        //   return 0;
        // },
        field: "CanonicalName",
        sortable: true, filter: true, width: 170
      }
    ];
  }

  private makeStaticMapLayers(){
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

  public initializeMap(): void {
    this.mapExplorerService.getMask().subscribe(maskString => {
      this.maskLayer = L.geoJSON(maskString, {
        invert: true,
        style: function (feature) {
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

      // if (window.innerWidth > 991) {
      //   this.mapElement.nativeElement.scrollIntoView();
      // }
    });
  }

  public initializeTpnrdLayers() {

    var geojsonMarkerOptions = {
      radius: 4,
      fillColor: "#0000ff",
      color: "#000",
      weight: 1,
      opacity: 1,
      fillOpacity: 0.8
    };

    this.certAcresLayer = esri.featureLayer({  url: `${environment.certAcresLayerUrl}?token=${environment.arcToken}` });
    this.wellsLayer = esri.featureLayer({
      url: `${environment.wellsLayerUrl}?token=${environment.arcToken}`,
      pointToLayer: function (feature, latlng) {
        // if well is in the list from geooptix, symbolize more prominently
        return L.circleMarker(latlng, geojsonMarkerOptions);
      }
    });

    this.layerControl.addOverlay(this.wellsLayer, "Wells");
    this.layerControl.addOverlay(this.certAcresLayer, "Cert Acres");

    this.wellsLayer.addTo(this.map);
  }

  public initializePanes(): void {
    let zybachOverlayPane = this.map.createPane("zybachOverlayPane");
    zybachOverlayPane.style.zIndex = 10000;
    this.map.getPane("markerPane").style.zIndex = 10001;
    this.map.getPane("popupPane").style.zIndex = 10002;
  }

  public setControl(): void {
    this.layerControl = new L.Control.Layers(this.tileLayers, this.overlayLayers)
      .addTo(this.map);
    this.map.zoomControl.setPosition('topright');

    this.afterSetControl.emit(this.layerControl);
  }

  public initializeMapEvents(): void {
    this.map.on('load', (event: L.LeafletEvent) => {
      this.afterLoadMap.emit(event);
    });
    
    this.map.on("moveend", (event: L.LeafletEvent) => {
      this.onMapMoveEnd.emit(event);
    });

    this.map.on('click', (event: L.LeafletEvent) =>{
      this.clearSelection(event);
    })

    this.wellsLayer.on("click", (event: L.LeafletEvent) => {
      this.selectWellByFeatureFromArc(event.layer.feature)
      L.DomEvent.stop(event);
    });

    let dblClickTimer = null;
  }

  public selectWellByFeatureFromArc(feature: Feature){
    this.activeWell = this.remapWellFeatureProperties(feature);
    this.selectFeature(feature);
  }

  public clearSelection(event: L.LeafletEvent){
    this.activeWell = null;
    this.clearLayer(this.selectedFeatureLayer);
  }

  public clearLayer(layer: L.Layer): void {
    if (layer) {
      this.map.removeLayer(layer);
      layer = null;
    }
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

  public remapWellFeatureProperties(feature: any): any {
    return {
      FID: feature.properties.FID,
      OBJECTID: feature.properties.Active_Irr,
      WellID: feature.properties.Active_I_1,
      RegCD: feature.properties.Active_I_2,
      Replacemen: feature.properties.Active_I_3,
      Status: feature.properties.Active_I_4,
      Useid: feature.properties.Active_I_5,
      NrdName: feature.properties.Active_I_6,
      NrdID: feature.properties.Active_I_7,
      Countyname: feature.properties.Active_I_8,
      CountyID: feature.properties.Active_I_9,
      Township: feature.properties.Active__10,
      Range: feature.properties.Active__11,
      RangeDir: feature.properties.Active__12,
      Section_: feature.properties.Active__13,
      SubSection: feature.properties.Active__14,
      FootageNS: feature.properties.Active__15,
      FootageDir: feature.properties.Active__16,
      FootageEW: feature.properties.Active__17,
      FootageD_1: feature.properties.Active__18,
      NrdPermit: feature.properties.Active__19,
      Acres: feature.properties.Active__20,
      SeriesType: feature.properties.Active__21,
      SeriesEnd: feature.properties.Active__22,
      PumpRate: feature.properties.Active__23,
      PColDiam: feature.properties.Active__24,
      PumpDepth: feature.properties.Active__25,
      TotalDepth: feature.properties.Active__26,
      SWL: feature.properties.Active__27,
      PWL: feature.properties.Active__28,
      CertifID: feature.properties.Active__29,
      OwnerID: feature.properties.Active__30,
      FirstName: feature.properties.Active__31,
      LastName: feature.properties.Active__32,
      Address: feature.properties.Active__33,
      CityNameID: feature.properties.Active__34,
      StateRID: feature.properties.Active__35,
      PostalCD: feature.properties.Active__36,
      RegDate: feature.properties.Active__37,
      Compdate: feature.properties.Active__38,
      LastChgDat: feature.properties.Active__39,
      DecommDate: feature.properties.Active__40,
      LatDD: feature.properties.Active__41,
      LongDD: feature.properties.Active__42,
      CalcGPS: feature.properties.Active__43,
      Hyperlink: feature.properties.Active__44,
      tpid_OBJECTID: feature.properties.tpid_wells,
      tpid_regcd: feature.properties.tpid_wel_1,
      tpid: feature.properties.tpid_wel_2
    }
  }

  public hasGeoOptixDetails(wellRegistrationNumber: string): boolean {
    return this.wells.find(well => well.CanonicalName === wellRegistrationNumber) !== undefined;
  }

  public onSelectionChanged(event: any) {

    const wellFeature: Feature = this.gridApi.getSelectedNodes()[0].data.Location;

    // we need to grab the properties from the GIS layer.
    const cName: string = this.gridApi.getSelectedNodes()[0].data.CanonicalName;

    this.arcService.getWellFromArcByRegCD(cName).subscribe(x=>{
      this.selectWellByFeatureFromArc(x.features[0])
    });

    this.selectFeature(wellFeature);
  }

  public selectFeature(feature: Feature) {

    var geojsonMarkerOptions = {
      radius: 4,
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

  onGridReady(params) {
    this.gridApi = params.api;
  }
}
