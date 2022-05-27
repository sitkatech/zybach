import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ClearinghouseWqiReportComponent } from './clearinghouse-wqi-report.component';

describe('ClearinghouseWqiReportComponent', () => {
  let component: ClearinghouseWqiReportComponent;
  let fixture: ComponentFixture<ClearinghouseWqiReportComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ClearinghouseWqiReportComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ClearinghouseWqiReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
