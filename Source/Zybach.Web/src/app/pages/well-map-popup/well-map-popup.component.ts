import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'zybach-well-map-popup',
  templateUrl: './well-map-popup.component.html',
  styleUrls: ['./well-map-popup.component.scss']
})
export class WellMapPopupComponent implements OnInit {

  @Input() feature;

  constructor() { }

  ngOnInit(): void {
  }

  public getFeature() {
    console.log(this.feature);
  }

  public getDistinctSensorTypes() : Array<string> {
    return Array.from(new Set(this.feature.properties.sensorTypes));
  }
}
