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
  LatLng,
  DomUtil
} from 'leaflet';
import 'leaflet.snogylop';
import 'leaflet.icon.glyph';
import 'leaflet.fullscreen';
import { GestureHandling } from 'leaflet-gesture-handling'
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
import { NgElement, WithProperties } from '@angular/elements';
import { WellMapPopupComponent } from '../well-map-popup/well-map-popup.component';
import { SensorStatusMapPopupComponent } from '../sensor-status-map-popup/sensor-status-map-popup.component';

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
  selectedFeatureLayer: any;

  public dataSourceDropdownList: { item_id: number, item_text: string }[] = [];
  public selectedDataSources: { item_id: number, item_text: string }[] = [];
  public dataSourceDropdownSettings: IDropdownSettings = {};

  public mapSearchQuery: string;
  public maxZoom: number = 17;

  searchErrormessage: string = "Sorry, the address you searched is not within the NRD area. Click a well on the map or search another address.";

  showZeroToThirty: boolean = true;
  showThirtyToSixty: boolean = true;
  showSixtyPlus: boolean = true;
  showNoData: boolean = false;

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
      { item_id: MessageAgeFilterOption.TWO_HOURS, item_text: "0-2 Hours" },
      { item_id: MessageAgeFilterOption.EIGHT_HOURS, item_text: "2-8 Hours" },
      { item_id: MessageAgeFilterOption.EIGHT_PLUS_HOURS, item_text: ">8 Hours" },
      { item_id: MessageAgeFilterOption.NO_DATA, item_text: "No Data" }
    ];
    this.selectedDataSources = [
      { item_id: MessageAgeFilterOption.TWO_HOURS, item_text: "0-2 Hours" },
      { item_id: MessageAgeFilterOption.EIGHT_HOURS, item_text: "2-8 Hours" },
      { item_id: MessageAgeFilterOption.EIGHT_PLUS_HOURS, item_text: ">8 Hours" },
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
      gestureHandling: true
    } as MapOptions;
    this.map = map(this.mapID, mapOptions);

    this.map.fitBounds([[this.boundingBox.Bottom, this.boundingBox.Left], [this.boundingBox.Top, this.boundingBox.Right]], this.defaultFitBoundsOptions);

    const blueMarkerIcon = icon.glyph({
      prefix: "fas",
      glyph: "tint",
      iconUrl: "/assets/main/0b2c7a.png"
    });
    const yellowMarkerIcon = icon.glyph({
      prefix: "fas",
      glyph: "tint",
      iconUrl: "/assets/main/fcf003.png"
    });
    const redMarkerIcon = icon.glyph({
      prefix: "fas",
      glyph: "tint",
      iconUrl: "/assets/main/c2523c.png"
    });
    const greyMarkerIcon = icon.glyph({
      prefix: "fas",
      glyph: "tint",
      iconUrl: "/assets/main/noDataSourceMarker.png"
    });


    this.wellsLayer = new GeoJSON(this.wellsGeoJson, {
      pointToLayer: (feature, latlng) => {
        var sensorMessageAges = feature.properties.sensors.map(x => x.MessageAge);
        var maxMessageAge = Math.max(...sensorMessageAges);

        let icon;
        if (!sensorMessageAges.some(x => x !== null)) {
          icon = greyMarkerIcon;
        } else if (maxMessageAge <= 3600 * 2) {
          icon = blueMarkerIcon;
        } else if (maxMessageAge <= 3600 * 8) {
          icon = yellowMarkerIcon;
        } else {
          icon = redMarkerIcon;
        }

        return marker(latlng, { icon: icon })
      },
      filter: (feature) => {
        var sensorMessageAges = feature.properties.sensors.map(x => x.MessageAge);
        var maxMessageAge = !sensorMessageAges.some(x => x !== null) ? null : Math.max(...sensorMessageAges);

        return (this.showNoData && maxMessageAge === null) ||
          (this.showZeroToThirty && maxMessageAge <= 3600 * 2 && maxMessageAge != null) ||
          (this.showThirtyToSixty && 3600 * 2 < maxMessageAge && maxMessageAge <= 3600 * 8) ||
          (this.showSixtyPlus && maxMessageAge > 3600 * 8 )
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

      const SensorStatusLegend = Control.extend({
        onAdd: function(map) {
          var legendElement = DomUtil.create("div", "legend-control");
          legendElement.style.borderRadius = "5px";
          legendElement.style.backgroundColor = "white";
          legendElement.style.cursor = "default";
          legendElement.style.padding = "6px";
  
          legendElement.innerHTML += "<img style='height: 20px; width: 12px;  margin-right: 30px; display: inline-block' src='/assets/main/0b2c7a.png'/> 0-2 Hours<br/>"
          legendElement.innerHTML += "<img style='height: 20px; width: 12px;  margin-right: 30px; display: inline-block' src='/assets/main/fcf003.png'/> 2-8 Hours<br/>"
          legendElement.innerHTML += "<img style='height: 20px; width: 12px;  margin-right: 30px; display: inline-block' src='/assets/main/c2523c.png'/> >8 Hours<br/>"
          legendElement.innerHTML += "<img style='height: 20px; width: 12px;  margin-right: 30px; display: inline-block' src='/assets/main/noDataSourceMarker.png'/>No Data<br/>"
  
          return legendElement;
        }
      });  
  
      new SensorStatusLegend().addTo(this.map);
  }

  public onDataSourceFilterChange(event: Event) {
    const selectedDataSourceOptions = this.selectedDataSources.map(x => x.item_id);

    this.showZeroToThirty = selectedDataSourceOptions.includes(MessageAgeFilterOption.TWO_HOURS)
    this.showThirtyToSixty = selectedDataSourceOptions.includes(MessageAgeFilterOption.EIGHT_HOURS)
    this.showSixtyPlus = selectedDataSourceOptions.includes(MessageAgeFilterOption.EIGHT_PLUS_HOURS)
    this.showNoData = selectedDataSourceOptions.includes(MessageAgeFilterOption.NO_DATA)

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
        this.toastr.warning("", this.searchErrormessage, { timeOut: 10000 });
      }

      const nominatimResult = x[0];

      if (this.nominatimResultWithinTpnrdBoundary(nominatimResult)) {
        this.map.setView(latLng(nominatimResult.lat, nominatimResult.lon), this.maxZoom)
      } else {
        this.toastr.warning("", this.searchErrormessage, { timeOut: 10000 });
      }
    })
  }

  nominatimResultWithinTpnrdBoundary(nominatimResult: { lat: number, lon: number }): boolean {
    const turfPoint = point([nominatimResult.lon, nominatimResult.lat]);
    const turfPolygon = polygon(TwinPlatteBoundaryGeoJson.features[0].geometry.coordinates);
    return booleanWithin(turfPoint, turfPolygon);
  }

  public selectWell(wellRegistrationID: string): void {
    const wellFeature = this.wellsGeoJson.features.find(x => x.properties.wellRegistrationID === wellRegistrationID);
    this.selectFeature(wellFeature);
  }

  selectFeature(feature): void {
    this.clearLayer(this.selectedFeatureLayer);
    const markerIcon = icon.glyph({
      prefix: "fas",
      glyph: "tint",
      iconUrl: "/assets/main/selectionMarker.png"
    });

    this.selectedFeatureLayer = new GeoJSON(feature, {
      pointToLayer: (feature, latlng) => {
        return marker(latlng, { icon: markerIcon });
      },
      onEachFeature: (feature, layer) => {
        layer.bindPopup(() => {
          const popupEl: NgElement & WithProperties<SensorStatusMapPopupComponent> = document.createElement('sensor-status-map-popup-element') as any;
          popupEl.registrationID = feature.properties.wellRegistrationID;
          popupEl.sensors = feature.properties.sensors;
          popupEl.landownerName = feature.properties.landownerName;
          popupEl.fieldName = feature.properties.fieldName;
          return popupEl;
        }, { maxWidth: 500 });
      }
    })

    this.selectedFeatureLayer
      .addTo(this.map);

    this.selectedFeatureLayer.eachLayer(function (layer) {
      layer.openPopup();
    })
  }

  public clearLayer(layer: Layer): void {
    if (layer) {
      this.map.removeLayer(layer);
      layer = null;
    }
  }

}

enum MessageAgeFilterOption {
  TWO_HOURS = 1,
  EIGHT_HOURS = 2,
  EIGHT_PLUS_HOURS = 3,
  NO_DATA = 4
}
