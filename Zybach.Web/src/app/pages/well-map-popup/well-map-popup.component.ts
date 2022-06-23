import { Component, OnInit, Input } from '@angular/core';
import { SensorSimpleDto } from 'src/app/shared/generated/model/sensor-simple-dto';

@Component({
  selector: 'zybach-well-map-popup',
  templateUrl: './well-map-popup.component.html',
  styleUrls: ['./well-map-popup.component.scss']
})
export class WellMapPopupComponent implements OnInit {

  @Input() wellID: number;
  @Input() wellRegistrationID: string;
  @Input() sensors: SensorSimpleDto[];
  @Input() AgHubRegisteredUser: string;
  @Input() fieldName: string;


  constructor() { }

  ngOnInit(): void {
  }

  getDataSourceDisplay(): string {
    return `Data Source${this.sensors.length > 1 ? "s":""}:`;
  }

  getSensorDisplay(sensor: SensorSimpleDto): string {
    if (sensor.SensorName == null || sensor.SensorName == undefined) {
      return sensor.SensorTypeName;
    }

    return `${sensor.SensorTypeName} (${sensor.SensorName})`;
  }
}
