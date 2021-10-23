import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import {
  GeoJSON,
  marker,
  map,
  Map as LeafletMap,
  MapOptions,
  tileLayer,
  icon
} from 'leaflet';
import 'leaflet.icon.glyph';
import 'leaflet.fullscreen';
import { GestureHandling } from 'leaflet-gesture-handling';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { WellService } from 'src/app/services/well.service';
import { UserDetailedDto } from 'src/app/shared/models';
import { Alert } from 'src/app/shared/models/alert';
import { AlertContext } from 'src/app/shared/models/enums/alert-context.enum';
import { AlertService } from 'src/app/shared/services/alert.service';
import { BoundingBoxDto } from 'src/app/shared/models/bounding-box-dto';
import L from 'leaflet';
import { WellNewDto } from 'src/app/shared/models/well-new-dto';

@Component({
  selector: 'zybach-well-new',
  templateUrl: './well-new.component.html',
  styleUrls: ['./well-new.component.scss']
})
export class WellNewComponent implements OnInit, OnDestroy, AfterViewInit {
  private watchUserChangeSubscription: any;
  private currentUser: UserDetailedDto;

  private maxZoom: number = 17;

  public model: WellNewDto;
  public isLoadingSubmit: boolean = false;

  public currentMarker: any;
  private boundingBox: BoundingBoxDto;
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

    this.watchUserChangeSubscription = this.authenticationService.currentUserSetObservable.subscribe(currentUser => {
      this.currentUser = currentUser;
      if (!this.authenticationService.isUserAnAdministrator(this.currentUser)) {
        this.router.navigateByUrl("/not-found")
          .then();
        return;
      }
    });
  }

  ngOnDestroy() {
    this.watchUserChangeSubscription.unsubscribe();
    this.authenticationService.dispose();
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

    this.map.fitBounds([[this.boundingBox.Bottom, this.boundingBox.Left], [this.boundingBox.Top, this.boundingBox.Right]], null);
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