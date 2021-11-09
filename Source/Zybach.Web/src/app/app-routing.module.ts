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
import { FieldDefinitionListComponent } from './pages/field-definition-list/field-definition-list.component';
import { FieldDefinitionEditComponent } from './pages/field-definition-edit/field-definition-edit.component';
import { TrainingComponent } from './pages/training/training.component';
import { TestAPIComponent } from './pages/test-api/test-api.component';
import { WellExplorerComponent } from './pages/well-explorer/well-explorer.component';
import { WellDetailComponent } from './pages/well-detail/well-detail.component';
import { DashboardComponent } from './pages/dashboard/dashboard.component';
import { RobustReviewScenarioComponent } from './pages/robust-review-scenario/robust-review-scenario.component';
import { SensorStatusComponent } from './pages/sensor-status/sensor-status.component';
import { ChemigationLandingComponent } from './pages/chemigation-landing/chemigation-landing.component';
import { WellNewComponent } from './pages/well-new/well-new.component';
import { ReportsListComponent } from './pages/reports-list/reports-list.component';
import { ReportTemplateDetailComponent } from './pages/report-template-detail/report-template-detail.component';
import { ReportTemplateEditComponent } from './pages/report-template-edit/report-template-edit.component';
import { ChemigationPermitListComponent } from './pages/chemigation-permit-list/chemigation-permit-list.component';
import { ChemigationNewPermitComponent } from './pages/chemigation-new-permit/chemigation-new-permit.component';
import { ChemigationPermitDetailComponent } from './pages/chemigation-permit-detail/chemigation-permit-detail.component';

const routes: Routes = [
  { path: "test-api", component: TestAPIComponent, canActivate: [UnauthenticatedAccessGuard, ManagerOnlyGuard, AcknowledgedDisclaimerGuard] },
  { path: "well-map", component: WellExplorerComponent, canActivate: [UnauthenticatedAccessGuard, AcknowledgedDisclaimerGuard] },
  { path: "dashboard", component: DashboardComponent, canActivate: [UnauthenticatedAccessGuard, AcknowledgedDisclaimerGuard] },
  { path: "sensor-status", component: SensorStatusComponent, canActivate: [UnauthenticatedAccessGuard, AcknowledgedDisclaimerGuard] },
  { path: "new-well", component: WellNewComponent, canActivate: [UnauthenticatedAccessGuard, ManagerOnlyGuard, AcknowledgedDisclaimerGuard] },
  { path: "wells/:wellRegistrationID", component: WellDetailComponent, canActivate: [UnauthenticatedAccessGuard, AcknowledgedDisclaimerGuard] },
  { path: "robust-review-scenario",  component: RobustReviewScenarioComponent, canActivate: [UnauthenticatedAccessGuard,AcknowledgedDisclaimerGuard]},
  { path: "chemigation",  component: ChemigationLandingComponent, canActivate: [UnauthenticatedAccessGuard, AcknowledgedDisclaimerGuard]},
  { path: "reports",  component: ReportsListComponent, canActivate: [UnauthenticatedAccessGuard, ManagerOnlyGuard, AcknowledgedDisclaimerGuard]},
  { path: "reports/new",  component: ReportTemplateEditComponent, canActivate: [UnauthenticatedAccessGuard, ManagerOnlyGuard, AcknowledgedDisclaimerGuard]},
  { path: "reports/:id", component: ReportTemplateDetailComponent, canActivate: [UnauthenticatedAccessGuard, ManagerOnlyGuard, AcknowledgedDisclaimerGuard] },
  { path: "reports/:id/edit", component: ReportTemplateEditComponent, canActivate: [UnauthenticatedAccessGuard, ManagerOnlyGuard, AcknowledgedDisclaimerGuard] },
  { path: "chemigation-permits",  component: ChemigationPermitListComponent, canActivate: [UnauthenticatedAccessGuard, AcknowledgedDisclaimerGuard]},
  { path: "chemigation-permits/:permit-number",  component: ChemigationPermitDetailComponent, canActivate: [UnauthenticatedAccessGuard, AcknowledgedDisclaimerGuard]},
  { path: "chemigation-permits/new",  component: ChemigationNewPermitComponent, canActivate: [UnauthenticatedAccessGuard, ManagerOnlyGuard, AcknowledgedDisclaimerGuard]},
  { path: "labels-and-definitions/:id", component: FieldDefinitionEditComponent, canActivate: [UnauthenticatedAccessGuard, ManagerOnlyGuard, AcknowledgedDisclaimerGuard] },
  { path: "labels-and-definitions", component: FieldDefinitionListComponent, canActivate: [UnauthenticatedAccessGuard, ManagerOnlyGuard, AcknowledgedDisclaimerGuard] },
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
  { path: "signin-oidc", component: LoginCallbackComponent },
  { path: "create-user-callback", component: CreateUserCallbackComponent },
  { path: "not-found", component: NotFoundComponent },
  { path: 'subscription-insufficient', component: SubscriptionInsufficientComponent },
  { path: 'unauthenticated', component: UnauthenticatedComponent },
  { path: '**', component: NotFoundComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { relativeLinkResolution: 'legacy' })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
