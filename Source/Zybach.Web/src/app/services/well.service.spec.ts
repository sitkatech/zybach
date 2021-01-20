import { TestBed } from '@angular/core/testing';

import { WellService } from './well.service';

describe('WellService', () => {
  let service: WellService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(WellService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
