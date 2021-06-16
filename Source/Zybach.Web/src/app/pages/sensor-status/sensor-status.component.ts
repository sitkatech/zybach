import { literalMap } from '@angular/compiler';
import { Component, OnDestroy, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import * as moment from 'moment';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { SensorStatusService } from 'src/app/services/sensor-status.service';
import { WellService } from 'src/app/services/well.service';
import { LinkRendererComponent } from 'src/app/shared/components/ag-grid/link-renderer/link-renderer.component';
import { DataSourceFilterOption, DataSourceSensorTypeMap } from 'src/app/shared/models/enums/data-source-filter-option.enum';
import { UserDto } from 'src/app/shared/models/generated/user-dto';
import { SensorMessageAgeDto, WellWithSensorMessageAgeDto, WellWithSensorSummaryDto } from 'src/app/shared/models/well-with-sensor-summary-dto';
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
  public columnDefs: any[]

  constructor(private authenticationService: AuthenticationService,
    private sensorStatusService: SensorStatusService) { }

  ngOnInit(): void {
    this.columnDefs = [
      { headerName: 'Well Number', field: 'wellRegistrationID', sortable: true, filter: true, resizable: true },
      { headerName: 'Sensor Number', field: 'sensorName', sortable: true, filter: true, resizable: true},
      { headerName: 'Last Message Age (Hours)', field: 'messageAge', sortable: true, filter: true, resizable: true, valueFormatter: (params) => `${Math.floor(params.value / 3600)} hours` },
      { headerName: 'Sensor Type', field: 'sensorType', sortable: true, filter: true, resizable: true},
    ];


    this.watchUserChangeSubscription = this.authenticationService.currentUserSetObservable.subscribe(currentUser => {
      this.currentUser = currentUser;
      this.wellsObservable = this.sensorStatusService.getSensorStatusByWell().subscribe(wells => {

        this.wellsGeoJson =
        {
          type: "FeatureCollection",
          features:

            wells.map(x => {
              const geoJsonPoint = x.location;
              geoJsonPoint.properties = {
                wellRegistrationID: x.wellRegistrationID,
                sensors: x.sensors
              };
              return geoJsonPoint;
            })
        }

        this.redSensors = wells.reduce((sensors, well) => sensors.concat(well.sensors.map(sensor => ({ ...sensor, wellRegistrationID: well.wellRegistrationID }))), []).filter(sensor => sensor.messageAge > 3600 * 8);
console.log(this.redSensors);

      })
    });
  }

  ngOnDestroy(): void {
    this.watchUserChangeSubscription.unsubscribe();
    this.wellsObservable.unsubscribe();
  }
}

