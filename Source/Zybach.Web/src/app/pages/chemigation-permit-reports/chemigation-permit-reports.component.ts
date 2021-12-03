import { ChangeDetectorRef, Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { AgGridAngular } from 'ag-grid-angular';
import { ColDef } from 'ag-grid-community';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { ChemigationPermitService } from 'src/app/services/chemigation-permit.service';
import { UserDto } from 'src/app/shared/generated/model/user-dto';
import { CustomDropdownFilterComponent } from 'src/app/shared/components/custom-dropdown-filter/custom-dropdown-filter.component';
import { UtilityFunctionsService } from 'src/app/services/utility-functions.service';
import { ChemigationPermitAnnualRecordDto } from 'src/app/shared/generated/model/chemigation-permit-annual-record-dto';

@Component({
  selector: 'zybach-chemigation-permit-reports',
  templateUrl: './chemigation-permit-reports.component.html',
  styleUrls: ['./chemigation-permit-reports.component.scss']
})
export class ChemigationPermitReportsComponent implements OnInit {
  @ViewChild('chemigationPermitReportGrid') chemigationPermitReportGrid: AgGridAngular;
  
  private watchUserChangeSubscription: any;
  private currentUser: UserDto;
  
  public rowData: Array<ChemigationPermitAnnualRecordDto>;
  public columnDefs: ColDef[];
  
  public gridApi: any;

  constructor(
    private authenticationService: AuthenticationService,
    private chemigationPermitService: ChemigationPermitService,
    private cdr: ChangeDetectorRef,
    private utilityFunctionsService: UtilityFunctionsService
  ) { }

  ngOnInit(): void {
    this.watchUserChangeSubscription = this.authenticationService.currentUserSetObservable.subscribe(currentUser => {
      this.currentUser = currentUser;
      this.chemigationPermitReportGrid.api.showLoadingOverlay();
      this.initializeGrid();
    });
  }

  initializeGrid() {
    this.columnDefs = [
      { headerName: 'Permit #', field: 'ChemigationPermit.ChemigationPermitNumber', filter: true, resizable: true, sort: 'asc' },
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
        filterFramework: CustomDropdownFilterComponent,
        filterParams: {
          field: 'RecordYear',
        }, 
        resizable: true, sortable: true
      },
      { headerName: 'Renewal Status', field: 'ChemigationPermitAnnualRecordStatus.ChemigationPermitAnnualRecordStatusDisplayName',
        filterFramework: CustomDropdownFilterComponent,
        filterParams: {
          field: 'ChemigationPermitAnnualRecordStatus.ChemigationPermitAnnualREcordStatusDisplayName'
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
        return params.data.ApplicantPhone ? params.data.ApplicantPhone : '-';
        },
        filter: true, resizable: true, sortable: true 
      },
      { headerName: 'Mobile Phone', valueGetter: function (params: any) {
        return params.data.ApplicantMobilePhone ? params.data.ApplicantMobilePhone : '-';
        },
        filter: true, resizable: true, sortable: true 
      }
    ]; 
  }
  
  public exportToCsv() {
    this.utilityFunctionsService.exportGridToCsv(this.chemigationPermitReportGrid, 'chemigation-permit-report.csv', null);
  }
  
  public onGridReady(gridEvent) {
    this.chemigationPermitService.getAllAnnualRecords().subscribe(annualRecords => {
      this.rowData = annualRecords;
      this.chemigationPermitReportGrid.api.hideOverlay();
      this.chemigationPermitReportGrid.api.sizeColumnsToFit();
    });
  
  }
  
  onFilterChanged(gridEvent) {

  }
  
  ngOnDestroy(): void {
    this.watchUserChangeSubscription.unsubscribe();
    this.authenticationService.dispose();
    this.cdr.detach();
  }

}

  



