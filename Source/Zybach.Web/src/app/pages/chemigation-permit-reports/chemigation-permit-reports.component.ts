import { ChangeDetectorRef, Component, OnInit, ViewChild } from '@angular/core';
import { AgGridAngular } from 'ag-grid-angular';
import { ColDef } from 'ag-grid-community';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { ChemigationPermitService } from 'src/app/services/chemigation-permit.service';
import { UserDto } from 'src/app/shared/generated/model/user-dto';
import { CustomDropdownFilterComponent } from 'src/app/shared/components/custom-dropdown-filter/custom-dropdown-filter.component';
import { UtilityFunctionsService } from 'src/app/services/utility-functions.service';
import { ChemigationPermitAnnualRecordDto } from 'src/app/shared/generated/model/chemigation-permit-annual-record-dto';
import { CustomRichTextType } from 'src/app/shared/models/enums/custom-rich-text-type.enum';
import { LinkRendererComponent } from 'src/app/shared/components/ag-grid/link-renderer/link-renderer.component';
import { CustomPinnedRowRendererComponent } from 'src/app/shared/components/ag-grid/custom-pinned-row-renderer/custom-pinned-row-renderer.component';
import { AlertService } from 'src/app/shared/services/alert.service';
import { AlertContext } from 'src/app/shared/models/enums/alert-context.enum';
import { ReportTemplateService } from 'src/app/shared/services/report-template.service';
import { Alert } from 'src/app/shared/models/alert';
import { ReportTemplateDto } from 'src/app/shared/generated/model/report-template-dto';
import { ReportTemplateModelEnum } from 'src/app/shared/models/enums/report-template-model-enum';
import { GenerateReportsDto } from 'src/app/shared/generated/model/generate-reports-dto';

@Component({
  selector: 'zybach-chemigation-permit-reports',
  templateUrl: './chemigation-permit-reports.component.html',
  styleUrls: ['./chemigation-permit-reports.component.scss']
})
export class ChemigationPermitReportsComponent implements OnInit {
  @ViewChild('chemigationPermitReportGrid') chemigationPermitReportGrid: AgGridAngular;
  
  private watchUserChangeSubscription: any;
  private currentUser: UserDto;

  public richTextTypeID : number = CustomRichTextType.ChemigationPermitReport;
  
  public rowData: Array<ChemigationPermitAnnualRecordDto>;
  public columnDefs: ColDef[];
  
  public gridApi: any;
  public gridColumnApi: any;

  public allYearsSelected: boolean = false;
  public yearToDisplay: number;
  public currentYear: number;

  public NDEEAmountTotal: number;
  public pinnedBottomRowData: { NDEEAmountTotal: number; }[];

  public isLoadingSubmit: boolean;
  public reportTemplates: Array<ReportTemplateDto>;
  public selectedReportTemplate: ReportTemplateDto;
  public selectedReportTemplateID: number;

  constructor(
    private alertService: AlertService,
    private authenticationService: AuthenticationService,
    private chemigationPermitService: ChemigationPermitService,
    private cdr: ChangeDetectorRef,
    private reportTemplateService: ReportTemplateService,
    private utilityFunctionsService: UtilityFunctionsService
  ) { }

  ngOnInit(): void {
    this.currentYear = new Date().getFullYear();
    this.yearToDisplay = new Date().getFullYear(); 
    
    this.reportTemplateService.getReportTemplatesByModelID(ReportTemplateModelEnum.ChemigationPermitAnnualRecord).subscribe(reportTemplates => {
      this.reportTemplates = reportTemplates;
      if (this.reportTemplates.length == 1) {
        this.selectedReportTemplateID = reportTemplates[0].ReportTemplateID;
      }
    });

    this.watchUserChangeSubscription = this.authenticationService.currentUserSetObservable.subscribe(currentUser => {
      this.currentUser = currentUser;
      this.chemigationPermitReportGrid.api.showLoadingOverlay();
      this.initializeGrid();
    });
  }

  initializeGrid() {
    this.columnDefs = [
      {
        sortable: false, filter: false, width: 50, headerCheckboxSelection: true, checkboxSelection: true, headerCheckboxSelectionFilteredOnly: true
      },
      {
        headerName: 'Permit #',
        valueGetter: function (params: any) {
          return { LinkValue: params.data.ChemigationPermit?.ChemigationPermitNumber, LinkDisplay: params.data.ChemigationPermit?.ChemigationPermitNumberDisplay };
        },
        cellRendererFramework: LinkRendererComponent,
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
          return params.data.ChemigationPermit.ChemigationPermitNumber;
        },
        filter: 'agNumberColumnFilter',
        resizable: true,
        sortable: true,
        sort: 'asc',
      },
      { headerName: 'Permit Status', field: 'ChemigationPermit.ChemigationPermitStatus.ChemigationPermitStatusDisplayName',
        filterFramework: CustomDropdownFilterComponent,
        filterParams: {
          field: 'ChemigationPermit.ChemigationPermitStatus.ChemigationPermitStatusDisplayName'
        },
        resizable: true, sortable: true 
      },
      { headerName: 'Township-Range-Section', field: 'ChemigationPermit.TownshipRangeSection', filter: true, resizable: true, sortable: true },
      { headerName: 'County', field: 'ChemigationPermit.ChemigationCounty.ChemigationCountyDisplayName',
        filterFramework: CustomDropdownFilterComponent,
        filterParams: {
          field: 'ChemigationPermit.ChemigationCounty.ChemigationCountyDisplayName',
        }, 
        resizable: true, sortable: true 
      },
      { headerName: 'Renewal Year', field: 'RecordYear',
        filterValueGetter: function (params: any) {
          return params.data.RecordYear;
        },
        filter: 'agNumberColumnFilter',
        resizable: true, 
        sortable: true
      },
      { headerName: 'Renewal Status', field: 'ChemigationPermitAnnualRecordStatus.ChemigationPermitAnnualRecordStatusDisplayName',
        filterFramework: CustomDropdownFilterComponent,
        filterParams: {
          field: 'ChemigationPermitAnnualRecordStatus.ChemigationPermitAnnualRecordStatusDisplayName'
        },
        resizable: true, sortable: true 
      },
      { headerName: 'Applicant', field: 'ApplicantName', filter: true, resizable: true, sortable: true },
      { headerName: 'Street Address', field: 'ApplicantMailingAddress', filter: true, resizable: true, sortable: true },
      { headerName: 'City', field: 'ApplicantCity', filter: true, resizable: true, sortable: true },
      { headerName: 'State', field: 'ApplicantState',
        filterFramework: CustomDropdownFilterComponent,
        filterParams: {
          field: 'ApplicantState'
        },
        resizable: true, sortable: true 
      },
      { headerName: 'Zip Code', field: 'ApplicantZipCode', filter: true, resizable: true, sortable: true },
      { headerName: 'Home Phone', valueGetter: function (params: any) {
        return params.node.rowPinned ? null : params.data.ApplicantPhone ? params.data.ApplicantPhone : '-';
        },
        filter: true, resizable: true, sortable: true 
      },
      { headerName: 'Mobile Phone', valueGetter: function (params: any) {
        return params.node.rowPinned ? null : params.data.ApplicantMobilePhone ? params.data.ApplicantMobilePhone : '-';
        },
        filter: true, resizable: true, sortable: true 
      },
      { 
        headerName: 'NDEE Amount ($)',  
        valueGetter: function (params: any) {
          return params.node.rowPinned ? "Total: " + params.data.NDEEAmountTotal : 
            params.data.NDEEAmount ?? '-';
        },
        pinnedRowCellRendererFramework: CustomPinnedRowRendererComponent,
        pinnedRowCellRendererParams: { filter: true },
        filter: 'agNumberColumnFilter',
        resizable: true,
        sortable: true
      }
    ]; 
  }
  
  public exportToCsv() {
    this.utilityFunctionsService.exportGridToCsv(this.chemigationPermitReportGrid, 'chemigation-permit-report.csv', null);
  }

  public onGridReady(gridEvent) {
    this.populateAnnualRecords();
  }

  public onFilterChanged(gridEvent) {
    gridEvent.api.setPinnedBottomRowData([
      {
        NDEEAmountTotal: gridEvent.api.getModel().rowsToDisplay.map(x => x.data.NDEEAmount ?? 0).reduce((sum, x) => sum+x, 0)
      }
    ]);
  }
  
  public updateAnnualData(): void {
    this.populateAnnualRecords();
  }

  private populateAnnualRecords(): void {
    this.chemigationPermitService.getAllAnnualRecords().subscribe(annualRecords => {
      this.rowData = annualRecords.filter(x => x.RecordYear == this.yearToDisplay);
      this.chemigationPermitReportGrid.api.hideOverlay();
      this.pinnedBottomRowData = [
        { 
          NDEEAmountTotal: this.rowData.map(x => x.NDEEAmount ?? 0).reduce((sum, x) => sum + x, 0)
        }
      ];
      this.chemigationPermitReportGrid.api.sizeColumnsToFit();
    });
  }

  public getSelectedRows() {
    let selectedFilteredSortedRows = [];
    if(this.chemigationPermitReportGrid){
      this.chemigationPermitReportGrid.api.forEachNodeAfterFilterAndSort(node => {
        if(node.isSelected()){
          selectedFilteredSortedRows.push(node.data.ChemigationPermitAnnualRecordID);
        }
      });
    }
    return selectedFilteredSortedRows;
  }

  public getFilteredRowsCount() {
    var count = 0;
    if(this.chemigationPermitReportGrid){
      this.chemigationPermitReportGrid.api.forEachNodeAfterFilterAndSort(node => {
        count++;
      });
    }
    return count;
  }

  public modelHasMultipleTemplates(): boolean {
    return this.reportTemplates?.length > 1;
  }

  public generateReport(): void {
    if(!this.selectedReportTemplateID){
      this.alertService.pushAlert(new Alert("No report template selected.", AlertContext.Danger));
    } else {
      let selectedFilteredSortedRows = [];
      this.chemigationPermitReportGrid.api.forEachNodeAfterFilterAndSort(node => {
        if(node.isSelected()){
          selectedFilteredSortedRows.push(parseInt(node.data.ChemigationPermitAnnualRecordID));
        }
      });
  
      if(selectedFilteredSortedRows.length === 0){
        this.alertService.pushAlert(new Alert("No permit record selected.", AlertContext.Danger));
      } else {
        this.isLoadingSubmit = true;
        var generateCPARReportsDto = new GenerateReportsDto();
        generateCPARReportsDto.ReportTemplateID = this.selectedReportTemplateID;
        generateCPARReportsDto.ModelIDList = selectedFilteredSortedRows;
        this.reportTemplateService.generateReport(generateCPARReportsDto)
          .subscribe(response => {
            this.isLoadingSubmit = false;
    
            var a = document.createElement("a");
            a.href = URL.createObjectURL(response);
            a.download = "Generated Report";
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
    this.watchUserChangeSubscription.unsubscribe();
    this.authenticationService.dispose();
    this.cdr.detach();
  }

}

  



