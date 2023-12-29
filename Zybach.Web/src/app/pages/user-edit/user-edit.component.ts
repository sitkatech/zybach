import { Component, OnInit, ChangeDetectionStrategy, ChangeDetectorRef, OnDestroy } from '@angular/core';
import { UserService } from 'src/app/shared/generated/api/user.service';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { Router, ActivatedRoute } from '@angular/router';
import { RoleService } from 'src/app/shared/generated/api/role.service';
import { forkJoin } from 'rxjs';
import { AlertService } from 'src/app/shared/services/alert.service';
import { Alert } from 'src/app/shared/models/alert';
import { AlertContext } from 'src/app/shared/models/enums/alert-context.enum';
import { RoleDto } from 'src/app/shared/generated/model/role-dto';
import { UserDto } from 'src/app/shared/generated/model/user-dto';
import { UserUpsertDto } from 'src/app/shared/generated/model/user-upsert-dto';
import { RoleEnum } from 'src/app/shared/generated/enum/role-enum';

@Component({
  selector: 'zybach-user-edit',
  templateUrl: './user-edit.component.html',
  styleUrls: ['./user-edit.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class UserEditComponent implements OnInit, OnDestroy {
  
  private currentUser: UserDto;

  public userID: number;
  public user: UserDto;
  public model: UserUpsertDto;
  public roles: Array<RoleDto>;
  public isLoadingSubmit: boolean = false;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private authenticationService: AuthenticationService,
    private userService: UserService,
    private roleService: RoleService,
    private cdr: ChangeDetectorRef,
    private alertService: AlertService
  ) {
  }

  ngOnInit() {
    this.authenticationService.getCurrentUser().subscribe(currentUser => {
      this.currentUser = currentUser;
      if (!this.authenticationService.isUserAnAdministrator(this.currentUser)) {
        this.router.navigateByUrl("/not-found")
          .then();
        return;
      }

      this.userID = parseInt(this.route.snapshot.paramMap.get("id"));

      forkJoin([
        this.userService.usersUserIDGet(this.userID),
        this.roleService.rolesGet()
      ]).subscribe(([user, roles]) => {
        this.user = user instanceof Array
          ? null
          : user as UserDto;

        this.roles = roles.sort((a, b) => {
          return a.RoleDisplayName > b.RoleDisplayName ? 1 : a.RoleDisplayName < b.RoleDisplayName ? -1 : 0;
        });

        this.model = new UserUpsertDto();
        this.model.RoleID = user.Role.RoleID;
        this.model.ReceiveSupportEmails = user.ReceiveSupportEmails;
        this.model.PerformsChemigationInspections = user.PerformsChemigationInspections;
        this.cdr.detectChanges();
      });
    });
  }

  ngOnDestroy() {
    this.cdr.detach();
  }

  onSubmit(editUserForm: HTMLFormElement): void {
    this.isLoadingSubmit = true;

    this.userService.usersUserIDPut(this.userID, this.model)
      .subscribe(response => {
        this.isLoadingSubmit = false;
        this.router.navigateByUrl("/users/" + this.userID).then(x => {
          this.alertService.pushAlert(new Alert("The user was successfully updated.", AlertContext.Success));
        });
      }
        ,
        error => {
          this.isLoadingSubmit = false;
          this.cdr.detectChanges();
        }
      );
  }

  selectedRoleIsAdmin(): boolean {
    return this.model.RoleID == RoleEnum.Admin;
  }

  checkReceiveSupportEmails(): void {
    if (this.model.RoleID != 1) {
      this.model.ReceiveSupportEmails = false;
    }
  }
}