import { AfterViewInit, ApplicationRef, ChangeDetectionStrategy, Component, EventEmitter, OnInit, ViewChild, ElementRef } from '@angular/core';
import { environment } from "src/environments/environment";
import * as L from 'leaflet';
import { GestureHandling } from "leaflet-gesture-handling";
import '../../../../node_modules/leaflet.snogylop/src/leaflet.snogylop.js';
import '../../../../node_modules/leaflet.fullscreen/Control.FullScreen.js';
import * as esri from 'esri-leaflet'
import { CustomCompileService } from '../../shared/services/custom-compile.service';
import { NeighborhoodExplorerService } from 'src/app/services/neighborhood-explorer/neighborhood-explorer.service';
import { NominatimService } from '../../shared/services/nominatim.service';
import { WfsService } from '../../shared/services/wfs.service';
import { FeatureCollection } from 'geojson';

declare var $: any;

@Component({
  selector: 'drooltool-neighborhood-explorer',
  templateUrl: './neighborhood-explorer.component.html',
  styleUrls: ['./neighborhood-explorer.component.scss']
})
export class NeighborhoodExplorerComponent implements OnInit {
  @ViewChild("mapDiv", {static:false}) mapElement: ElementRef;

  public defaultMapZoom = 12;
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
  public neighborhoodsWhereItIsOkayToClickIDs: number[];

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

  public selectedNeighborhoodID: number;

  constructor(
    private appRef: ApplicationRef,
    private compileService: CustomCompileService,
    private neighborhoodExplorerService: NeighborhoodExplorerService,
    private nominatimService: NominatimService,
    private wfsService: WfsService
  ) {
  }

  public ngOnInit(): void {

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

    let neighborhoodsWMSOptions = ({
      layers: "DroolTool:Neighborhoods",
      transparent: true,
      format: "image/png",
      tiled: true,
      pane: "droolToolOverlayPane"
    } as L.WMSOptions);

    let backboneWMSOptions = ({
      layers: "DroolTool:Backbones",
      transparent: true,
      format: "image/png",
      tiled: true,
      pane: "droolToolOverlayPane"
    } as L.WMSOptions);

    let watershedsWMSOptions = ({
      layers: "DroolTool:Watersheds",
      transparent: true,
      format: "image/png",
      tiled: true,
      pane: "droolToolOverlayPane"
    } as L.WMSOptions);



    this.overlayLayers = Object.assign({}, {
      "<span><img src='../../assets/neighborhood-explorer/neighborhood.png' height='12px' style='margin-bottom:3px;' /> Neighborhoods</span>": L.tileLayer.wms(environment.geoserverMapServiceUrl + "/wms?", neighborhoodsWMSOptions),
      "<span><img src='../../assets/neighborhood-explorer/backbone.png' height='12px' style='margin-bottom:3px;' /> Streams</span>": L.tileLayer.wms(environment.geoserverMapServiceUrl + "/wms?", backboneWMSOptions),
      "<span><img src='../../assets/neighborhood-explorer/backbone.png' height='12px' style='margin-bottom:3px;' /> Watersheds</span>": L.tileLayer.wms(environment.geoserverMapServiceUrl + "/wms?", watershedsWMSOptions),
      "<span>Stormwater Network <br/> <img src='../../assets/neighborhood-explorer/stormwaterNetwork.png' height='50'/> </span>": esri.dynamicMapLayer({ url: "https://ocgis.com/arcpub/rest/services/Flood/Stormwater_Network/MapServer/" })
    })

    this.compileService.configure(this.appRef);
  }

  public ngAfterViewInit(): void {

    this.neighborhoodExplorerService.getServicedNeighborhoodIds().subscribe(result => {
      this.neighborhoodsWhereItIsOkayToClickIDs = result;
    })

    this.initializeMap();
  }

  public initializeMap(): void {
    this.neighborhoodExplorerService.getMask().subscribe(maskString => {
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
          this.tileLayers["Street"],
          this.overlayLayers["<span><img src='../../assets/neighborhood-explorer/backbone.png' height='12px' style='margin-bottom:3px;' /> Streams</span>"],
          this.overlayLayers["<span><img src='../../assets/neighborhood-explorer/backbone.png' height='12px' style='margin-bottom:3px;' /> Watersheds</span>"]
        ],
        gestureHandling: true

      } as L.MapOptions;

      this.map = L.map(this.mapID, mapOptions);

      this.initializePanes();
      this.initializeMapEvents();
      this.setControl();

      this.maskLayer.addTo(this.map);
      this.defaultFitBounds();

      if(window.innerWidth > 991)
      {
        this.mapElement.nativeElement.scrollIntoView();
      }
    });
  }

  public initializePanes(): void {
    let droolToolOverlayPane = this.map.createPane("droolToolOverlayPane");
    droolToolOverlayPane.style.zIndex = 10000;
    this.map.getPane("markerPane").style.zIndex = 10001;
    this.map.getPane("popupPane").style.zIndex = 10002;
  }

  public setControl(): void {
    this.layerControl = new L.Control.Layers(this.tileLayers, this.overlayLayers)
      .addTo(this.map);
    this.map.zoomControl.setPosition('topright');
    // L.control.fullscreen({
    //   position: 'topright',
    //   title: 'View Fullscreen', // change the title of the button, default Full Screen
    //   titleCancel: 'Exit fullscreen mode', // change the title of the button when fullscreen is on, default Exit Full Screen
    //   content: null, // change the content of the button, can be HTML, default null
    //   forceSeparateButton: true, // force seperate button to detach from zoom buttons, default false
    //   forcePseudoFullscreen: true, // force use of pseudo full screen even if full screen API is available, default false
    //   fullscreenElement: false // Dom element to render in full screen, false by default, fallback to map._container
    // }).addTo(this.map);
    this.afterSetControl.emit(this.layerControl);
  }

  public initializeMapEvents(): void {
    this.map.on('load', (event: L.LeafletEvent) => {
      this.afterLoadMap.emit(event);
    });
    this.map.on("moveend", (event: L.LeafletEvent) => {
      this.onMapMoveEnd.emit(event);
    });

    let dblClickTimer = null;

    //to handle click for select area vs double click for zoom
    this.map.on("click", (event: L.LeafletEvent) => {
      if (dblClickTimer !== null) {
        return;
      }
      dblClickTimer = setTimeout(() => {
        this.getNeighborhoodFromLatLong(event.latlng, true);
        dblClickTimer = null;
      }, 200);
    }).on("dblclick", () => {
      clearTimeout(dblClickTimer);
      dblClickTimer = null;
      this.map.zoomIn();
    })
  }

  public makeNominatimRequest(searchText: any): void {
    this.clearSearchResults();
    this.searchAddress = searchText.value;
    this.nominatimService.makeNominatimRequest(this.searchAddress).subscribe(response => {
      if (response.length === 0) {
        this.searchAddressNotFoundOrNotServiced();
        return null;
      }

      let lat = +response[0].lat;
      let lng = +response[0].lon;
      let latlng = { 'lat': lat, 'lng': lng };

      this.getNeighborhoodFromLatLong(latlng, false);
    });
    searchText.value = '';
  }

  public getNeighborhoodFromLatLong(latlng: Object, mapClick: boolean): void {
    if (mapClick) {
      this.clearSearchResults();
    }
    this.wfsService.geoserverNeighborhoodLookup(latlng).subscribe(response => {
      if (response.features.length === 0) {
        this.searchAddressNotFoundOrNotServiced();
        return null;
      }

      this.selectedNeighborhoodID = response.features[0].properties.NeighborhoodID;
      if (this.neighborhoodsWhereItIsOkayToClickIDs.includes(this.selectedNeighborhoodID)) {
        this.displaySearchResults(response, latlng);
        this.displayStormshedAndBackboneDetail(this.selectedNeighborhoodID);
      }
      else {
        this.searchAddressNotFoundOrNotServiced();
      }
    });
  }

  public displaySearchResults(response: FeatureCollection, latlng: Object): void {

    this.currentSearchLayer = L.geoJSON(response, {
      style: function (feature) {
        return {
          fillColor: "#34FFCC",
          fill: true,
          fillOpacity: 0.3,
          stroke: false
        };
      }
    }).addTo(this.map);

    this.currentMask = L.geoJSON(response, {
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
    }).addTo(this.map);

    let icon = L.divIcon({
      html: '<i class="fas fa-map-marker-alt fa-2x" style="color:#105745"></i>',
      iconSize: [20,20],
      className: "search-popup"
    });

    let popupContent = "Neighborhood area for <span id='search-popup-address' class='search-popup-address'>" + (this.searchAddress !== undefined && this.searchAddress !== null ? this.searchAddress : "my selected neighborhood") + "</span>";
    let popupOptions = {
      'className': 'search-popup'
    }
    this.clickMarker = L.marker({ lat: latlng["lat"], lon: latlng["lng"] }, { icon: icon });

    this.currentMask.bringToFront();
    this.currentSearchLayer.bringToFront();
    this.clickMarker.addTo(this.map)
      .bindPopup(popupContent, popupOptions)
      .openPopup();

    setTimeout(() => {this.clickMarker.closePopup();}, 5000);

    this.searchActive = true;
  }

  public displayStormshedAndBackboneDetail(neighborhoodID: number): void {
    this.neighborhoodExplorerService.getStormshed(neighborhoodID).subscribe(response => {
      let featureCollection = (response) as any as FeatureCollection;
      if (featureCollection.features.length === 0) {
        return null;
      }

      this.stormshedLayer = L.geoJson(featureCollection, {
        style: function (feature) {
          return {
            fillColor: "#C0FF6C",
            fill: true,
            fillOpacity: 0.3,
            stroke: false
          };
        }
      })

      this.stormshedLayer.addTo(this.map);
      this.stormshedLayer.bringToBack();

      //if we get a stormshed, move the mask out
      this.clearLayer(this.currentMask);
      this.currentMask = L.geoJSON(featureCollection, {
        invert: true,
        style: function (feature) {
          return {
            fillColor: "#323232",
            fill: true,
            fillOpacity: 0.4,
            color: "#EA842C",
            weight: 5,
            stroke: true
          };
        }
      }).addTo(this.map);

      let neighborhoodIDs = featureCollection.features[0].properties["NeighborhoodIDs"];
      let cql_filter = "NeighborhoodID in (" + neighborhoodIDs.join(",") + ")";

      let backboneWMSOptions = ({
        layers: "DroolTool:Backbones",
        transparent: true,
        format: "image/png",
        tiled: true,
        styles: "backbone_narrow",
        wmsParameterThatDoesNotExist: new Date(),
        pane: "droolToolOverlayPane",
        cql_filter: cql_filter
      } as L.WMSOptions);

      this.backboneDetailLayer = L.tileLayer.wms(environment.geoserverMapServiceUrl + "/wms?", backboneWMSOptions);
      this.backboneDetailLayer.addTo(this.map);
      this.backboneDetailLayer.bringToFront();

      this.fitBoundsWithPaddingAndFeatureGroup(new L.featureGroup([this.clickMarker, this.stormshedLayer]));
    });
  }

  public displayTraceOrZoomToNeighborhood(event: Event): void {
    //Button lies on top of map, so we don't to be selecting a new area
    event.stopPropagation();
    if (!this.traceActive)
    {
      this.clearLayer(this.traceLayer);
      this.neighborhoodExplorerService.getDownstreamBackboneTrace(this.selectedNeighborhoodID).subscribe(response => {
        this.traceLayer = L.geoJSON(response,
          {
            style: function (feature) {
              return {
                color: "#FF20F9",
                weight: 3,
                stroke: true
              }
            },
            pane: "droolToolOverlayPane"
          })
        this.traceLayer.addTo(this.map);

        this.traceActive = true;
        this.fitBoundsWithPaddingAndFeatureGroup(new L.featureGroup([this.traceLayer, this.clickMarker, this.stormshedLayer]));     
      })
    }
    else {
      this.fitBoundsWithPaddingAndFeatureGroup(new L.featureGroup([this.clickMarker, this.stormshedLayer]));
      this.map.removeLayer(this.traceLayer);
      this.traceActive = false;
    }
  }

  public clearSearchResults(): void {
    this.searchAddress = null;
    this.searchActive = false;
    this.activeSearchNotFound = false;
    this.traceActive = false;
    this.removeCurrentSearchLayer();
  }

  public returnToDefault(): void {
    this.clearSearchResults();
    this.defaultFitBounds();
  }

  public searchAddressNotFoundOrNotServiced(): void {
    this.searchAddress = null;
    this.activeSearchNotFound = true;
  }

  public removeCurrentSearchLayer(): void {
    [this.clickMarker,
    this.currentSearchLayer,
    this.currentMask,
    this.stormshedLayer,
    this.backboneDetailLayer,
    this.traceLayer].forEach((x) => {
      this.clearLayer(x);
    });
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

  public fitBoundsWithPaddingAndFeatureGroup(featureGroup: L.featureGroup): void {
    let paddingHeight = 0;
    let popupContent = $("#search-popup-address");
    if (popupContent !== null && popupContent !== undefined && popupContent.length == 1)
    {
      paddingHeight = popupContent.parent().parent().innerHeight();
    }

    this.map.fitBounds(featureGroup.getBounds(), {padding: [paddingHeight, paddingHeight]});
  }
}
