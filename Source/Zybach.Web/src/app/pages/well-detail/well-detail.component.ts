import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { WellService } from 'src/app/services/well.service';
import { UserDetailedDto } from 'src/app/shared/models';
import { WellWithSensorSummaryDto } from 'src/app/shared/models/well-with-sensor-summary-dto';
import { default as vegaEmbed } from 'vega-embed';
@Component({
  selector: 'zybach-well-detail',
  templateUrl: './well-detail.component.html',
  styleUrls: ['./well-detail.component.scss']
})
export class WellDetailComponent implements OnInit, OnDestroy {
  public chartID: string = "wellChart";

  public watchUserChangeSubscription: any;

  currentUser: UserDetailedDto;
  chartSubscription: any;
  well: WellWithSensorSummaryDto;
  rawResults: string;
  timeSeries: any[];
  vegaView: any;
  rangeMax: number;
  wellRegistrationID: string;
  tooltipFields: any;


  constructor(
    private wellService: WellService,
    private authenticationService: AuthenticationService,
    private route: ActivatedRoute,
  ) { }

  ngOnInit(): void {
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

  getChartDataAndBuildChart(){
    
    this.chartSubscription = this.wellService.getChartData(this.wellRegistrationID).subscribe(response => {
      this.timeSeries = response.timeSeries;

      const gallonsMax = this.timeSeries.sort((a, b) => b.gallons - a.gallons)[0].gallons;
      if (gallonsMax !== 0) {
        this.rangeMax = gallonsMax * 1.05;
      } else{
        this.rangeMax = 10000;
      }

      this.tooltipFields = response.sensors.map(x=>({"field": x.sensorType, "type": "ordinal"}));

      this.buildChart();
    });
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
            {"mark": "line"},
            {"transform": [{"filter": {"selection": "hover"}}], "mark": "point"}
          ]
        },
        {
          "transform": [{"pivot": "dataSource", "value": "gallonsString", "groupby": ["time"], "op": "max"}],
          "mark": "rule",
          "encoding": {
            "opacity": {
              "condition": {"value": 0.3, "selection": "hover"},
              "value": 0
            },
            "tooltip": [
              {"field": "time", "type": "temporal", "title": "Date"},
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

}
