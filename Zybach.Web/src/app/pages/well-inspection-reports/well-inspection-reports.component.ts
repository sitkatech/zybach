import { ChangeDetectorRef, Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { AgGridAngular } from 'ag-grid-angular';
import { ColDef } from 'ag-grid-community';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { UserDto } from 'src/app/shared/generated/model/user-dto';
import { CustomDropdownFilterComponent } from 'src/app/shared/components/custom-dropdown-filter/custom-dropdown-filter.component';
import { UtilityFunctionsService } from 'src/app/services/utility-functions.service';
import { CustomRichTextTypeEnum } from 'src/app/shared/generated/enum/custom-rich-text-type-enum';
import { LinkRendererComponent } from 'src/app/shared/components/ag-grid/link-renderer/link-renderer.component';
import { AlertService } from 'src/app/shared/services/alert.service';
import { AlertContext } from 'src/app/shared/models/enums/alert-context.enum';
import { ReportService } from 'src/app/shared/generated/api/report.service';
import { Alert } from 'src/app/shared/models/alert';
import { ReportTemplateDto } from 'src/app/shared/generated/model/report-template-dto';
import { ReportTemplateModelEnum } from 'src/app/shared/generated/enum/report-template-model-enum';
import { GenerateReportsDto } from 'src/app/shared/generated/model/generate-reports-dto';
import { WellInspectionSummaryDto } from 'src/app/shared/generated/model/well-inspection-summary-dto';
import { WellService } from 'src/app/shared/generated/api/well.service';
import { DatePipe } from '@angular/common';
import { FieldDefinitionGridHeaderComponent } from 'src/app/shared/components/field-definition-grid-header/field-definition-grid-header.component';
import { FieldDefinitionTypeEnum } from 'src/app/shared/generated/enum/field-definition-type-enum';

@Component({
  selector: 'zybach-well-inspection-reports',
  templateUrl: './well-inspection-reports.component.html',
  styleUrls: ['./well-inspection-reports.component.scss']
})

export class WellInspectionReportsComponent implements OnInit, OnDestroy {
  @ViewChild('wellInspectionsGrid') wellInspectionsGrid: AgGridAngular;

  private currentUser: UserDto;

  public richTextTypeID : number = CustomRichTextTypeEnum.WellInspectionReports;
  
  public rowData: Array<WellInspectionSummaryDto>;
  public columnDefs: any[];
  
  public gridApi: any;
  public gridColumnApi: any;

  public isLoadingSubmit: boolean;
  public reportTemplates: Array<ReportTemplateDto>;
  public selectedReportTemplate: ReportTemplateDto;
  public selectedReportTemplateID: number;

  constructor(
    private alertService: AlertService,
    private authenticationService: AuthenticationService,
    private datePipe: DatePipe,
    private wellService: WellService,
    private cdr: ChangeDetectorRef,
    private reportService: ReportService,
    private utilityFunctionsService: UtilityFunctionsService
  ) { }

  ngOnInit(): void {
    this.reportService.reportTemplatesGet().subscribe(reportTemplates => {
      this.reportTemplates = reportTemplates.filter(x => 
        x.ReportTemplateModel.ReportTemplateModelID == ReportTemplateModelEnum.WellWaterQualityInspection ||
        x.ReportTemplateModel.ReportTemplateModelID == ReportTemplateModelEnum.WellWaterLevelInspection);
    });
    
    this.authenticationService.getCurrentUser().subscribe(currentUser => {
      this.currentUser = currentUser;
      this.wellInspectionsGrid?.api.showLoadingOverlay();
      this.initializeGrid();
    });
  }

  initializeGrid() {
    let datePipe = this.datePipe;
    this.columnDefs = [
      {
        sortable: false, filter: false, width: 40, headerCheckboxSelection: true, checkboxSelection: true, headerCheckboxSelectionFilteredOnly: true
      },
      {
        headerName: 'Well',
        valueGetter: function (params: any) {
          return { LinkValue: params.data.Well.WellID, LinkDisplay: params.data.Well.WellRegistrationID };
        },
        cellRendererFramework: LinkRendererComponent,
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
          return params.data.Well.WellRegistrationID;
        },
        filter: true,
        headerComponentFramework: FieldDefinitionGridHeaderComponent, headerComponentParams: {fieldDefinitionTypeID: FieldDefinitionTypeEnum.WellRegistrationNumber},
        resizable: true,
        sortable: true,
      },
      {
        headerName: "Well Nickname",
        valueGetter: function (params: any) {
          return params.data.Well.WellNickname;
        },
        headerComponentFramework: FieldDefinitionGridHeaderComponent, headerComponentParams: {fieldDefinitionTypeID: FieldDefinitionTypeEnum.WellNickname},
        sortable: true, filter: true, resizable: true,        
      },
      {
        headerName: "Participation",
        valueGetter: function (params: any) {
          return params.data.Well.WellParticipationName;
        },
        filterFramework: CustomDropdownFilterComponent,
        filterParams: {
          field: 'params.data.Well.WellParticipationName'
        },
        headerComponentFramework: FieldDefinitionGridHeaderComponent, headerComponentParams: {fieldDefinitionTypeID: FieldDefinitionTypeEnum.WellProgramParticipation},
        sortable: true, filter: true, resizable: true,        
      },
      {
        headerName: "Water Quality Inspections?",
        valueGetter: function (params) {
          if (params.data.HasWaterQualityInspections) {
            return "Yes";
          } else {
            return "No";
          }
        },
        filterFramework: CustomDropdownFilterComponent,
        filterParams: {
          field: 'params.data.HasWaterQualityInspections'
        },
        headerComponentFramework: FieldDefinitionGridHeaderComponent, headerComponentParams: {fieldDefinitionTypeID: FieldDefinitionTypeEnum.HasWaterQualityInspections },
        sortable: true, resizable: true
      },
      this.createDateColumnDef(datePipe, 'Last Water Quality Inspection', 'LatestWaterQualityInspectionDate', 'M/d/yyyy'),
      {
        headerName: "Water Level Inspections?",
        valueGetter: function (params) {
          if (params.data.HasWaterLevelInspections) {
            return "Yes";
          } else {
            return "No";
          }
        },
        filterFramework: CustomDropdownFilterComponent,
        filterParams: {
          field: 'params.data.HasWaterLevelInspections'
        },
        headerComponentFramework: FieldDefinitionGridHeaderComponent, headerComponentParams: {fieldDefinitionTypeID: FieldDefinitionTypeEnum.HasWaterLevelInspections },
        sortable: true, resizable: true
      },
      this.createDateColumnDef(datePipe, 'Last Water Level Inspection', 'LatestWaterLevelInspectionDate', 'M/d/yyyy'),
      {  
        headerName: 'Well Owner',
        children: [
            { headerName: 'Name', field: "Well.OwnerName", filter: true, resizable: true, sortable: true },
            { headerName: 'Address', field: "Well.OwnerAddress", filter: true, resizable: true, sortable: true },
            { headerName: 'City', field: "Well.OwnerCity", filter: true, resizable: true, sortable: true },
            { headerName: 'State', field: "Well.OwnerState", filter: true, resizable: true, sortable: true },
            { headerName: 'Zip', field: "Well.OwnerZipCode", filter: true, resizable: true, sortable: true },        ]
      },
      {  
        headerName: 'Additional Contact',
        children: [
            { headerName: 'Name', field: "Well.AdditionalContactName", filter: true, resizable: true, sortable: true },
            { headerName: 'Address', field: "Well.AdditionalContactAddress", filter: true, resizable: true, sortable: true },
            { headerName: 'City', field: "Well.AdditionalContactCity", filter: true, resizable: true, sortable: true },
            { headerName: 'State', field: "Well.AdditionalContactState", filter: true, resizable: true, sortable: true },
            { headerName: 'Zip', field: "Well.AdditionalContactZipCode", filter: true, resizable: true, sortable: true },        ]
      }
    ];
  }

  private dateSortComparer (id1: any, id2: any) {
    const date1 = id1 ? Date.parse(id1) : Date.parse("1/1/1900");
    const date2 = id2 ? Date.parse(id2) : Date.parse("1/1/1900");
    if (date1 < date2) {
      return -1;
    }
    return (date1 > date2)  ?  1 : 0;
  }

  private dateFilterComparator(filterLocalDateAtMidnight, cellValue) {
    if(cellValue === null) return -1;
    const cellDate = Date.parse(cellValue);
    if (cellDate == filterLocalDateAtMidnight) {
      return 0;
    }
    return (cellDate < filterLocalDateAtMidnight) ? -1 : 1;
  }

  private createDateColumnDef(datePipe: DatePipe, headerName: string, fieldName: string, dateFormat: string): ColDef {
    let thisFieldDefinitionTypeID = FieldDefinitionTypeEnum[`${fieldName}`];
    return {
      headerName: headerName, valueGetter: function (params: any) {
        return datePipe.transform(params.data[fieldName], dateFormat);
      },
      comparator: this.dateSortComparer,
      filter: 'agDateColumnFilter',
      filterParams: {
        filterOptions: ['inRange'],
        comparator: this.dateFilterComparator
      },
      headerComponentFramework: FieldDefinitionGridHeaderComponent, headerComponentParams: { fieldDefinitionTypeID: thisFieldDefinitionTypeID },
      resizable: true,
      sortable: true
    };
  }

  public exportToCsv() {
    this.utilityFunctionsService.exportGridToCsv(this.wellInspectionsGrid, 'well-inspection-summary.csv', null);
  }

  public onGridReady(gridEvent) {
    this.populateWellInspections();
  }

  private populateWellInspections(): void {
    this.wellService.wellsInspectionSummariesGet().subscribe(wells => {
      this.rowData = wells;
      this.wellInspectionsGrid.columnApi.autoSizeAllColumns();
      this.wellInspectionsGrid.api.hideOverlay();
    });
  }

  public getSelectedRows() {
    let selectedFilteredSortedRows = [];
    if(this.wellInspectionsGrid){
      this.wellInspectionsGrid.api.forEachNodeAfterFilterAndSort(node => {
        if(node.isSelected()){
          selectedFilteredSortedRows.push(node.data.Well.WellID);
        }
      });
    }
    return selectedFilteredSortedRows;
  }

  public getFilteredRowsCount() {
    var count = 0;
    if(this.wellInspectionsGrid){
      this.wellInspectionsGrid.api.forEachNodeAfterFilterAndSort(node => {
        count++;
      });
    }
    return count;
  }

  public generateReport(): void {
    this.alertService.clearAlerts();
    if(!this.selectedReportTemplateID){
      this.alertService.pushAlert(new Alert("No report template selected.", AlertContext.Danger));
    } else {
      let selectedFilteredSortedRows = [];
      this.wellInspectionsGrid.api.forEachNodeAfterFilterAndSort(node => {
        if(node.isSelected()){
          selectedFilteredSortedRows.push(parseInt(node.data.Well.WellID));
        }
      });
  
      if (selectedFilteredSortedRows.length === 0){
        this.alertService.pushAlert(new Alert("No wells selected.", AlertContext.Danger));
      } else {
        this.isLoadingSubmit = true;
        var generateChemigationPermitReportsDto = new GenerateReportsDto();
        generateChemigationPermitReportsDto.ReportTemplateID = this.selectedReportTemplateID;
        generateChemigationPermitReportsDto.ModelIDList = selectedFilteredSortedRows;
        this.reportService.reportTemplatesGenerateReportsPost(generateChemigationPermitReportsDto)
          .subscribe(response => {
            this.isLoadingSubmit = false;
    
            var a = document.createElement("a");
            a.href = URL.createObjectURL(response);
            a.download = "Well Inspection Reports";
            // start download
            a.click();
    
            this.alertService.pushAlert(new Alert("Report Generated for selected rows", AlertContext.Success));
          }
            ,
            error => {
              this.isLoadingSubmit = false;
              this.cdr.detectChanges();
            }
          );
      }

    }
  }

  ngOnDestroy(): void {
    this.cdr.detach();
  }
}