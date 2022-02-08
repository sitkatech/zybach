import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WaterLevelInspectionDetailComponent } from './water-level-inspection-detail.component';

describe('WaterLevelInspectionDetailComponent', () => {
  let component: WaterLevelInspectionDetailComponent;
  let fixture: ComponentFixture<WaterLevelInspectionDetailComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WaterLevelInspectionDetailComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(WaterLevelInspectionDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
