import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';

@Component({
  selector: 'zybach-water-year-select',
  templateUrl: './water-year-select.component.html',
  styleUrls: ['./water-year-select.component.scss']
})
export class WaterYearSelectComponent implements OnInit {
  years: Array<number> = new Array<number>();
  @Input() disabled: Boolean;
  @Input() displayAllYearsOption: Boolean = false;
  @Input() selectYearLabel: string = "Viewing year ";

  @Input()
  get selectedYear(): number {
    return this.selectedYearValue
  }

  set selectedYear(val: number) {
    this.selectedYearValue = val;
    this.selectedYearChange.emit(this.selectedYearValue);
  }

  @Input()
  get allYearsSelected(): boolean {
    return this.allYearsSelectedValue;
  }

  set allYearsSelected(val: boolean){
    this.allYearsSelectedValue = val;
    this.allYearsSelectedChange.emit(this.allYearsSelectedValue);
  }

  selectedYearValue: number;
  allYearsSelectedValue: boolean = false;

  @Output() selectedYearChange: EventEmitter<number> = new EventEmitter<number>();
  @Output() allYearsSelectedChange: EventEmitter<boolean> = new EventEmitter<boolean>();

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
