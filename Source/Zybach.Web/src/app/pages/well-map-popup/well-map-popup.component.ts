import { Component, OnInit, Input } from '@angular/core';
import { SensorSummaryDto } from 'src/app/shared/generated/model/sensor-summary-dto';

@Component({
  selector: 'zybach-well-map-popup',
  templateUrl: './well-map-popup.component.html',
  styleUrls: ['./well-map-popup.component.scss']
})
export class WellMapPopupComponent implements OnInit {

  @Input() registrationID: string;
  @Input() sensors: SensorSummaryDto[];
  @Input() AgHubRegisteredUser: string;
  @Input() fieldName: string;


  constructor() { }

  ngOnInit(): void {
  }

  getDataSourceDisplay(): string {
    return `Data Source${this.sensors.length > 1 ? "s":""}:`;
  }

  getSensorDisplay(sensor: SensorSummaryDto): string {
    if (sensor.SensorName == null || sensor.SensorName == undefined) {
      return sensor.SensorType;
    }

    return `${sensor.SensorType} (${sensor.SensorName})`;
  }
}
