import { Component, OnInit, OnDestroy } from '@angular/core';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { RoleEnum } from 'src/app/shared/models/enums/role.enum';
import { environment } from 'src/environments/environment';
import { UserDto } from 'src/app/shared/models/user/user-dto';
import { SlickCarouselModule } from 'ngx-slick-carousel';

@Component({
    selector: 'app-home-index',
    templateUrl: './home-index.component.html',
    styleUrls: ['./home-index.component.scss']
})
export class HomeIndexComponent implements OnInit, OnDestroy {
    public watchUserChangeSubscription: any;
    public currentUser: UserDto;

    public slides = [
        {
            date: "Wednesday, March 2, 2020",
            title: "Join Fix A Leak Week Through March"
        },
        {
            date: "Friday, March 19, 2020",
            title: "Landscape Workshop"
        }
    ];
    public slideConfig = {
        "dots": false,
        "slidesToShow": 2,
        "slidesToScroll": 2,
        "arrows": false,
        "prevArrow": "<button type='button' class='prev'><i class='fas fa-chevron-left' aria-hidden='true'></i></button>",
        "nextArrow": "<button type='button' class='next'><i class='fas fa-chevron-right' aria-hidden='true'></i></button>",
        "responsive": [
            {
                "breakpoint": 991,
                "settings": {
                    "slidesToShow": 1,
                    "slidesToScroll": 1,
                    "dots": true,
                    "arrows": true
                }
            },
            //although we only really have space for one, leave arrows on this size to help show it's a carousel
            {
                "breakpoint": 768,
                "settings": {
                    "slidesToShow": 1,
                    "slidesToScroll": 1,
                    "dots": true
                }
            },
            {
                "breakpoint": 470,
                "settings": {
                    "slidesToShow": 1,
                    "slidesToScroll": 1,
                    "dots": true
                }
            }
        ]

    };

    constructor(private authenticationService: AuthenticationService) {
    }

    public ngOnInit(): void {
        if (localStorage.getItem("loginOnReturn")) {
            localStorage.removeItem("loginOnReturn");
            this.authenticationService.login();
        }
        this.watchUserChangeSubscription = this.authenticationService.currentUserSetObservable.subscribe(currentUser => {
            this.currentUser = currentUser;
        });
    }

    ngOnDestroy(): void {
        this.watchUserChangeSubscription.unsubscribe();
    }

    public userIsUnassigned() {
        if (!this.currentUser) {
            return false; // doesn't exist != unassigned
        }

        return this.currentUser.Role.RoleID === RoleEnum.Unassigned;
    }

    public userRoleIsDisabled() {
        if (!this.currentUser) {
            return false; // doesn't exist != unassigned
        }

        return this.currentUser.Role.RoleID === RoleEnum.Disabled;
    }

    public isUserAnAdministrator() {
        return this.authenticationService.isUserAnAdministrator(this.currentUser);
    }

    public isUserALandowner() {
        return this.authenticationService.isUserALandOwner(this.currentUser);
    }

    public login(): void {
        this.authenticationService.login();
    }

    public createAccount(): void {
        this.authenticationService.createAccount();
    }

    public forgotPasswordUrl(): string {
        return `${environment.keystoneSupportBaseUrl}/ForgotPassword`;
    }

    public forgotUsernameUrl(): string {
        return `${environment.keystoneSupportBaseUrl}/ForgotUsername`;
    }

    public keystoneSupportUrl(): string {
        return `${environment.keystoneSupportBaseUrl}/Support/20`;
    }

    public platformLongName(): string {
        return environment.platformLongName;
    }

    public platformShortName(): string {
        return environment.platformShortName;
    }

    public leadOrganizationShortName(): string {
        return environment.leadOrganizationShortName;
    }

    public leadOrganizationLongName(): string {
        return environment.leadOrganizationLongName;
    }

    public leadOrganizationHomeUrl(): string {
        return environment.leadOrganizationHomeUrl;
    }

    slickInit(e) {
        console.log('slick initialized');
    }

    public breakpoint(e) {
        console.log('breakpoint');
    }

    public afterChange(e) {
        console.log('afterChange');
    }

    public beforeChange(e) {
        console.log('beforeChange');
    }
}
