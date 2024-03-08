import { NgModule, ModuleWithProviders, SkipSelf, Optional } from '@angular/core';
import { Configuration } from './configuration';
import { HttpClient } from '@angular/common/http';


import { ChemigationInspectionService } from './api/chemigation-inspection.service';
import { ChemigationPermitService } from './api/chemigation-permit.service';
import { ChemigationPermitAnnualRecordService } from './api/chemigation-permit-annual-record.service';
import { ChemigationPermitAnnualRecordChemicalFormulationService } from './api/chemigation-permit-annual-record-chemical-formulation.service';
import { CountyService } from './api/county.service';
import { CustomRichTextService } from './api/custom-rich-text.service';
import { FieldDefinitionService } from './api/field-definition.service';
import { FileResourceService } from './api/file-resource.service';
import { IrrigationUnitService } from './api/irrigation-unit.service';
import { ManagerDashboardService } from './api/manager-dashboard.service';
import { MapDataService } from './api/map-data.service';
import { OpenETService } from './api/open-et.service';
import { ReportService } from './api/report.service';
import { ReportTemplateModelService } from './api/report-template-model.service';
import { RobustReviewScenarioService } from './api/robust-review-scenario.service';
import { RoleService } from './api/role.service';
import { SearchService } from './api/search.service';
import { SensorService } from './api/sensor.service';
import { SensorAnomalyService } from './api/sensor-anomaly.service';
import { SensorStatusService } from './api/sensor-status.service';
import { StreamFlowZoneService } from './api/stream-flow-zone.service';
import { SupportTicketService } from './api/support-ticket.service';
import { SystemInfoService } from './api/system-info.service';
import { UserService } from './api/user.service';
import { WaterLevelInspectionService } from './api/water-level-inspection.service';
import { WaterQualityInspectionService } from './api/water-quality-inspection.service';
import { WellService } from './api/well.service';
import { WellGroupService } from './api/well-group.service';

@NgModule({
  imports:      [],
  declarations: [],
  exports:      [],
  providers: [
    ChemigationInspectionService,
    ChemigationPermitService,
    ChemigationPermitAnnualRecordService,
    ChemigationPermitAnnualRecordChemicalFormulationService,
    CountyService,
    CustomRichTextService,
    FieldDefinitionService,
    FileResourceService,
    IrrigationUnitService,
    ManagerDashboardService,
    MapDataService,
    OpenETService,
    ReportService,
    ReportTemplateModelService,
    RobustReviewScenarioService,
    RoleService,
    SearchService,
    SensorService,
    SensorAnomalyService,
    SensorStatusService,
    StreamFlowZoneService,
    SupportTicketService,
    SystemInfoService,
    UserService,
    WaterLevelInspectionService,
    WaterQualityInspectionService,
    WellService,
    WellGroupService,
     ]
})
export class ApiModule {
    public static forRoot(configurationFactory: () => Configuration): ModuleWithProviders<ApiModule> {
        return {
            ngModule: ApiModule,
            providers: [ { provide: Configuration, useFactory: configurationFactory } ]
        };
    }

    constructor( @Optional() @SkipSelf() parentModule: ApiModule,
                 @Optional() http: HttpClient) {
        if (parentModule) {
            throw new Error('ApiModule is already loaded. Import in your base AppModule only.');
        }
        if (!http) {
            throw new Error('You need to import the HttpClientModule in your AppModule! \n' +
            'See also https://github.com/angular/angular/issues/20575');
        }
    }
}
