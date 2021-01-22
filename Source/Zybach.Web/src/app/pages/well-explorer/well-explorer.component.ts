import { Component, OnDestroy, OnInit } from '@angular/core';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { WellService } from 'src/app/services/well.service';
import { UserDto } from 'src/app/shared/models/generated/user-dto';
import { WellWithSensorSummaryDto } from 'src/app/shared/models/well-with-sensor-summary-dto';

@Component({
  selector: 'zybach-well-explorer',
  templateUrl: './well-explorer.component.html',
  styleUrls: ['./well-explorer.component.scss']
})
export class WellExplorerComponent implements OnInit, OnDestroy {
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

}
