import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WaterQualityInspectionEditComponent } from './water-quality-inspection-edit.component';

describe('WaterQualityInspectionEditComponent', () => {
  let component: WaterQualityInspectionEditComponent;
  let fixture: ComponentFixture<WaterQualityInspectionEditComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WaterQualityInspectionEditComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(WaterQualityInspectionEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
