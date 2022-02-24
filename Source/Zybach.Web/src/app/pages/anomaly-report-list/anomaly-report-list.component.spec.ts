import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AnomalyReportListComponent } from './anomaly-report-list.component';

describe('AnomalyReportListComponent', () => {
  let component: AnomalyReportListComponent;
  let fixture: ComponentFixture<AnomalyReportListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AnomalyReportListComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AnomalyReportListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
