import { Injectable } from '@angular/core';
import { OAuthService } from 'angular-oauth2-oidc';
import { UserService } from './user/user.service';
import { UserDetailedDto } from '../shared/models';
import { Subject } from 'rxjs';
import { filter } from 'rxjs/operators';
import { CookieStorageService } from '../shared/services/cookies/cookie-storage.service';
import { Router, NavigationEnd, NavigationStart } from '@angular/router';
import { RoleEnum } from '../shared/models/enums/role.enum';
import { AlertService } from '../shared/services/alert.service';
import { Alert } from '../shared/models/alert';
import { AlertContext } from '../shared/models/enums/alert-context.enum';
import { UserCreateDto } from '../shared/models/user/user-create-dto';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {
  private currentUser: UserDetailedDto;

  private getUserObservable: any;

  private _currentUserSetSubject = new Subject<UserDetailedDto>();
  public currentUserSetObservable = this._currentUserSetSubject.asObservable();


  constructor(private router: Router,
    private oauthService: OAuthService,
    private cookieStorageService: CookieStorageService,
    private userService: UserService,
    private alertService: AlertService) {
    this.router.events
      .pipe(filter(e => e instanceof NavigationEnd))
      .subscribe((e: NavigationEnd) => {
        if (this.isAuthenticated()) {
          var claims = this.oauthService.getIdentityClaims();
          var globalID = claims["sub"];

          this.getUserObservable = this.userService.getUserFromGlobalID(globalID).subscribe(result => {
            this.getUserCallback(result);
          }, error => {
            if (error.status !== 404) {
              this.alertService.pushAlert(new Alert("There was an error logging into the application.", AlertContext.Danger));
              this.router.navigate(['/']);
            } else {
              this.alertService.clearAlerts();
              const newUser = new UserCreateDto({
                FirstName: claims["given_name"],
                LastName: claims["family_name"],
                Email: claims["email"],
                RoleID: RoleEnum.Unassigned,
                LoginName: claims["login_name"],
                UserGuid: claims["sub"],
              });

              this.userService.createNewUser(newUser).subscribe(user => {
                this.getUserCallback(user);
              })

            }
          });

        } else {
          this.currentUser = null;
          this._currentUserSetSubject.next(null);
        }
      });

    // check for a currentUser at NavigationStart so that authorization-based guards can work with promises.
    this.router.events
      .pipe(filter(e => e instanceof NavigationStart))
      .subscribe((e: NavigationStart) => {
        if (this.isAuthenticated() && !this.currentUser) {
          var claims = this.oauthService.getIdentityClaims();
          var globalID = claims["sub"];
          console.log("Authenticated but no user found...")
          this.getUserObservable = this.userService.getUserFromGlobalID(globalID).subscribe(user => {
            this.currentUser = user;
            this._currentUserSetSubject.next(this.currentUser);
          });
        }
      })
  }

  private getUserCallback(user: UserDetailedDto) {
    this.currentUser = user;
    this._currentUserSetSubject.next(this.currentUser);
  }

  public refreshUserInfo(user: UserDetailedDto) {
    this.getUserCallback(user);
  }

  dispose() {
    this.getUserObservable.unsubscribe();
  }

  public isAuthenticated(): boolean {
    return this.oauthService.hasValidAccessToken();
  }

  public handleUnauthorized(): void {
    this.logout();
  }

  public login() {
    this.oauthService.initImplicitFlow();
  }

  public createAccount() {
    localStorage.setItem("loginOnReturn", "true");
    const redirectUrl = encodeURIComponent(environment.createAccountRedirectUrl);
    window.location.href = `${environment.createAccountUrl}${redirectUrl}`;
  }

  public logout() {
    this.oauthService.logOut();

    setTimeout(() => {
      this.cookieStorageService.removeAll();
    });
  }

  public isUserAnAdministrator(user: UserDetailedDto): boolean {
    const role = user && user.Role
      ? user.Role.RoleID
      : null;
    return role === RoleEnum.Admin;
  }

  public isCurrentUserAnAdministrator(): boolean {
    return this.isUserAnAdministrator(this.currentUser);
  }

  public isUserUnassigned(user: UserDetailedDto): boolean {
    const role = user && user.Role
      ? user.Role.RoleID
      : null;
    return role === RoleEnum.Unassigned;
  }

  public isUserRoleDisabled(user: UserDetailedDto): boolean {
    const role = user && user.Role
      ? user.Role.RoleID
      : null;
    return role === RoleEnum.Disabled;
  }

  public isCurrentUserNullOrUndefined(): boolean {
    return !this.currentUser;
  }

  public hasCurrentUserAcknowledgedDisclaimer(): boolean {
    return this.currentUser != null && this.currentUser.DisclaimerAcknowledgedDate != null;
  }
}
