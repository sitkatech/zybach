import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { WellService } from 'src/app/services/well.service';
import { UserDetailedDto } from 'src/app/shared/models';
import { WellWithSensorSummaryDto } from 'src/app/shared/models/well-with-sensor-summary-dto';
import {default as vegaEmbed} from 'vega-embed';
@Component({
  selector: 'zybach-well-detail',
  templateUrl: './well-detail.component.html',
  styleUrls: ['./well-detail.component.scss']
})
export class WellDetailComponent implements OnInit, OnDestroy {
  public chartID: string = "wellChart";

  public watchUserChangeSubscription: any;

  currentUser: UserDetailedDto;
  wellSubscription: any;
  chartSubscription: any;
  well: WellWithSensorSummaryDto;
  rawResults: string;
  timeSeries: any;
  vegaView: any;


  constructor(
    private wellService: WellService,
    private authenticationService: AuthenticationService,
    private route: ActivatedRoute,
  ) { }

  ngOnInit(): void {
    this.watchUserChangeSubscription = this.authenticationService.currentUserSetObservable.subscribe(currentUser => {
      this.currentUser = currentUser;
      const id = this.route.snapshot.paramMap.get("wellRegistrationID");
      this.wellSubscription = this.wellService.getWell(id).subscribe(response=>{
        this.well = response.result;
      })

      this.chartSubscription = this.wellService.getElectricalBasedFlowEstimateSeries(id).subscribe(response => {
        this.timeSeries = response;
        this.rawResults = JSON.stringify(response);
        this.buildChart();
      })
    })
  }

  ngOnDestroy(){
    this.watchUserChangeSubscription.unsubscribe();
    this.wellSubscription.unsubscribe();
    this.chartSubscription.unsubscribe();
  }

  buildChart() {
    var self = this;
    debugger;
    vegaEmbed(`#${this.chartID}`, this.getVegaSpec()).then(function (res){
      self.vegaView = res.view;
    });
  }

  getVegaSpec() : any{
    return {
      "$schema": "https://vega.github.io/schema/vega-lite/v4.json",
      "description": "A charmt",
      "data": {"values": this.timeSeries, "name": "data"},
      "mark": {
        "type": "line",
      },
      "encoding": {
        "x": { "field": "time", "timeUnit": "yearmonthdate" },
        "y": { "field": "gallons", "type": "quantitative"},
        "color": {"field": "dataSource", "type": "nominal"}
      }
    }
  }

}
