import { ChangeDetectorRef, Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { AgGridAngular } from 'ag-grid-angular';
import { ColDef } from 'ag-grid-community';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { ChemicalFormulationYearlyTotalDto } from 'src/app/shared/generated/model/chemical-formulation-yearly-total-dto';
import { CustomRichTextTypeEnum } from 'src/app/shared/generated/enum/custom-rich-text-type-enum';
import { UserDto } from 'src/app/shared/generated/model/user-dto';
import { CustomDropdownFilterComponent } from 'src/app/shared/components/custom-dropdown-filter/custom-dropdown-filter.component';
import { UtilityFunctionsService } from 'src/app/services/utility-functions.service';
import { ChemigationPermitAnnualRecordChemicalFormulationService } from 'src/app/shared/generated/api/chemigation-permit-annual-record-chemical-formulation.service';

@Component({
  selector: 'zybach-ndee-chemicals-report',
  templateUrl: './ndee-chemicals-report.component.html',
  styleUrls: ['./ndee-chemicals-report.component.scss']
})
export class NdeeChemicalsReportComponent implements OnInit, OnDestroy {
    @ViewChild('chemicalReportGrid') chemicalReportGrid: AgGridAngular;
  
    
    private currentUser: UserDto;

    public richTextTypeID : number = CustomRichTextTypeEnum.NDEEChemicalsReport;
    
    public rowData: Array<ChemicalFormulationYearlyTotalDto>;
    public columnDefs: ColDef[];
  
    public gridApi: any;
    public pinnedBottomRowData: any;
  
    constructor(
      private authenticationService: AuthenticationService,
      private chemigationPermiAnnualRecordChemicalFormulationService: ChemigationPermitAnnualRecordChemicalFormulationService,
      private cdr: ChangeDetectorRef,
      private utilityFunctionsService: UtilityFunctionsService
    ) { }

  ngOnInit(): void {
    this.authenticationService.getCurrentUser().subscribe(currentUser => {
      this.currentUser = currentUser;
      this.chemicalReportGrid?.api.showLoadingOverlay();
      this.initializeGrid();
    });
  }

  initializeGrid() {
    this.columnDefs = [
      {
        headerName: 'Record Year',
        field: 'RecordYear',
        filter: CustomDropdownFilterComponent,
        filterParams: {
          field: 'RecordYear'
        },
        resizable: true,
        sortable: true,
        sort: 'desc'
      },
      {
        headerName: 'Chemical Formulation',
        field: 'ChemicalFormulation',
        filter: true,
        resizable: true,
        sortable: true
      },
      { 
        headerName: 'Total Applied', 
        field: 'TotalApplied',
        filter: 'agNumberColumnFilter',
        resizable: true,
        sortable: true
      },
      { 
        headerName: 'Measurement Units', 
        field: 'ChemicalUnit.ChemicalUnitPluralName',
        filter: CustomDropdownFilterComponent,
        filterParams: {
          field: 'ChemicalUnit.ChemicalUnitPluralName'
        },
        resizable: true,
        sortable: true
      },
      { 
        headerName: 'Total Acres Treated',  
        valueGetter: function (params: any) {
          return params.node.rowPinned ? "Total: " + params.data.AcresTreatedTotal : params.data.AcresTreated;
        },
        filter: 'agNumberColumnFilter',
        resizable: true,
        sortable: true
      }
    ];

  }

  public exportToCsv() {
    this.utilityFunctionsService.exportGridToCsv(this.chemicalReportGrid, 'chemicals-report.csv', null);
  }

  public onGridReady(params) {
    this.chemigationPermiAnnualRecordChemicalFormulationService.chemicalFormulationYearlyTotalsGet().subscribe(chemicalFormulationYearlyTotals => {
      this.rowData = chemicalFormulationYearlyTotals;
      this.chemicalReportGrid.api.hideOverlay();
      this.pinnedBottomRowData = [
        { 
          AcresTreatedTotal: this.rowData.map(x => x.AcresTreated).reduce((sum, x) => sum + x, 0).toFixed(2)
        }
      ];
      this.chemicalReportGrid.api.sizeColumnsToFit();
    });
  
  }

  
  onFilterChanged(gridEvent) {
    gridEvent.api.setPinnedBottomRowData([
      {
        AcresTreatedTotal: gridEvent.api.getModel().rowsToDisplay.map(x => x.data.AcresTreated).reduce((sum, x) => sum + x, 0).toFixed(2)
      }
    ]);
  }
  
  ngOnDestroy(): void {
    
       this.cdr.detach();
  }
}
