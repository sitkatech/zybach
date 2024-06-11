import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FlowTestReportListComponent } from './flow-test-report-list.component';

describe('FlowTestReportListComponent', () => {
  let component: FlowTestReportListComponent;
  let fixture: ComponentFixture<FlowTestReportListComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [FlowTestReportListComponent]
    });
    fixture = TestBed.createComponent(FlowTestReportListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
