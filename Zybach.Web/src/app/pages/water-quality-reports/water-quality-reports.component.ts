import { ChangeDetectorRef, Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { AgGridAngular } from 'ag-grid-angular';
import { ColDef, ColGroupDef } from 'ag-grid-community';
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
import { ReportTemplateModelEnum } from 'src/app/shared/generated/enum/report-template-model-enum';
import { GenerateReportsDto } from 'src/app/shared/generated/model/generate-reports-dto';
import { WellInspectionSummaryDto } from 'src/app/shared/generated/model/well-inspection-summary-dto';
import { WellService } from 'src/app/shared/generated/api/well.service';
import { DatePipe } from '@angular/common';
import { FieldDefinitionGridHeaderComponent } from 'src/app/shared/components/field-definition-grid-header/field-definition-grid-header.component';
import { FieldDefinitionTypeEnum } from 'src/app/shared/generated/enum/field-definition-type-enum';

@Component({
  selector: 'zybach-water-quality-reports',
  templateUrl: './water-quality-reports.component.html',
  styleUrls: ['./water-quality-reports.component.scss']
})

export class WaterQualityReoprtsComponent implements OnInit, OnDestroy {
  @ViewChild('wellsGrid') wellsGrid: AgGridAngular;

  private currentUser: UserDto;

  
  public wells: Array<WellInspectionSummaryDto>;
  public columnDefs: (ColDef | ColGroupDef)[];
  public defaultColDef: ColDef;
  public selectedWellsCount: number;
  public rowsDisplayedCount: number;
  public reportTemplateID: number;

  public isLoadingSubmit: boolean;
  public richTextTypeID : number = CustomRichTextTypeEnum.WellInspectionReports;

  constructor(
    private alertService: AlertService,
    private authenticationService: AuthenticationService,
    private wellService: WellService,
    private cdr: ChangeDetectorRef,
    private reportService: ReportService,
    private utilityFunctionsService: UtilityFunctionsService
  ) { }

  ngOnInit(): void {
    this.authenticationService.getCurrentUser().subscribe(currentUser => {
      this.currentUser = currentUser;

      this.reportService.reportTemplatesGet().subscribe(reportTemplates => {
        this.reportTemplateID = reportTemplates.find(x => x.ReportTemplateModel.ReportTemplateModelID == 
          ReportTemplateModelEnum.WellWaterQualityInspection)?.ReportTemplateID;
      });

      this.wellService.wellsInspectionSummariesGet().subscribe(wells => {
        this.wells = wells;
        this.rowsDisplayedCount = wells.length;
      });

      this.initializeGrid();
    });
  }

  ngOnDestroy(): void {
    this.cdr.detach();
  }

  initializeGrid() {
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
        comparator: this.utilityFunctionsService.linkRendererComparator,
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
      this.utilityFunctionsService.createDateColumnDef('Last Water Quality Inspection', 'LatestWaterQualityInspectionDate', 'M/d/yyyy'),
      {  
        headerName: 'Well Owner',
        children: [
          { headerName: 'Name', field: "Well.OwnerName", filter: true, resizable: true, sortable: true },
          { headerName: 'Address', field: "Well.OwnerAddress", filter: true, resizable: true, sortable: true },
          { headerName: 'City', field: "Well.OwnerCity", filter: true, resizable: true, sortable: true },
          { headerName: 'State', field: "Well.OwnerState", filter: true, resizable: true, sortable: true },
          { headerName: 'Zip', field: "Well.OwnerZipCode", filter: true, resizable: true, sortable: true }    
        ]
      },
      {  
        headerName: 'Additional Contact',
        children: [
          { headerName: 'Name', field: "Well.AdditionalContactName", filter: true, resizable: true, sortable: true },
          { headerName: 'Address', field: "Well.AdditionalContactAddress", filter: true, resizable: true, sortable: true },
          { headerName: 'City', field: "Well.AdditionalContactCity", filter: true, resizable: true, sortable: true },
          { headerName: 'State', field: "Well.AdditionalContactState", filter: true, resizable: true, sortable: true },
          { headerName: 'Zip', field: "Well.AdditionalContactZipCode", filter: true, resizable: true, sortable: true }
        ]
      }
    ];
  }

  public exportToCsv() {
    this.utilityFunctionsService.exportGridToCsv(this.wellsGrid, 'water-quality-report-summary.csv', null);
  }

  public onGridReady(gridEvent) {
    this.wellsGrid.columnApi.autoSizeAllColumns();
  }

  public onSelectionChanged() {
    var selectedWellsCount = 0;
    this.wellsGrid.api.forEachNodeAfterFilterAndSort(node => {
      if (node.isSelected()) selectedWellsCount++;
    });

    this.selectedWellsCount = selectedWellsCount;
  }

  public onFilterChanged() {
    this.rowsDisplayedCount = this.wellsGrid.api.getDisplayedRowCount();
  }

  public generateReport(): void {
    this.alertService.clearAlerts();
    
    let selectedFilteredSortedRows = [];
    this.wellsGrid.api.forEachNodeAfterFilterAndSort(node => {
      if (node.isSelected()) {
        selectedFilteredSortedRows.push(node.data.Well.WellID);
      }
    });

    if (selectedFilteredSortedRows.length == 0){
      this.alertService.pushAlert(new Alert("No Wells selected", AlertContext.Danger));
      return;
    }

    this.isLoadingSubmit = true;

    var generateChemigationPermitReportsDto = new GenerateReportsDto();
    generateChemigationPermitReportsDto.ReportTemplateID = this.reportTemplateID;
    generateChemigationPermitReportsDto.ModelIDList = selectedFilteredSortedRows;

    this.reportService.reportTemplatesGenerateReportsPost(generateChemigationPermitReportsDto).subscribe(response => {
      this.isLoadingSubmit = false;

      var a = document.createElement("a");
      a.href = URL.createObjectURL(response);
      a.download = "Water Quality Report";
      a.click();

      this.alertService.pushAlert(new Alert("Water Quality Report generated for selected Wells", AlertContext.Success));
    }, error => {
      this.isLoadingSubmit = false;
      this.cdr.detectChanges();
    });
  }
}