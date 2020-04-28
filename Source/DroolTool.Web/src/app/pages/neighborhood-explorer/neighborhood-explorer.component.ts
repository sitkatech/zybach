import { AfterViewInit, ApplicationRef, ChangeDetectionStrategy, Component, EventEmitter, OnInit} from '@angular/core';
import { environment } from "src/environments/environment";
import * as L from 'leaflet';
import '../../../../node_modules/leaflet.snogylop/src/leaflet.snogylop.js';
import '../../../../node_modules/leaflet.fullscreen/Control.FullScreen.js';
import * as esri from 'esri-leaflet'
import { BoundingBoxDto } from '../../shared/models/bounding-box-dto';
import { CustomCompileService } from '../../shared/services/custom-compile.service';
import { NeighborhoodExplorerService } from 'src/app/services/neighborhood-explorer/neighborhood-explorer.service'

@Component({
  selector: 'drooltool-neighborhood-explorer',
  templateUrl: './neighborhood-explorer.component.html',
  styleUrls: ['./neighborhood-explorer.component.scss']
})
export class NeighborhoodExplorerComponent implements OnInit {

  public zoomMapToDefaultExtent = true;
    public defaultFitBoundsOptions?: L.FitBoundsOptions = null;
    public afterSetControl = new EventEmitter();
    public afterLoadMap = new EventEmitter();
    public onMapMoveEnd = new EventEmitter();

    public component: any;

    public mapID = "NeighborhoodExplorerMap";
    public mapHeight = window.innerHeight + "px";
    public map: L.Map;
    public featureLayer: any;
    public layerControl: L.Control.Layers;
    public tileLayers: { [key: string]: any } = {};
    public overlayLayers: { [key: string]: any } = {};
    public maskLayer: any;
    
    boundingBox: BoundingBoxDto;

    constructor(
        private appRef: ApplicationRef,
        private compileService: CustomCompileService,
        private neighborhoodExplorerService : NeighborhoodExplorerService
    ) {
    }

    public ngOnInit(): void {

        // Default bounding box
        this.boundingBox = new BoundingBoxDto();
        this.boundingBox.Left = -117.36883651141554;
        this.boundingBox.Bottom = 33.45695062823788;
        this.boundingBox.Right = -117.82471349197476;
        this.boundingBox.Top = 33.71689407051289;

        this.tileLayers = Object.assign({}, {
            "Aerial": L.tileLayer('https://services.arcgisonline.com/ArcGIS/rest/services/World_Imagery/MapServer/tile/{z}/{y}/{x}', {
                attribution: 'Aerial',
                maxNativeZoom: 16,
                maxZoom: 22
            }),
            "Street": L.tileLayer('https://services.arcgisonline.com/ArcGIS/rest/services/World_Street_Map/MapServer/tile/{z}/{y}/{x}', {
                attribution: 'Street',
                maxNativeZoom: 16,
                maxZoom: 22
            }),
            "Terrain": L.tileLayer('https://server.arcgisonline.com/ArcGIS/rest/services/World_Topo_Map/MapServer/tile/{z}/{y}/{x}', {
                attribution: 'Terrain',
                maxNativeZoom: 16,
                maxZoom: 22
            }),
            "Hillshade": L.tileLayer('https://wtb.maptiles.arcgis.com/arcgis/rest/services/World_Topo_Base/MapServer/tile/{z}/{y}/{x}', {
                attribution: 'Hillshade',
                maxNativeZoom: 15,
                maxZoom: 22
            })
        }, this.tileLayers);

        let neighborhoodsWMSOptions = ({
          layers: "DroolTool:Neighborhoods",
          transparent: true,
          format: "image/png",
          tiled: true
        } as L.WMSOptions);

        let backboneWMSOptions = ({
          layers: "DroolTool:Backbone",
          transparent: true,
          format: "image/png",
          tiled: true
        } as L.WMSOptions);

        let watershedsWMSOptions = ({
          layers: "DroolTool:Watersheds",
          transparent: true,
          format: "image/png",
          tiled: true
        } as L.WMSOptions);



        this.overlayLayers = Object.assign({}, {
          "<span><img src='../../assets/neighborhood-explorer/neighborhood.png' height='12px' style='margin-bottom:3px;' /> Neighborhoods</span>": L.tileLayer.wms(environment.geoserverMapServiceUrl + "/wms?", neighborhoodsWMSOptions),
          "<span><img src='../../assets/neighborhood-explorer/backbone.png' height='12px' style='margin-bottom:3px;' /> Streams</span>": L.tileLayer.wms(environment.geoserverMapServiceUrl + "/wms?", backboneWMSOptions),
          "<span><img src='../../assets/neighborhood-explorer/backbone.png' height='12px' style='margin-bottom:3px;' /> Watersheds</span>": L.tileLayer.wms(environment.geoserverMapServiceUrl + "/wms?", watershedsWMSOptions),
          "<span>Stormwater Network <br/> <img src='../../assets/neighborhood-explorer/stormwaterNetwork.png' height='50'/> </span>": esri.dynamicMapLayer({url:"https://ocgis.com/arcpub/rest/services/Flood/Stormwater_Network/MapServer/"})
        })

        this.compileService.configure(this.appRef);
    }

    public ngAfterViewInit(): void {

      this.neighborhoodExplorerService.getMask().subscribe(maskString => {
        this.maskLayer = L.geoJSON(maskString, {
                                        invert: true,
                                        style: function (feature) {
                                            return {
                                                fillColor: "#323232",
                                                fill: true,
                                                fillOpacity: 0.4,
                                                color: "#3388ff",
                                                weight: 5,
                                                stroke: true
                                            };
                                        }
                                    });

          const mapOptions: L.MapOptions = {
            minZoom: 6,
            maxZoom: 22,
            layers: [
                this.tileLayers["Hillshade"],
                this.overlayLayers["<span><img src='../../assets/neighborhood-explorer/backbone.png' height='12px' style='margin-bottom:3px;' /> Streams</span>"],
                this.overlayLayers["<span><img src='../../assets/neighborhood-explorer/backbone.png' height='12px' style='margin-bottom:3px;' /> Watersheds</span>"]
            ]
            
        } as L.MapOptions;

        this.map = L.map(this.mapID, mapOptions);

        this.map.on('load', (event: L.LeafletEvent) => {
            this.afterLoadMap.emit(event);
        });
        this.map.on("moveend", (event: L.LeafletEvent) => {
            this.onMapMoveEnd.emit(event);
        });
        this.map.fitBounds(this.maskLayer.getBounds(), this.defaultFitBoundsOptions);
        this.map.setZoom(12);
        
        this.setControl();
        this.maskLayer.addTo(this.map);

        let el = document.getElementById('NeighborhoodExplorerMap');
        el.scrollIntoView();
      });              
    }

    public setControl(): void {
        this.layerControl = new L.Control.Layers(this.tileLayers, this.overlayLayers)
            .addTo(this.map);
        this.map.zoomControl.setPosition('topright');
        L.control.fullscreen({
          position: 'topright',
          title: 'View Fullscreen', // change the title of the button, default Full Screen
          titleCancel: 'Exit fullscreen mode', // change the title of the button when fullscreen is on, default Exit Full Screen
          content: null, // change the content of the button, can be HTML, default null
          forceSeparateButton: true, // force seperate button to detach from zoom buttons, default false
          forcePseudoFullscreen: true, // force use of pseudo full screen even if full screen API is available, default false
          fullscreenElement: false // Dom element to render in full screen, false by default, fallback to map._container
        }).addTo(this.map);
        this.afterSetControl.emit(this.layerControl);
    }
}
