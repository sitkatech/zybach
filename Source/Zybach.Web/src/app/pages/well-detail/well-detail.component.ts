import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import moment from 'moment';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { WellService } from 'src/app/services/well.service';
import { environment } from 'src/environments/environment';
import { AlertService } from 'src/app/shared/services/alert.service';
import { Alert } from 'src/app/shared/models/alert';
import { UserDto } from 'src/app/shared/generated/model/user-dto';
import { WellDetailDto } from 'src/app/shared/generated/model/well-detail-dto';
import { InstallationRecordDto } from 'src/app/shared/generated/model/installation-record-dto';
import { UtilityFunctionsService } from 'src/app/services/utility-functions.service';

@Component({
  selector: 'zybach-well-detail',
  templateUrl: './well-detail.component.html',
  styleUrls: ['./well-detail.component.scss']
})

export class WellDetailComponent implements OnInit {
  public watchUserChangeSubscription: any;

  currentUser: UserDto;
  well: WellDetailDto;

  installations: InstallationRecordDto[] = [];
  installationPhotos: Map<string, any[]>; 
  wellID: number;
  wellRegistrationID: string;
  sensors: any;

  photoDataUrl: string | ArrayBuffer;

  public isLoadingSubmit: boolean = false;

  constructor(
    private wellService: WellService,
    private authenticationService: AuthenticationService,
    private route: ActivatedRoute,
    private router: Router,
    private cdr: ChangeDetectorRef,
    private utilityFunctionsService: UtilityFunctionsService,
    private alertService: AlertService
  ) {
    // force route reload whenever params change;
    this.router.routeReuseStrategy.shouldReuseRoute = () => false;
  }

  ngOnInit(): void {
    this.wellID = parseInt(this.route.snapshot.paramMap.get("id"));
    this.authenticationService.getCurrentUser().subscribe(currentUser => {
      this.currentUser = currentUser;
      this.getWellDetails();
    })
  }

  getWellDetails(){
    this.wellService.getWellDetails(this.wellID).subscribe((well: WellDetailDto)=>{
      this.well = well;
      this.wellRegistrationID = well.WellRegistrationID;
      this.cdr.detectChanges();
    })
  }


  // getDataSourcesLabel() {
  //   let plural = true;
  //   let sensorCount = this.getSensorTypes().size;
  //   if ((sensorCount == 0 && this.well.HasElectricalData) || (sensorCount == 1 && !this.well.HasElectricalData)) {
  //     plural = false;
  //   }

  //   return `Data Source${plural ? "s": ""}: `
  // }
  
  // wellInGeoOptixUrl(): string {
  //   return `${environment.geoOptixWebUrl}/program/main/(inner:site)?projectCName=water-data-program&siteCName=${this.wellRegistrationID}`;
  // }

  // getSensorTypes() {
  //   return new Set(this.well.Sensors.map(sensor => {return sensor.SensorType}));
  // }

  // getLastReadingDate() {
  //   if (!this.well.LastReadingDate) {
  //     return ""
  //   }
  //   const time = moment(this.well.LastReadingDate)
  //   //const timepiece = time.format('h:mm a');
  //   return time.format('M/D/yyyy');// + timepiece;
  // }

  // getInstallationDate(installation: InstallationRecordDto) {
  //   if (!installation.Date) {
  //     return ""
  //   }
  //   const time = moment(installation.Date)
  //   const timepiece = time.format('h:mm a');
  //   return time.format('M/D/yyyy ') + timepiece;
  // }

}
