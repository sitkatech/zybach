import { Injectable } from '@angular/core';
import { OAuthService } from 'angular-oauth2-oidc';
import { UserService } from 'src/app/shared/generated/api/user.service';
import { Observable, race, Subject } from 'rxjs';
import { filter, finalize, first, map } from 'rxjs/operators';
import { CookieStorageService } from '../shared/services/cookies/cookie-storage.service';
import { Router } from '@angular/router';
import { RoleEnum } from 'src/app/shared/generated/enum/role-enum';
import { AlertService } from '../shared/services/alert.service';
import { Alert } from '../shared/models/alert';
import { AlertContext } from '../shared/models/enums/alert-context.enum';
import { environment } from 'src/environments/environment';
import { UserCreateDto } from '../shared/generated/model/user-create-dto';
import { UserDto } from '../shared/generated/model/user-dto';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {
  private currentUser: UserDto;

  private _currentUserSetSubject = new Subject<UserDto>();
  private currentUserSetObservable = this._currentUserSetSubject.asObservable();


  constructor(private router: Router,
    private oauthService: OAuthService,
    private cookieStorageService: CookieStorageService,
    private userService: UserService,
    private alertService: AlertService) {
      this.oauthService.events
      .pipe(filter(e => ['discovery_document_loaded'].includes(e.type)))
      .subscribe(e => { 
        this.checkAuthentication();
      });

    this.oauthService.events
      .pipe(filter(e => ['token_received'].includes(e.type)))
      .subscribe(e => { 
        this.checkAuthentication();
        this.oauthService.loadUserProfile();
      });

    this.oauthService.events
      .pipe(filter(e => ['session_terminated', 'session_error', 'token_error', 'token_refresh_error', 'silent_refresh_error', 'token_validation_error'].includes(e.type)))
      .subscribe(e => this.router.navigateByUrl("/"));

    this.oauthService.setupAutomaticSilentRefresh();
  }

  public initialLoginSequence() {
    this.oauthService.loadDiscoveryDocument()
      .then(() => this.oauthService.tryLogin())
      .then(() => Promise.resolve()).catch(() => {});
  }

  public checkAuthentication() {
    if (this.isAuthenticated() && !this.currentUser) {
      console.log("Authenticated but no user found...");
      var claims = this.oauthService.getIdentityClaims();
      this.getUser(claims);
    }
  }

  public getUser(claims: any) {
    var globalID = claims["sub"];

    this.userService.usersUserClaimsGlobalIDGet(globalID).subscribe(
      result => { this.updateUser(result); },
      error => { this.onGetUserError(error, claims) }
    );
  }

  private onGetUserError(error: any, claims: any) {
    if (error.status !== 404) {
      this.alertService.pushAlert(new Alert("There was an error logging into the application.", AlertContext.Danger));
      this.router.navigate(['/']);
    } else {
      this.alertService.clearAlerts();
      const newUser = new UserCreateDto({
        FirstName: claims["given_name"],
        LastName: claims["family_name"],
        Email: claims["email"],
        LoginName: claims["login_name"],
        UserGuid: claims["sub"],
      });

      this.userService.usersPost(newUser).subscribe(user => {
        this.updateUser(user);
      })
    }
  }

  private updateUser(user: UserDto) {
    this.currentUser = user;
    this._currentUserSetSubject.next(this.currentUser);
  }

  public refreshUserInfo(user: UserDto) {
    this.updateUser(user);
  }

  public getCurrentUser(): Observable<UserDto> {
    return race(
      new Observable(subscriber => {
        if (this.currentUser) {
          subscriber.next(this.currentUser);
          subscriber.complete();
        }
      }),
      this.currentUserSetObservable.pipe(first())
    );
  }

  public getCurrentUserID(): Observable<number> {
    return race(
      new Observable(subscriber => {
        if (this.currentUser) {
          subscriber.next(this.currentUser.UserID);
          subscriber.complete();
        }
      }),
      this.currentUserSetObservable.pipe(first(), map(
        (user) => user.UserID
      ))
    );
  }

  public isAuthenticated(): boolean {
    return this.oauthService.hasValidAccessToken();
  }

  public login() {
    this.oauthService.initCodeFlow();
  }

  public createAccount() {
    localStorage.setItem("loginOnReturn", "true");
    window.location.href = `${environment.keystoneAuthConfiguration.issuer}/Account/Register?${this.getClientIDAndRedirectUrlForKeystone()}`;
  }

  public getClientIDAndRedirectUrlForKeystone() {
    return `ClientID=${environment.keystoneAuthConfiguration.clientId}&RedirectUrl=${encodeURIComponent(environment.createAccountRedirectUrl)}`;
  }

  public forcedLogout() {
    sessionStorage["authRedirectUrl"] = window.location.href;
    this.logout();
  }

  public logout() {
    this.oauthService.logOut();

    setTimeout(() => {
      this.cookieStorageService.removeAll();
    });
  }

  public isUserAnAdministrator(user: UserDto): boolean {
    const role = user && user.Role
      ? user.Role.RoleID
      : null;
    return role === RoleEnum.Admin;
  }
  
  public isUserReadOnly(user: UserDto): boolean {
    const role = user && user.Role
    ? user.Role.RoleID
    : null;
    return role === RoleEnum.WaterDataProgramReadOnly;
  }
  
  public isUserNormalOrHigher(user: UserDto): boolean {
    const normalPlusRoles = new Array(RoleEnum.Admin, RoleEnum.Normal);
    const role = user && user.Role
      ? user.Role.RoleID
      : null;
    return normalPlusRoles.includes(role); 
  }

  public doesUserHaveReadOnlyRightsOrHigher(user: UserDto): boolean {
    const readOnlyRoles = new Array(RoleEnum.Admin, RoleEnum.Normal, RoleEnum.WaterDataProgramReadOnly);
    const role = user && user.Role
      ? user.Role.RoleID
      : null;
    return readOnlyRoles.includes(role); 
  }

  public isCurrentUserNormalOrHigher(): boolean {
    return this.isUserNormalOrHigher(this.currentUser);
  }

  public isCurrentUserReadOnly(): boolean {
    return this.isUserReadOnly(this.currentUser);
  }

  public doesCurrentUserhaveReadOnlyRightsOrHigher(): boolean {
    return this.doesUserHaveReadOnlyRightsOrHigher(this.currentUser);
  }

  public isCurrentUserAnAdministrator(): boolean {
    return this.isUserAnAdministrator(this.currentUser);
  }

  public isCurrentUserDisabled(): boolean {
    return this.isUserRoleDisabled(this.currentUser);
  }

  public isUserUnassigned(user: UserDto): boolean {
    const role = user && user.Role
      ? user.Role.RoleID
      : null;
    return role === RoleEnum.Unassigned;
  }

  public isUserRoleDisabled(user: UserDto): boolean {
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

  public getAuthRedirectUrl() {
    return sessionStorage["authRedirectUrl"];
  }

  public setAuthRedirectUrl(url: string) {
    sessionStorage["authRedirectUrl"] = url;
  }

  public clearAuthRedirectUrl() {
    this.setAuthRedirectUrl("");
  }
}
