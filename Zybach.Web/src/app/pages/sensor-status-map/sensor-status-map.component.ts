import { AfterViewInit, ApplicationRef, Component, EventEmitter, Input, OnInit, Output, ViewEncapsulation } from '@angular/core';
import {
  Control,
  control,
  FitBoundsOptions,
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
  LeafletEvent,
  layerGroup
} from 'leaflet';
import 'leaflet.snogylop';
import 'leaflet.icon.glyph';
import 'leaflet.fullscreen';
import { GestureHandling } from 'leaflet-gesture-handling'
import { BoundingBoxDto } from 'src/app/shared/models/bounding-box-dto';
import { CustomCompileService } from 'src/app/shared/services/custom-compile.service';
import { TwinPlatteBoundaryGeoJson } from '../../shared/models/tpnrd-boundary';
import { NominatimService } from 'src/app/services/nominatim.service';
import { point, polygon } from '@turf/helpers';
import booleanWithin from '@turf/boolean-within';
import { ToastrService } from 'ngx-toastr';
import { NgElement, WithProperties } from '@angular/elements';
import { SensorStatusMapPopupComponent } from '../sensor-status-map-popup/sensor-status-map-popup.component';
import { DefaultBoundingBox } from 'src/app/shared/models/default-bounding-box';
import { UserDto } from 'src/app/shared/generated/model/user-dto';

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

  public filterOptionsDropdownList: { group_name: string, item_id: number, item_text: string }[];
  public selectedFilterOptions: MapFilterOption[];
  public sensorStatusLegend: Control;

  public mapSearchQuery: string;
  public maxZoom: number = 17;

  searchErrormessage: string = "Sorry, the address you searched is not within the NRD area. Click a well on the map or search another address.";

  public filterMapByLastMessageAge: boolean = true;

  showLessThan24Hours: boolean = true;
  showLessThan24HoursWithSupportTicket: boolean = true;
  showGreaterThan24Hours: boolean = true;
  showNoMessageData: boolean = true;

  showLessThan2500: boolean = false;
  show2500To2700: boolean = false;
  show2700To4000: boolean = false;
  showGreaterThan4000: boolean = false;
  showNoVoltageData: boolean = false;

  static blueMarkerIcon = icon.glyph({
    prefix: "fas",
    glyph: "tint",
    iconUrl: "/assets/main/0b2c7a.png"
  });
  static lightBlueMarkerIcon = icon.glyph({
    prefix: "fas",
    glyph: "tint",
    iconUrl: "/assets/main/flowMeterMarker.png"
  });
  static yellowMarkerIcon = icon.glyph({
    prefix: "fas",
    glyph: "tint",
    iconUrl: "/assets/main/fcf003.png"
  });
  static redMarkerIcon = icon.glyph({
    prefix: "fas",
    glyph: "tint",
    iconUrl: "/assets/main/c2523c.png"
  });
  static greenMarkerIcon = icon.glyph({
    prefix: "fas",
    glyph: "tint",
    iconUrl: "/assets/main/continuityMeterMarker.png"
  });
  static greyMarkerIcon = icon.glyph({
    prefix: "fas",
    glyph: "tint",
    iconUrl: "/assets/main/noDataSourceMarker.png"
  });

  constructor(
    private appRef: ApplicationRef,
    private compileService: CustomCompileService,
    private nominatimService: NominatimService,
    private toastr: ToastrService
  ) { }

  public ngOnInit(): void {
    this.boundingBox = DefaultBoundingBox;

    const esriAerialTileLayer = tileLayer('https://server.arcgisonline.com/ArcGIS/rest/services/World_Imagery/MapServer/tile/{z}/{y}/{x}', {
      maxZoom: 18,
      minZoom: 3,
      attribution: 'Source: Esri, Maxar, GeoEye, Earthstar Geographics, CNES/Airbus DS, USDA, USGS, AeroGRID, IGN, and the GIS User Community'
    });
    const esriStreets = tileLayer('https://services.arcgisonline.com/ArcGIS/rest/services/Reference/World_Transportation/MapServer/tile/{z}/{y}/{x}', {
      maxZoom: 18,
      minZoom: 3,
      attribution: 'Source: Esri, Maxar, GeoEye, Earthstar Geographics, CNES/Airbus DS, USDA, USGS, AeroGRID, IGN, and the GIS User Community'
    });

    this.tileLayers = Object.assign({}, {
      "Aerial": layerGroup([esriAerialTileLayer, esriStreets]),
      "Street": tileLayer('https://services.arcgisonline.com/ArcGIS/rest/services/World_Street_Map/MapServer/tile/{z}/{y}/{x}', {
        attribution: 'Aerial',
      }),
      "Terrain": tileLayer('https://server.arcgisonline.com/ArcGIS/rest/services/World_Topo_Map/MapServer/tile/{z}/{y}/{x}', {
        attribution: 'Terrain',
      }),
    }, this.tileLayers);

    this.compileService.configure(this.appRef);

    this.setLastMessageAgeFilterOptionsDropdownList();
    this.selectedFilterOptions = [MapFilterOption.LESS_THAN_24_HOURS, MapFilterOption.LESS_THAN_24_HOURS_WITH_SUPPORT_TICKET, MapFilterOption.GREATER_THAN_24_HOURS, MapFilterOption.NO_MESSAGE_DATA];
  }

  public ngAfterViewInit(): void {
    Map.addInitHook("addHandler", "gestureHandling", GestureHandling);
    const mapOptions: MapOptions = {
      maxZoom: this.maxZoom,
      layers: [
        this.tileLayers['Aerial'],
      ],
      fullscreenControl: true,
      gestureHandling: true
    } as MapOptions;
    this.map = map(this.mapID, mapOptions);

    this.map.fitBounds([[this.boundingBox.Bottom, this.boundingBox.Left], [this.boundingBox.Top, this.boundingBox.Right]], this.defaultFitBoundsOptions);

    this.wellsLayer = this.filterMapByLastMessageAge ? this.getWellLayerGeoJSONFilteredByLastMessageAge() : this.getWellLayerGeoJSONFilteredByLastVoltageReading();

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

  private getWellLayerGeoJSONFilteredByLastMessageAge(): GeoJSON {
    return new GeoJSON(this.wellsGeoJson, {
      pointToLayer: (feature, latlng) => {
        const sensorMessageAges = feature.properties.sensors.map(x => x.LastMessageAgeInHours);
        var maxMessageAge = Math.max(...sensorMessageAges);

        let icon;
        if (!sensorMessageAges.some(x => x != null)) {
          icon = SensorStatusMapComponent.greyMarkerIcon;
        } else if (maxMessageAge > 24) {
          icon = SensorStatusMapComponent.redMarkerIcon;
        } else if (feature.properties.sensors.find(x => x.MostRecentSupportTicketID) == null) {
          icon = SensorStatusMapComponent.blueMarkerIcon;
        } else {
          icon = SensorStatusMapComponent.greenMarkerIcon;
        }

        return marker(latlng, { icon: icon })
      },
      filter: (feature) => {
        var sensorMessageAges = feature.properties.sensors.map(x => x.LastMessageAgeInHours);
        var maxMessageAge = !sensorMessageAges.some(x => x !== null) ? null : Math.max(...sensorMessageAges);

        return (this.showNoMessageData && maxMessageAge === null) ||
          (this.showLessThan24Hours && maxMessageAge != null && maxMessageAge <= 24 && feature.properties.sensors.find(x => x.MostRecentSupportTicketID) == null) ||
          (this.showLessThan24HoursWithSupportTicket && maxMessageAge != null && maxMessageAge <= 24 && feature.properties.sensors.find(x => x.MostRecentSupportTicketID) != null) ||
          (this.showGreaterThan24Hours && maxMessageAge > 24 )
      }
    });
  }

  private getWellLayerGeoJSONFilteredByLastVoltageReading(): GeoJSON {
    return new GeoJSON(this.wellsGeoJson, {
      pointToLayer: (feature, latlng) => {
        var lastVoltageReadings = feature.properties.sensors.map(x => x.LastVoltageReading);
        var minVoltageReading = !lastVoltageReadings.some(x => x !== null) ? null : Math.min(...lastVoltageReadings.filter(x => x != null));

        let icon;
        if (minVoltageReading == null) {
          icon = SensorStatusMapComponent.greyMarkerIcon;
        } else if (minVoltageReading >= 4000) {
          icon = SensorStatusMapComponent.lightBlueMarkerIcon;
        } else if (minVoltageReading >= 2700) {
          icon = SensorStatusMapComponent.blueMarkerIcon;
        } else if (minVoltageReading >= 2500) {
          icon = SensorStatusMapComponent.yellowMarkerIcon;
        } else {
          icon = SensorStatusMapComponent.redMarkerIcon;
        }

        return marker(latlng, { icon: icon })
      },
      filter: (feature) => {
        var lastVoltageReadings = feature.properties.sensors.map(x => x.LastVoltageReading);
        var minVoltageReading = !lastVoltageReadings.some(x => x !== null) ? null : Math.min(...lastVoltageReadings.filter(x => x != null));

        return (minVoltageReading == null && this.showNoVoltageData) ||  
          minVoltageReading != null && (
            (this.showLessThan2500 && minVoltageReading < 2500) ||
            (this.show2500To2700 && minVoltageReading >= 2500 && minVoltageReading < 2700) ||
            (this.show2700To4000 && minVoltageReading >= 2700 && minVoltageReading < 4000) ||
            (this.showGreaterThan4000 && minVoltageReading >= 4000)
          );
      }
    });
  }

  public setControl(): void {
    if (!this.layerControl) {
      this.layerControl = new Control.Layers(this.tileLayers, this.overlayLayers, { collapsed: true }).addTo(this.map);
    }
    if (this.sensorStatusLegend) {
      this.sensorStatusLegend.remove();
    }
  }

  private setLastMessageAgeFilterOptionsDropdownList() {
    this.filterOptionsDropdownList = [
      { group_name: "Select All", item_id: MapFilterOption.LESS_THAN_24_HOURS, item_text: "0-24 Hours" },
      { group_name: "Select All", item_id: MapFilterOption.GREATER_THAN_24_HOURS, item_text: ">24 Hours" },
      { group_name: "Select All", item_id: MapFilterOption.NO_MESSAGE_DATA, item_text: "No Data" },
      { group_name: "Select All", item_id: MapFilterOption.LESS_THAN_24_HOURS_WITH_SUPPORT_TICKET, item_text: "Has Active Support Ticket" }
    ];
  }

  private setVoltageFilterOptionsDropdownList() {
    this.filterOptionsDropdownList = [
      { group_name: "Select All", item_id: MapFilterOption.LESS_THAN_2500, item_text: "<2500 mV" },
      { group_name: "Select All", item_id: MapFilterOption.FROM_2500_TO_2700, item_text: "2500-2700 mV" },
      { group_name: "Select All", item_id: MapFilterOption.FROM_2700_TO_4000, item_text: "2700-4000 mV" },
      { group_name: "Select All", item_id: MapFilterOption.GREATER_THAN_4000, item_text: ">4000 mV" },
      { group_name: "Select All", item_id: MapFilterOption.NO_VOLTAGE_DATA, item_text: "No Data" }
    ];
  }

  public onMapFilterChange(filterByLastMessageAge: string) {
    this.filterMapByLastMessageAge = filterByLastMessageAge == 'true';

    this.clearLayer(this.selectedFeatureLayer);
    this.setControl();

    if (this.filterMapByLastMessageAge) {
      this.setLastMessageAgeFilterOptionsDropdownList();
      this.selectedFilterOptions = [MapFilterOption.LESS_THAN_24_HOURS, MapFilterOption.GREATER_THAN_24_HOURS, MapFilterOption.NO_MESSAGE_DATA, MapFilterOption.LESS_THAN_24_HOURS_WITH_SUPPORT_TICKET];

      this.showLessThan24Hours = true;
      this.showLessThan24HoursWithSupportTicket = true;
      this.showGreaterThan24Hours = true;
      this.showNoMessageData = true;

      this.showLessThan2500 = false;
      this.show2500To2700 = false;
      this.show2700To4000 = false;
      this.showGreaterThan4000 = false;
      this.showNoVoltageData = false;
    } else {
      this.setVoltageFilterOptionsDropdownList();
      this.selectedFilterOptions = [MapFilterOption.LESS_THAN_2500, MapFilterOption.FROM_2500_TO_2700, MapFilterOption.FROM_2700_TO_4000, MapFilterOption.GREATER_THAN_4000, MapFilterOption.NO_VOLTAGE_DATA];
  
      this.showLessThan24Hours = false;
      this.showLessThan24HoursWithSupportTicket = false;
      this.showGreaterThan24Hours = false;
      this.showNoMessageData = false;
  
      this.showLessThan2500 = true;
      this.show2500To2700 = true;
      this.show2700To4000 = true;
      this.showGreaterThan4000 = true;
      this.showNoVoltageData = true;
    }

    this.clearLayer(this.wellsLayer);

    this.wellsLayer = this.filterMapByLastMessageAge ? this.getWellLayerGeoJSONFilteredByLastMessageAge() : this.getWellLayerGeoJSONFilteredByLastVoltageReading();
    this.wellsLayer.addTo(this.map);

    this.wellsLayer.on("click", (event: LeafletEvent) => {
      this.selectFeature(event.propagatedFrom.feature);
      this.onWellSelected.emit(event.propagatedFrom.feature.properties.wellRegistrationID);
    });
  }

  public onSelectedFilterOptionsChange() {
    this.clearLayer(this.selectedFeatureLayer);
    
    if (this.filterMapByLastMessageAge) {
      this.updateLastMessageAgeMapFilter();
    } else {
      this.updateVoltageMapFilter();
    }

    this.wellsLayer.clearLayers();
    this.wellsLayer.addData(this.wellsGeoJson);
  }

  private updateLastMessageAgeMapFilter() {
    this.showLessThan24Hours = this.selectedFilterOptions.includes(MapFilterOption.LESS_THAN_24_HOURS)
    this.showLessThan24HoursWithSupportTicket = this.selectedFilterOptions.includes(MapFilterOption.LESS_THAN_24_HOURS_WITH_SUPPORT_TICKET)
    this.showGreaterThan24Hours = this.selectedFilterOptions.includes(MapFilterOption.GREATER_THAN_24_HOURS)
    this.showNoMessageData = this.selectedFilterOptions.includes(MapFilterOption.NO_MESSAGE_DATA)
  }

  private updateVoltageMapFilter() {
    this.showLessThan2500 = this.selectedFilterOptions.includes(MapFilterOption.LESS_THAN_2500);
    this.show2500To2700 = this.selectedFilterOptions.includes(MapFilterOption.FROM_2500_TO_2700);
    this.show2700To4000 = this.selectedFilterOptions.includes(MapFilterOption.FROM_2700_TO_4000);
    this.showGreaterThan4000 = this.selectedFilterOptions.includes(MapFilterOption.GREATER_THAN_4000);
    this.showNoVoltageData = this.selectedFilterOptions.includes(MapFilterOption.NO_VOLTAGE_DATA);
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
          popupEl.wellID = feature.properties.wellID;
          popupEl.wellRegistrationID = feature.properties.wellRegistrationID;
          popupEl.sensors = feature.properties.sensors;
          popupEl.AgHubRegisteredUser = feature.properties.AgHubRegisteredUser;
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

enum MapFilterOption {
  LESS_THAN_24_HOURS = 1,
  LESS_THAN_24_HOURS_WITH_SUPPORT_TICKET = 2,
  GREATER_THAN_24_HOURS = 3,
  NO_MESSAGE_DATA = 4,

  LESS_THAN_2500 = 5,
  FROM_2500_TO_2700 = 6,
  FROM_2700_TO_4000 = 7,
  GREATER_THAN_4000 = 8,
  NO_VOLTAGE_DATA = 9
}
