import { DecimalPipe } from '@angular/common';
import { Component, OnInit, Input } from '@angular/core';
import { SensorMessageAgeDto } from 'src/app/shared/generated/model/sensor-message-age-dto';
import { SensorSimpleDto } from 'src/app/shared/generated/model/sensor-simple-dto';

@Component({
  selector: 'zybach-sensor-status-map-popup',
  templateUrl: './sensor-status-map-popup.component.html',
  styleUrls: ['./sensor-status-map-popup.component.scss']
})
export class SensorStatusMapPopupComponent implements OnInit {

  @Input() wellID: number;
  @Input() wellRegistrationID: string;
  @Input() sensors: SensorMessageAgeDto[];
  @Input() AgHubRegisteredUser: string;
  @Input() fieldName: string;

  constructor(private decimalPipe: DecimalPipe) { }

  ngOnInit(): void {
  }

  getDataSourceDisplay(): string {
    return `Sensor${this.sensors.length != 1 ? "s":""}:`;
  }

  getSensorDisplay(sensor: SensorSimpleDto): string {
    if (sensor.SensorName == null || sensor.SensorName == undefined) {
      return sensor.SensorTypeName;
    }

    return `${sensor.SensorTypeName} (${sensor.SensorName})`;
  }

  getLastMessageReceived(sensor: SensorMessageAgeDto){
    if (sensor.MessageAge === null){
      return 'N/A'
    }

    return `${Math.floor(sensor.MessageAge / 3600)} hours`;
  }

  getLastVoltageReading(sensor: SensorMessageAgeDto){
    if (sensor.LastVoltageReading === null){
      return 'N/A'
    }

    return `${this.decimalPipe.transform(sensor.LastVoltageReading, "1.0-0")} millivolts`;
  }
}
