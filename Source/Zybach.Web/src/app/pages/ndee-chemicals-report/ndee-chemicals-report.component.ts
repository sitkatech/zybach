import { ChangeDetectorRef, Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { AgGridAngular } from 'ag-grid-angular';
import { ColDef } from 'ag-grid-community';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { ChemigationPermitService } from 'src/app/services/chemigation-permit.service';
import { ChemicalFormulationYearlyTotalDto } from 'src/app/shared/generated/model/chemical-formulation-yearly-total-dto';
import { UserDto } from 'src/app/shared/generated/model/user-dto';
import { CustomDropdownFilterComponent } from 'src/app/shared/components/custom-dropdown-filter/custom-dropdown-filter.component';
import { CustomPinnedRowRendererComponent } from 'src/app/shared/components/ag-grid/custom-pinned-row-renderer/custom-pinned-row-renderer.component';
import { UtilityFunctionsService } from 'src/app/services/utility-functions.service';

@Component({
  selector: 'zybach-ndee-chemicals-report',
  templateUrl: './ndee-chemicals-report.component.html',
  styleUrls: ['./ndee-chemicals-report.component.scss']
})
export class NdeeChemicalsReportComponent implements OnInit, OnDestroy {
    @ViewChild('chemicalReportGrid') chemicalReportGrid: AgGridAngular;
  
    private watchUserChangeSubscription: any;
    private currentUser: UserDto;
    
    public chemicalFormulationYearlyTotals: Array<ChemicalFormulationYearlyTotalDto>;
    public columnDefs: ColDef[];
  
    public gridApi: any;
    public pinnedBottomRowData: any;
  
    constructor(
      private authenticationService: AuthenticationService,
      private chemigationPermitService: ChemigationPermitService,
      private cdr: ChangeDetectorRef,
      private utilityFunctionsService: UtilityFunctionsService
    ) { }

  ngOnInit(): void {
    this.watchUserChangeSubscription = this.authenticationService.currentUserSetObservable.subscribe(currentUser => {
      this.currentUser = currentUser;
      this.chemicalReportGrid.api.showLoadingOverlay();
      this.initializeGrid();
      this.chemigationPermitService.getChemicalFormulationYearlyTotals().subscribe(chemicalFormulationYearlyTotals => {
        this.chemicalFormulationYearlyTotals = chemicalFormulationYearlyTotals;
        this.chemicalReportGrid.api.hideOverlay();
        this.cdr.detectChanges();
      });

      this.chemicalReportGrid.api.hideOverlay();
    });
  }

  initializeGrid() {
    this.columnDefs = [
      {
        headerName: 'Record Year',
        field: 'RecordYear',
        resizable: true,
        sort: 'desc'
      },
      {
        headerName: 'Chemical Formulation',
        field: 'ChemicalFormulation',
        // filterFramework: CustomDropdownFilterComponent,
        // filterParams: {
        //   field: ''
        // },
        filter: true,
        resizable: true
      },
      { 
        headerName: 'Total Applied', 
        field: 'TotalApplied',
        resizable: true,
        sortable: true
      },
      { 
        headerName: 'Measurement Units', 
        field: 'ChemicalUnit.ChemicalUnitPluralName',
        resizable: true,
        sortable: true
      },
      { 
        headerName: 'Total Acres Treated',  
        valueGetter: function (params: any) {
        return params.node.rowPinned ? "Total: " + params.data.AcresTreated : params.data.AcresTreated;
        },
        pinnedRowCellRendererFramework: CustomPinnedRowRendererComponent,
        pinnedRowCellRendererParams: { filter: true },
        filter: true,
        resizable: true,
        sortable: true,
      }
    ];

  }

  public exportToCsv() {
    this.utilityFunctionsService.exportGridToCsv(this.chemicalReportGrid, 'chemical-report.csv', null);
  }

  public onFirstDataRendered(params): void {
    this.gridApi = params.api;

    this.pinnedBottomRowData = [
      { 
        AcresTreatedTotal: this.chemicalFormulationYearlyTotals.map(x => x.AcresTreated).reduce((sum, x) => sum + x, 0)
      }
    ];

    this.gridApi.sizeColumnsToFit();
  }
  
  ngOnDestroy(): void {
    this.watchUserChangeSubscription.unsubscribe();
    this.authenticationService.dispose();
    this.cdr.detach();
  }
}
