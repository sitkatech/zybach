import { NgModule, ModuleWithProviders } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { NotFoundComponent } from './pages';
import { HeaderNavComponent } from './components';
import { UnauthenticatedComponent } from './pages/unauthenticated/unauthenticated.component';
import { SubscriptionInsufficientComponent } from './pages/subscription-insufficient/subscription-insufficient.component';
import { NgProgressModule } from 'ngx-progressbar';
import { RouterModule } from '@angular/router';
import { LinkRendererComponent } from './components/ag-grid/link-renderer/link-renderer.component';
import { FontAwesomeIconLinkRendererComponent } from './components/ag-grid/fontawesome-icon-link-renderer/fontawesome-icon-link-renderer.component';
import { MultiLinkRendererComponent } from './components/ag-grid/multi-link-renderer/multi-link-renderer.component';
import { CustomRichTextComponent } from './components/custom-rich-text/custom-rich-text.component'
import { CKEditorModule } from '@ckeditor/ckeditor5-angular';
import { FieldDefinitionComponent } from './components/field-definition/field-definition.component';
import { NgbModule } from "@ng-bootstrap/ng-bootstrap";
import { AlertDisplayComponent } from './components/alert-display/alert-display.component';
import { FieldDefinitionGridHeaderComponent } from './components/field-definition-grid-header/field-definition-grid-header.component';
import { AutoCompleteModule } from "primeng/autocomplete";
import { WaterYearSelectComponent } from './components/water-year-select/water-year-select.component'
import { CustomDropdownFilterComponent } from './components/custom-dropdown-filter/custom-dropdown-filter.component';
import { ClearGridFiltersButtonComponent } from './components/clear-grid-filters-button/clear-grid-filters-button.component';

@NgModule({
    declarations: [
        AlertDisplayComponent,
        HeaderNavComponent,
        NotFoundComponent,
        UnauthenticatedComponent,
        SubscriptionInsufficientComponent,
        LinkRendererComponent,
        FontAwesomeIconLinkRendererComponent,
        MultiLinkRendererComponent,
        CustomRichTextComponent,
        FieldDefinitionComponent,
        FieldDefinitionGridHeaderComponent,
        WaterYearSelectComponent,
        CustomDropdownFilterComponent,
        ClearGridFiltersButtonComponent
    ],
    imports: [
        CommonModule,
        FormsModule,
        HttpClientModule,
        NgProgressModule,
        RouterModule,
        CKEditorModule,
        NgbModule,
        AutoCompleteModule,
    ],
    exports: [
        AlertDisplayComponent,
        CommonModule,
        FormsModule,
        NotFoundComponent,
        HeaderNavComponent,
        CustomRichTextComponent,
        FieldDefinitionComponent,
        FieldDefinitionGridHeaderComponent,
        WaterYearSelectComponent,
        ClearGridFiltersButtonComponent
    ]
})
export class SharedModule {
    static forRoot(): ModuleWithProviders<SharedModule> {
    return {
        ngModule: SharedModule,
        providers: []
    };
}

    static forChild(): ModuleWithProviders<SharedModule> {
    return {
        ngModule: SharedModule,
        providers: []
    };
}
}
