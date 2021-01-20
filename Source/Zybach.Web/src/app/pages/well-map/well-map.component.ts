import { ApplicationRef, Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import {
  Control, FitBoundsOptions,
  GeoJSON,
  GeoJSONOptions,
  LatLng, LeafletEvent,
  LeafletMouseEvent,
  map,
  Map,
  MapOptions,
  Popup,
  tileLayer,
  WMSOptions,
  DomEvent,
  DomUtil,
  LayerGroup
} from 'leaflet';
import { BoundingBoxDto } from 'src/app/shared/models/bounding-box-dto';
import { CustomCompileService } from 'src/app/shared/services/custom-compile.service';
@Component({
  selector: 'zybach-well-map',
  templateUrl: './well-map.component.html',
  styleUrls: ['./well-map.component.scss']
})
export class WellMapComponent implements OnInit {

  public mapID: string = 'wellMap';
  public onEachFeatureCallback?: (feature, layer) => void;
  public zoomMapToDefaultExtent: boolean = true;
  public disableDefaultClick: boolean = false;
  public mapHeight: string = '300px';
  public defaultFitBoundsOptions?: FitBoundsOptions = null;

  public component: any;

  public map: Map;
  public featureLayer: any;
  public layerControl: Control.Layers;
  public tileLayers: { [key: string]: any } = {};
  public overlayLayers: { [key: string]: any } = {};
  public boundingBox: BoundingBoxDto;

  constructor(
    private appRef: ApplicationRef,
    private compileService: CustomCompileService
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
  }

}
