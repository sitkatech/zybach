import { HttpClientModule } from '@angular/common/http';
import { TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { OAuthModule } from 'angular-oauth2-oidc';

import { RoleService } from './role.service';

describe('RoleService', () => {
  beforeEach(() => TestBed.configureTestingModule({
      imports: [ RouterTestingModule, OAuthModule.forRoot(), HttpClientModule ],
  }));

  it('should be created', () => {
    const service: RoleService = TestBed.get(RoleService);
    expect(service).toBeTruthy();
  });
});
