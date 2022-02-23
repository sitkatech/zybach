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
import { WellExplorerComponent } from './pages/well-explorer/well-explorer.component';
import { WellDetailComponent } from './pages/well-detail/well-detail.component';
import { DashboardComponent } from './pages/dashboard/dashboard.component';
import { RobustReviewScenarioComponent } from './pages/robust-review-scenario/robust-review-scenario.component';
import { SensorStatusComponent } from './pages/sensor-status/sensor-status.component';
import { WellNewComponent } from './pages/well-new/well-new.component';
import { ReportsListComponent } from './pages/reports-list/reports-list.component';
import { ReportTemplateDetailComponent } from './pages/report-template-detail/report-template-detail.component';
import { ReportTemplateEditComponent } from './pages/report-template-edit/report-template-edit.component';
import { ChemigationPermitListComponent } from './pages/chemigation-permit-list/chemigation-permit-list.component';
import { ChemigationNewPermitComponent } from './pages/chemigation-new-permit/chemigation-new-permit.component';
import { ChemigationPermitDetailComponent } from './pages/chemigation-permit-detail/chemigation-permit-detail.component';
import { ChemigationPermitEditComponent } from './pages/chemigation-permit-edit/chemigation-permit-edit.component';
import { ChemigationPermitAddRecordComponent } from './pages/chemigation-permit-add-record/chemigation-permit-add-record.component';
import { ChemigationPermitEditRecordComponent } from './pages/chemigation-permit-edit-record/chemigation-permit-edit-record.component';
import { NdeeChemicalsReportComponent } from './pages/ndee-chemicals-report/ndee-chemicals-report.component';
import { ChemigationPermitReportsComponent } from './pages/chemigation-permit-reports/chemigation-permit-reports.component';
import { ChemigationInspectionsListComponent } from './pages/chemigation-inspections-list/chemigation-inspections-list.component';
import { ChemigationInspectionNewComponent } from './pages/chemigation-inspection-new/chemigation-inspection-new.component';
import { ChemigationInspectionEditComponent } from './pages/chemigation-inspection-edit/chemigation-inspection-edit.component';
import { WaterQualityInspectionListComponent } from './pages/water-quality-inspection-list/water-quality-inspection-list.component';
import { WaterQualityInspectionDetailComponent } from './pages/water-quality-inspection-detail/water-quality-inspection-detail.component';
import { WaterQualityInspectionNewComponent } from './pages/water-quality-inspection-new/water-quality-inspection-new.component';
import { WaterQualityInspectionEditComponent } from './pages/water-quality-inspection-edit/water-quality-inspection-edit.component';
import { WaterLevelInspectionListComponent } from './pages/water-level-inspection-list/water-level-inspection-list.component';
import { WellContactEditComponent } from './pages/well-contact-edit/well-contact-edit.component';
import { WellParticipationEditComponent } from './pages/well-participation-edit/well-participation-edit.component';
import { WellRegistrationIdEditComponent } from './pages/well-registration-id-edit/well-registration-id-edit.component';
import { ClearinghouseWqiReportComponent } from './pages/clearinghouse-wqi-report/clearinghouse-wqi-report.component';
import { SensorListComponent } from './pages/sensor-list/sensor-list.component';
import { SensorDetailComponent } from './pages/sensor-detail/sensor-detail.component';
import { WaterLevelInspectionDetailComponent } from './pages/water-level-inspection-detail/water-level-inspection-detail.component';
import { WaterLevelInspectionNewComponent } from './pages/water-level-inspection-new/water-level-inspection-new.component';
import { WaterLevelInspectionEditComponent } from './pages/water-level-inspection-edit/water-level-inspection-edit.component';
import { WellInspectionReportsComponent } from './pages/well-inspection-reports/well-inspection-reports.component';
import { ReadOnlyGuard } from "./shared/guards/unauthenticated-access/read-only-guard";
import { NormalPlusGuard } from './shared/guards/unauthenticated-access/normal-plus-guard';

const routes: Routes = [
  { path: "well-map", component: WellExplorerComponent, canActivate: [UnauthenticatedAccessGuard, ReadOnlyGuard, AcknowledgedDisclaimerGuard] },
  { path: "dashboard", component: DashboardComponent, canActivate: [UnauthenticatedAccessGuard, ReadOnlyGuard, AcknowledgedDisclaimerGuard] },
  { path: "sensors", component: SensorListComponent, canActivate: [UnauthenticatedAccessGuard, ReadOnlyGuard, AcknowledgedDisclaimerGuard] },
  { path: "sensors/:id", component: SensorDetailComponent, canActivate: [UnauthenticatedAccessGuard, ReadOnlyGuard, AcknowledgedDisclaimerGuard] },
  { path: "sensor-status", component: SensorStatusComponent, canActivate: [UnauthenticatedAccessGuard, ReadOnlyGuard, AcknowledgedDisclaimerGuard] },
  { path: "new-well", component: WellNewComponent, canActivate: [UnauthenticatedAccessGuard, NormalPlusGuard, AcknowledgedDisclaimerGuard] },
  { path: "wells/:id", component: WellDetailComponent, canActivate: [UnauthenticatedAccessGuard, ReadOnlyGuard, AcknowledgedDisclaimerGuard] },
  { path: "wells/:id/new-water-level-inspection", component: WaterLevelInspectionNewComponent, canActivate: [UnauthenticatedAccessGuard, NormalPlusGuard, AcknowledgedDisclaimerGuard]},
  { path: "wells/:id/new-water-quality-inspection", component: WaterQualityInspectionNewComponent, canActivate: [UnauthenticatedAccessGuard, NormalPlusGuard, AcknowledgedDisclaimerGuard]},
  { path: "wells/:id/edit-registration-id", component: WellRegistrationIdEditComponent, canActivate: [UnauthenticatedAccessGuard, NormalPlusGuard, AcknowledgedDisclaimerGuard] },
  { path: "wells/:id/edit-contact", component: WellContactEditComponent, canActivate: [UnauthenticatedAccessGuard, NormalPlusGuard, AcknowledgedDisclaimerGuard] },
  { path: "wells/:id/edit-participation", component: WellParticipationEditComponent, canActivate: [UnauthenticatedAccessGuard, NormalPlusGuard, AcknowledgedDisclaimerGuard] },
  { path: "robust-review-scenario",  component: RobustReviewScenarioComponent, canActivate: [UnauthenticatedAccessGuard, ManagerOnlyGuard, AcknowledgedDisclaimerGuard]},
  { path: "reports",  component: ReportsListComponent, canActivate: [UnauthenticatedAccessGuard, ManagerOnlyGuard, AcknowledgedDisclaimerGuard]},
  { path: "reports/chemigation-permit-reports",  component: ChemigationPermitReportsComponent, canActivate: [UnauthenticatedAccessGuard, NormalPlusGuard, AcknowledgedDisclaimerGuard]},
  { path: "reports/clearinghouse-report",  component: ClearinghouseWqiReportComponent, canActivate: [UnauthenticatedAccessGuard, NormalPlusGuard, AcknowledgedDisclaimerGuard]},
  { path: "reports/ndee-report",  component: NdeeChemicalsReportComponent, canActivate: [UnauthenticatedAccessGuard, NormalPlusGuard, AcknowledgedDisclaimerGuard]},
  { path: "reports/well-inspections-report",  component: WellInspectionReportsComponent, canActivate: [UnauthenticatedAccessGuard, NormalPlusGuard, AcknowledgedDisclaimerGuard]},
  { path: "reports/new",  component: ReportTemplateEditComponent, canActivate: [UnauthenticatedAccessGuard, ManagerOnlyGuard, AcknowledgedDisclaimerGuard]},
  { path: "reports/:id", component: ReportTemplateDetailComponent, canActivate: [UnauthenticatedAccessGuard, ManagerOnlyGuard, AcknowledgedDisclaimerGuard] },
  { path: "reports/:id/edit", component: ReportTemplateEditComponent, canActivate: [UnauthenticatedAccessGuard, ManagerOnlyGuard, AcknowledgedDisclaimerGuard] },
  { path: "chemigation-inspections",  component: ChemigationInspectionsListComponent, canActivate: [UnauthenticatedAccessGuard, NormalPlusGuard, AcknowledgedDisclaimerGuard]},
  { path: "chemigation-inspections/:inspection-id",  component: ChemigationInspectionEditComponent, canActivate: [UnauthenticatedAccessGuard, NormalPlusGuard, AcknowledgedDisclaimerGuard]},
  { path: "chemigation-permits",  component: ChemigationPermitListComponent, canActivate: [UnauthenticatedAccessGuard, NormalPlusGuard, AcknowledgedDisclaimerGuard]},
  { path: "chemigation-permits/new",  component: ChemigationNewPermitComponent, canActivate: [UnauthenticatedAccessGuard, NormalPlusGuard, AcknowledgedDisclaimerGuard]},
  { path: "chemigation-permits/:permit-number",  component: ChemigationPermitDetailComponent, canActivate: [UnauthenticatedAccessGuard, NormalPlusGuard, AcknowledgedDisclaimerGuard]},
  { path: "chemigation-permits/:permit-number/edit",  component: ChemigationPermitEditComponent, canActivate: [UnauthenticatedAccessGuard, NormalPlusGuard, AcknowledgedDisclaimerGuard]},
  { path: "chemigation-permits/:permit-number/add-record",  component: ChemigationPermitAddRecordComponent, canActivate: [UnauthenticatedAccessGuard, NormalPlusGuard, AcknowledgedDisclaimerGuard]},
  { path: "chemigation-permits/:permit-number/:record-year/edit",  component: ChemigationPermitEditRecordComponent, canActivate: [UnauthenticatedAccessGuard, NormalPlusGuard, AcknowledgedDisclaimerGuard]},
  { path: "chemigation-permits/:permit-number/:record-year/add-inspection",  component: ChemigationInspectionNewComponent, canActivate: [UnauthenticatedAccessGuard, NormalPlusGuard, AcknowledgedDisclaimerGuard]},
  { path: "labels-and-definitions/:id", component: FieldDefinitionEditComponent, canActivate: [UnauthenticatedAccessGuard, ManagerOnlyGuard, AcknowledgedDisclaimerGuard] },
  { path: "labels-and-definitions", component: FieldDefinitionListComponent, canActivate: [UnauthenticatedAccessGuard, ManagerOnlyGuard, AcknowledgedDisclaimerGuard] },
  { path: "users", component: UserListComponent, canActivate: [UnauthenticatedAccessGuard, ManagerOnlyGuard, AcknowledgedDisclaimerGuard]},
  { path: "users/:id", component: UserDetailComponent, canActivate: [UnauthenticatedAccessGuard, ManagerOnlyGuard, AcknowledgedDisclaimerGuard] },
  { path: "users/:id/edit", component: UserEditComponent, canActivate: [UnauthenticatedAccessGuard, ManagerOnlyGuard, AcknowledgedDisclaimerGuard] },
  { path: "invite-user/:userID", component: UserInviteComponent, canActivate: [UnauthenticatedAccessGuard, ManagerOnlyGuard, AcknowledgedDisclaimerGuard] },
  { path: "invite-user", component: UserInviteComponent, canActivate: [UnauthenticatedAccessGuard, ManagerOnlyGuard, AcknowledgedDisclaimerGuard] },
  { path: "water-quality-inspections",  component: WaterQualityInspectionListComponent, canActivate: [UnauthenticatedAccessGuard, NormalPlusGuard, AcknowledgedDisclaimerGuard]},
  { path: "water-quality-inspections/new",  component: WaterQualityInspectionNewComponent, canActivate: [UnauthenticatedAccessGuard, NormalPlusGuard, AcknowledgedDisclaimerGuard]},
  { path: "water-quality-inspections/:id",  component: WaterQualityInspectionDetailComponent, canActivate: [UnauthenticatedAccessGuard, NormalPlusGuard, AcknowledgedDisclaimerGuard]},
  { path: "water-quality-inspections/:id/edit",  component: WaterQualityInspectionEditComponent, canActivate: [UnauthenticatedAccessGuard, NormalPlusGuard, AcknowledgedDisclaimerGuard]},
  { path: "water-level-inspections",  component: WaterLevelInspectionListComponent, canActivate: [UnauthenticatedAccessGuard, NormalPlusGuard, AcknowledgedDisclaimerGuard]},
  { path: "water-level-inspections/new",  component: WaterLevelInspectionNewComponent, canActivate: [UnauthenticatedAccessGuard, NormalPlusGuard, AcknowledgedDisclaimerGuard]},
  { path: "water-level-inspections/:id",  component: WaterLevelInspectionDetailComponent, canActivate: [UnauthenticatedAccessGuard, NormalPlusGuard, AcknowledgedDisclaimerGuard]},
  { path: "water-level-inspections/:id/edit",  component: WaterLevelInspectionEditComponent, canActivate: [UnauthenticatedAccessGuard, NormalPlusGuard, AcknowledgedDisclaimerGuard]},
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
