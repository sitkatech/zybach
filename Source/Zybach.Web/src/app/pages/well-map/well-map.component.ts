import { AfterViewInit, ApplicationRef, Component, Input, OnInit } from '@angular/core';
import {
  Control, FitBoundsOptions,
  GeoJSON,
  marker,
  map,
  Map,
  MapOptions,
  tileLayer,
  Icon,
  geoJSON} from 'leaflet';
  import 'leaflet.snogylop'
import { Observable } from 'rxjs';
import { BoundingBoxDto } from 'src/app/shared/models/bounding-box-dto';
import { UserDto } from 'src/app/shared/models/generated/user-dto';
import { CustomCompileService } from 'src/app/shared/services/custom-compile.service';
import { TwinPlatteBoundaryGeoJson } from '../well-explorer/tpnrd-boundary';
@Component({
  selector: 'zybach-well-map',
  templateUrl: './well-map.component.html',
  styleUrls: ['./well-map.component.scss']
})
export class WellMapComponent implements OnInit, AfterViewInit {

  public mapID: string = 'wellMap';
  public onEachFeatureCallback?: (feature, layer) => void;
  public zoomMapToDefaultExtent: boolean = true;
  public disableDefaultClick: boolean = false;
  public mapHeight: string = '500px';
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

  constructor(
    private appRef: ApplicationRef,
    private compileService: CustomCompileService  ) { }


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

  public ngAfterViewInit(): void {

    const mapOptions: MapOptions = {
      maxZoom: 17,
      layers: [
        this.tileLayers["Terrain"],
      ],
      fullscreenControl: true
    } as MapOptions;
    this.map = map(this.mapID, mapOptions);

    this.map.fitBounds([[this.boundingBox.Bottom, this.boundingBox.Left], [this.boundingBox.Top, this.boundingBox.Right]], this.defaultFitBoundsOptions);
    
    console.log(this.wellsGeoJson);


    var wellIcon = new Icon({
      iconUrl: "/assets/main/noun_Well_190658.png",
      iconSize: [30,30]
    });

    // var wellMarkerOptions = {
    //   radius: 4,
    //   fillColor: "#666",
    //   color: "#000",
    //   weight: 1,
    //   opacity: 1,
    //   fillOpacity: 0.4
    // };

    // todo: rename, make into class property
    this.wellsLayer = new GeoJSON(this.wellsGeoJson, {
      pointToLayer: function (feature, latlng) {
        // if well is in the list from geooptix, symbolize more prominently
        
        return marker(latlng, {icon: wellIcon});
      }
    });

    this.wellsLayer.addTo(this.map);

    this.tpnrdBoundaryLayer  = geoJSON(TwinPlatteBoundaryGeoJson as any, {
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

    this.overlayLayers ={
      "Wells" : this.wellsLayer,
      "District Boundary": this.tpnrdBoundaryLayer
    };

    this.setControl();

  }


  public setControl(): void {
    this.layerControl = new Control.Layers(this.tileLayers, this.overlayLayers, { collapsed: false })
      .addTo(this.map);
  }

}
