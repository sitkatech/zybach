import { Component, OnInit, HostListener, ChangeDetectorRef, OnDestroy } from '@angular/core';
import { CookieStorageService } from '../../services/cookies/cookie-storage.service';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { UserDetailedDto } from '../../models';
import { UserService } from 'src/app/services/user/user.service';
import { AlertService } from '../../services/alert.service';
import { Alert } from '../../models/alert';
import { environment } from 'src/environments/environment';
import { AlertContext } from '../../models/enums/alert-context.enum';

@Component({
    selector: 'header-nav',
    templateUrl: './header-nav.component.html',
    styleUrls: ['./header-nav.component.scss']
})

export class HeaderNavComponent implements OnInit, OnDestroy {
    private watchUserChangeSubscription: any;
    private currentUser: UserDetailedDto;

    windowWidth: number;

    @HostListener('window:resize', ['$event'])
    resize() {
        this.windowWidth = window.innerWidth;
    }

    constructor(
        private authenticationService: AuthenticationService,
        private cookieStorageService: CookieStorageService,
        private userService: UserService,
        private alertService: AlertService,
        private cdr: ChangeDetectorRef) {
    }

    ngOnInit() {
        this.watchUserChangeSubscription = this.authenticationService.currentUserSetObservable.subscribe(currentUser => {
            this.currentUser = currentUser;

            if (currentUser && this.isAdministrator()){
                this.userService.getUnassignedUserReport().subscribe(report =>{
                    if (report.Count > 0){
                        this.alertService.pushAlert(new Alert(`There are ${report.Count} users who are waiting for you to configure their account. <a href='/users'>Manage Users</a>.`, AlertContext.Info, true, AlertService.USERS_AWAITING_CONFIGURATION));
                    }
                })
            }
        });
    }

    ngOnDestroy() {
        this.watchUserChangeSubscription.unsubscribe();
        this.authenticationService.dispose();
        this.cdr.detach();
    }

    public isAuthenticated(): boolean {
        return this.authenticationService.isAuthenticated();
    }

    public isAdministrator(): boolean {
        return this.authenticationService.isUserAnAdministrator(this.currentUser);
    }

    public isUnassigned(): boolean{
        return this.authenticationService.isUserUnassigned(this.currentUser);
    }

    public isUnassignedOrDisabled(): boolean{
        return this.authenticationService.isUserUnassigned(this.currentUser) || this.authenticationService.isUserRoleDisabled(this.currentUser);
    }

    public getUserName() {
        return this.currentUser ? this.currentUser.FullName
            : null;
    }

    public login(): void {
        this.authenticationService.login();
    }

    public logout(): void {
        this.authenticationService.logout();

        setTimeout(() => {
            this.cookieStorageService.removeAll();
            this.cdr.detectChanges();
        });
    }


    public platformShortName(): string{
        return environment.platformShortName;
    }

    public leadOrganizationHomeUrl(): string{
        return environment.leadOrganizationHomeUrl;
    }

    public leadOrganizationLogoSrc(): string{
        return `assets/main/logos/${environment.leadOrganizationLogoFilename}`;
    }
}
