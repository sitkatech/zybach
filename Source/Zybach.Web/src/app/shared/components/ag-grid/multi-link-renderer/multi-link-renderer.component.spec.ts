import { HttpClientModule } from '@angular/common/http';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { OAuthModule } from 'angular-oauth2-oidc';

import { MultiLinkRendererComponent } from './multi-link-renderer.component';

describe('MultiLinkRendererComponent', () => {
  let component: MultiLinkRendererComponent;
  let fixture: ComponentFixture<MultiLinkRendererComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MultiLinkRendererComponent ],
      imports: [ RouterTestingModule, OAuthModule.forRoot(), HttpClientModule ],
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MultiLinkRendererComponent);
    component = fixture.componentInstance;
    component.params = {
      value: {
        links: []
      }
    }
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
