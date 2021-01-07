import { TestBed } from '@angular/core/testing';

import { ArcService } from './arc.service';

describe('ArcService', () => {
  let service: ArcService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ArcService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
