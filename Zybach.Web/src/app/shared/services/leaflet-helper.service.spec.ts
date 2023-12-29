import { TestBed } from '@angular/core/testing';

import { LeafletHelperService } from './leaflet-helper.service';

describe('LeafletHelperService', () => {
  let service: LeafletHelperService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(LeafletHelperService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
