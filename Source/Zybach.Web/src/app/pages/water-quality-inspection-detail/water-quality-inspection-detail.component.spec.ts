import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WaterQualityInspectionDetailComponent } from './water-quality-inspection-detail.component';

describe('WaterQualityInspectionDetailComponent', () => {
  let component: WaterQualityInspectionDetailComponent;
  let fixture: ComponentFixture<WaterQualityInspectionDetailComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WaterQualityInspectionDetailComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(WaterQualityInspectionDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
