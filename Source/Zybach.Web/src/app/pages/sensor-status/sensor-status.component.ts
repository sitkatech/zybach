import { Component, OnDestroy, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { SensorStatusService } from 'src/app/services/sensor-status.service';
import { UtilityFunctionsService } from 'src/app/services/utility-functions.service';
import { LinkRendererComponent } from 'src/app/shared/components/ag-grid/link-renderer/link-renderer.component';
import { CustomDropdownFilterComponent } from 'src/app/shared/components/custom-dropdown-filter/custom-dropdown-filter.component';
import { SensorMessageAgeDto } from 'src/app/shared/generated/model/sensor-message-age-dto';
import { UserDto } from 'src/app/shared/generated/model/user-dto';
import { WellWithSensorMessageAgeDto } from 'src/app/shared/generated/model/well-with-sensor-message-age-dto';
import { WellMapComponent } from '../well-map/well-map.component';

@Component({
  selector: 'zybach-sensor-status',
  templateUrl: './sensor-status.component.html',
  styleUrls: ['./sensor-status.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class SensorStatusComponent implements OnInit, OnDestroy {
  @ViewChild("wellsGrid") wellsGrid: any;
  @ViewChild("wellMap") wellMap: WellMapComponent;

  public watchUserChangeSubscription: any;
  public currentUser: UserDto;
  public wellsObservable: any;
  public wellsGeoJson: any;
  public redSensors: any[];
  public columnDefs: any[];
  public gridApi: any;

  constructor(
    private authenticationService: AuthenticationService,
    private sensorStatusService: SensorStatusService,
    private utilityFunctionsService: UtilityFunctionsService
    ) { }

  ngOnInit(): void {
    this.columnDefs = [
      {
        headerName: '', valueGetter: function (params: any) {
          return { LinkValue: params.data.WellID, LinkDisplay: "View Well", CssClasses: "btn-sm btn-zybach" };
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
        width: 160,
        resizable: true
      },
      { headerName: 'Well Number', field: 'WellRegistrationID', width: 120, sortable: true, filter: true, resizable: true },
      {
        headerName: "AgHub Registered User",
        field: "AgHubRegisteredUser",
        width: 170,
        sortable: true, filter: true, resizable: true
      },
      {
        headerName: "Field Name",
        field: "fieldName",
        width: 115,
        sortable: true, filter: true, resizable: true
      },
      {
        headerName: 'Sensor', valueGetter: function (params: any) {
          if(params.data.SensorName)
          {
            return { LinkValue: params.data.SensorID, LinkDisplay: params.data.SensorName };
          }
          else
          {
            return { LinkValue: null, LinkDisplay: null };
          }
        }, 
        cellRendererFramework: LinkRendererComponent,
        cellRendererParams: { inRouterLink: "/sensors/" },
        comparator: function (id1: any, id2: any) {
          let link1 = id1.LinkValue;
          let link2 = id2.LinkValue;
          if (link1 < link2) {
            return -1;
          }
          if (link1 > link2) {
            return 1;
          }
          return 0;
        },
        filterValueGetter: function (params: any) {
          return params.data.SensorName;
        },
        filter: true,
        resizable: true,
        sortable: true,
        width: 120
      },
      { headerName: 'Last Message Age (Hours)',
        valueGetter: (params) => Math.floor(params.data.MessageAge / 3600),
        filter: 'agNumberColumnFilter',
        sortable: true, resizable: true
      },
      { headerName: 'Sensor Type', field: 'SensorType',
        filterFramework: CustomDropdownFilterComponent,
        filterParams: {
          field: 'SensorType'
        },
        resizable: true, sortable: true
      }
    ];


    this.authenticationService.getCurrentUser().subscribe(currentUser => {
      this.currentUser = currentUser;
      this.wellsObservable = this.sensorStatusService.getSensorStatusByWell().subscribe(wells => {
        this.wellsGeoJson =
        {
          type: "FeatureCollection",
          features:

            wells.map(x => {
              const geoJsonPoint = x.Location;
              geoJsonPoint.properties = {
                wellID: x.WellID,
                wellRegistrationID: x.WellRegistrationID,
                sensors: x.Sensors.filter(y => y.IsActive) || [],
                AgHubRegisteredUser: x.AgHubRegisteredUser,
                fieldName: x.FieldName
              };
              return geoJsonPoint;
            })
        }

        this.redSensors = wells.reduce((sensors: SensorMessageAgeDto[], well: WellWithSensorMessageAgeDto) => sensors.concat(well.Sensors.map(sensor => ({ ...sensor, WellID: well.WellID, WellRegistrationID: well.WellRegistrationID, AgHubRegisteredUser: well.AgHubRegisteredUser, fieldName: well.FieldName }))), []).filter(sensor => sensor.MessageAge > 3600 * 8 && sensor.IsActive);
      })
    });
  }

  ngOnDestroy(): void {
    
    this.wellsObservable.unsubscribe();
  }
  
  public downloadCsv(){
    this.utilityFunctionsService.exportGridToCsv(this.wellsGrid, 'wells.csv', null);
  }
  
  public onGridReady(params) {
    this.gridApi = params.api;
  }

  public onSelectionChanged(event: Event) {
    const selectedNode = this.gridApi.getSelectedNodes()[0];
    if (!selectedNode) {
      // event was fired automatically when we updated the grid after a map click
      return;
    }
    this.gridApi.forEachNode(node => {
      if (node.data.WellRegistrationID === selectedNode.data.WellRegistrationID) {
        node.setSelected(true);
      }
    })
    this.wellMap.selectWell(selectedNode.data.WellRegistrationID);
  }

  public onMapSelection(wellRegistrationID: string) {
    this.gridApi.deselectAll();
    let firstRecordMadeVisible = false;
    this.gridApi.forEachNode(node => {
      if (node.data.WellRegistrationID === wellRegistrationID) {
        node.setSelected(true);
        if (!firstRecordMadeVisible) {
          this.gridApi.ensureIndexVisible(node.rowIndex, "top");
          firstRecordMadeVisible = true;
        }
      }
    })
  }
}

