import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';

@Component({
  selector: 'zybach-water-year-select',
  templateUrl: './water-year-select.component.html',
  styleUrls: ['./water-year-select.component.scss']
})
export class WaterYearSelectComponent implements OnInit {
  years: Array<number> = new Array<number>();
  @Input() disabled: Boolean;
  @Input() selectYearLabel: string = "Viewing year";

  @Input()
  get selectedYear(): number {
    return this.selectedYearValue
  }

  set selectedYear(val: number) {
    this.selectedYearValue = val;
    this.selectedYearChange.emit(this.selectedYearValue);
  }

  selectedYearValue: number;

  @Output() selectedYearChange: EventEmitter<number> = new EventEmitter<number>();
  

  constructor() { }

  ngOnInit() {
    this.years = []
    const currentDate = new Date();
    const currentYear = currentDate.getFullYear();
    for (var year = currentYear; year >= 2019; year--){
      this.years.push(year);
    }
  }

}
