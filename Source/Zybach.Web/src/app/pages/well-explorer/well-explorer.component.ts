import { literalMap } from '@angular/compiler';
import { Component, OnDestroy, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import * as moment from 'moment';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { WellService } from 'src/app/services/well.service';
import { LinkRendererComponent } from 'src/app/shared/components/ag-grid/link-renderer/link-renderer.component';
import { DataSourceFilterOption, DataSourceSensorTypeMap } from 'src/app/shared/models/enums/data-source-filter-option.enum';
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

        this.wellsGeoJson =
        {
          type: "FeatureCollection",
          features:

            wells.result.map(x => {
              const geoJsonPoint = x.location;
              geoJsonPoint.properties = {
                wellRegistrationID: x.wellRegistrationID,
                sensors: x.sensors
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
    this.onFilterChange({
      showFlowMeters: true,
      showContinuityMeters: true,
      showElectricalData: true,
      showNoEstimate: false
    });
  }

  private makeColumnDefs() {
    this.columnDefs = [
      {
        headerName: '', valueGetter: function (params: any) {
          return { LinkValue: params.data.wellRegistrationID, LinkDisplay: "View", CssClasses: "btn-sm btn-zybach" };
        }, cellRendererFramework: LinkRendererComponent,
        cellRendererParams: { inRouterLink: "/wells/" },
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
        sortable: true, filter: true, resizable: true,
        
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
      },
      {
        headerName: "First Reading Date",
        field: "firstReadingDate",
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
      },
      {
        headerName: "Has Flow Meter?",
        valueGetter: function (params) {
          const flowMeters = params.data.sensors.filter(x => x.sensorType == "Flow Meter").map(x => x.sensorName);
          if (flowMeters.length > 0) {
            return `Yes (${flowMeters.join('; ')})`;
          } else {
            return "No";
          }
        },
        sortable: true, filter: true, resizable: true
      },
      {
        headerName: "Has Continuity Meter?",
        valueGetter: function (params) {
          const continuityMeters = params.data.sensors.filter(x => x.sensorType == "Continuity Meter").map(x => x.sensorName);
          if (continuityMeters.length > 0) {
            return `Yes (${continuityMeters.join('; ')})`;
          } else {
            return "No";
          }
        },
        sortable: true, filter: true, resizable: true
      },
      {
        headerName: "Has Electrical Use Meter?",
        valueGetter: function (params) {
          const sensorTypes = params.data.sensors.map(x => x.sensorType);
          if (sensorTypes.includes("Electrical Usage")) {
            return "Yes";
          } else {
            return "No";
          }
        },
        sortable: true, filter: true, resizable: true
      },
      {
        headerName: "In AgHub?",
        valueGetter: function (params) {
          if (params.data.wellTPID) {
            return "Yes"
          } else {
            return "No"
          }
        },
        sortable: true, filter: true, resizable: true
      },
      {
        headerName: "In GeoOptix?",
        valueGetter: function (params) {
          if (params.data.inGeoOptix) {
            return "Yes"
          } else {
            return "No"
          }
        },
        sortable: true, filter: true, resizable: true
      },
      {
        headerName: "Last Fetched from AgHub",
        field: "fetchDate",
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
    this.gridApi.deselectAll();
    this.gridApi.forEachNode(node => {
      if (node.data.wellRegistrationID === wellRegistrationID) {
        node.setSelected(true);
        this.gridApi.ensureIndexVisible(node.rowIndex, "top")
      }
    })
  }

  public onFilterChange(dataSourceOptions: any) {
    const filteredWells = this.wells.filter(x => {
      if (x.sensors === null || x.sensors.length === 0) {
        return dataSourceOptions.showNoEstimate;
      }

      const sensorTypes = x.sensors.map(s => s.sensorType);

      return (dataSourceOptions.showFlowMeters && sensorTypes.includes("Flow Meter")) ||
        (dataSourceOptions.showContinuityMeters && sensorTypes.includes("Continuity Meter")) ||
        (dataSourceOptions.showElectricalData && sensorTypes.includes("Electrical Usage"));
    });

    this.gridApi.setRowData(filteredWells);
  }

  public downloadCsv(){

    // quick and easy way to exclude the column with the "View" buttons from the download:
    // since it's the first column in the grid, we can just get all columns and ignore the first one
    const columns = this.gridApi.columnController.getAllGridColumns();
    const columnsToDownload = columns.slice(1);


    // the columnKeys parameter can be either a column object (obtained as above from the grid api)
    // or a string, set as the "colId" property of a column in the column definitions.
    // if we ever need to add more columns that won't be intended for download, we'll need to set
    // colIds on the column definitions and pass an explicit list of the desired ids to exportDataAsCsv,
    // because the trick we're using here only works since the one column we want to ignore is the first one.
    this.gridApi.exportDataAsCsv({columnKeys: columnsToDownload});
  }

  public getParams() {
    throw new Error('Function not implemented.');
  }
}

