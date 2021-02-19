import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import moment from 'moment';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { WellService } from 'src/app/services/well.service';
import { UserDetailedDto } from 'src/app/shared/models';
import { WellWithSensorSummaryDto } from 'src/app/shared/models/well-with-sensor-summary-dto';
import { default as vegaEmbed } from 'vega-embed';

import {
  Control, FitBoundsOptions,
  GeoJSON,
  marker,
  map,
  Map,
  MapOptions,
  tileLayer,
  Icon,
  geoJSON,
  icon,
  latLng,
  Layer,
  LeafletEvent,
  layerGroup
} from 'leaflet';
import 'leaflet.icon.glyph';
import 'leaflet.fullscreen';
import { BoundingBoxDto } from 'src/app/shared/models/bounding-box-dto';

@Component({
  selector: 'zybach-well-detail',
  templateUrl: './well-detail.component.html',
  styleUrls: ['./well-detail.component.scss']
})
export class WellDetailComponent implements OnInit, OnDestroy, AfterViewInit {
  public chartID: string = "wellChart";

  public watchUserChangeSubscription: any;

  maxZoom = 17;

  currentUser: UserDetailedDto;
  chartSubscription: any;
  well: WellWithSensorSummaryDto;
  rawResults: string;
  timeSeries: any[];
  vegaView: any;
  rangeMax: number;
  wellRegistrationID: string;
  tooltipFields: any;
  noTimeSeriesData: boolean = false; sensors: any;
  boundingBox: any;
  tileLayers: any;
  map: Map;
  mapID = "wellLocation"
    ;


  constructor(
    private wellService: WellService,
    private authenticationService: AuthenticationService,
    private route: ActivatedRoute,
    private cdr: ChangeDetectorRef
  ) { }

  ngOnInit(): void {
    this.initMapConstants();

    this.watchUserChangeSubscription = this.authenticationService.currentUserSetObservable.subscribe(currentUser => {
      this.currentUser = currentUser;
      this.wellRegistrationID = this.route.snapshot.paramMap.get("wellRegistrationID");
      this.getChartDataAndBuildChart();
    })
  }

  ngOnDestroy() {
    this.watchUserChangeSubscription.unsubscribe();
    this.chartSubscription.unsubscribe();
  }

  public ngAfterViewInit(): void {
    const mapOptions: MapOptions = {
      maxZoom: this.maxZoom,
      layers: [
        this.tileLayers["Aerial"],
      ],
      fullscreenControl: true
    } as MapOptions;
    this.map = map(this.mapID, mapOptions);

    this.map.fitBounds([[this.boundingBox.Bottom, this.boundingBox.Left], [this.boundingBox.Top, this.boundingBox.Right]], null);

  }

  public initMapConstants() {
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

  getChartDataAndBuildChart() {

    this.chartSubscription = this.wellService.getChartData(this.wellRegistrationID).subscribe(response => {
      this.well = response.well;
      this.sensors = response.sensors;
      this.addWellToMap();
      this.cdr.detectChanges();

      console.log(this.well);

      if (!response) {
        this.noTimeSeriesData = true;
        return;
      }
      this.timeSeries = response.timeSeries;

      const gallonsMax = this.timeSeries.sort((a, b) => b.gallons - a.gallons)[0].gallons;
      if (gallonsMax !== 0) {
        this.rangeMax = gallonsMax * 1.05;
      } else {
        this.rangeMax = 10000;
      }

      this.tooltipFields = response.sensors.map(x => ({ "field": x.sensorType, "type": "ordinal" }));

      this.buildChart();
    });
  }

  addWellToMap() {
    const sensorTypes = this.sensors.map(x => x.sensorType);
    let mapIcon;
    debugger;

    if (sensorTypes.includes("Flow Meter")) {
      mapIcon = icon.glyph({
        prefix: "fas",
        glyph: "tint",
        iconUrl: "/assets/main/flowMeterMarker.png"
      });
    } else if (sensorTypes.includes("Continuity Meter")) {
      mapIcon = icon.glyph({
        prefix: "fas",
        glyph: "tint",
        iconUrl: "/assets/main/continuityMeterMarker.png"
      });
    } else if (sensorTypes.includes("Electrical Data")) {
      mapIcon = icon.glyph({
        prefix: "fas",
        glyph: "tint",
        iconUrl: "/assets/main/electricalDataMarker.png"
      });
    } else {
      mapIcon = icon.glyph({
        prefix: "fas",
        glyph: "tint",
        iconUrl: "/assets/main/noDataSourceMarker.png"
      });
    }
    const wellLayer = new GeoJSON(this.well.location, {
      pointToLayer: function (feature, latlng) {
        return marker(latlng, {icon: mapIcon});
      }
    });
    wellLayer.addTo(this.map);
    
    let target = (this.map as any)._getBoundsCenterZoom(wellLayer.getBounds(), null);
    this.map.setView(target.center, 16, null);
  }

  buildChart() {
    var self = this;
    vegaEmbed(`#${this.chartID}`, this.getVegaSpec(), {
      actions: false, tooltip: true, renderer: "svg"
    }).then(function (res) {
      self.vegaView = res.view;
    });
  }

  getVegaSpec(): any {
    return {
      "$schema": "https://vega.github.io/schema/vega-lite/v4.json",
      "description": "A charmt",
      "width": "container",
      "height": "container",
      "data": { "values": this.timeSeries, "name": "data" },
      "encoding": {
        "x": {
          "field": "time",
          "timeUnit": "yearmonthdate",
          "type": "temporal",
          "axis": {
            "title": "Date"
          }
        }
      },

      "layer": [
        {
          "encoding": {
            "y": {
              "field": "gallons",
              "type": "quantitative",
              "axis": {
                "title": "Gallons"
              },
              "scale": {
                "domain": [0, this.rangeMax]
              }
            },
            "color": {
              "field": "dataSource",
              "type": "nominal",
              "axis": {
                "title": "Data Source"
              },
              "scale": {
                "domain": ["Flow Meter", "Continuity Meter", "Electrical Data"],
                "range": ["#13B5EA", "#4AAA42", "#0076C0"],
              }
            }
          },
          "layer": [
            { "mark": "line" },
            { "transform": [{ "filter": { "selection": "hover" } }], "mark": "point" }
          ]
        },
        {
          "transform": [{ "pivot": "dataSource", "value": "gallonsString", "groupby": ["time"], "op": "max" }],
          "mark": "rule",
          "encoding": {
            "opacity": {
              "condition": { "value": 0.3, "selection": "hover" },
              "value": 0
            },
            "tooltip": [
              { "field": "time", "type": "temporal", "title": "Date" },
              ...this.tooltipFields
            ]
          },
          "selection": {
            "hover": {
              "type": "single",
              "fields": ["time"],
              "nearest": true,
              "on": "mouseover",
              "empty": "none",
              "clear": "mouseout"
            }
          }
        }
      ]
    }
  }

  getSensorTypes() {
    return this.sensors.map(x => x.sensorType).join(", ");
  }

  getLastReadingDate() {
    if (!this.well.lastReadingDate) {
      return ""
    }
    const time = moment(this.well.lastReadingDate)
    const timepiece = time.format('h:mm a');
    return time.format('M/D/yyyy ') + timepiece;
  }
}
