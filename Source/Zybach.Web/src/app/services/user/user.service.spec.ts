import { HttpClientModule } from '@angular/common/http';
import { TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { OAuthModule } from 'angular-oauth2-oidc';

import { UserService } from './user.service';

describe('UserService', () => {
  beforeEach(() => TestBed.configureTestingModule({
    imports: [RouterTestingModule, OAuthModule.forRoot(), HttpClientModule]
  }));

  it('should be created', () => {
    const service: UserService= TestBed.get(UserService);
    expect(service).toBeTruthy();
  });
});
