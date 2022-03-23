import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbDateAdapter } from '@ng-bootstrap/ng-bootstrap';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { SensorAnomalyService } from 'src/app/services/sensor-anomaly.service';
import { NgbDateAdapterFromString } from 'src/app/shared/components/ngb-date-adapter-from-string';
import { SensorAnomalySimpleDto } from 'src/app/shared/generated/model/sensor-anomaly-simple-dto';
import { SensorAnomalyUpsertDto } from 'src/app/shared/generated/model/sensor-anomaly-upsert-dto';
import { UserDto } from 'src/app/shared/generated/model/user-dto';
import { Alert } from 'src/app/shared/models/alert';
import { AlertContext } from 'src/app/shared/models/enums/alert-context.enum';
import { AlertService } from 'src/app/shared/services/alert.service';

@Component({
  selector: 'zybach-sensor-anomaly-edit',
  templateUrl: './sensor-anomaly-edit.component.html',
  styleUrls: ['./sensor-anomaly-edit.component.scss'],
  providers: [{provide: NgbDateAdapter, useClass: NgbDateAdapterFromString}]
})
export class SensorAnomalyEditComponent implements OnInit {

  private currentUser: UserDto;

  public sensorAnomalyID: number;
  public sensorAnomaly: SensorAnomalySimpleDto;
  public sensorAnomalyModel: SensorAnomalyUpsertDto;

  public isLoadingSubmit = false;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private authenticationService: AuthenticationService,
    private sensorAnomalyService: SensorAnomalyService,
    private alertService: AlertService
  ) { }

  ngOnInit(): void {
    this.authenticationService.getCurrentUser().subscribe(currentUser => {
      this.currentUser = currentUser;

      this.sensorAnomalyID = parseInt(this.route.snapshot.paramMap.get("id"));
      this.sensorAnomalyService.getSensorAnomalyByID(this.sensorAnomalyID).subscribe(sensorAnomaly => {
        this.sensorAnomaly = sensorAnomaly;

        this.sensorAnomalyModel = new SensorAnomalyUpsertDto();
        this.sensorAnomalyModel.SensorAnomalyID = this.sensorAnomaly.SensorAnomalyID;
        this.sensorAnomalyModel.SensorID = this.sensorAnomaly.SensorID;
        this.sensorAnomalyModel.Notes = this.sensorAnomaly.Notes;
        this.sensorAnomalyModel.StartDate = this.sensorAnomaly.StartDate;
        this.sensorAnomalyModel.EndDate = this.sensorAnomaly.EndDate;
      });
    });
  }

  public onSubmit(editSensorAnomalyForm: HTMLFormElement) { 
    this.isLoadingSubmit = true;

    this.sensorAnomalyService.updateSensorAnomaly(this.sensorAnomalyModel).subscribe(() => {
      this.isLoadingSubmit = false;
      this.router.navigateByUrl("/sensor-anomalies").then(x => {
        this.alertService.pushAlert(new Alert("Sensor anomaly report was successfully updated.", AlertContext.Success));
      });
    }, error => {
      this.isLoadingSubmit = false;
      window.scroll(0,0);
    });
  }
}
