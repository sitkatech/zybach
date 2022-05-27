import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WaterQualityInspectionNewComponent } from './water-quality-inspection-new.component';

describe('WaterQualityInspectionNewComponent', () => {
  let component: WaterQualityInspectionNewComponent;
  let fixture: ComponentFixture<WaterQualityInspectionNewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WaterQualityInspectionNewComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(WaterQualityInspectionNewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
