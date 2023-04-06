import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { WellGroupService } from 'src/app/shared/generated/api/well-group.service';
import { UserDto } from 'src/app/shared/generated/model/user-dto';
import { WellGroupSummaryDto } from 'src/app/shared/generated/model/well-group-summary-dto';
import * as L from 'leaflet';
import GestureHandling from 'leaflet-gesture-handling';
import { BoundingBoxDto } from 'src/app/shared/generated/model/bounding-box-dto';

@Component({
  selector: 'zybach-well-group-detail',
  templateUrl: './well-group-detail.component.html',
  styleUrls: ['./well-group-detail.component.scss']
})
export class WellGroupDetailComponent implements OnInit {
  @ViewChild('wellGroupMap') wellGroupMap: ElementRef;  

  private currentUser: UserDto;

  public wellGroup: WellGroupSummaryDto;
  public wellLayer: L.LayerGroup;

  public map: L.Map;
  public mapID = "wellGroupMap";
  private maxZoom: number = 17;
  private boundingBox: BoundingBoxDto;
  private tileLayers: { [key: string]: any } = {};

  private wellMarkerIcon = L.icon.glyph({
    prefix: "fas",
    glyph: "tint",
    iconUrl: "/assets/main/continuityMeterMarker.png"
  });

  constructor(
    private wellGroupService: WellGroupService,
    private authenticationService: AuthenticationService,
    private route: ActivatedRoute,
    private router: Router
  ) { 
    // force route reload whenever params change;
    this.router.routeReuseStrategy.shouldReuseRoute = () => false;
  }

  ngOnInit(): void {
    this.authenticationService.getCurrentUser().subscribe(currentUser => {
      this.currentUser = currentUser;

      const wellGroupID = parseInt(this.route.snapshot.paramMap.get("id"));
      this.wellGroupService.wellGroupsWellGroupIDSummaryGet(wellGroupID).subscribe(wellGroup => {
        this.wellGroup = wellGroup;
        this.boundingBox = wellGroup.BoundingBox;

        this.initializeMap();
      });
    });
  }

  private initializeMap() {
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

    const mapOptions: L.MapOptions = {
      maxZoom: this.maxZoom,
      layers: [
        this.tileLayers["Aerial"],
      ],
      gestureHandling: true,
      fullscreenControl: true
    } as L.MapOptions;

    this.map = L.map(this.mapID, mapOptions);
    L.Map.addInitHook("addHandler", "gestureHandling", GestureHandling);

    this.map.fitBounds([[this.boundingBox.Bottom, this.boundingBox.Left], [this.boundingBox.Top, this.boundingBox.Right]], null);

    this.addWellLayer();
  }

  private addWellLayer() {
    this.wellLayer = new L.LayerGroup();

    this.wellGroup.WellGroupWells.forEach(x => {
      if (!x.Latitude || !x.Longitude) return;

      L.marker([x.Latitude, x.Longitude], { icon: this.wellMarkerIcon })
      .bindPopup(`<b>${x.WellRegistrationID}</b> ${x.IsPrimary ? "(Primary)" : ""}`)
      .addTo(this.wellLayer);
    });

    this.wellLayer.addTo(this.map);
  }
  
  public resizeWindow(){
    window.dispatchEvent(new Event('resize'));
  }
}
