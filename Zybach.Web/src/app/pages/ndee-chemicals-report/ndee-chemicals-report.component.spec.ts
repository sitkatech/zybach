import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NdeeChemicalsReportComponent } from './ndee-chemicals-report.component';

describe('NdeeChemicalsReportComponent', () => {
  let component: NdeeChemicalsReportComponent;
  let fixture: ComponentFixture<NdeeChemicalsReportComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ NdeeChemicalsReportComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(NdeeChemicalsReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
