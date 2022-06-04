import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WellInspectionReportsComponent } from './well-inspection-reports.component';

describe('WellInspectionReportsComponent', () => {
  let component: WellInspectionReportsComponent;
  let fixture: ComponentFixture<WellInspectionReportsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WellInspectionReportsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(WellInspectionReportsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
