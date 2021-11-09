import { HttpClientModule } from '@angular/common/http';
import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { OAuthModule } from 'angular-oauth2-oidc';

import { TestAPIComponent } from './test-api.component';

import * as config from '../../../assets/config.json';
declare var window: any;

describe('TestAPIComponent', () => {
  let component: TestAPIComponent;
  let fixture: ComponentFixture<TestAPIComponent>;

  beforeEach(waitForAsync(() => {
    window.config = config;
    TestBed.configureTestingModule({
      declarations: [ TestAPIComponent ],
      imports: [ RouterTestingModule, OAuthModule.forRoot(), HttpClientModule ],
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TestAPIComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
