import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { WellMapPopupComponent } from './well-map-popup.component';

describe('WellMapPopupComponent', () => {
  let component: WellMapPopupComponent;
  let fixture: ComponentFixture<WellMapPopupComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ WellMapPopupComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WellMapPopupComponent);
    component = fixture.componentInstance;
    component.sensorTypes = []
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
