import { ChangeDetectorRef, Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
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

  constructor(
    private authenticationService: AuthenticationService,
    private chemigationPermitService: ChemigationPermitService,
    private cdr: ChangeDetectorRef,
    private utilityFunctionsService: UtilityFunctionsService
  ) { }

  ngOnInit(): void {
    this.currentYear = new Date().getFullYear();
    this.yearToDisplay = new Date().getFullYear(); 

    this.watchUserChangeSubscription = this.authenticationService.currentUserSetObservable.subscribe(currentUser => {
      this.currentUser = currentUser;
      this.chemigationPermitReportGrid.api.showLoadingOverlay();
      this.initializeGrid();
    });
  }

  initializeGrid() {
    this.columnDefs = [
      {
        headerName: 'Permit #',
        valueGetter: function (params: any) {
          return { LinkValue: params.data.ChemigationPermit?.ChemigationPermitNumber, LinkDisplay: params.data.ChemigationPermit?.ChemigationPermitNumber };
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
      { headerName: 'First Name', field: 'ApplicantFirstName', filter: true, resizable: true, sortable: true },
      { headerName: 'Last Name', field: 'ApplicantLastName', filter: true, resizable: true, sortable: true },
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
  
  ngOnDestroy(): void {
    this.watchUserChangeSubscription.unsubscribe();
    this.authenticationService.dispose();
    this.cdr.detach();
  }

}

  



