import { HttpClientModule } from '@angular/common/http';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { OAuthModule } from 'angular-oauth2-oidc';

import { CustomRichTextComponent } from './custom-rich-text.component';

import * as config from '../../../../assets/config.json';
declare var window: any;

describe('CustomRichTextComponent', () => {
  let component: CustomRichTextComponent;
  let fixture: ComponentFixture<CustomRichTextComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CustomRichTextComponent ],
      imports: [ RouterTestingModule, OAuthModule.forRoot(), HttpClientModule ],
    })
    .compileComponents();
  }));

  beforeEach(() => {
    window.config = config;
    fixture = TestBed.createComponent(CustomRichTextComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
