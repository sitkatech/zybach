import { Component, Input } from '@angular/core';
import { SensorSimpleDto } from 'src/app/shared/generated/model/sensor-simple-dto';

@Component({
  selector: 'zybach-sensor-status-map-popup',
  templateUrl: './sensor-status-map-popup.component.html',
  styleUrls: ['./sensor-status-map-popup.component.scss']
})
export class SensorStatusMapPopupComponent {
  @Input() wellID: number;
  @Input() wellRegistrationID: string;
  @Input() sensors: SensorSimpleDto[];
  @Input() AgHubRegisteredUser: string;
  @Input() fieldName: string;

  public math = Math;
}
