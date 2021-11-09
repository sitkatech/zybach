import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule, APP_INITIALIZER, ErrorHandler, Injector } from '@angular/core';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { SharedModule } from './shared/shared.module';
import { OAuthModule } from 'angular-oauth2-oidc';
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
import { SelectDropDownModule } from 'ngx-select-dropdown'
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
import { TestAPIComponent } from './pages/test-api/test-api.component';
import { WellMapComponent } from './pages/well-map/well-map.component';
import { WellExplorerComponent } from './pages/well-explorer/well-explorer.component';
import { NgMultiSelectDropDownModule } from 'ng-multiselect-dropdown';
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
import { ChemigationLandingComponent } from './pages/chemigation-landing/chemigation-landing.component';
import { WellNewComponent } from './pages/well-new/well-new.component';
import { ReportsListComponent } from './pages/reports-list/reports-list.component';
import { ReportTemplateDetailComponent } from './pages/report-template-detail/report-template-detail.component';
import { ReportTemplateEditComponent } from './pages/report-template-edit/report-template-edit.component';
import { ChemigationNewPermitComponent } from './pages/chemigation-new-permit/chemigation-new-permit.component';
import { ChemigationPermitListComponent } from './pages/chemigation-permit-list/chemigation-permit-list.component';
import { ChemigationPermitDetailComponent } from './pages/chemigation-permit-detail/chemigation-permit-detail.component';

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
    TestAPIComponent,
    WellMapComponent,
    WellExplorerComponent,
    WellDetailComponent,
    DashboardComponent,
    WellMapPopupComponent,
    RobustReviewScenarioComponent,
    SensorStatusComponent,
    SensorStatusMapComponent,
    SensorStatusMapPopupComponent,
    ChemigationLandingComponent,
    WellNewComponent,
    ReportsListComponent,
    ReportTemplateDetailComponent,
    ReportTemplateEditComponent,
    ChemigationNewPermitComponent,
    ChemigationPermitListComponent,
    ChemigationPermitDetailComponent,
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
    SelectDropDownModule,
    CKEditorModule,
    NgMultiSelectDropDownModule.forRoot(),
    BrowserAnimationsModule,
    ToastrModule.forRoot({
      positionClass: "toast-top-right"
    }),
    AngularMyDatePickerModule
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
    DecimalPipe, CurrencyPipe, DatePipe
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
