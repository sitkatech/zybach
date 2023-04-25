import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription, timer } from 'rxjs';
import { switchMap, tap } from 'rxjs/operators';
import { SensorService } from 'src/app/shared/generated/api/sensor.service';
import { CustomRichTextTypeEnum } from 'src/app/shared/generated/enum/custom-rich-text-type-enum';

@Component({
  selector: 'zybach-sensor-health-check',
  templateUrl: './sensor-health-check.component.html',
  styleUrls: ['./sensor-health-check.component.scss']
})
export class SensorHealthCheckComponent implements OnInit, OnDestroy {

  public sensorName: string;
  public sensorNameInput: string;

  public eventMessage: JSON | string;
  public receivedDate: Date;
  public refreshSubscription: Subscription;
  public lastUpdated: Date;
  public recentPulseCutoff: Date; 

  public customRichTextTypeID = CustomRichTextTypeEnum.SensorHealthCheck;

  constructor(
    private sensorService: SensorService
  ) {}

  ngOnInit(): void {
    this.recentPulseCutoff = new Date();
    this.recentPulseCutoff.setHours(this.recentPulseCutoff.getHours() - 6); 
  }

  ngOnDestroy(): void {
    this.refreshSubscription.unsubscribe();
  }

  public getSensorPulse() {
    this.eventMessage = null;

    this.refreshSubscription?.unsubscribe();
    this.sensorName = this.sensorNameInput;

    this.refreshSubscription = timer(0, 30000).pipe(
      switchMap(() => this.sensorService.sensorsSensorNamePulseGet(this.sensorName)),
      tap(() => this.lastUpdated = new Date)
    ).subscribe(paigeWirelessPulseDto => {
      this.receivedDate = new Date(paigeWirelessPulseDto?.ReceivedDate);
      
      try {
        this.eventMessage = JSON.parse(paigeWirelessPulseDto?.EventMessage);
      } catch {
        this.eventMessage = paigeWirelessPulseDto?.EventMessage;
      }
    });
  }
}
