import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { WellService } from 'src/app/services/well.service';
import { LinkRendererComponent } from 'src/app/shared/components/ag-grid/link-renderer/link-renderer.component';
import { UserDto } from 'src/app/shared/models/generated/user-dto';
import { WellWithSensorSummaryDto } from 'src/app/shared/models/well-with-sensor-summary-dto';

@Component({
  selector: 'zybach-well-explorer',
  templateUrl: './well-explorer.component.html',
  styleUrls: ['./well-explorer.component.scss']
})
export class WellExplorerComponent implements OnInit, OnDestroy {
  @ViewChild("wellsGrid") wellsGrid: any;
  
  public watchUserChangeSubscription: any;
  public currentUser: UserDto;
  public wellsObservable: any;
  public wells: WellWithSensorSummaryDto[];
  public wellsGeoJson: any;
  
  public columnDefs: any[];
  public gridApi: any;

  constructor(private authenticationService: AuthenticationService,
    private wellService: WellService) { }

  ngOnInit(): void {
    this.makeColumnDefs();

    this.watchUserChangeSubscription = this.authenticationService.currentUserSetObservable.subscribe(currentUser => {
      this.currentUser = currentUser;
      this.wellsObservable = this.wellService.getWellsMapData().subscribe(wells => {
        this.wells = wells.result;

        this.wellsGeoJson =
        {
          type: "FeatureCollection",
          features:

            wells.result.map(x => {
              const geoJsonPoint = x.location;
              geoJsonPoint.properties = {
                wellRegistrationID: x.wellRegistrationID,
                sensorTypes: x.sensors.map(x => x.sensorType)
              };
              return geoJsonPoint;
            })
        }
      })
    });
  }

  ngOnDestroy(): void {
    this.watchUserChangeSubscription.unsubscribe();
    this.wellsObservable.unsubscribe();
  }

  
  public onGridReady(params) {
    this.gridApi = params.api;
  }

  private makeColumnDefs() {
    this.columnDefs = [
      {
        headerName: "Registration #",
        field: "wellRegistrationID"
      }
    ]
    // this.columnDefs = [
    //   {
    //     headerName: 'Well Name', valueGetter: function (params: any) {
    //       return { LinkValue: params.data.CanonicalName, LinkDisplay: params.data.CanonicalName };
    //     }, cellRendererFramework: LinkRendererComponent,
    //     cellRendererParams: { inRouterLink: "/wells/" },
    //     filterValueGetter: function (params: any) {
    //       return params.data.FullName;
    //     },
    //     comparator: function (id1: any, id2: any) {
    //       let link1 = id1.LinkDisplay;
    //       let link2 = id2.LinkDisplay;
    //       if (link1 < link2) {
    //         return -1;
    //       }
    //       if (link1 > link2) {
    //         return 1;
    //       }
    //       return 0;
    //     },
    //     sortable: true, filter: true, width: 170
    //   },
    //   {
    //     headerName: "Sensor Name",
    //     field: "Sensor.CanonicalName",
    //     sortable: true, filter: true, width: 120
    //   },
    //   {
    //     headerName: "Last Reading",
    //     field: "LastReading",
    //     sortable: true, filter: true, width: 170,
    //     valueFormatter: function (params) {
    //       if (params.value) {
    //         const time = moment(params.value)
    //         const timepiece = time.format('h:mm a');
    //         return time.format('M/D/yyyy ') + timepiece;
    //       }
    //       else {
    //         return null;
    //       }
    //     },
    //   }
    // ];
  }
}
