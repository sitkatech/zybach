import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit } from '@angular/core';
import {
  map,
  Map as LeafletMap,
  MapOptions,
  tileLayer,
  icon,
  geoJSON
} from 'leaflet';
import 'leaflet.icon.glyph';
import 'leaflet.fullscreen';
import { GestureHandling } from 'leaflet-gesture-handling';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { WellService } from 'src/app/services/well.service';
import { Alert } from 'src/app/shared/models/alert';
import { AlertContext } from 'src/app/shared/models/enums/alert-context.enum';
import { AlertService } from 'src/app/shared/services/alert.service';
import L from 'leaflet';
import { TwinPlatteBoundaryGeoJson } from 'src/app/shared/models/tpnrd-boundary';
import { UserDto } from 'src/app/shared/generated/model/user-dto';
import { WellNewDto } from 'src/app/shared/generated/model/well-new-dto';

@Component({
  selector: 'zybach-well-new',
  templateUrl: './well-new.component.html',
  styleUrls: ['./well-new.component.scss']
})
export class WellNewComponent implements OnInit, OnDestroy, AfterViewInit {
  
  private currentUser: UserDto;

  private maxZoom: number = 17;

  public model: WellNewDto;
  public isLoadingSubmit: boolean = false;

  public currentMarker: any;
  private tileLayers: any;
  public map: LeafletMap;
  public mapID = "wellNewLocation";

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private authenticationService: AuthenticationService,
    private wellService: WellService,
    private cdr: ChangeDetectorRef,
    private alertService: AlertService
  ) {
  }

  ngOnInit() {
    this.model = new WellNewDto();
    this.initMapConstants();

    this.authenticationService.getCurrentUser().subscribe(currentUser => {
      this.currentUser = currentUser;
      if (!this.authenticationService.isUserAnAdministrator(this.currentUser)) {
        this.router.navigateByUrl("/not-found")
          .then();
        return;
      }
    });
  }

  ngOnDestroy() {
    
       this.cdr.detach();
  }

  // Begin section: location map

  public ngAfterViewInit(): void {
    LeafletMap.addInitHook("addHandler", "gestureHandling", GestureHandling);
    const mapOptions: MapOptions = {
      maxZoom: this.maxZoom,
      layers: [
        this.tileLayers["Aerial"],
      ],
      gestureHandling: true,
      fullscreenControl: true
    } as MapOptions;
    this.map = map(this.mapID, mapOptions);

    const tpnrdBoundaryLayer = geoJSON(TwinPlatteBoundaryGeoJson as any, {
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

    tpnrdBoundaryLayer.addTo(this.map);

    this.map.fitBounds(tpnrdBoundaryLayer.getBounds());

    this.map.on("click", e => {
      this.changeMarkerOnMap(e);
    });
  }

  public onLatLngChange() {
    if (this.model.Latitude && this.model.Longitude) {
      var latlng = L.latLng(this.model.Latitude, this.model.Longitude);
      this.setCurrentMarker(latlng);
    }
  }

  public setCurrentMarker(latlng: L.LatLng) {
    this.removeLayerFromMap(this.currentMarker);
    this.currentMarker = L.marker(latlng, {
      icon: icon.glyph({
        prefix: "fas",
        glyph: "tint",
        iconUrl: "/assets/main/noDataSourceMarker.png"
      })
    });
    this.currentMarker.addTo(this.map);
  }


  public changeMarkerOnMap(e) {
    this.setCurrentMarker(e.latlng);
    this.model.Latitude = e.latlng.lat.toFixed(4);
    this.model.Longitude = e.latlng.lng.toFixed(4);
  }

  public removeLayerFromMap(layerToRemove) {
    if (layerToRemove) {
      this.map.removeLayer(layerToRemove);
    }
  }

  private initMapConstants() {
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
  }

  public onSubmit(newWellForm: HTMLFormElement): void {
    this.isLoadingSubmit = true;
    this.wellService.newWell(this.model)
      .subscribe(response => {
        this.isLoadingSubmit = false;
        newWellForm.reset();
        this.router.navigateByUrl("/wells/" + response.WellRegistrationID).then(x => {
          this.alertService.pushAlert(new Alert("Well '" + response.WellRegistrationID + "' successfully created.", AlertContext.Success));
        });
      }
        ,
        error => {
          this.isLoadingSubmit = false;
          this.cdr.detectChanges();
        }
      );
  }
}