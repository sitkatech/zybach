import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { SensorStatusMapPopupComponent } from './sensor-status-map-popup.component';

describe('SensorStatusMapPopupComponent', () => {
  let component: SensorStatusMapPopupComponent;
  let fixture: ComponentFixture<SensorStatusMapPopupComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ SensorStatusMapPopupComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SensorStatusMapPopupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
