import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import moment from 'moment';
import { forkJoin } from 'rxjs';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { SensorStatusService } from 'src/app/services/sensor-status.service';
import { SensorService } from 'src/app/services/sensor.service';
import { UtilityFunctionsService } from 'src/app/services/utility-functions.service';
import { WellService } from 'src/app/services/well.service';
import { InstallationRecordDto } from 'src/app/shared/generated/model/installation-record-dto';
import { SensorSimpleDto } from 'src/app/shared/generated/model/sensor-simple-dto';
import { SensorSummaryDto } from 'src/app/shared/generated/model/sensor-summary-dto';
import { UserDto } from 'src/app/shared/generated/model/user-dto';
import { AlertService } from 'src/app/shared/services/alert.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'zybach-sensor-detail',
  templateUrl: './sensor-detail.component.html',
  styleUrls: ['./sensor-detail.component.scss']
})
export class SensorDetailComponent implements OnInit {
  public sensorID: number;
  public wellID: number;
  
  public currentUser: UserDto;
  public sensor: SensorSimpleDto;

  public installations: InstallationRecordDto[] = [];
  public installationPhotos: Map<string, any[]>; 

  public isLoadingSubmit: boolean = false;

  constructor(
    private authenticationService: AuthenticationService,
    private sensorService: SensorService,
    private sensorStatusService: SensorStatusService,
    private wellService: WellService,
    private route: ActivatedRoute,
    private router: Router,
    private cdr: ChangeDetectorRef,
    private utilityFunctionsService: UtilityFunctionsService,
    private alertService: AlertService
  ) { }

  ngOnInit(): void {
    this.sensorID = parseInt(this.route.snapshot.paramMap.get("id"));
    this.authenticationService.getCurrentUser().subscribe(currentUser => {
      this.currentUser = currentUser;
      
      this.getSensorDetails();
    })
  }
  
  private getSensorDetails() {
    this.sensorService.getSensorByID(this.sensorID).subscribe(sensor => {
      this.sensor = sensor;
      this.wellID = this.sensor.WellID;

      this.getInstallationDetails();
    });
  }

  private getInstallationDetails() {
    this.wellService.getInstallationDetails(this.wellID).subscribe(installations => {
      this.installations = installations.filter(x => x.SensorSerialNumber == this.sensor.SensorName);
      this.installationPhotos = new Map();
      for (const installation of installations) {
        const installationPhotoDataUrls = this.getPhotoRecords(installation);
        this.installationPhotos.set(installation.InstallationCanonicalName, installationPhotoDataUrls);
      }
    });
  }

  private getPhotoRecords(installation: InstallationRecordDto) : any[]{
    const installationPhotoDataUrls = [];
    const photos = installation.Photos;

    const photoObservables = photos.map(
      photo => this.wellService.getPhoto(this.wellID, installation.InstallationCanonicalName, photo)
    );

    let foundPhoto = false;

    forkJoin(photoObservables).subscribe((blobs: any[]) => {
      for (const blob of blobs){
        if (!blob){
          // we're ignoring errors that come from the GO request by sending 204 in their place, 
          // so skip through this iteration if the current blob is null/undefined
          continue; 
        }

        foundPhoto = true;

        const reader = new FileReader();
        reader.readAsDataURL(blob);
        reader.onloadend = () => {
          // result includes identifier 'data:image/png;base64,' plus the base64 data
          installationPhotoDataUrls.push({path: reader.result});
        };
      }
    });

    return installationPhotoDataUrls;
  }

  public getInstallationDate(installation: InstallationRecordDto) {
    if (!installation.Date) {
      return ""
    }
    const time = moment(installation.Date)
    const timepiece = time.format('h:mm a');
    return time.format('M/D/yyyy ') + timepiece;
  }
  
  public toggleIsActive(isActive : boolean): void {
    this.isLoadingSubmit = true;
    var sensorSummaryDto = new SensorSummaryDto();
    sensorSummaryDto.SensorName = this.sensor.SensorName;
    sensorSummaryDto.SensorID = this.sensor.SensorID;
    sensorSummaryDto.IsActive = isActive
    this.sensorStatusService.updateSensorIsActive(sensorSummaryDto)
      .subscribe(response => {
        this.isLoadingSubmit = false;
        this.sensor.IsActive = isActive;
        // this.alertService.pushAlert(new Alert(`Sensor '${sensorName}' now ${isActive ? "enabled" : "disabled"}`, AlertContext.Success));
      }
        ,
        error => {
          this.isLoadingSubmit = false;
          this.cdr.detectChanges();
        }
      );
  }

  public wellInGeoOptixUrl(): string {
    return `${environment.geoOptixWebUrl}/program/main/(inner:site)?projectCName=water-data-program&siteCName=${this.sensor.WellRegistrationID}`;
  }


}
