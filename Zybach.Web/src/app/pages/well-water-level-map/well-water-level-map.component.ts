
import { AfterViewInit, ApplicationRef, Component, EventEmitter, Input, OnInit, Output, ViewEncapsulation } from '@angular/core';
import {
  Control, FitBoundsOptions,
  GeoJSON,
  marker,
  map,
  Map,
  MapOptions,
  tileLayer,
  geoJSON,
  icon,
  latLng,
  Layer,
  LeafletEvent} from 'leaflet';
import 'leaflet.snogylop';
import 'leaflet.icon.glyph';
import 'leaflet.fullscreen';
import { GestureHandling } from 'leaflet-gesture-handling'
import { CustomCompileService } from 'src/app/shared/services/custom-compile.service';
import { TwinPlatteBoundaryGeoJson } from '../../shared/models/tpnrd-boundary';

import { NominatimService } from 'src/app/services/nominatim.service';

import { point, polygon } from '@turf/helpers';
import booleanWithin from '@turf/boolean-within';
import { ToastrService } from 'ngx-toastr';
import { NgElement, WithProperties } from '@angular/elements';
import { WellMapPopupComponent } from '../well-map-popup/well-map-popup.component';
import { DefaultBoundingBox } from 'src/app/shared/models/default-bounding-box';
import { UserDto } from 'src/app/shared/generated/model/user-dto';
import { BoundingBoxDto } from 'src/app/shared/models/bounding-box-dto';

@Component({
  selector: 'zybach-well-water-level-map',
  templateUrl: './well-water-level-map.component.html',
  styleUrls: ['./well-water-level-map.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class WellWaterLevelMapComponent implements OnInit, AfterViewInit {
  
  public mapID: string = 'wellMap';
  public onEachFeatureCallback?: (feature, layer) => void;
  public zoomMapToDefaultExtent: boolean = true;
  public disableDefaultClick: boolean = false;
  public mapHeight: string = '650px';
  public defaultFitBoundsOptions?: FitBoundsOptions = null;
  
  @Input()
  public wellsGeoJson: any;

  @Output()
  public onWellSelected: EventEmitter<any> = new EventEmitter();

  public map: Map;
  public featureLayer: any;
  public layerControl: Control.Layers;
  public tileLayers: { [key: string]: any } = {};
  public overlayLayers: { [key: string]: any } = {};
  public boundingBox: BoundingBoxDto;
  public watchUserChangeSubscription: any;
  public currentUser: UserDto;

  public tpnrdBoundaryLayer: GeoJSON<any>;
  public wellsLayer: GeoJSON<any>;
  public selectedFeatureLayer: any;

  public mapSearchQuery: string;
  public maxZoom: number = 16;

  searchErrormessage: string = "Sorry, the address you searched is not within the NRD area. Click a well on the map or search another address.";
  
  showContinuityMeters: boolean = true;
  showElectricalData: boolean = true;
  showNoEstimate: boolean = true;

  constructor(
    private appRef: ApplicationRef,
    private compileService: CustomCompileService,
    private nominatimService: NominatimService,
    private toastr: ToastrService
  ) { }

  ngOnInit(): void {
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

    this.compileService.configure(this.appRef);
  }

  public ngAfterViewInit(): void {
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
    
    const markerIcon = icon.glyph({
      prefix: "fas",
      glyph: "tint",
      iconUrl: "/assets/main/electricalDataMarker.png"
    });

    this.wellsLayer = new GeoJSON(this.wellsGeoJson, {
      pointToLayer: function (feature, latlng) {
        return marker(latlng, { icon: markerIcon})
      }
    });

    this.wellsLayer.addTo(this.map);

    this.wellsLayer.on("click", (event: LeafletEvent) => {
      this.selectFeature(event.propagatedFrom.feature);
      this.onWellSelected.emit(event.propagatedFrom.feature.properties.wellID);
    })

    this.tpnrdBoundaryLayer = geoJSON(TwinPlatteBoundaryGeoJson as any, {
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
    } as any)

    this.tpnrdBoundaryLayer.addTo(this.map);

    this.map.fitBounds(this.tpnrdBoundaryLayer.getBounds());

    this.overlayLayers = {
      "District Boundary": this.tpnrdBoundaryLayer
    };

    this.setControl();
  }

  public setControl(): void {
    this.layerControl = new Control.Layers(this.tileLayers, this.overlayLayers, { collapsed: false })
      .addTo(this.map);
  }

  public mapSearch() {
    this.nominatimService.makeNominatimRequest(this.mapSearchQuery).subscribe(x => {
      if (!x.length) {
        this.toastr.warning("", this.searchErrormessage, {timeOut: 10000});
      }

      const nominatimResult = x[0];

      if (this.nominatimResultWithinTpnrdBoundary(nominatimResult)) {
        this.map.setView(latLng(nominatimResult.lat, nominatimResult.lon), this.maxZoom)
      } else {
        this.toastr.warning("", this.searchErrormessage, {timeOut: 10000});
      }
    })
  }

  nominatimResultWithinTpnrdBoundary(nominatimResult: { lat: number, lon: number }): boolean {
    const turfPoint = point([nominatimResult.lon, nominatimResult.lat]);
    const turfPolygon = polygon(TwinPlatteBoundaryGeoJson.features[0].geometry.coordinates);
    return booleanWithin(turfPoint, turfPolygon);
  }

  public selectWell(wellID: number): void {
    const wellFeature = this.wellsGeoJson.features.find(x => x.properties.wellID === wellID);
    this.selectFeature(wellFeature);
  }

  selectFeature(feature) : void {
    this.clearLayer(this.selectedFeatureLayer);

    const markerIcon = icon.glyph({
      prefix: "fas",
      glyph: "tint",
      iconUrl: "/assets/main/selectionMarker.png"
    });

    this.selectedFeatureLayer = new GeoJSON(feature, {
      pointToLayer: (feature,latlng) =>{
        return marker(latlng, {icon: markerIcon});
      }
    });

    this.selectedFeatureLayer
      .addTo(this.map);
  }
  
  public clearLayer(layer: Layer): void {
    if (layer) {
      this.map.removeLayer(layer);
      layer = null;
    }
  }

}


  

 







 





