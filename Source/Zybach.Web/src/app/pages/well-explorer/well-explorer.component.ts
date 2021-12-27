import { DatePipe } from '@angular/common';
import { Component, OnDestroy, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { ColDef } from 'ag-grid-community';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { WellService } from 'src/app/services/well.service';
import { LinkRendererComponent } from 'src/app/shared/components/ag-grid/link-renderer/link-renderer.component';
import { CustomDropdownFilterComponent } from 'src/app/shared/components/custom-dropdown-filter/custom-dropdown-filter.component';
import { UserDto } from 'src/app/shared/generated/model/user-dto';
import { WellWithSensorSummaryDto } from 'src/app/shared/generated/model/well-with-sensor-summary-dto';
import agGridDateFormatter from 'src/app/util/agGridDateFormatter';
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
    private datePipe: DatePipe,
    private wellService: WellService) { }

  ngOnInit(): void {
    this.makeColumnDefs();

    this.watchUserChangeSubscription = this.authenticationService.currentUserSetObservable.subscribe(currentUser => {
      this.currentUser = currentUser;
      this.wellsObservable = this.wellService.getWellsMapData().subscribe(wells => {
        this.wells = wells;
        wells.filter(x => x.Location == null || x.Location ==  undefined).forEach(x => console.log(x));
        this.wellsGeoJson =
        {
          type: "FeatureCollection",
          features:

            wells.filter(x => x.Location != null && x.Location != undefined).map(x => {
              const geoJsonPoint = x.Location;
              geoJsonPoint.properties = {
                wellRegistrationID: x.WellRegistrationID,
                sensors: x.Sensors || [],
                AgHubRegisteredUser: x.AgHubRegisteredUser,
                fieldName: x.FieldName
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
    let datePipe = this.datePipe;
    this.columnDefs = [
      {
        headerName: '', valueGetter: function (params: any) {
          return { LinkValue: params.data.WellRegistrationID, LinkDisplay: "View", CssClasses: "btn-sm btn-zybach" };
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
        field: "WellRegistrationID",
        width: 125,
        sortable: true, filter: true, resizable: true,
        
      },
      {
        headerName: "TPID",
        field: "WellTPID",
        width: 85,
        sortable: true, filter: true, resizable: true
      },
      {
        headerName: "AgHub Registered User",
        field: "AgHubRegisteredUser",
        width: 170,
        sortable: true, filter: true, resizable: true
      },
      {
        headerName: "Field Name",
        field: "FieldName",
        width: 115,
        sortable: true, filter: true, resizable: true
      },
      this.createDateColumnDef(datePipe, 'Last Reading Date', 'LastReadingDate', 'M/d/yyyy'),
      this.createDateColumnDef(datePipe, 'First Reading Date', 'FirstReadingDate', 'M/d/yyyy'),
      {
        headerName: "Has Flow Meter?",
        valueGetter: function (params) {
          const flowMeters = params.data.Sensors.filter(x => x.SensorType == "Flow Meter").map(x => x.SensorName);
          if (flowMeters.length > 0) {
            return `Yes (${flowMeters.join('; ')})`;
          } else {
            return "No";
          }
        },
        sortable: true, filter: true, resizable: true,
        width: 136
      },
      {
        headerName: "Has Continuity Meter?",
        valueGetter: function (params) {
          const continuityMeters = params.data.Sensors.filter(x => x.SensorType == "Continuity Meter").map(x => x.SensorName);
          if (continuityMeters.length > 0) {
            return `Yes (${continuityMeters.join('; ')})`;
          } else {
            return "No";
          }
        },
        filter: true,
        sortable: true, resizable: true,
        width: 170
      },
      {
        headerName: "Has Electrical Use Meter?",
        valueGetter: function (params) {
          if (params.data.HasElectricalData) {
            return "Yes";
          } else {
            return "No";
          }
        },
        filterFramework: CustomDropdownFilterComponent,
        filterParams: {
          field: 'params.data.HasElectricalData'
        },
        sortable: true, resizable: true
      },
      {
        headerName: "In AgHub?",
        valueGetter: function (params) {
          if (params.data.InAgHub) {
            return "Yes"
          } else {
            return "No"
          }
        },
        filterFramework: CustomDropdownFilterComponent,
        filterParams: {
          field: 'params.data.InAgHub'
        },
        sortable: true, resizable: true, width: 110
      },
      {
        headerName: "In GeoOptix?",
        valueGetter: function (params) {
          if (params.data.InGeoOptix) {
            return "Yes"
          } else {
            return "No"
          }
        },
        filterFramework: CustomDropdownFilterComponent,
        filterParams: {
          field: 'params.data.InGeoOptix'
        },
        sortable: true, resizable: true, width: 115
      },
      this.createDateColumnDef(datePipe, 'Last Fetched from AgHub', 'FetchDate', 'M/d/yyyy')
    ]
  }

  
  private dateSortComparer (id1: any, id2: any) {
    const date1 = id1 ? Date.parse(id1) : Date.parse("1/1/1900");
    const date2 = id2 ? Date.parse(id2) : Date.parse("1/1/1900");
    if (date1 < date2) {
      return -1;
    }
    return (date1 > date2)  ?  1 : 0;
  }

  private dateFilterComparator(filterLocalDateAtMidnight, cellValue) {
    if(cellValue === null) return -1;
    const cellDate = Date.parse(cellValue);
    if (cellDate == filterLocalDateAtMidnight) {
      return 0;
    }
    return (cellDate < filterLocalDateAtMidnight) ? -1 : 1;
  }

  private createDateColumnDef(datePipe: DatePipe, headerName: string, fieldName: string, dateFormat: string): ColDef {
    return {
      headerName: headerName, valueGetter: function (params: any) {
        return datePipe.transform(params.data[fieldName], dateFormat);
      },
      comparator: this.dateSortComparer,
      filter: 'agDateColumnFilter',
      filterParams: {
        filterOptions: ['inRange'],
        comparator: this.dateFilterComparator
      }, 
      width: 110,
      resizable: true,
      sortable: true
    };
  }
  
  public onSelectionChanged(event: Event) {
    const selectedNode = this.gridApi.getSelectedNodes()[0];
    if (!selectedNode) {
      // event was fired automatically when we updated the grid after a map click
      return;
    }
    this.wellMap.selectWell(selectedNode.data.WellRegistrationID);
  }

  public onMapSelection(wellRegistrationID: string) {
    this.gridApi.deselectAll();
    this.gridApi.forEachNode(node => {
      if (node.data.WellRegistrationID === wellRegistrationID) {
        node.setSelected(true);
        this.gridApi.ensureIndexVisible(node.rowIndex, "top")
      }
    })
  }

  public onFilterChange(dataSourceOptions: any) {
    const filteredWells = this.wells.filter(x => {
      if (x.Sensors === null || x.Sensors.length === 0 || (x.Sensors.length === 1 && x.Sensors[0].SensorType === "Well Pressure")) {
        return dataSourceOptions.showNoEstimate;
      }

      const sensorTypes = x.Sensors.map(s => s.SensorType);

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

