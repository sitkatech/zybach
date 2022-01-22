import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WaterQualityInspectionUpsertComponent } from './water-quality-inspection-upsert.component';

describe('WaterQualityInspectionUpsertComponent', () => {
  let component: WaterQualityInspectionUpsertComponent;
  let fixture: ComponentFixture<WaterQualityInspectionUpsertComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WaterQualityInspectionUpsertComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(WaterQualityInspectionUpsertComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
