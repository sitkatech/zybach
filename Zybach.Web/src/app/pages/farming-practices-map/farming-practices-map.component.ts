import { AfterViewInit, Component, EventEmitter, Input, Output } from '@angular/core';
import { Feature, FeatureCollection } from 'geojson';
import * as L from 'leaflet';
import GestureHandling from 'leaflet-gesture-handling';
import { LeafletHelperService } from 'src/app/shared/services/leaflet-helper.service';
import { WfsService } from 'src/app/shared/services/wfs.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'farming-practices-map',
  templateUrl: './farming-practices-map.component.html',
  styleUrls: ['./farming-practices-map.component.scss']
})
export class FarmingPracticesMapComponent implements AfterViewInit {
  @Input() set selectedYear (value)  {
    this._selectedYear = value;
    this.updateAgHubIrrigationUnitCropTypeMapLayer();
  };
  public _selectedYear: number;

  @Input() set selectedIrrigationUnitID (value: number) {
    if (this._selectedYear && this._selectedIrrigationUnitID != value) {
      this._selectedIrrigationUnitID = value;
      this.updateSelectedIrrigationUnit();
    }
  }
  public _selectedIrrigationUnitID;

  @Input() set summaryStatsField (value: "CropType" | "TillageType") {
    this._summaryStatsField = value;
    this.updateAgHubIrrigationUnitCropTypeMapLayer();
  }
  public _summaryStatsField;

  @Output() mapSelectionChanged = new EventEmitter<number>();
  
  public map: L.Map;
  public mapID: string = crypto.randomUUID();
  public tileLayers: { [key: string]: any } = LeafletHelperService.GetDefaultTileLayers();

  private layerControl: L.Control.Layers;
  private irrigationUnitCropTypeLayer: L.Layer;
  private defaultIrrigationUnitWMSOptions: L.WMSOptions = {
    layers: "Zybach:AgHubIrrigationUnits",
    transparent: true,
    format: "image/png",
    tiled: true,
  } as L.WMSOptions;

  private selectedIrrigationUnitLayer: L.GeoJSON;
  private highlightStyle = {
    "color": "#fcfc12",
    "weight": 3,
    "opacity": 0.65,
    "fillOpacity": 0
  };

  constructor(
    private leafletHelperService: LeafletHelperService,
    private wfsService: WfsService
  ) {}

  ngAfterViewInit(): void {
    const mapOptions: L.MapOptions = {
      minZoom: 6,
      maxZoom: 17,
      layers: [
        this.tileLayers["Aerial"],
      ],
      fullscreenControl: true,
      gestureHandling: true
    } as L.MapOptions;

    this.map = L.map(this.mapID, mapOptions);
    L.Map.addInitHook("addHandler", "gestureHandling", GestureHandling);
    this.layerControl = new L.Control.Layers(this.tileLayers, null, { collapsed: false}).addTo(this.map);
    this.leafletHelperService.fitMapToDefaultBoundingBox(this.map);

    this.updateAgHubIrrigationUnitCropTypeMapLayer();
    this.registerMapClickEvents();
  }

  public updateAgHubIrrigationUnitCropTypeMapLayer() {
    if (!this.map) return;

    if (this.irrigationUnitCropTypeLayer) {
      this.map.removeLayer(this.irrigationUnitCropTypeLayer);
      this.layerControl.removeLayer(this.irrigationUnitCropTypeLayer);
    }

    var cql_filter = `IrrigationYear = ${this._selectedYear}`;
    if (this._selectedIrrigationUnitID) {
      cql_filter += `and AgHubIrrigationUnitID not in (${this._selectedIrrigationUnitID})`;
    }

    var wmsParams = Object.assign({ cql_filter: cql_filter, styles: this.getWMSLayerStyle() }, this.defaultIrrigationUnitWMSOptions, null);
    this.irrigationUnitCropTypeLayer = L.tileLayer.wms(environment.geoserverMapServiceUrl + "/wms?", wmsParams);

    this.irrigationUnitCropTypeLayer.addTo(this.map);
    this.layerControl.addOverlay(this.irrigationUnitCropTypeLayer, "AgHub Irrigation Units");
  }

  private getWMSLayerStyle(): string {
    return this._summaryStatsField == "CropType" ? "AgHubIrrigationUnitCropType" : "AgHubIrrigationUnitTillageType";
  }

  private registerMapClickEvents() {
    this.map.on("click", (event: L.LeafletMouseEvent): void => {
      this.wfsService.getAgHubIrrigationUnitByCoordinateAndIrrigationYear(event.latlng.lng, event.latlng.lat, this._selectedYear)
      .subscribe((featureCollection: FeatureCollection) => {
        this.addSelectedIrrigationUnitMapLayer(featureCollection);
      });
    });
  }

  private updateSelectedIrrigationUnit() {
    this.wfsService.getAgHubIrrigationUnitByIDAndIrrigationYear(this._selectedIrrigationUnitID, this._selectedYear)
    .subscribe((featureCollection: FeatureCollection) => {
      this.addSelectedIrrigationUnitMapLayer(featureCollection, true);
    });
  }

  private addSelectedIrrigationUnitMapLayer(featureCollection: FeatureCollection, fromGridSelection: boolean = false) {
    if (!featureCollection.features || featureCollection.features.length == 0) return;
    let feature: Feature = featureCollection.features[0];
    if (!fromGridSelection && this._selectedIrrigationUnitID == feature.properties.AgHubIrrigationUnitID) return;

    if (this.selectedIrrigationUnitLayer) {
      this.selectedIrrigationUnitLayer.removeFrom(this.map);
    }
    
    this._selectedIrrigationUnitID = feature.properties.AgHubIrrigationUnitID;
    this.mapSelectionChanged.emit(this._selectedIrrigationUnitID);

    this.selectedIrrigationUnitLayer = L.geoJSON(feature, { style: this.highlightStyle });
    this.map.fitBounds(this.selectedIrrigationUnitLayer.getBounds(), { maxZoom: 14 });

    this.selectedIrrigationUnitLayer.addTo(this.map);
  }
}
