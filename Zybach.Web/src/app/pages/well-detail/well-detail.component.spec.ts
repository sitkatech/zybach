import { HttpClientModule } from '@angular/common/http';
import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { OAuthModule } from 'angular-oauth2-oidc';

import { WellDetailComponent } from './well-detail.component';

describe('WellDetailComponent', () => {
  let component: WellDetailComponent;
  let fixture: ComponentFixture<WellDetailComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ WellDetailComponent ],
      imports: [RouterTestingModule, OAuthModule.forRoot(), HttpClientModule]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WellDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
