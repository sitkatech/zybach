import { Component, OnInit, Input } from '@angular/core';
import { SensorMessageAgeDto, SensorSummaryDto } from 'src/app/shared/models/well-with-sensor-summary-dto';

@Component({
  selector: 'zybach-sensor-status-map-popup',
  templateUrl: './sensor-status-map-popup.component.html',
  styleUrls: ['./sensor-status-map-popup.component.scss']
})
export class SensorStatusMapPopupComponent implements OnInit {

  @Input() registrationID: string;
  @Input() sensors: SensorMessageAgeDto[];

  constructor() { }

  ngOnInit(): void {
  }

  getDataSourceDisplay(): string {
    return `Sensor${this.sensors.length != 1 ? "s":""}:`;
  }

  getSensorDisplay(sensor: SensorSummaryDto): string {
    if (sensor.sensorName == null || sensor.sensorName == undefined) {
      return sensor.sensorType;
    }

    return `${sensor.sensorType} (${sensor.sensorName})`;
  }

  getLastMessageReceived(sensor: SensorMessageAgeDto){
    if (sensor.messageAge === null){
      return 'N/A'
    }

    return `${Math.floor(sensor.messageAge / 60)} min`;
  }
}