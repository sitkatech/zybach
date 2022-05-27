import { TestBed } from '@angular/core/testing';

import { ChemigationInspectionService } from './chemigation-inspection.service';

describe('ChemigationInspectionService', () => {
  let service: ChemigationInspectionService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ChemigationInspectionService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
