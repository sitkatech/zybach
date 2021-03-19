import { TestBed } from '@angular/core/testing';

import { ManagerDashboardService } from './manager-dashboard.service';

describe('ManagerDashboardService', () => {
  let service: ManagerDashboardService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ManagerDashboardService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
