import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'zybach-well-map-popup',
  templateUrl: './well-map-popup.component.html',
  styleUrls: ['./well-map-popup.component.scss']
})
export class WellMapPopupComponent implements OnInit {

  @Input() registrationID: string;
  @Input() sensorTypes: Array<string>;

  constructor() { }

  ngOnInit(): void {
  }
}
