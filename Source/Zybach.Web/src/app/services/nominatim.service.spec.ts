import { HttpClientModule } from '@angular/common/http';
import { TestBed } from '@angular/core/testing';
import { OAuthModule } from 'angular-oauth2-oidc';

import { NominatimService } from './nominatim.service';

describe('NominatimService', () => {
  let service: NominatimService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [OAuthModule.forRoot(), HttpClientModule]
    });
    service = TestBed.inject(NominatimService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
