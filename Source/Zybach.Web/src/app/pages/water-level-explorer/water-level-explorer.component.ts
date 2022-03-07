import { DatePipe } from '@angular/common';
import { Component, OnInit, ViewChild } from '@angular/core';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { WellService } from 'src/app/services/well.service';
import { UserDto } from 'src/app/shared/generated/model/user-dto';
import { WellWithSensorSummaryDto } from 'src/app/shared/generated/model/well-with-sensor-summary-dto';
import { CustomRichTextType } from 'src/app/shared/models/enums/custom-rich-text-type.enum';
import { WellMapComponent } from '../well-map/well-map.component';

@Component({
  selector: 'zybach-water-level-explorer',
  templateUrl: './water-level-explorer.component.html',
  styleUrls: ['./water-level-explorer.component.scss']
})
export class WaterLevelExplorerComponent implements OnInit {
  @ViewChild("wellMap") wellMap: WellMapComponent;

  public currentUser: UserDto;
  public wellsObservable: any;
  public wells: WellWithSensorSummaryDto[];
  public wellsGeoJson: any;
  
  public richTextTypeID : number = CustomRichTextType.WaterLevelExplorerMap;
  public disclaimerRichTextTypeID : number = CustomRichTextType.WaterLevelExplorerMapDisclaimer;

  constructor(
    private authenticationService: AuthenticationService,
    private datePipe: DatePipe,
    private wellService: WellService
  ) { }

  ngOnInit(): void {
    this.authenticationService.getCurrentUser().subscribe(currentUser => {
      this.currentUser = currentUser;
      this.wellsObservable = this.wellService.getWellsMapData().subscribe(wells => {
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
      });
    });
  }

}
