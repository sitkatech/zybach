import { AfterViewInit, ApplicationRef, Component, EventEmitter, Input, OnInit, Output, ViewEncapsulation } from '@angular/core';
import {
  Control, FitBoundsOptions,
  GeoJSON,
  marker,
  map,
  Map,
  MapOptions,
  tileLayer,
  Icon,
  geoJSON,
  icon,
  latLng,
  Layer,
  LeafletEvent,
  layerGroup,
  LatLng
} from 'leaflet';
import 'leaflet.snogylop';
import 'leaflet.icon.glyph';
import 'leaflet.fullscreen';
import {GestureHandling} from 'leaflet-gesture-handling'
import { Observable } from 'rxjs';
import { BoundingBoxDto } from 'src/app/shared/models/bounding-box-dto';
import { UserDto } from 'src/app/shared/models/generated/user-dto';
import { CustomCompileService } from 'src/app/shared/services/custom-compile.service';
import { TwinPlatteBoundaryGeoJson } from '../../shared/models/tpnrd-boundary';

import { IDropdownSettings } from 'ng-multiselect-dropdown';
import { NominatimService } from 'src/app/services/nominatim.service';

import { point, polygon } from '@turf/helpers';
import booleanWithin from '@turf/boolean-within';
import { ToastrService } from 'ngx-toastr';
import { DataSourceFilterOption, DataSourceSensorTypeMap } from 'src/app/shared/models/enums/data-source-filter-option.enum';
import { NgElement, WithProperties } from '@angular/elements';
import { WellMapPopupComponent } from '../well-map-popup/well-map-popup.component';

@Component({
  selector: 'zybach-sensor-status-map',
  templateUrl: './sensor-status-map.component.html',
  styleUrls: ['./sensor-status-map.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class SensorStatusMapComponent implements OnInit, AfterViewInit {
  public mapID: string = 'wellMap';
  public onEachFeatureCallback?: (feature, layer) => void;
  public zoomMapToDefaultExtent: boolean = true;
  public disableDefaultClick: boolean = false;
  public mapHeight: string = '650px';
  public defaultFitBoundsOptions?: FitBoundsOptions = null;

  @Input()
  public wellsGeoJson: any;

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
  selectedFeatureLayer: any;

  public dataSourceDropdownList: { item_id: number, item_text: string }[] = [];
  public selectedDataSources: { item_id: number, item_text: string }[] = [];
  public dataSourceDropdownSettings: IDropdownSettings = {};

  public mapSearchQuery: string;
  public maxZoom: number = 17;

  searchErrormessage: string = "Sorry, the address you searched is not within the NRD area. Click a well on the map or search another address.";
  
  showFlowMeters: boolean = true;
  showContinuityMeters: boolean = true;
  showElectricalData: boolean = true;
  showNoEstimate: boolean = false;

  constructor(
    private appRef: ApplicationRef,
    private compileService: CustomCompileService,
    private nominatimService: NominatimService,
    private toastr: ToastrService
  ) { }


  public ngOnInit(): void {
    // Default bounding box
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

    this.compileService.configure(this.appRef);

    this.dataSourceDropdownList = [
      { item_id: DataSourceFilterOption.FLOW, item_text: "Flow Meter" },
      { item_id: DataSourceFilterOption.CONTINUITY, item_text: "Continuity Meter" },
      { item_id: DataSourceFilterOption.ELECTRICAL, item_text: "Electrical Usage" },
      { item_id: DataSourceFilterOption.NODATA, item_text: "No Estimate Available" }
    ];
    this.selectedDataSources = [
      { item_id: DataSourceFilterOption.FLOW, item_text: "Flow Meter" },
      { item_id: DataSourceFilterOption.CONTINUITY, item_text: "Continuity Meter" },
      { item_id: DataSourceFilterOption.ELECTRICAL, item_text: "Electrical Usage" },
    ];
    this.dataSourceDropdownSettings = {
      singleSelection: false,
      idField: "item_id",
      textField: "item_text",
      selectAllText: "Select All",
      unSelectAllText: "Unselect All",
      allowSearchFilter: false,
      enableCheckAll: true
    }
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
    
    const flowMeterMarkerIcon = icon.glyph({
      prefix: "fas",
      glyph: "tint",
      iconUrl: "/assets/main/flowMeterMarker.png"
    });
    const continuityMeterMarkerIcon = icon.glyph({
      prefix: "fas",
      glyph: "tint",
      iconUrl: "/assets/main/continuityMeterMarker.png"
    });
    const electricalDataMarkerIcon = icon.glyph({
      prefix: "fas",
      glyph: "tint",
      iconUrl: "/assets/main/electricalDataMarker.png"
    });
    const noDataSourceMarkerIcon = icon.glyph({
      prefix: "fas",
      glyph: "tint",
      iconUrl: "/assets/main/noDataSourceMarker.png"
    });
    

    this.wellsLayer = new GeoJSON(this.wellsGeoJson, {
      pointToLayer: function (feature, latlng) {
        var sensorTypes = feature.properties.sensors.map(x => x.sensorType);
        if (sensorTypes.includes("Flow Meter")) {
          var icon = flowMeterMarkerIcon
        } else if (sensorTypes.includes("Continuity Meter")) {
          var icon = continuityMeterMarkerIcon
        } else if (sensorTypes.includes("Electrical Usage")) {
          var icon = electricalDataMarkerIcon
        } else {
          var icon = noDataSourceMarkerIcon
        }
        return marker(latlng, { icon: icon})
      },
      filter: (feature) => {
        if (feature.properties.sensors === null || feature.properties.sensors === 0) {
          return this.showNoEstimate;
        }

        var sensorTypes = feature.properties.sensors.map(x => x.sensorType);

        return (this.showFlowMeters && sensorTypes.includes("Flow Meter")) || 
          (this.showContinuityMeters && sensorTypes.includes("Continuity Meter")) ||
          (this.showElectricalData && sensorTypes.includes("Electrical Usage"));
      }
    });

    this.wellsLayer.addTo(this.map);

    this.wellsLayer.on("click", (event: LeafletEvent) => {
      this.selectFeature(event.propagatedFrom.feature);
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

  public onDataSourceFilterChange(event: Event) {
    const selectedDataSourceOptions = this.selectedDataSources.map(x=>x.item_id);

    this.showFlowMeters = selectedDataSourceOptions.includes(DataSourceFilterOption.FLOW)
    this.showContinuityMeters = selectedDataSourceOptions.includes(DataSourceFilterOption.CONTINUITY)
    this.showElectricalData = selectedDataSourceOptions.includes(DataSourceFilterOption.ELECTRICAL)
    this.showNoEstimate = selectedDataSourceOptions.includes(DataSourceFilterOption.NODATA)
    
    this.wellsLayer.clearLayers();
    this.wellsLayer.addData(this.wellsGeoJson);
  }

  // the select-all/deselect-all events fire before the model updates.
  // not sure if this is a fundamental limitation or just an annoying bug in the multiselect library,
  // but manually filling/clearing the selectedDataSources array lets select/deselect all work
  public onDataSourceFilterSelectAll(event) {
    this.selectedDataSources = [...this.dataSourceDropdownList];
    this.onDataSourceFilterChange(event)
  }

  public onDataSourceFilterDeselectAll(event) {
    this.selectedDataSources = [];
    this.onDataSourceFilterChange(event);
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

  public selectWell(wellRegistrationID: string): void {
    const wellFeature = this.wellsGeoJson.features.find(x=>x.properties.wellRegistrationID === wellRegistrationID);
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
      },
      onEachFeature: (feature, layer) => {
        layer.bindPopup(() => {
          const popupEl: NgElement & WithProperties<WellMapPopupComponent> = document.createElement('sensor-status-map-popup-element') as any;
          popupEl.registrationID = feature.properties.wellRegistrationID;
          popupEl.sensors = feature.properties.sensors;
          return popupEl;
        }, {maxWidth:500});
      }
    })

    this.selectedFeatureLayer
      .addTo(this.map);
    let target = (this.map as any)._getBoundsCenterZoom(this.selectedFeatureLayer.getBounds(), null);
    this.map.setView(target.center, 16, null);

    this.selectedFeatureLayer.eachLayer(function (layer) {
      layer.openPopup();
    })
  }

  public getPopupContentForWellFeature(feature: any) : NgElement & WithProperties<WellMapPopupComponent> {
    const popupEl: NgElement & WithProperties<WellMapPopupComponent> = document.createElement('sensor-status-map-popup-element') as any;
    popupEl.registrationID = feature.properties.wellRegistrationID;
    popupEl.sensors = feature.properties.sensors;
    return popupEl;
  }
  
  public clearLayer(layer: Layer): void {
    if (layer) {
      this.map.removeLayer(layer);
      layer = null;
    }
  }

}
