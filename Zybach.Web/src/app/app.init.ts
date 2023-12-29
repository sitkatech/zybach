import { Injectable } from '@angular/core';;
import { firstValueFrom, of } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
declare var window: any;

@Injectable()
export class AppInitService {

  // This is the method you want to call at bootstrap
  // Important: It should return a Promise
  public init() {
    const environment$ = of(environment).pipe(
      map((environment) => {
      environment.keystoneAuthConfiguration.redirectUri = window.location.origin + environment.keystoneAuthConfiguration.redirectUriRelative;
      environment.keystoneAuthConfiguration.postLogoutRedirectUri = window.location.origin + environment.keystoneAuthConfiguration.postLogoutRedirectUri
      window.config = environment;
    }));
    
    return firstValueFrom(environment$);
  }
}