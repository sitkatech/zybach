import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { ReportTemplateEditComponent } from './report-template-edit.component';

describe('ReportTemplateEditComponent', () => {
  let component: ReportTemplateEditComponent;
  let fixture: ComponentFixture<ReportTemplateEditComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ ReportTemplateEditComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ReportTemplateEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
