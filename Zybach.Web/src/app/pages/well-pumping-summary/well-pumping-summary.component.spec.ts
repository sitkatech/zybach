import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WellPumpingSummaryComponent } from './well-pumping-summary.component';

describe('WellPumpingSummaryComponent', () => {
  let component: WellPumpingSummaryComponent;
  let fixture: ComponentFixture<WellPumpingSummaryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WellPumpingSummaryComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(WellPumpingSummaryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
