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
  wellCanonicalName: string;

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
  timeSeriesData: any[];
  noTimeSeriesDataMessage: string = "Loading...";
  sensorCanonicalName: any;
  showTimeSeriesExplorer: boolean = false;
  folderCanonicalName: any;
  installationCanonicalName: any;
  installationRecord: any;
  installationRecordSimple: any;

  constructor(private wellService: WellService,
    private arcService: ArcService,
    private route: ActivatedRoute,
    private cdr: ChangeDetectorRef) { }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.wellCanonicalName = params["canonicalName"];
      this.makeTileLayers();
      this.getWellDetails();
      this.getTimeSeriesData();
      this.getInstallationDetails();
    });
  }

  getInstallationDetails() {
    this.wellService.getInstallationName(this.wellCanonicalName).subscribe(installations =>{
      if (installations.length !=0){
        this.installationCanonicalName = installations[0].CanonicalName;
        
        this.wellService.getInstallation(this.wellCanonicalName, this.installationCanonicalName).subscribe(installation =>{
          const installationRecord = installation[0].MethodInstance.RecordSets[0].Records[0].Fields;

          this.installationRecordSimple = {
            affiliation:installationRecord["installer-affiliation"][0].toUpperCase(),
            initials: installationRecord["installer-initials"],
            date: installationRecord["install-date"],
            lon: installationRecord["gps-location"].geometry.coordinates[0],
            lat: installationRecord["gps-location"].geometry.coordinates[1],
          }
        })
      }
    })
  }

  getTimeSeriesData() {
    this.wellService.getSensorName(this.wellCanonicalName).subscribe(sensor => {
      if (sensor.length != 0) {
        this.sensorCanonicalName = sensor[0].CanonicalName;

        this.wellService.getSensorFolder(this.wellCanonicalName, this.sensorCanonicalName).subscribe(folder => {
          if (folder.length != 0) {
            this.folderCanonicalName = folder[0].CanonicalName;

            this.wellService.getFiles(this.wellCanonicalName, this.sensorCanonicalName, this.folderCanonicalName).subscribe(files => {
              if (files.length != 0) {
                this.wellService.getTimeSeriesData(this.wellCanonicalName, this.sensorCanonicalName, this.folderCanonicalName).subscribe(response=>{
                  this.timeSeriesData = response.data                
                });
              } else {
                this.noTimeSeriesDataMessage = "No time series data was found in GeoOptix for this well."
              }
            });
          }
          else {
            this.noTimeSeriesDataMessage = "No time series data was found in GeoOptix for this well."
          }
        });
      } else {
        this.noTimeSeriesDataMessage = "No sensor was found in GeoOptix for this well.";
      }
    });
  }

  public downloadTimeSeriesCsv(): void{
    this.wellService.downloadTimeSeriesCsv(this.wellCanonicalName, this.sensorCanonicalName, this.folderCanonicalName).subscribe(x=>{
      this.downloadFile(x);
    })
  }

  downloadFile(data: any) {
    const blob = new Blob([data], { type: 'text/csv' });
    const url= window.URL.createObjectURL(blob);
    // this is super gross, but it's the only way to set a filename for a download
    var anchor = document.createElement("a");
    anchor.download = `${this.wellCanonicalName}_well_sensor_data.csv`
    anchor.href = url;
    anchor.click();
  }

  openTimeSeriesExplorer() {
    this.showTimeSeriesExplorer = true;
    
    this.cdr.detectChanges();
    this.cdr.markForCheck();
  }

  sensorInGeoOptixUrl():string {
    return `https://tpnrd.qa.geooptix.com/program/main/(inner:station)?projectCName=water-data-program&stationCName=${this.sensorCanonicalName}`;
  }

  wellInGeoOptixUrl() : string {
    return `https://tpnrd.qa.geooptix.com/program/main/(inner:site)?projectCName=water-data-program&siteCName=${this.wellCanonicalName}`;
  }

  getWellDetails() {
    forkJoin(
      this.wellService.getSite(this.wellCanonicalName),
      this.arcService.getWellFromArcByRegCD(this.wellCanonicalName)
    ).subscribe(([wellFromGeoOptix, wellFromArc]) => {
      this.well = wellFromGeoOptix;
      this.wellFromArc = wellFromArc
      
      if (wellFromArc) {
        this.wellPropertiesFromArc = remapWellFeaturePropertiesFromArc(wellFromArc);
      }

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
  public timeSeriesTitle():string {
    return `${this.wellCanonicalName} - Sensor Data`;
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

    const wellFeatureForMap = this.wellFromArc || this.well.Location;

    this.wellLayer = L.geoJSON(wellFeatureForMap, {
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
