import { HttpClientModule } from '@angular/common/http';
import { TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { OAuthModule } from 'angular-oauth2-oidc';

import { WellService } from './well.service';

describe('WellService', () => {
  let service: WellService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [ RouterTestingModule, OAuthModule.forRoot(), HttpClientModule ],
    });
    service = TestBed.inject(WellService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
