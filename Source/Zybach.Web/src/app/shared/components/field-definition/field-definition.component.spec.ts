import { HttpClientModule } from '@angular/common/http';
import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { OAuthModule } from 'angular-oauth2-oidc';

import { FieldDefinitionComponent } from './field-definition.component';

import * as config from '../../../../assets/config.json';
declare var window: any;

describe('FieldDefinitionComponent', () => {
  let component: FieldDefinitionComponent;
  let fixture: ComponentFixture<FieldDefinitionComponent>;

  beforeEach(waitForAsync(() => {
    window.config = config;
    TestBed.configureTestingModule({
      declarations: [ FieldDefinitionComponent ],
      imports: [ RouterTestingModule, OAuthModule.forRoot(), HttpClientModule ],
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FieldDefinitionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
