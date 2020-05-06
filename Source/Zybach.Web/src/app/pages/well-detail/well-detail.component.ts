import { Component, OnInit, ChangeDetectorRef, EventEmitter } from '@angular/core';
import { WellService } from 'src/app/services/well.service';
import { ActivatedRoute } from '@angular/router';
import { SiteDto } from 'src/app/shared/models/geooptix/site-dto';
import * as L from 'leaflet';
import { GestureHandling } from "leaflet-gesture-handling";
import * as esri from 'esri-leaflet'
import { AlertService } from 'src/app/shared/services/alert.service';
import { Alert } from 'src/app/shared/models/alert';
import { AlertContext } from 'src/app/shared/models/enums/alert-context.enum';

@Component({
  selector: 'zybach-well-detail',
  templateUrl: './well-detail.component.html',
  styleUrls: ['./well-detail.component.scss']
})
export class WellDetailComponent implements OnInit {
  canonicalName: string;
  well: SiteDto;
  tileLayers: { [key: string]: any } = {};
  mapID: string = "wellMap";
  map: L.Map;
  wellLayer: any;
  defaultMapZoom: number = 9;
  public layerControl: L.Control.Layers;
  public afterSetControl = new EventEmitter();

  constructor(private wellService: WellService,
    private alertService: AlertService,
    private route: ActivatedRoute,
    private cdr: ChangeDetectorRef) { }

  ngOnInit(): void {
    this.route.params.subscribe(params =>{
      this.canonicalName = params["canonicalName"];
      this.getWellDetails();
    });    
  }

  getWellDetails() {
    this.wellService.getSite(this.canonicalName).subscribe(well => {
      this.well = well;

      this.cdr.detectChanges();
      
      this.initMap();
    }, error=>{
      this.alertService.pushAlert(new Alert(`No data found in GeoOptix for Well ${this.canonicalName}`, AlertContext.Danger, true));
      this.cdr.detectChanges();
    })
  }

  initMap() {
    this.tileLayers ={
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
    };

    L.Map.addInitHook("addHandler", "gestureHandling", GestureHandling);

      const mapOptions: L.MapOptions = {
        minZoom: 6,
        maxZoom: 22,
        layers: [
          this.tileLayers["Street"]
        ],
        gestureHandling: true

      } as L.MapOptions;

      this.map = L.map(this.mapID, mapOptions);
      this.setControl();
      this.showWellLocationOnMap();
      this.defaultFitBounds();
  }

  public setControl(): void {
    this.layerControl = new L.Control.Layers(this.tileLayers, [])
      .addTo(this.map);
    this.map.zoomControl.setPosition('topright');

    this.afterSetControl.emit(this.layerControl);
  }

  showWellLocationOnMap() {
    this.wellLayer = L.geoJSON(this.well.Location);
    this.wellLayer.addTo(this.map);
  }

  public defaultFitBounds(): void {
    let target = this.map._getBoundsCenterZoom(this.wellLayer.getBounds(), null);
    this.map.setView(target.center, this.defaultMapZoom, null);
  }

}
