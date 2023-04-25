import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SensorHealthCheckComponent } from './sensor-health-check.component';

describe('SensorHealthCheckComponent', () => {
  let component: SensorHealthCheckComponent;
  let fixture: ComponentFixture<SensorHealthCheckComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SensorHealthCheckComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SensorHealthCheckComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
