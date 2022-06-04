import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WaterQualityInspectionListComponent } from './water-quality-inspection-list.component';

describe('WaterQualityInspectionListComponent', () => {
  let component: WaterQualityInspectionListComponent;
  let fixture: ComponentFixture<WaterQualityInspectionListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WaterQualityInspectionListComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(WaterQualityInspectionListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
