import { Component, OnDestroy, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { PropertiesPlugin } from '@microsoft/applicationinsights-properties-js';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { SensorStatusService } from 'src/app/services/sensor-status.service';
import { LinkRendererComponent } from 'src/app/shared/components/ag-grid/link-renderer/link-renderer.component';
import { UserDto } from 'src/app/shared/models/generated/user-dto';
import { SensorMessageAgeDto, WellWithSensorMessageAgeDto } from 'src/app/shared/models/well-with-sensor-summary-dto';
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

  constructor(private authenticationService: AuthenticationService,
    private sensorStatusService: SensorStatusService) { }

  ngOnInit(): void {
    this.columnDefs = [
      {
        headerName: '', valueGetter: function (params: any) {
          return { LinkValue: params.data.WellRegistrationID, LinkDisplay: "View Well", CssClasses: "btn-sm btn-zybach" };
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
      { headerName: 'Well Number', field: 'WellRegistrationID', sortable: true, filter: true, resizable: true },
      {
        headerName: "Landowner",
        field: "landownerName",
        width: 125,
        sortable: true, filter: true, resizable: true
      },
      {
        headerName: "Field Name",
        field: "fieldName",
        width: 115,
        sortable: true, filter: true, resizable: true
      },
      { headerName: 'Sensor Number', field: 'SensorName', sortable: true, filter: true, resizable: true},
      { headerName: 'Last Message Age (Hours)', sortable: true, filter: true, resizable: true, valueGetter: (params) => Math.floor(params.data.MessageAge / 3600)},
      { headerName: 'Sensor Type', field: 'SensorType', sortable: true, filter: true, resizable: true},
    ];


    this.watchUserChangeSubscription = this.authenticationService.currentUserSetObservable.subscribe(currentUser => {
      this.currentUser = currentUser;
      this.wellsObservable = this.sensorStatusService.getSensorStatusByWell().subscribe(wells => {
console.log(wells);
        this.wellsGeoJson =
        {
          type: "FeatureCollection",
          features:

            wells.map(x => {
              const geoJsonPoint = x.Location;
              geoJsonPoint.properties = {
                wellRegistrationID: x.WellRegistrationID,
                sensors: x.Sensors || [],
                landownerName: x.LandownerName,
                fieldName: x.FieldName
              };
              return geoJsonPoint;
            })
        }

        console.log(this.wellsGeoJson);

        this.redSensors = wells.reduce((sensors: SensorMessageAgeDto[], well: WellWithSensorMessageAgeDto) => sensors.concat(well.Sensors.map(sensor => ({ ...sensor, WellRegistrationID: well.WellRegistrationID, landownerName: well.LandownerName, fieldName: well.FieldName }))), []).filter(sensor => sensor.MessageAge > 3600 * 8);
        console.log(this.redSensors);
      })
    });
  }

  ngOnDestroy(): void {
    this.watchUserChangeSubscription.unsubscribe();
    this.wellsObservable.unsubscribe();
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

  
  public onGridReady(params) {
    this.gridApi = params.api;
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

