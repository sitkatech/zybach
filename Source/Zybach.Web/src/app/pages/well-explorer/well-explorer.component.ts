import { Component, OnDestroy, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import * as moment from 'moment';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { WellService } from 'src/app/services/well.service';
import { LinkRendererComponent } from 'src/app/shared/components/ag-grid/link-renderer/link-renderer.component';
import { UserDto } from 'src/app/shared/models/generated/user-dto';
import { WellWithSensorSummaryDto } from 'src/app/shared/models/well-with-sensor-summary-dto';
import { WellMapComponent } from '../well-map/well-map.component';

@Component({
  selector: 'zybach-well-explorer',
  templateUrl: './well-explorer.component.html',
  styleUrls: ['./well-explorer.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class WellExplorerComponent implements OnInit, OnDestroy {
  @ViewChild("wellsGrid") wellsGrid: any;
  @ViewChild("wellMap") wellMap: WellMapComponent;

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
        console.log(wells);

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
        headerName: '', valueGetter: function (params: any) {
          return { LinkValue: params.data.wellRegistrationID, LinkDisplay: "View", CssClasses: "btn-sm btn-zybach" };
        }, cellRendererFramework: LinkRendererComponent,
        cellRendererParams: { inRouterLink: "/wells/" },
        // filterValueGetter: function (params: any) {
        //   return params.data.FullName;
        // },
        comparator: function (id1: any, id2: any) {
          let link1 = id1.LinkDisplay;
          let link2 = id2.LinkDisplay;
          if (link1 < link2) {
            return -1;
          }
          if (link1 > link2) {
            return 1;
          }
          return 0;
        },
        width: 70,
        resizable: true
      },
      {
        headerName: "Registration #",
        field: "wellRegistrationID",
        width: 125,
        sortable: true, filter: true, resizable: true
      },
      {
        headerName: "TPID",
        field: "wellTPID",
        width: 125,
        sortable: true, filter: true, resizable: true
      },
      {
        headerName: "Last Reading Date",
        field: "lastReadingDate",
        valueFormatter: function (params) {
          if (params.value) {
            const time = moment(params.value)
            const timepiece = time.format('h:mm a');
            return time.format('M/D/yyyy ') + timepiece;
          }
          else {
            return null;
          }
        },
        width: 150,
        sortable: true, filter: true, resizable: true
      }
    ]
    // this.columnDefs = [
    //   {
    //     headerName: "Sensor Name",
    //     field: "Sensor.CanonicalName",
    //     sortable: true, filter: true, width: 120
    //   },
    //   {
    //     headerName: "Last Reading",
    //     field: "LastReading",
    //     sortable: true, filter: true, width: 170,
    //     
    //   }
    // ];
  }

  public onSelectionChanged(event: Event) {
    const selectedNode = this.gridApi.getSelectedNodes()[0];
    if (!selectedNode) {
      // event was fired automatically when we updated the grid after a map click
      return;
    }
    this.wellMap.selectWell(selectedNode.data.wellRegistrationID);
  }

  public onMapSelection(wellRegistrationID: string) {
    debugger;
    this.gridApi.deselectAll();
    this.gridApi.forEachNode(node => {
      if (node.data.wellRegistrationID === wellRegistrationID) {
        node.setSelected(true);
        this.gridApi.ensureIndexVisible(node.rowIndex, "top")
      }
    })
  }
}
