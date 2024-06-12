import { ChangeDetectorRef, Component, OnInit, ViewChild } from "@angular/core";
import { AgGridAngular } from "ag-grid-angular";
import { ColDef } from "ag-grid-community";
import { AuthenticationService } from "src/app/services/authentication.service";
import { UserDto } from "src/app/shared/generated/model/user-dto";
import { CustomDropdownFilterComponent } from "src/app/shared/components/custom-dropdown-filter/custom-dropdown-filter.component";
import { UtilityFunctionsService } from "src/app/services/utility-functions.service";
import { CustomRichTextTypeEnum } from "src/app/shared/generated/enum/custom-rich-text-type-enum";
import { LinkRendererComponent } from "src/app/shared/components/ag-grid/link-renderer/link-renderer.component";
import { AlertService } from "src/app/shared/services/alert.service";
import { DatePipe } from "@angular/common";
import { WellForFlowTestReportPageDto } from "src/app/shared/generated/model/models";
import { FlowTestReportService } from "src/app/shared/generated/api/flow-test-report.service";
import { FieldDefinitionGridHeaderComponent } from "src/app/shared/components/field-definition-grid-header/field-definition-grid-header.component";
import { FieldDefinitionTypeEnum } from "src/app/shared/generated/enum/field-definition-type-enum";

@Component({
    selector: "flow-test-report-list",
    templateUrl: "./flow-test-report-list.component.html",
    styleUrls: ["./flow-test-report-list.component.scss"],
})
export class FlowTestReportListComponent implements OnInit {
    @ViewChild("flowTestReportGrid") flowTestReportGrid: AgGridAngular;

    private currentUser: UserDto;

    public richTextTypeID: number = CustomRichTextTypeEnum.FlowTestReport;

    public rowData: Array<WellForFlowTestReportPageDto>;
    public columnDefs: ColDef[];

    public gridApi: any;
    public gridColumnApi: any;

    public isLoadingSubmit: boolean;

    constructor(
        private flowTestReportService: FlowTestReportService,
        private alertService: AlertService,
        private authenticationService: AuthenticationService,
        private cdr: ChangeDetectorRef,
        private utilityFunctionsService: UtilityFunctionsService,
        private datePipe: DatePipe
    ) {}

    ngOnInit(): void {
        this.authenticationService.getCurrentUser().subscribe((currentUser) => {
            this.currentUser = currentUser;
            this.flowTestReportGrid?.api.showLoadingOverlay();
            this.initializeGrid();
        });
    }

    initializeGrid() {
        let datePipe = this.datePipe;
        this.columnDefs = [
            {
                headerName: "Registration #",
                valueGetter: function (params: any) {
                    return {
                        LinkValue: params.data.WellID,
                        LinkDisplay: params.data.WellRegistrationID,
                    };
                },
                cellRenderer: LinkRendererComponent,
                cellRendererParams: { inRouterLink: "/wells/" },
                comparator: function (id1: any, id2: any) {
                    let link1 = id1.LinkDisplay;
                    let link2 = id2.LinkDisplay;
                    if (link1 < link2) {
                        return -1;
                    }
                    if (link1 > link2) {
                        return 1;
                    }
                    return 0;
                },
                filterValueGetter: function (params: any) {
                    return params.data.WellRegistrationID;
                },
                headerComponent: FieldDefinitionGridHeaderComponent,
                headerComponentParams: {
                    fieldDefinitionTypeID:
                        FieldDefinitionTypeEnum.WellRegistrationNumber,
                },
                sortable: true,
                filter: true,
                resizable: true,
                sort: "asc",
            },
            {
                headerName: "Permit #",
                valueGetter: function (params: any) {
                    return {
                        LinkValue: params.data.ChemigationPermitNumber,
                        LinkDisplay: params.data.ChemigationPermitNumberDisplay,
                    };
                },
                cellRenderer: LinkRendererComponent,
                cellRendererParams: { inRouterLink: "/chemigation-permits/" },
                comparator: function (id1: any, id2: any) {
                    let link1 = id1.LinkDisplay;
                    let link2 = id2.LinkDisplay;
                    if (link1 < link2) {
                        return -1;
                    }
                    if (link1 > link2) {
                        return 1;
                    }
                    return 0;
                },
                filterValueGetter: function (params: any) {
                    return params.data.ChemigationPermitNumber;
                },
                filter: "agNumberColumnFilter",
                resizable: true,
                sortable: true,
            },
            {
                headerName: "Field Name",
                field: "FieldName",
                headerComponent: FieldDefinitionGridHeaderComponent,
                headerComponentParams: {
                    fieldDefinitionTypeID:
                        FieldDefinitionTypeEnum.WellFieldName,
                },
                sortable: true,
                filter: true,
                resizable: true,
            },
            {
                headerName: "Last Inspected",
                valueGetter: function (params: any) {
                    return datePipe.transform(
                        params.data.LastInspected,
                        "M/d/yyyy"
                    );
                },
                comparator: function (id1: any, id2: any) {
                    const date1 = Date.parse(id1);
                    const date2 = Date.parse(id2);
                    if (date1 < date2) {
                        return -1;
                    }
                    return date1 > date2 ? 1 : 0;
                },
                filterValueGetter: function (params: any) {
                    return datePipe.transform(
                        params.data.LastInspected,
                        "M/d/yyyy"
                    );
                },
                filter: "agDateColumnFilter",
                filterParams: {
                    filterOptions: ["inRange"],
                    comparator: this.dateFilterComparator,
                },
                width: 110,
                resizable: true,
                sortable: true,
            },
            {
                headerName: "Last Flow Test",
                valueGetter: function (params: any) {
                    return datePipe.transform(
                        params.data.LastFlowTest,
                        "M/d/yyyy"
                    );
                },
                comparator: function (id1: any, id2: any) {
                    const date1 = Date.parse(id1);
                    const date2 = Date.parse(id2);
                    if (date1 < date2) {
                        return -1;
                    }
                    return date1 > date2 ? 1 : 0;
                },
                filterValueGetter: function (params: any) {
                    return datePipe.transform(
                        params.data.LastFlowTest,
                        "M/d/yyyy"
                    );
                },
                filter: "agDateColumnFilter",
                filterParams: {
                    filterOptions: ["inRange"],
                    comparator: this.dateFilterComparator,
                },
                width: 110,
                resizable: true,
                sortable: true,
            },
            {
                headerName: "AgHub Registered User",
                field: "AgHubRegisteredUser",
                headerComponent: FieldDefinitionGridHeaderComponent,
                headerComponentParams: {
                    fieldDefinitionTypeID:
                        FieldDefinitionTypeEnum.AgHubRegisteredUser,
                },
                sortable: true,
                filter: true,
                resizable: true,
            },
            {
                headerName: "Chemigation Permit Applicant First Name",
                field: "ChemigationPermitApplicantFirstName",
                sortable: true,
                filter: true,
                resizable: true,
            },
            {
                headerName: "Chemigation Permit Applicant Last Name",
                field: "ChemigationPermitApplicantLastName",
                sortable: true,
                filter: true,
                resizable: true,
            },
            {
                headerName: "Chemigation Permit Applicators",
                field: "ChemigationPermitApplicatorNames",
                sortable: true,
                filter: true,
                resizable: true,
            },
        ];
    }

    private dateFilterComparator(filterLocalDate, cellValue) {
        const cellDate = Date.parse(cellValue);
        const filterLocalDateAtMidnight = filterLocalDate.getTime();
        if (cellDate == filterLocalDateAtMidnight) {
            return 0;
        }
        return cellDate < filterLocalDateAtMidnight ? -1 : 1;
    }

    public exportToCsv() {
        this.utilityFunctionsService.exportGridToCsv(
            this.flowTestReportGrid,
            "flow-test-report.csv",
            null
        );
    }

    public onGridReady(gridEvent) {
        this.populateFlowReportGrid();
    }

    private populateFlowReportGrid(): void {
        this.flowTestReportService
            .wellsRequiringActionGet()
            .subscribe((chemigationInspections) => {
                this.rowData = chemigationInspections;
                this.flowTestReportGrid.api.hideOverlay();
                this.flowTestReportGrid.api.sizeColumnsToFit();
            });
    }

    ngOnDestroy(): void {
        this.cdr.detach();
    }
}
