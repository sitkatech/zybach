import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SensorDetailComponent } from './sensor-detail.component';

describe('SensorDetailComponent', () => {
  let component: SensorDetailComponent;
  let fixture: ComponentFixture<SensorDetailComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SensorDetailComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SensorDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
