import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ReportTemplateDetailComponent } from './report-template-detail.component';

describe('ReportTemplateDetailComponent', () => {
  let component: ReportTemplateDetailComponent;
  let fixture: ComponentFixture<ReportTemplateDetailComponent>;

  beforeEach(async(() => {
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
