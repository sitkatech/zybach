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
  @Input() AgHubRegisteredUser: string;
  @Input() fieldName: string;

  constructor() { }

  ngOnInit(): void {
  }

  getDataSourceDisplay(): string {
    return `Sensor${this.sensors.length != 1 ? "s":""}:`;
  }

  getSensorDisplay(sensor: SensorSummaryDto): string {
    if (sensor.SensorName == null || sensor.SensorName == undefined) {
      return sensor.SensorType;
    }

    return `${sensor.SensorType} (${sensor.SensorName})`;
  }

  getLastMessageReceived(sensor: SensorMessageAgeDto){
    if (sensor.MessageAge === null){
      return 'N/A'
    }

    return `${Math.floor(sensor.MessageAge / 3600)} hours`;
  }
}
