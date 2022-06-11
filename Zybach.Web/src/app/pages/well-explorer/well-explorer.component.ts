import { DatePipe } from '@angular/common';
import { Component, OnDestroy, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { ColDef } from 'ag-grid-community';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { UtilityFunctionsService } from 'src/app/services/utility-functions.service';
import { WellService } from 'src/app/shared/generated/api/well.service';
import { LinkRendererComponent } from 'src/app/shared/components/ag-grid/link-renderer/link-renderer.component';
import { CustomDropdownFilterComponent } from 'src/app/shared/components/custom-dropdown-filter/custom-dropdown-filter.component';
import { UserDto } from 'src/app/shared/generated/model/user-dto';
import { WellWithSensorSummaryDto } from 'src/app/shared/generated/model/well-with-sensor-summary-dto';
import { WellMapComponent } from '../well-map/well-map.component';
import { MapDataService } from 'src/app/shared/generated/api/map-data.service';
import { SensorTypeEnum } from 'src/app/shared/generated/enum/sensor-type-enum';
import { FieldDefinitionGridHeaderComponent } from 'src/app/shared/components/field-definition-grid-header/field-definition-grid-header.component';
import { FieldDefinitionTypeEnum } from 'src/app/shared/generated/enum/field-definition-type-enum';

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
    private utilityFunctionsService: UtilityFunctionsService,
    private mapDataService: MapDataService,
    private wellService: WellService) { }

  ngOnInit(): void {
    this.makeColumnDefs();

    this.authenticationService.getCurrentUser().subscribe(currentUser => {
      this.currentUser = currentUser;
      this.wellsObservable = this.mapDataService.mapDataWellsGet().subscribe(wells => {
        this.wells = wells;
        wells.filter(x => x.Location == null || x.Location ==  undefined).forEach(x => console.log(x));
        this.wellsGeoJson =
        {
          type: "FeatureCollection",
          features:

            wells.filter(x => x.Location != null && x.Location != undefined).map(x => {
              const geoJsonPoint = x.Location;
              geoJsonPoint.properties = {
                wellID: x.WellID,
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
    
    this.wellsObservable.unsubscribe();
  }


  public onGridReady(params) {
    this.gridApi = params.api;
    this.wellsGrid.columnApi.autoSizeAllColumns();
    this.gridApi.setRowData(this.wells);
  }

  private makeColumnDefs() {
    let datePipe = this.datePipe;
    this.columnDefs = [
      {
        headerName: 'Registration #', valueGetter: function (params: any) {
          return { LinkValue: params.data.WellID, LinkDisplay: params.data.WellRegistrationID };
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
        filterValueGetter: function (params: any) {
          return params.data.WellRegistrationID;
        },
        headerComponentFramework: FieldDefinitionGridHeaderComponent, headerComponentParams: {fieldDefinitionTypeID: FieldDefinitionTypeEnum.WellRegistrationNumber},
        sortable: true, filter: true, resizable: true,        
      },
      {
        headerName: "Nickname",
        field: "WellNickname",
        headerComponentFramework: FieldDefinitionGridHeaderComponent, headerComponentParams: {fieldDefinitionTypeID: FieldDefinitionTypeEnum.WellNickname},
        sortable: true, filter: true, resizable: true,        
      },
      {
        headerName: "AgHub Registered User",
        field: "AgHubRegisteredUser",
        headerComponentFramework: FieldDefinitionGridHeaderComponent, headerComponentParams: {fieldDefinitionTypeID: FieldDefinitionTypeEnum.AgHubRegisteredUser},
        sortable: true, filter: true, resizable: true
      },
      {
        headerName: "Field Name",
        field: "FieldName",
        headerComponentFramework: FieldDefinitionGridHeaderComponent, headerComponentParams: {fieldDefinitionTypeID: FieldDefinitionTypeEnum.WellFieldName},
        sortable: true, filter: true, resizable: true
      },      
      {
        headerName: "Page #",
        field: "PageNumber",
        sortable: true, filter: 'agNumberColumnFilter', resizable: true
      },
      {
        headerName: "Owner Name",
        field: "OwnerName",
        headerComponentFramework: FieldDefinitionGridHeaderComponent, headerComponentParams: {fieldDefinitionTypeID: FieldDefinitionTypeEnum.WellOwnerName},
        sortable: true, filter: true, resizable: true
      },
      {
        headerName: "Legal",
        field: "TownshipRangeSection",
        sortable: true, filter: true, resizable: true
      },
      {
        headerName: "Water Quality?", field: 'HasWaterQualityInspections',
        valueGetter: (params) => {
          return params.data.HasWaterQualityInspections ? 'Yes' : 'No';
        },
        filterFramework: CustomDropdownFilterComponent,
        filterParams: {
        field: 'HasWaterQualityInspections'
        },
        headerComponentFramework: FieldDefinitionGridHeaderComponent, headerComponentParams: {fieldDefinitionTypeID: FieldDefinitionTypeEnum.WellWaterQualityInspectionParticipation, labelOverride: 'Has Water Quality Inspections?'},
        sortable: true, resizable: true
      },
      {
        headerName: "Water Level?", field: 'HasWaterLevelInspections',
        valueGetter: (params) => {
          return params.data.HasWaterLevelInspections ? 'Yes' : 'No';
        },
        filterFramework: CustomDropdownFilterComponent,
        filterParams: {
        field: 'HasWaterLevelInspections'
        },
        headerComponentFramework: FieldDefinitionGridHeaderComponent, headerComponentParams: {fieldDefinitionTypeID: FieldDefinitionTypeEnum.WellWaterLevelInspectionParticipation},
        sortable: true, resizable: true
      },
      this.createDateColumnDef(datePipe, 'Last Reading Date', 'LastReadingDate', 'M/d/yyyy', FieldDefinitionTypeEnum.SensorLastReadingDate),
      this.createDateColumnDef(datePipe, 'First Reading Date', 'FirstReadingDate', 'M/d/yyyy', FieldDefinitionTypeEnum.SensorFirstReadingDate),
      {
        headerName: "Has Flow Meter?",
        valueGetter: function (params) {
          const flowMeters = params.data.Sensors.filter(x => x.SensorTypeID == SensorTypeEnum.FlowMeter).map(x => x.SensorName);
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
          const continuityMeters = params.data.Sensors.filter(x => x.SensorTypeID == SensorTypeEnum.PumpMonitor).map(x => x.SensorName);
          if (continuityMeters.length > 0) {
            return `Yes (${continuityMeters.join('; ')})`;
          } else {
            return "No";
          }
        },
        filter: true,
        sortable: true, resizable: true
      },
      {
        headerName: "Has Pressure Sensor?",
        valueGetter: function (params) {
          const pressureSensors = params.data.Sensors.filter(x => x.SensorTypeID == SensorTypeEnum.WellPressure).map(x => x.SensorName);
          if (pressureSensors.length > 0) {
            return `Yes (${pressureSensors.join('; ')})`;
          } else {
            return "No";
          }
        },
        filter: true,
        sortable: true, resizable: true
      },
      {
        headerName: "Well Connected Meter?",
        valueGetter: function (params) {
          if (params.data.WellConnectedMeter) {
            return "Yes";
          } else {
            return "No";
          }
        },
        filterFramework: CustomDropdownFilterComponent,
        filterParams: {
          field: 'params.data.WellConnectedMeter'
        },
        sortable: true, resizable: true
      },
      {
        headerName: "Irrigation Unit ID",
        field: "WellTPID",
        headerComponentFramework: FieldDefinitionGridHeaderComponent, headerComponentParams: {fieldDefinitionTypeID: FieldDefinitionTypeEnum.IrrigationUnitID},
        sortable: true, filter: true, resizable: true
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
        sortable: true, resizable: true
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
        sortable: true, resizable: true
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

  private createDateColumnDef(datePipe: DatePipe, headerName: string, fieldName: string, dateFormat: string, fieldDefinitionTypeID?: number): ColDef {
    var dateColDef: ColDef = 
    {
      headerName: headerName, valueGetter: function (params: any) {
        return datePipe.transform(params.data[fieldName], dateFormat);
      },
      comparator: this.dateSortComparer,
      filter: 'agDateColumnFilter',
      filterParams: {
        filterOptions: ['inRange'],
        comparator: this.dateFilterComparator
      },
      resizable: true,
      sortable: true
    };

    if (fieldDefinitionTypeID) {
      dateColDef.headerComponentFramework = FieldDefinitionGridHeaderComponent;
      dateColDef.headerComponentParams = { fieldDefinitionTypeID: fieldDefinitionTypeID }
    }

    return dateColDef;
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

  public downloadCsv(){
    this.utilityFunctionsService.exportGridToCsv(this.wellsGrid, 'wells.csv', null);
  }

  public getParams() {
    throw new Error('Function not implemented.');
  }
}

