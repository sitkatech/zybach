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
  layerGroup
} from 'leaflet';
import 'leaflet.snogylop';
import 'leaflet.icon.glyph';
import 'leaflet.fullscreen';
import {GestureHandling} from 'leaflet-gesture-handling'
import { Observable } from 'rxjs';
import { BoundingBoxDto } from 'src/app/shared/models/bounding-box-dto';
import { UserDto } from 'src/app/shared/models/generated/user-dto';
import { CustomCompileService } from 'src/app/shared/services/custom-compile.service';
import { TwinPlatteBoundaryGeoJson } from '../well-explorer/tpnrd-boundary';

import { IDropdownSettings } from 'ng-multiselect-dropdown';
import { NominatimService } from 'src/app/services/nominatim.service';

import { point, polygon } from '@turf/helpers';
import booleanWithin from '@turf/boolean-within';
import { ToastrService } from 'ngx-toastr';
import { DataSourceFilterOption, DataSourceSensorTypeMap } from 'src/app/shared/models/enums/data-source-filter-option.enum';
import { NgElement, WithProperties } from '@angular/elements';
import { WellMapPopupComponent } from '../well-map-popup/well-map-popup.component';

@Component({
  selector: 'zybach-well-map',
  templateUrl: './well-map.component.html',
  styleUrls: ['./well-map.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class WellMapComponent implements OnInit, AfterViewInit {
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
  
  @Output()
  public onFilterChange: EventEmitter<any> = new EventEmitter();

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
      { item_id: DataSourceFilterOption.ELECTRICAL, item_text: "Electrical Data" },
      { item_id: DataSourceFilterOption.NODATA, item_text: "No Estimate Available" }
    ];
    this.selectedDataSources = [
      { item_id: DataSourceFilterOption.FLOW, item_text: "Flow Meter" },
      { item_id: DataSourceFilterOption.CONTINUITY, item_text: "Continuity Meter" },
      { item_id: DataSourceFilterOption.ELECTRICAL, item_text: "Electrical Data" },
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

        if (feature.properties.sensorTypes.includes("Flow Meter")) {
          var icon = flowMeterMarkerIcon
        } else if (feature.properties.sensorTypes.includes("Continuity Meter")) {
          var icon = continuityMeterMarkerIcon
        } else if (feature.properties.sensorTypes.includes("Electrical Data")) {
          var icon = electricalDataMarkerIcon
        } else {
          var icon = noDataSourceMarkerIcon
        }
        return marker(latlng, { icon: icon})
      },
      filter: (feature) => {
        if (feature.properties.sensorTypes === null || feature.properties.sensorTypes.length === 0) {
          return this.showNoEstimate;
        }

        return (this.showFlowMeters && feature.properties.sensorTypes.includes("Flow Meter")) || 
          (this.showContinuityMeters && feature.properties.sensorTypes.includes("Continuity Meter")) ||
          (this.showElectricalData && feature.properties.sensorTypes.includes("Electrical Data"));
      }
    });

    this.wellsLayer.addTo(this.map);

    this.wellsLayer.on("click", (event: LeafletEvent) => {
      this.selectFeature(event.propagatedFrom.feature);
      this.onWellSelected.emit(event.propagatedFrom.feature.properties.wellRegistrationID);
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
    
    // todo: change this to emit an object holding the above values, then change the handler on the other side

    this.onFilterChange.emit({
      showFlowMeters: this.showFlowMeters,
      showContinuityMeters: this.showContinuityMeters,
      showElectricalData: this.showElectricalData,
      showNoEstimate: this.showNoEstimate
    });

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
      }
    })

    let popupContent = this.getPopupContentForWellFeature(feature);

    this.selectedFeatureLayer.addTo(this.map)
      .bindPopup(this.getPopupContentForWellFeature(feature))
      .openPopup();
    
    let target = (this.map as any)._getBoundsCenterZoom(this.selectedFeatureLayer.getBounds(), null);
    this.map.setView(target.center, 16, null);
  }

  //In order to render a routerLink in the Popup we needed a slightly more robust solution
  public getPopupContentForWellFeature(feature: any) : NgElement & WithProperties<WellMapPopupComponent> {
    const popupEl: NgElement & WithProperties<WellMapPopupComponent> = document.createElement('well-map-popup-element') as any;
    popupEl.feature = feature;
    return popupEl;
  }
  
  public clearLayer(layer: Layer): void {
    if (layer) {
      this.map.removeLayer(layer);
      layer = null;
    }
  }

}

