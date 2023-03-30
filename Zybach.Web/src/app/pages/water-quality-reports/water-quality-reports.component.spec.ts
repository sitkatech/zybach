import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WaterQualityReoprtsComponent } from './water-quality-reports.component';

describe('WaterQualityReportsComponent', () => {
  let component: WaterQualityReoprtsComponent;
  let fixture: ComponentFixture<WaterQualityReoprtsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WaterQualityReoprtsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(WaterQualityReoprtsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
