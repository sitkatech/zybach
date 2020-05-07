import { Component, OnInit, ChangeDetectorRef, EventEmitter } from '@angular/core';
import { WellService } from 'src/app/services/well.service';
import { ActivatedRoute } from '@angular/router';
import { SiteDto } from 'src/app/shared/models/geooptix/site-dto';
import * as L from 'leaflet';
import { GestureHandling } from "leaflet-gesture-handling";
import { forkJoin } from 'rxjs';
import { ArcService, remapWellFeaturePropertiesFromArc } from 'src/app/services/arc.service';
import { Feature } from 'geojson';

@Component({
  selector: 'zybach-well-detail',
  templateUrl: './well-detail.component.html',
  styleUrls: ['./well-detail.component.scss']
})
export class WellDetailComponent implements OnInit {
  canonicalName: string;

  public well: SiteDto;
  public wellFromArc: Feature;

  tileLayers: { [key: string]: any } = {};
  mapID: string = "wellMap";
  map: L.Map;
  wellLayer: any;
  defaultMapZoom: number = 9;
  layerControl: L.Control.Layers;
  afterSetControl = new EventEmitter();
  wellPropertiesFromArc: any;

  constructor(private wellService: WellService,
    private arcService: ArcService,
    private route: ActivatedRoute,
    private cdr: ChangeDetectorRef) { }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.canonicalName = params["canonicalName"];
      this.makeTileLayers();
      this.getWellDetails();
    });
  }

  getWellDetails() {
    forkJoin(
      this.wellService.getSite(this.canonicalName),
      this.arcService.getWellFromArcByRegCD(this.canonicalName)
    ).subscribe(([wellFromGeoOptix, wellFromArc]) => {
      this.well = wellFromGeoOptix;
      this.wellFromArc = wellFromArc
      this.wellPropertiesFromArc = remapWellFeaturePropertiesFromArc(wellFromArc);

      this.cdr.detectChanges();

      this.initMap();
    });
  }

  public trs() {
    return `${this.wellPropertiesFromArc.Township} ${this.wellPropertiesFromArc.Range}${this.wellPropertiesFromArc.RangeDir} ${this.wellPropertiesFromArc.Section_}`;
  }
  public owner() {
    return `${this.wellPropertiesFromArc.FirstName} ${this.wellPropertiesFromArc.LastName}`;
  }

  private initMap() {
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

  showWellLocationOnMap() {


    var meteredWellMarkerOptions = {
      radius: 8,
      fillColor: "#0076c0",
      color: "#000",
      weight: 1,
      opacity: 1,
      fillOpacity: 0.8

    }
    this.wellLayer = L.geoJSON(this.wellFromArc, {
      pointToLayer: function (feature, latlng) {
        // if well is in the list from geooptix, symbolize more prominently
        return L.circleMarker(latlng, meteredWellMarkerOptions);
      }
    });
    this.wellLayer.addTo(this.map);
  }

  private setControl(): void {
    this.layerControl = new L.Control.Layers(this.tileLayers, [])
      .addTo(this.map);
    this.map.zoomControl.setPosition('topright');

    this.afterSetControl.emit(this.layerControl);
  }

  private defaultFitBounds(): void {
    let target = this.map._getBoundsCenterZoom(this.wellLayer.getBounds(), null);
    this.map.setView(target.center, this.defaultMapZoom, null);
  }

  private makeTileLayers(): void {
    this.tileLayers = {
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
  }
}
