import { TestBed } from '@angular/core/testing';

import { IrrigationUnitService } from './irrigation-unit.service';

describe('IrrigationUnitService', () => {
  let service: IrrigationUnitService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(IrrigationUnitService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
