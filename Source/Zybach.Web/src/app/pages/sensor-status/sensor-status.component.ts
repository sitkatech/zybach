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
  public wells: WellWithSensorSummaryDto[];
  public wellsGeoJson: any;

  constructor(private authenticationService: AuthenticationService,
    private wellService: WellService) { }

  ngOnInit(): void {
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
}
