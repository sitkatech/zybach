import { AfterViewInit, ApplicationRef, Component, Input, OnInit, ViewEncapsulation } from '@angular/core';
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
  latLng
} from 'leaflet';
import 'leaflet.snogylop';
import 'leaflet.icon.glyph';
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

  public map: Map;
  public featureLayer: any;
  public layerControl: Control.Layers;
  public tileLayers: { [key: string]: any } = {};
  public overlayLayers: { [key: string]: any } = {};
  public boundingBox: BoundingBoxDto;
  public wellsObservable: Observable<any>;
  public watchUserChangeSubscription: any;
  public currentUser: UserDto;

  public tpnrdBoundaryLayer: GeoJSON<any>;
  public wellsLayer: GeoJSON<any>;

  public dataSourceDropdownList: { item_id: number, item_text: string }[] = [];
  public selectedDataSources: { item_id: number, item_text: string }[] = [];
  public dataSourceDropdownSettings: IDropdownSettings = {};

  public mapSearchQuery: string;
  public maxZoom: number = 17;

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
      { item_id: 1, item_text: DataSourceFilterOption.FLOW },
      { item_id: 2, item_text: DataSourceFilterOption.CONTINUITY },
      { item_id: 3, item_text: DataSourceFilterOption.ELECTRICAL },
      { item_id: 4, item_text: DataSourceFilterOption.NODATA }
    ];
    this.selectedDataSources = [
      { item_id: 1, item_text: "Flowmeter" },
      { item_id: 2, item_text: "Continuity Devices" },
      { item_id: 3, item_text: "Electrical Data" },
      { item_id: 4, item_text: "No Estimate Available" }
    ]
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

    const mapOptions: MapOptions = {
      maxZoom: this.maxZoom,
      layers: [
        this.tileLayers["Terrain"],
      ],
      fullscreenControl: true
    } as MapOptions;
    this.map = map(this.mapID, mapOptions);

    this.map.fitBounds([[this.boundingBox.Bottom, this.boundingBox.Left], [this.boundingBox.Top, this.boundingBox.Right]], this.defaultFitBoundsOptions);

    var wellIcon = new Icon({
      iconUrl: "/assets/main/noun_Well_190658.png",
      iconSize: [30, 30]
    });

    // var wellMarkerOptions = {
    //   radius: 4,
    //   fillColor: "#666",
    //   color: "#000",
    //   weight: 1,
    //   opacity: 1,
    //   fillOpacity: 0.4
    // };

    this.wellsLayer = new GeoJSON(this.wellsGeoJson, {
      pointToLayer: function (feature, latlng) {
        const markerIcon = icon.glyph({
          prefix: "fas",
          glyph: "tint",

        })
        return marker(latlng, { icon: markerIcon });
      },
      filter: (feature) => {
        const selectedDataSourceOptions = this.selectedDataSources.map(x => x.item_text);

        const allowedSensorTypes = selectedDataSourceOptions.map(x => DataSourceSensorTypeMap[x]);

        if (feature.properties.sensorTypes.some(st => allowedSensorTypes.includes(st))) {
          return true;
        }

        return false;
      }
    });

    this.wellsLayer.addTo(this.map);

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
        this.toastr.error("There was an error!", "You are sad :(", {timeOut: 60000});
      }

      const nominatimResult = x[0];

      if (this.nominatimResultWithinTpnrdBoundary(nominatimResult)) {
        this.map.setView(latLng(nominatimResult.lat, nominatimResult.lon), this.maxZoom)
      } else {
        this.toastr.error("There was an error!", "You are sad :(", {timeOut: 60000});
      }
    })
  }

  nominatimResultWithinTpnrdBoundary(nominatimResult: { lat: number, lon: number }): boolean {
    const turfPoint = point([nominatimResult.lon, nominatimResult.lat]);
    const turfPolygon = polygon(TwinPlatteBoundaryGeoJson.features[0].geometry.coordinates);
    return booleanWithin(turfPoint, turfPolygon);
  }

}

enum DataSourceFilterOption {
  FLOW = "Flowmeter",
  CONTINUITY = "Continuity Devices",
  ELECTRICAL = "Electrical Data",
  NODATA = "No Estimate Available"
}

const DataSourceSensorTypeMap = {
  "Flowmeter": "FlowMeter",
  "Continuity Devices": "PumpMonitor",
  "Electrical Data": "N/A",
  "No Estimate Available": "N/A"
}
