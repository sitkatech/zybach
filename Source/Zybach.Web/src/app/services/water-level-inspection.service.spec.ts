import { TestBed } from '@angular/core/testing';

import { WaterLevelInspectionService } from './water-level-inspection.service';

describe('WaterLevelInspectionService', () => {
  let service: WaterLevelInspectionService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(WaterLevelInspectionService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
