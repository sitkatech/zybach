import { HttpClientModule } from '@angular/common/http';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { OAuthModule } from 'angular-oauth2-oidc';

import { FieldDefinitionListComponent } from './field-definition-list.component';

describe('FieldDefinitionListComponent', () => {
  let component: FieldDefinitionListComponent;
  let fixture: ComponentFixture<FieldDefinitionListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FieldDefinitionListComponent ],
      imports: [ RouterTestingModule, OAuthModule.forRoot(), HttpClientModule ],
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FieldDefinitionListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
