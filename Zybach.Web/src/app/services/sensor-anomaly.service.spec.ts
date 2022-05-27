import { TestBed } from '@angular/core/testing';

import { SensorAnomalyService } from './sensor-anomaly.service';

describe('SensorAnomalyService', () => {
  let service: SensorAnomalyService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(SensorAnomalyService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
