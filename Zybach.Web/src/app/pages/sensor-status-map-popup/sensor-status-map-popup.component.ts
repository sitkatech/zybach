import { Component, Input } from '@angular/core';
import { SensorMessageAgeDto } from 'src/app/shared/generated/model/sensor-message-age-dto';

@Component({
  selector: 'zybach-sensor-status-map-popup',
  templateUrl: './sensor-status-map-popup.component.html',
  styleUrls: ['./sensor-status-map-popup.component.scss']
})
export class SensorStatusMapPopupComponent {
  @Input() wellID: number;
  @Input() wellRegistrationID: string;
  @Input() sensors: SensorMessageAgeDto[];
  @Input() AgHubRegisteredUser: string;
  @Input() fieldName: string;

  public math = Math;
}
