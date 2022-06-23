import { ChangeDetectorRef, Component, OnInit, ViewChild } from '@angular/core';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { SensorSimpleDto } from 'src/app/shared/generated/model/sensor-simple-dto';
import { UserDto } from 'src/app/shared/generated/model/user-dto';
import { WellWaterLevelMapSummaryDto } from 'src/app/shared/generated/model/well-water-level-map-summary-dto';
import { CustomRichTextTypeEnum } from 'src/app/shared/generated/enum/custom-rich-text-type-enum';
import { WellWaterLevelMapComponent } from '../well-water-level-map/well-water-level-map.component';
import { forkJoin } from 'rxjs';
import { MapDataService } from 'src/app/shared/generated/api/map-data.service';
import { WellService } from 'src/app/shared/generated/api/well.service';
import { SensorChartDataDto } from 'src/app/shared/generated/model/sensor-chart-data-dto';
@Component({
  selector: 'zybach-water-level-explorer',
  templateUrl: './water-level-explorer.component.html',
  styleUrls: ['./water-level-explorer.component.scss']
})
export class WaterLevelExplorerComponent implements OnInit {
  @ViewChild("wellMap") wellMap: WellWaterLevelMapComponent;

  public currentUser: UserDto;
  public wells: WellWaterLevelMapSummaryDto[];
  public selectedWell: WellWaterLevelMapSummaryDto;
  public selectedSensors: SensorSimpleDto[];
  public wellsGeoJson: any;
  
  public richTextTypeID : number = CustomRichTextTypeEnum.WaterLevelExplorerMap;
  public disclaimerRichTextTypeID : number = CustomRichTextTypeEnum.WaterLevelExplorerMapDisclaimer;

  public sensorChartData: SensorChartDataDto;
  
  constructor(
    private authenticationService: AuthenticationService,
    private cdr: ChangeDetectorRef,
    private mapDataService: MapDataService,
    private wellService: WellService  ) { }

  ngOnInit(): void {
    this.authenticationService.getCurrentUser().subscribe(currentUser => {
      this.currentUser = currentUser;
      this.mapDataService.mapDataWellsWithWellPressureSensorGet().subscribe(wells => {
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

  public onMapSelection(wellID: number) {
    this.selectedWell = this.wells.find(x => x.WellID === wellID);
    forkJoin({
      sensorChartData: this.wellService.wellsWellIDWaterLevelSensorsGet(wellID),
    })
    .subscribe(({sensorChartData}) => {
      this.sensorChartData = sensorChartData;
      this.cdr.detectChanges();
    });
  }
}
