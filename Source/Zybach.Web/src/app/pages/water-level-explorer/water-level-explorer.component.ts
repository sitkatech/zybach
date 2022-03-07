import { DatePipe } from '@angular/common';
import { Component, OnInit, ViewChild } from '@angular/core';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { WellService } from 'src/app/services/well.service';
import { UserDto } from 'src/app/shared/generated/model/user-dto';
import { WellWaterLevelMapSummaryDto } from 'src/app/shared/generated/model/well-water-level-map-summary-dto';
import { CustomRichTextType } from 'src/app/shared/models/enums/custom-rich-text-type.enum';
import { WellMapComponent } from '../well-map/well-map.component';
import { WellWaterLevelMapComponent } from '../well-water-level-map/well-water-level-map.component';

@Component({
  selector: 'zybach-water-level-explorer',
  templateUrl: './water-level-explorer.component.html',
  styleUrls: ['./water-level-explorer.component.scss']
})
export class WaterLevelExplorerComponent implements OnInit {
  @ViewChild("wellMap") wellMap: WellWaterLevelMapComponent;

  public currentUser: UserDto;
  public wells: WellWaterLevelMapSummaryDto[];
  public wellsGeoJson: any;
  
  public richTextTypeID : number = CustomRichTextType.WaterLevelExplorerMap;
  public disclaimerRichTextTypeID : number = CustomRichTextType.WaterLevelExplorerMapDisclaimer;

  public isDisplayingWaterLevelPanel : boolean = true;
  
  constructor(
    private authenticationService: AuthenticationService,
    private datePipe: DatePipe,
    private wellService: WellService
  ) { }

  ngOnInit(): void {
    this.authenticationService.getCurrentUser().subscribe(currentUser => {
      this.currentUser = currentUser;
      this.wellService.getWellsWithPressureSensorMapData().subscribe(wells => {
        this.wells = wells;
        this.wellsGeoJson =
        {
          type: "FeatureCollection",
          features:

            wells.filter(x => x.Location != null && x.Location != undefined).map(x => {
              const geoJsonPoint = x.Location;
              geoJsonPoint.properties = {
                wellID: x.WellID,
                wellRegistrationID: x.WellRegistrationID,
                sensors: x.Sensors || []
              };
              return geoJsonPoint;
            })
        }
      });
    });
  }

  public onMapSelection(wellRegistrationID: string) {
    // this.gridApi.deselectAll();
    // this.gridApi.forEachNode(node => {
    //   if (node.data.WellRegistrationID === wellRegistrationID) {
    //     node.setSelected(true);
    //     this.gridApi.ensureIndexVisible(node.rowIndex, "top")
    //   }
    // })
  }

}
