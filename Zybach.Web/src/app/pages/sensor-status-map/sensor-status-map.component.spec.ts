import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { SensorStatusMapComponent } from './sensor-status-map.component';

describe('SensorStatusMapComponent', () => {
  let component: SensorStatusMapComponent;
  let fixture: ComponentFixture<SensorStatusMapComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ SensorStatusMapComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SensorStatusMapComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
