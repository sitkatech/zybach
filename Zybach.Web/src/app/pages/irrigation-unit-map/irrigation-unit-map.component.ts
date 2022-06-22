import { AfterViewInit, Component, ElementRef, Input, OnInit, ViewChild } from '@angular/core';
import * as L from 'leaflet';
import 'leaflet.icon.glyph';
import 'leaflet.fullscreen';
import { GestureHandling } from 'leaflet-gesture-handling';
import { DefaultBoundingBox } from 'src/app/shared/models/default-bounding-box';
import { WellMinimalDto } from 'src/app/shared/generated/model/well-minimal-dto';

@Component({
  selector: 'zybach-irrigation-unit-map',
  templateUrl: './irrigation-unit-map.component.html',
  styleUrls: ['./irrigation-unit-map.component.scss']
})
export class IrrigationUnitMapComponent implements OnInit, AfterViewInit {
  @ViewChild('mapContainer') mapContainer: ElementRef;  

  public map: L.Map;
  private maxZoom: number = 17;
  private boundingBox: any;
  public tileLayers: any;

  @Input()
  public irrigationUnit: any;

  constructor() { }

  ngOnInit(): void {
  }

   
  // Begin section: location map

  public ngAfterViewInit(): void {
    this.initMapConstants();
    this.initializeMap();
  }

  public initMapConstants() {
    this.boundingBox = DefaultBoundingBox;

    this.tileLayers = Object.assign({}, {
      "Aerial": L.tileLayer('https://services.arcgisonline.com/ArcGIS/rest/services/World_Imagery/MapServer/tile/{z}/{y}/{x}', {
        attribution: 'Aerial',
      }),
      "Street": L.tileLayer('https://services.arcgisonline.com/ArcGIS/rest/services/World_Street_Map/MapServer/tile/{z}/{y}/{x}', {
        attribution: 'Aerial',
      }),
      "Terrain": L.tileLayer('https://server.arcgisonline.com/ArcGIS/rest/services/World_Topo_Map/MapServer/tile/{z}/{y}/{x}', {
        attribution: 'Terrain',
      }),
    }, this.tileLayers);
  }
  
  public initializeMap() {
    const mapOptions: L.MapOptions = {
      maxZoom: this.maxZoom,
      layers: [
        this.tileLayers["Aerial"],
      ],
      gestureHandling: true,
      fullscreenControl: true
    } as L.MapOptions;

    this.map = L.map(this.mapContainer.nativeElement, mapOptions);
    L.Map.addInitHook("addHandler", "gestureHandling", GestureHandling);

    this.map.fitBounds([[this.boundingBox.Bottom, this.boundingBox.Left], [this.boundingBox.Top, this.boundingBox.Right]], null);

    if (this.irrigationUnit.IrrigationUnitGeoJSON != null && this.irrigationUnit.IrrigationUnitGeoJSON != undefined && this.irrigationUnit.IrrigationUnitGeoJSON != "null") {
      this.addIrrigationUnitToMap();
    }

    this.irrigationUnit.AssociatedWells.forEach(x => this.addWellToMap(x));

  }

  public addIrrigationUnitToMap() : void {    
    const irrigationUnitGeoJSON = L.geoJSON(JSON.parse(this.irrigationUnit.IrrigationUnitGeoJSON)).addTo(this.map);
    const unitBoundingBox = irrigationUnitGeoJSON.getBounds();
    let target = (this.map as any)._getBoundsCenterZoom(unitBoundingBox, null);
    this.map.setView(target.center, 15, null);
    this.map.getBoundsZoom(unitBoundingBox, true);
    this.map.fitBounds(unitBoundingBox);
  }

  public addWellToMap(well: WellMinimalDto) {
    const sensorTypes = well.Sensors.map(x => x.SensorTypeName);
    let mapIcon;

    if (sensorTypes.includes("Flow Meter")) {
      mapIcon = L.icon.glyph({
        prefix: "fas",
        glyph: "tint",
        iconUrl: "/assets/main/flowMeterMarker.png"
      });
    } else if (sensorTypes.includes("Continuity Meter")) {
      mapIcon = L.icon.glyph({
        prefix: "fas",
        glyph: "tint",
        iconUrl: "/assets/main/continuityMeterMarker.png"
      });
    } else if (sensorTypes.includes("Electrical Usage")) {
      mapIcon = L.icon.glyph({
        prefix: "fas",
        glyph: "tint",
        iconUrl: "/assets/main/electricalDataMarker.png"
      });
    } else {
      mapIcon = L.icon.glyph({
        prefix: "fas",
        glyph: "tint",
        iconUrl: "/assets/main/noDataSourceMarker.png"
      });
    }
    const wellLayer = new L.GeoJSON(well.Location, {
      pointToLayer: function (feature, latlng) {
        return L.marker(latlng, {icon: mapIcon});
      }
    });
    wellLayer.addTo(this.map);

    if (this.irrigationUnit.IrrigationUnitGeoJSON == null || this.irrigationUnit.IrrigationUnitGeoJSON == undefined || this.irrigationUnit.IrrigationUnitGeoJSON == "null") {
      let target = (this.map as any)._getBoundsCenterZoom(wellLayer.getBounds(), null);
      this.map.setView(target.center, 16, null);
    }
  }

  // End section: location map

}
