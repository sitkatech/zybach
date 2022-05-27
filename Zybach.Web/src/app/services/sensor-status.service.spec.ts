import { TestBed } from '@angular/core/testing';

import { SensorStatusService } from './sensor-status.service';

describe('SensorStatusService', () => {
  let service: SensorStatusService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(SensorStatusService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
