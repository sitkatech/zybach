import { TestBed } from '@angular/core/testing';

import { ChemigationPermitService } from './chemigation-permit.service';

describe('ChemigationPermitService', () => {
  let service: ChemigationPermitService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ChemigationPermitService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
