import { HttpClientModule } from '@angular/common/http';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { OAuthModule } from 'angular-oauth2-oidc';

import { UserInviteComponent } from './user-invite.component';

import * as config from '../../../assets/config.json';
declare var window: any;

describe('UserInviteComponent', () => {
  let component: UserInviteComponent;
  let fixture: ComponentFixture<UserInviteComponent>;

  beforeEach(async(() => {
    window.config = config;
    TestBed.configureTestingModule({
      declarations: [ UserInviteComponent ],
      imports: [RouterTestingModule, OAuthModule.forRoot(), HttpClientModule]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UserInviteComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
