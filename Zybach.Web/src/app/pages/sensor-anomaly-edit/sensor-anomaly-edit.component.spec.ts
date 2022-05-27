import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SensorAnomalyEditComponent } from './sensor-anomaly-edit.component';

describe('SensorAnomalyEditComponent', () => {
  let component: SensorAnomalyEditComponent;
  let fixture: ComponentFixture<SensorAnomalyEditComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SensorAnomalyEditComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SensorAnomalyEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
