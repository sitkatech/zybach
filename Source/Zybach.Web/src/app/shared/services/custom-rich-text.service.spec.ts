import { HttpClientModule } from '@angular/common/http';
import { TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { OAuthModule } from 'angular-oauth2-oidc';

import { CustomRichTextService } from './custom-rich-text.service';

describe('CustomRichTextService', () => {
  beforeEach(() => TestBed.configureTestingModule({
    imports: [RouterTestingModule, OAuthModule.forRoot(), HttpClientModule]
  }));

  it('should be created', () => {
    const service: CustomRichTextService = TestBed.get(CustomRichTextService);
    expect(service).toBeTruthy();
  });
});
