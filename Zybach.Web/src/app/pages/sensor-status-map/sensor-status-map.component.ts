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
  LeafletEvent,
  DomUtil
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

  showZeroToTwo: boolean = true;
  showTwoToEight: boolean = true;
  showEightPlus: boolean = true;
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

    this.setLastMessageAgeFilterOptionsDropdownList();
    this.selectedFilterOptions = [MapFilterOption.TWO_HOURS, MapFilterOption.EIGHT_HOURS, MapFilterOption.EIGHT_PLUS_HOURS, MapFilterOption.NO_MESSAGE_DATA];
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
        } else if (maxMessageAge <= 2) {
          icon = SensorStatusMapComponent.blueMarkerIcon;
        } else if (maxMessageAge <= 8) {
          icon = SensorStatusMapComponent.yellowMarkerIcon;
        } else {
          icon = SensorStatusMapComponent.redMarkerIcon;
        }

        return marker(latlng, { icon: icon })
      },
      filter: (feature) => {
        var sensorMessageAges = feature.properties.sensors.map(x => x.LastMessageAgeInHours);
        var maxMessageAge = !sensorMessageAges.some(x => x !== null) ? null : Math.max(...sensorMessageAges);

        return (this.showNoMessageData && maxMessageAge === null) ||
          (this.showZeroToTwo && maxMessageAge <= 2 && maxMessageAge != null) ||
          (this.showTwoToEight && 2 < maxMessageAge && maxMessageAge <= 8) ||
          (this.showEightPlus && maxMessageAge > 8 )
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
      this.layerControl = new Control.Layers(this.tileLayers, this.overlayLayers, { collapsed: false }).addTo(this.map);
    }
    if (this.sensorStatusLegend) {
      this.sensorStatusLegend.remove();
    }

    const legend = (displayLastMessageAgeLegend: boolean) => {
      const SensorStatusLegend = Control.extend({
        onAdd: function(map) {
          var legendElement = DomUtil.create("div", "legend-control");
          legendElement.style.borderRadius = "5px";
          legendElement.style.backgroundColor = "white";
          legendElement.style.cursor = "default";
          legendElement.style.padding = "6px";
  
          if (displayLastMessageAgeLegend) {
            legendElement.innerHTML += "<img style='height: 20px; width: 12px;  margin-right: 30px; display: inline-block' src='/assets/main/0b2c7a.png'/> 0-2 Hours<br/>"
            legendElement.innerHTML += "<img style='height: 20px; width: 12px;  margin-right: 30px; display: inline-block' src='/assets/main/fcf003.png'/> 2-8 Hours<br/>"
            legendElement.innerHTML += "<img style='height: 20px; width: 12px;  margin-right: 30px; display: inline-block' src='/assets/main/c2523c.png'/> >8 Hours<br/>"
            legendElement.innerHTML += "<img style='height: 20px; width: 12px;  margin-right: 30px; display: inline-block' src='/assets/main/noDataSourceMarker.png'/>No Data<br/>"
          } else {
            legendElement.innerHTML += "<img style='height: 20px; width: 12px;  margin-right: 30px; display: inline-block' src='/assets/main/flowMeterMarker.png'/> >4000 mV <br/>"
            legendElement.innerHTML += "<img style='height: 20px; width: 12px;  margin-right: 30px; display: inline-block' src='/assets/main/0b2c7a.png'/> 2700-4000 mV <br/>"
            legendElement.innerHTML += "<img style='height: 20px; width: 12px;  margin-right: 30px; display: inline-block' src='/assets/main/fcf003.png'/> 2500-2700 mV <br/>"
            legendElement.innerHTML += "<img style='height: 20px; width: 12px;  margin-right: 30px; display: inline-block' src='/assets/main/c2523c.png'/> <2500 mV <br/>"
            legendElement.innerHTML += "<img style='height: 20px; width: 12px;  margin-right: 30px; display: inline-block' src='/assets/main/noDataSourceMarker.png'/>No Data<br/>"
          }
  
          return legendElement;
        }
      });  

      return new SensorStatusLegend();
    };

    this.sensorStatusLegend = legend(this.filterMapByLastMessageAge);
    this.sensorStatusLegend.addTo(this.map);
  }

  private setLastMessageAgeFilterOptionsDropdownList() {
    this.filterOptionsDropdownList = [
      { group_name: "Select All", item_id: MapFilterOption.TWO_HOURS, item_text: "0-2 Hours" },
      { group_name: "Select All", item_id: MapFilterOption.EIGHT_HOURS, item_text: "2-8 Hours" },
      { group_name: "Select All", item_id: MapFilterOption.EIGHT_PLUS_HOURS, item_text: ">8 Hours" },
      { group_name: "Select All", item_id: MapFilterOption.NO_MESSAGE_DATA, item_text: "No Data" }
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
      this.selectedFilterOptions = [MapFilterOption.TWO_HOURS, MapFilterOption.EIGHT_HOURS, MapFilterOption.EIGHT_PLUS_HOURS, MapFilterOption.NO_MESSAGE_DATA];

      this.showZeroToTwo = true;
      this.showTwoToEight = true;
      this.showEightPlus = true;
      this.showNoMessageData = true;

      this.showLessThan2500 = false;
      this.show2500To2700 = false;
      this.show2700To4000 = false;
      this.showGreaterThan4000 = false;
      this.showNoVoltageData = false;
    } else {
      this.setVoltageFilterOptionsDropdownList();
      this.selectedFilterOptions = [MapFilterOption.LESS_THAN_2500, MapFilterOption.FROM_2500_TO_2700, MapFilterOption.FROM_2700_TO_4000, MapFilterOption.GREATER_THAN_4000, MapFilterOption.NO_VOLTAGE_DATA];
  
      this.showZeroToTwo = false;
      this.showTwoToEight = false;
      this.showEightPlus = false;
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
    this.showZeroToTwo = this.selectedFilterOptions.includes(MapFilterOption.TWO_HOURS)
    this.showTwoToEight = this.selectedFilterOptions.includes(MapFilterOption.EIGHT_HOURS)
    this.showEightPlus = this.selectedFilterOptions.includes(MapFilterOption.EIGHT_PLUS_HOURS)
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
  TWO_HOURS = 1,
  EIGHT_HOURS = 2,
  EIGHT_PLUS_HOURS = 3,
  NO_MESSAGE_DATA = 4,

  LESS_THAN_2500 = 5,
  FROM_2500_TO_2700 = 6,
  FROM_2700_TO_4000 = 7,
  GREATER_THAN_4000 = 8,
  NO_VOLTAGE_DATA = 9
}
