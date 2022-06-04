import { HttpClientModule } from '@angular/common/http';
import { TestBed, inject, waitForAsync } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { OAuthModule } from 'angular-oauth2-oidc';

import { UnauthenticatedAccessGuard } from './unauthenticated-access.guard';

describe('UnauthenticatedAccessGuard', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [UnauthenticatedAccessGuard],
      imports: [RouterTestingModule, OAuthModule.forRoot(), HttpClientModule]
    });
  });

  it('should ...', inject([UnauthenticatedAccessGuard], (guard: UnauthenticatedAccessGuard) => {
    expect(guard).toBeTruthy();
  }));
});
