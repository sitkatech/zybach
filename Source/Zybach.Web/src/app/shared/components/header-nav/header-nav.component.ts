import { Component, OnInit, HostListener, ChangeDetectorRef, OnDestroy } from '@angular/core';
import { CookieStorageService } from '../../services/cookies/cookie-storage.service';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { UserService } from 'src/app/services/user/user.service';
import { AlertService } from '../../services/alert.service';
import { Alert } from '../../models/alert';
import { environment } from 'src/environments/environment';
import { AlertContext } from '../../models/enums/alert-context.enum';
import { Router } from '@angular/router';
import { SearchService } from 'src/app/services/search.service';
import { UserDto } from '../../generated/model/user-dto';

@Component({
    selector: 'header-nav',
    templateUrl: './header-nav.component.html',
    styleUrls: ['./header-nav.component.scss']
})

export class HeaderNavComponent implements OnInit, OnDestroy {
    
    private currentUser: UserDto;

    searchSuggestions: any[];
    text: string;
    private isSearching: boolean = false;

    windowWidth: number;
    public showCurrentPageHeader: boolean = true;

    @HostListener('window:resize', ['$event'])
    resize() {
        this.windowWidth = window.innerWidth;
    }

    constructor(
        private authenticationService: AuthenticationService,
        private cookieStorageService: CookieStorageService,
        private searchService: SearchService,
        private userService: UserService,
        private alertService: AlertService,
        private cdr: ChangeDetectorRef,
        private router: Router) {
    }

    ngOnInit() {
        this.authenticationService.getCurrentUser().subscribe(currentUser => {
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
        
               this.cdr.detach();
    }

    public isDataProgramCurrentPage(){
        return this.router.url.startsWith('/dashboard') 
        || this.router.url.startsWith('/sensor-status')
        || this.router.url.startsWith('/sensors');
    }

    public isMapCurrentPage(){
        return this.router.url === '/well-map';
    }

    public isChemigationCurrentPage(){
        return this.router.url.startsWith('/chemigation-permits') 
        || this.router.url.startsWith('/chemigation-inspections')
        || this.router.url.startsWith('/water-quality-inspections')
        || this.router.url.startsWith('/water-level-inspections');
    }

    public isHomepageCurrentPage(){
        return this.router.url === '/';
    }

    public toggleCurrentPageHeader(){
        this.showCurrentPageHeader = !this.showCurrentPageHeader;
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
        this.authenticationService.setAuthRedirectUrl(this.router.url);
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

    public search(event) {
        this.isSearching = true;
        this.searchService.getSearchSuggestions(event.query.trim()).subscribe(results => {
            this.searchSuggestions = results;
            this.isSearching = false;
        })
    }

    select(event) {
        this.text = '';
        this.router.navigateByUrl(`/wells/${event.WellID}`);
    }

    //The dropdown closes when we remove focus, so if we go back in and still have text we should show the search suggestions
    reFocus(geoOptixSearch) {
        if (this.text != undefined && this.text != '') {
            geoOptixSearch.show();
        }
    }

    
}
