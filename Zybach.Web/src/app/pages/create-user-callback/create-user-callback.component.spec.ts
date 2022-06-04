import { HttpClientModule } from '@angular/common/http';
import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { OAuthModule } from 'angular-oauth2-oidc';

import { CreateUserCallbackComponent } from './create-user-callback.component';

describe('CreateUserCallbackComponent', () => {
  let component: CreateUserCallbackComponent;
  let fixture: ComponentFixture<CreateUserCallbackComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ CreateUserCallbackComponent ],
      imports: [ RouterTestingModule, OAuthModule.forRoot(), HttpClientModule ],
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateUserCallbackComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
