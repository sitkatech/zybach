import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WaterQualityReportsComponent } from './water-quality-reports.component';

describe('WaterQualityReportsComponent', () => {
  let component: WaterQualityReportsComponent;
  let fixture: ComponentFixture<WaterQualityReportsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WaterQualityReportsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(WaterQualityReportsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
