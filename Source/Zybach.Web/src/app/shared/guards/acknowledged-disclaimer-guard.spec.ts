import { HttpClientModule } from '@angular/common/http';
import { TestBed, inject, waitForAsync } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { OAuthModule } from 'angular-oauth2-oidc';

import { AcknowledgedDisclaimerGuard } from './acknowledged-disclaimer-guard';

describe('AcknowledgedDisclaimerGuardGuard', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [AcknowledgedDisclaimerGuard],
      imports: [RouterTestingModule, OAuthModule.forRoot(), HttpClientModule]
    });
  });

  it('should ...', inject([AcknowledgedDisclaimerGuard], (guard: AcknowledgedDisclaimerGuard) => {
    expect(guard).toBeTruthy();
  }));
});
