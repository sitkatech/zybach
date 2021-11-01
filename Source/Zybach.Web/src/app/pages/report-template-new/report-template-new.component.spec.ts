import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ReportTemplateNewComponent } from './report-template-new.component';

describe('ReportTemplateNewComponent', () => {
  let component: ReportTemplateNewComponent;
  let fixture: ComponentFixture<ReportTemplateNewComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ReportTemplateNewComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ReportTemplateNewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
