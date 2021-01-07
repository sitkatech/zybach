import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { NotFoundComponent, UnauthenticatedComponent, SubscriptionInsufficientComponent } from './shared/pages';
import { UnauthenticatedAccessGuard } from './shared/guards/unauthenticated-access/unauthenticated-access.guard';
import { ManagerOnlyGuard } from "./shared/guards/unauthenticated-access/manager-only-guard";
import { AcknowledgedDisclaimerGuard } from "./shared/guards/acknowledged-disclaimer-guard";
import { UserListComponent } from './pages/user-list/user-list.component';
import { HomeIndexComponent } from './pages/home/home-index/home-index.component';
import { UserDetailComponent } from './pages/user-detail/user-detail.component';
import { UserInviteComponent } from './pages/user-invite/user-invite.component';
import { UserEditComponent } from './pages/user-edit/user-edit.component';
import { LoginCallbackComponent } from './pages/login-callback/login-callback.component';
import { HelpComponent } from './pages/help/help.component';
import { CreateUserCallbackComponent } from './pages/create-user-callback/create-user-callback.component';
import { AboutComponent } from './pages/about/about.component';
import { DisclaimerComponent } from './pages/disclaimer/disclaimer.component';
import { WatershedListComponent } from './pages/watershed-list/watershed-list.component';
import { WatershedDetailComponent } from './pages/watershed-detail/watershed-detail.component';
import { FieldDefinitionListComponent } from './pages/field-definition-list/field-definition-list.component';
import { FieldDefinitionEditComponent } from './pages/field-definition-edit/field-definition-edit.component';
import { TrainingComponent } from './pages/training/training.component';

const routes: Routes = [
  { path: "labels-and-definitions/:id", component: FieldDefinitionEditComponent, canActivate: [UnauthenticatedAccessGuard, ManagerOnlyGuard, AcknowledgedDisclaimerGuard] },
  { path: "labels-and-definitions", component: FieldDefinitionListComponent, canActivate: [UnauthenticatedAccessGuard, ManagerOnlyGuard, AcknowledgedDisclaimerGuard] },
  { path: "watersheds", component: WatershedListComponent, canActivate: [UnauthenticatedAccessGuard, ManagerOnlyGuard, AcknowledgedDisclaimerGuard] },
  { path: "watersheds/:id", component: WatershedDetailComponent, canActivate: [UnauthenticatedAccessGuard, ManagerOnlyGuard, AcknowledgedDisclaimerGuard] },
  { path: "users", component: UserListComponent, canActivate: [UnauthenticatedAccessGuard, ManagerOnlyGuard, AcknowledgedDisclaimerGuard]},
  { path: "users/:id", component: UserDetailComponent, canActivate: [UnauthenticatedAccessGuard, ManagerOnlyGuard, AcknowledgedDisclaimerGuard] },
  { path: "users/:id/edit", component: UserEditComponent, canActivate: [UnauthenticatedAccessGuard, ManagerOnlyGuard, AcknowledgedDisclaimerGuard] },
  { path: "invite-user/:userID", component: UserInviteComponent, canActivate: [UnauthenticatedAccessGuard, ManagerOnlyGuard, AcknowledgedDisclaimerGuard] },
  { path: "invite-user", component: UserInviteComponent, canActivate: [UnauthenticatedAccessGuard, ManagerOnlyGuard, AcknowledgedDisclaimerGuard] },
  { path: "", component: HomeIndexComponent},
  { path: "disclaimer", component: DisclaimerComponent },
  { path: "disclaimer/:forced", component: DisclaimerComponent },
  { path: "help", component: HelpComponent },
  { path: "training", component: TrainingComponent},
  { path: "platform-overview", component: AboutComponent},
  { path: "login-callback", component: LoginCallbackComponent },
  { path: "create-user-callback", component: CreateUserCallbackComponent },
  { path: "not-found", component: NotFoundComponent },
  { path: 'subscription-insufficient', component: SubscriptionInsufficientComponent },
  { path: 'unauthenticated', component: UnauthenticatedComponent },
  { path: '**', component: NotFoundComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
