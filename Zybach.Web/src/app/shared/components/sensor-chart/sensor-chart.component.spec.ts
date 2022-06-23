import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { SensorChartComponent } from './sensor-chart.component';

describe('SensorChartComponent', () => {
  let component: SensorChartComponent;
  let fixture: ComponentFixture<SensorChartComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ SensorChartComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SensorChartComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
