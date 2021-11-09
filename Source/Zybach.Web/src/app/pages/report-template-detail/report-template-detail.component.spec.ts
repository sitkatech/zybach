import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { ReportTemplateDetailComponent } from './report-template-detail.component';

describe('ReportTemplateDetailComponent', () => {
  let component: ReportTemplateDetailComponent;
  let fixture: ComponentFixture<ReportTemplateDetailComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ ReportTemplateDetailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ReportTemplateDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
