import { TestBed } from '@angular/core/testing';

import { WaterQualityInspectionService } from './water-quality-inspection.service';

describe('WaterQualityInspectionService', () => {
  let service: WaterQualityInspectionService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(WaterQualityInspectionService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
