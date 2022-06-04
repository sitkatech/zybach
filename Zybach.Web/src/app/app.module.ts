import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule, APP_INITIALIZER, ErrorHandler, Injector } from '@angular/core';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { SharedModule } from './shared/shared.module';
import { OAuthModule, OAuthStorage } from 'angular-oauth2-oidc';
import { CookieService } from 'ngx-cookie-service';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { AuthInterceptor } from './shared/interceptors/auth-interceptor';
import { NgbModule } from "@ng-bootstrap/ng-bootstrap";
import { HomeIndexComponent } from './pages/home/home-index/home-index.component';
import { UserListComponent } from './pages/user-list/user-list.component';
import { RouterModule } from '@angular/router';
import { UserInviteComponent } from './pages/user-invite/user-invite.component';
import { UserDetailComponent } from './pages/user-detail/user-detail.component';
import { UserEditComponent } from './pages/user-edit/user-edit.component';
import { AgGridModule } from 'ag-grid-angular';
import { DecimalPipe, CurrencyPipe, DatePipe } from '@angular/common';
import { LinkRendererComponent } from './shared/components/ag-grid/link-renderer/link-renderer.component';


import { FormsModule } from '@angular/forms';
import { FontAwesomeIconLinkRendererComponent } from './shared/components/ag-grid/fontawesome-icon-link-renderer/fontawesome-icon-link-renderer.component';
import { LoginCallbackComponent } from './pages/login-callback/login-callback.component';
import { HelpComponent } from './pages/help/help.component';
import { MultiLinkRendererComponent } from './shared/components/ag-grid/multi-link-renderer/multi-link-renderer.component';
import { CreateUserCallbackComponent } from './pages/create-user-callback/create-user-callback.component';
import { AboutComponent } from './pages/about/about.component';
import { DisclaimerComponent } from './pages/disclaimer/disclaimer.component';
import { AppInitService } from './app.init';
import { FieldDefinitionListComponent } from './pages/field-definition-list/field-definition-list.component';
import { FieldDefinitionEditComponent } from './pages/field-definition-edit/field-definition-edit.component';
import { CKEditorModule } from '@ckeditor/ckeditor5-angular';
import { TrainingComponent } from './pages/training/training.component';
import { environment } from 'src/environments/environment';
import { AppInsightsService } from './shared/services/app-insights.service';
import { GlobalErrorHandlerService } from './shared/services/global-error-handler.service';
import { WellMapComponent } from './pages/well-map/well-map.component';
import { WellExplorerComponent } from './pages/well-explorer/well-explorer.component';
import { ToastrModule } from 'ngx-toastr';
import { WellDetailComponent } from './pages/well-detail/well-detail.component';
import { DashboardComponent } from './pages/dashboard/dashboard.component';
import { WellMapPopupComponent } from './pages/well-map-popup/well-map-popup.component';
import { createCustomElement } from '@angular/elements';
import { AngularMyDatePickerModule } from 'angular-mydatepicker';
import { RobustReviewScenarioComponent } from './pages/robust-review-scenario/robust-review-scenario.component';
import { SensorStatusComponent } from './pages/sensor-status/sensor-status.component';
import { SensorStatusMapComponent } from './pages/sensor-status-map/sensor-status-map.component';
import { SensorStatusMapPopupComponent } from './pages/sensor-status-map-popup/sensor-status-map-popup.component';
import { WellNewComponent } from './pages/well-new/well-new.component';
import { ReportsListComponent } from './pages/reports-list/reports-list.component';
import { ReportTemplateDetailComponent } from './pages/report-template-detail/report-template-detail.component';
import { ReportTemplateEditComponent } from './pages/report-template-edit/report-template-edit.component';
import { ChemigationNewPermitComponent } from './pages/chemigation-new-permit/chemigation-new-permit.component';
import { ChemigationPermitListComponent } from './pages/chemigation-permit-list/chemigation-permit-list.component';
import { ChemigationPermitDetailComponent } from './pages/chemigation-permit-detail/chemigation-permit-detail.component';
import { ChemigationPermitEditComponent } from './pages/chemigation-permit-edit/chemigation-permit-edit.component';
import { CookieStorageService } from './shared/services/cookies/cookie-storage.service';
import { ChemigationPermitAddRecordComponent } from './pages/chemigation-permit-add-record/chemigation-permit-add-record.component';
import { ChemigationPermitEditRecordComponent } from './pages/chemigation-permit-edit-record/chemigation-permit-edit-record.component';
import { ChemigationPermitAnnualRecordUpsertComponent } from './shared/components/chemigation-permit-annual-record-upsert/chemigation-permit-annual-record-upsert.component';
import { ChemigationPermitChemicalFormulationsEditorComponent } from './shared/components/chemigation-permit-chemical-formulations-editor/chemigation-permit-chemical-formulations-editor.component';
import { ChemigationPermitApplicatorsEditorComponent } from './shared/components/chemigation-permit-applicators-editor/chemigation-permit-applicators-editor.component';
import { NdeeChemicalsReportComponent } from './pages/ndee-chemicals-report/ndee-chemicals-report.component';
import { NgSelectModule } from '@ng-select/ng-select';
import { ChemigationPermitReportsComponent } from './pages/chemigation-permit-reports/chemigation-permit-reports.component';
import { ChemigationInspectionsListComponent } from './pages/chemigation-inspections-list/chemigation-inspections-list.component';
import { ChemigationInspectionUpsertComponent } from './shared/components/chemigation-inspection-upsert/chemigation-inspection-upsert.component';
import { ChemigationInspectionNewComponent } from './pages/chemigation-inspection-new/chemigation-inspection-new.component';
import { ChemigationInspectionEditComponent } from './pages/chemigation-inspection-edit/chemigation-inspection-edit.component';
import { WaterQualityInspectionListComponent } from './pages/water-quality-inspection-list/water-quality-inspection-list.component';
import { WaterQualityInspectionDetailComponent } from './pages/water-quality-inspection-detail/water-quality-inspection-detail.component';
import { WaterQualityInspectionNewComponent } from './pages/water-quality-inspection-new/water-quality-inspection-new.component';
import { WaterQualityInspectionUpsertComponent } from './shared/components/water-quality-inspection-upsert/water-quality-inspection-upsert.component';
import { WaterQualityInspectionEditComponent } from './pages/water-quality-inspection-edit/water-quality-inspection-edit.component';
import { WaterLevelInspectionListComponent } from './pages/water-level-inspection-list/water-level-inspection-list.component';
import { WellContactEditComponent } from './pages/well-contact-edit/well-contact-edit.component';
import { WellParticipationEditComponent } from './pages/well-participation-edit/well-participation-edit.component';
import { WellRegistrationIdEditComponent } from './pages/well-registration-id-edit/well-registration-id-edit.component';
import { ClearinghouseWqiReportComponent } from './pages/clearinghouse-wqi-report/clearinghouse-wqi-report.component';
import { NgxMaskModule } from 'ngx-mask';
import { SensorListComponent } from './pages/sensor-list/sensor-list.component';
import { SensorDetailComponent } from './pages/sensor-detail/sensor-detail.component';
import { WaterLevelInspectionDetailComponent } from './pages/water-level-inspection-detail/water-level-inspection-detail.component';
import { WaterLevelInspectionNewComponent } from './pages/water-level-inspection-new/water-level-inspection-new.component';
import { WaterLevelInspectionUpsertComponent } from './shared/components/water-level-inspection-upsert/water-level-inspection-upsert.component';
import { WaterLevelInspectionEditComponent } from './pages/water-level-inspection-edit/water-level-inspection-edit.component';
import { WellInspectionReportsComponent } from './pages/well-inspection-reports/well-inspection-reports.component';
import { WellOverviewTabComponent } from './shared/components/well-overview-tab/well-overview-tab.component';
import { WellWaterDataTabComponent } from './shared/components/well-water-data-tab/well-water-data-tab.component';
import { WellPermitsInspectionsTabComponent } from './shared/components/well-permits-inspections-tab/well-permits-inspections-tab.component';
import { SensorAnomalyListComponent } from './pages/sensor-anomaly-list/sensor-anomaly-list.component';
import { SensorAnomalyEditComponent } from './pages/sensor-anomaly-edit/sensor-anomaly-edit.component';
import { WaterLevelExplorerComponent } from './pages/water-level-explorer/water-level-explorer.component';
import { WellWaterLevelMapComponent } from './pages/well-water-level-map/well-water-level-map.component';
import { IrrigationUnitListComponent } from './pages/irrigation-unit-list/irrigation-unit-list.component';
import { IrrigationUnitDetailComponent } from './pages/irrigation-unit-detail/irrigation-unit-detail.component';
import { IrrigationUnitMapComponent } from './pages/irrigation-unit-map/irrigation-unit-map.component';
import { OpenetSyncWaterYearMonthStatusListComponent } from './pages/openet-sync-water-year-month-status-list/openet-sync-water-year-month-status-list.component';
import { ApiModule } from './shared/generated/api.module';
import { Configuration } from './shared/generated/configuration';

export function init_app(appLoadService: AppInitService, appInsightsService:  AppInsightsService) {
  return () => appLoadService.init().then(() => {
    if (environment.appInsightsInstrumentationKey) {
      appInsightsService.initAppInsights();
    }
  });
}

@NgModule({
  declarations: [
    AppComponent,
    HomeIndexComponent,
    UserListComponent,
    UserInviteComponent,
    UserDetailComponent,
    UserEditComponent,
    LoginCallbackComponent,
    HelpComponent,
    CreateUserCallbackComponent,
    AboutComponent,
    DisclaimerComponent,
    FieldDefinitionListComponent,
    FieldDefinitionEditComponent,
    TrainingComponent,
    WellMapComponent,
    WellExplorerComponent,
    WellDetailComponent,
    DashboardComponent,
    WellMapPopupComponent,
    RobustReviewScenarioComponent,
    SensorStatusComponent,
    SensorStatusMapComponent,
    SensorStatusMapPopupComponent,
    WellNewComponent,
    ReportsListComponent,
    ReportTemplateDetailComponent,
    ReportTemplateEditComponent,
    ChemigationNewPermitComponent,
    ChemigationPermitListComponent,
    ChemigationPermitDetailComponent,
    ChemigationPermitEditComponent,
    ChemigationPermitAddRecordComponent,
    ChemigationPermitEditRecordComponent,
    ChemigationPermitAnnualRecordUpsertComponent,
    ChemigationPermitChemicalFormulationsEditorComponent,
    ChemigationPermitApplicatorsEditorComponent,
    NdeeChemicalsReportComponent,
    ChemigationPermitReportsComponent,
    ChemigationInspectionsListComponent,
    ChemigationInspectionUpsertComponent,
    ChemigationInspectionNewComponent,
    ChemigationInspectionEditComponent,
    WaterQualityInspectionListComponent,
    WaterQualityInspectionDetailComponent,
    WaterQualityInspectionUpsertComponent, 
    WaterQualityInspectionNewComponent, 
    WaterQualityInspectionEditComponent, 
    WaterLevelInspectionUpsertComponent,
    WellOverviewTabComponent,
    WellWaterDataTabComponent,
    WellPermitsInspectionsTabComponent,
    WaterLevelInspectionListComponent, WellContactEditComponent, WellParticipationEditComponent, WellRegistrationIdEditComponent, ClearinghouseWqiReportComponent, SensorListComponent, SensorDetailComponent, WaterLevelInspectionDetailComponent, WaterLevelInspectionNewComponent, WaterLevelInspectionEditComponent, WellInspectionReportsComponent, SensorAnomalyListComponent, SensorAnomalyEditComponent, WaterLevelExplorerComponent, WellWaterLevelMapComponent, IrrigationUnitListComponent, IrrigationUnitDetailComponent, IrrigationUnitMapComponent, OpenetSyncWaterYearMonthStatusListComponent
  ],
  imports: [
    AppRoutingModule,
    BrowserModule,
    BrowserAnimationsModule,
    NgbModule,
    RouterModule,
    OAuthModule.forRoot(),
    SharedModule.forRoot(),
    FormsModule,
    BrowserAnimationsModule,
    AgGridModule.withComponents([]),
    CKEditorModule,
    BrowserAnimationsModule,
    ToastrModule.forRoot({
      positionClass: "toast-top-right"
    }),
    AngularMyDatePickerModule,
    NgSelectModule,
    ApiModule.forRoot(() => {
      return new Configuration({
        basePath: `${environment.mainAppApiUrl}`,
      });
    }),
    NgxMaskModule.forRoot(),
  ],  
  providers: [
    CookieService,
    AppInitService,
    { provide: APP_INITIALIZER, useFactory: init_app, deps: [AppInitService, AppInsightsService], multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },
    {
      provide: ErrorHandler,
      useClass: GlobalErrorHandlerService
    },
    DecimalPipe, CurrencyPipe, DatePipe,
    {
      provide: OAuthStorage,
      useClass: CookieStorageService
    }
  ],
  entryComponents: [LinkRendererComponent, FontAwesomeIconLinkRendererComponent, MultiLinkRendererComponent, WellMapPopupComponent],
  bootstrap: [AppComponent]
})
export class AppModule { 
  //https://github.com/Asymmetrik/ngx-leaflet/issues/178 for explanation
  constructor(private injector: Injector) {
    const WellMapPopupElement = createCustomElement(WellMapPopupComponent, {injector});
    const SensorStatusMapPopupElement = createCustomElement(SensorStatusMapPopupComponent, {injector});
    // Register the custom element with the browser.
    customElements.define('well-map-popup-element', WellMapPopupElement);
    customElements.define('sensor-status-map-popup-element', SensorStatusMapPopupElement);
  }
}
