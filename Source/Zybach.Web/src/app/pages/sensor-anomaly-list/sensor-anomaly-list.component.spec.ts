import { ComponentFixture, TestBed } from '@angular/core/testing';
import { SensorAnomalyListComponent } from './sensor-anomaly-list.component';

describe('SensorAnomalyListComponent', () => {
  let component: SensorAnomalyListComponent;
  let fixture: ComponentFixture<SensorAnomalyListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SensorAnomalyListComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SensorAnomalyListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
