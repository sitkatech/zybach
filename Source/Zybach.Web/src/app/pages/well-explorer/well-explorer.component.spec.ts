import { HttpClientModule } from '@angular/common/http';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { OAuthModule } from 'angular-oauth2-oidc';

import { WellExplorerComponent } from './well-explorer.component';

describe('WellExplorerComponent', () => {
  let component: WellExplorerComponent;
  let fixture: ComponentFixture<WellExplorerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WellExplorerComponent ],
      imports: [ RouterTestingModule, OAuthModule.forRoot(), HttpClientModule ],
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WellExplorerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
