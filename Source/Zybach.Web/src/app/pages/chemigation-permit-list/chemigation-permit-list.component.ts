import { DatePipe } from '@angular/common';
import { ChangeDetectorRef, Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { AgGridAngular } from 'ag-grid-angular';
import { ColDef } from 'ag-grid-community';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { ChemigationPermitService } from 'src/app/services/chemigation-permit.service';
import { LinkRendererComponent } from 'src/app/shared/components/ag-grid/link-renderer/link-renderer.component';
import { CustomRichTextType } from 'src/app/shared/models/enums/custom-rich-text-type.enum';
import { CustomDropdownFilterComponent } from 'src/app/shared/components/custom-dropdown-filter/custom-dropdown-filter.component';
import { UserDto } from 'src/app/shared/generated/model/user-dto';
import { ChemigationPermitDetailedDto } from 'src/app/shared/generated/model/chemigation-permit-detailed-dto';
import { UtilityFunctionsService } from 'src/app/services/utility-functions.service';

@Component({
  selector: 'zybach-chemigation-permit-list',
  templateUrl: './chemigation-permit-list.component.html',
  styleUrls: ['./chemigation-permit-list.component.scss']
})
export class ChemigationPermitListComponent implements OnInit, OnDestroy {
  @ViewChild('permitGrid') permitGrid: AgGridAngular;

  private watchUserChangeSubscription: any;
  private currentUser: UserDto;

  public richTextTypeID : number = CustomRichTextType.Chemigation;

  public columnDefs: any[];
  public chemigationPermits: Array<ChemigationPermitDetailedDto>;
  public users: UserDto[];
  public unassignedUsers: UserDto[];

  public gridApi: any;

  constructor(
    private authenticationService: AuthenticationService,
    private chemigationPermitService: ChemigationPermitService,
    private utilityFunctionsService: UtilityFunctionsService,
    private cdr: ChangeDetectorRef
  ) { }

  ngOnInit(): void {
    this.watchUserChangeSubscription = this.authenticationService.currentUserSetObservable.subscribe(currentUser => {
      this.currentUser = currentUser;
      this.permitGrid.api.showLoadingOverlay();
      this.initializeGrid();
      this.chemigationPermitService.getChemigationPermits().subscribe(chemigationPermits => {
        this.chemigationPermits = chemigationPermits;
        this.permitGrid.api.hideOverlay();
        this.cdr.detectChanges();
      });
      this.permitGrid.api.hideOverlay();
    });
  }

  
  private initializeGrid(): void {
    let datePipe = new DatePipe('en-US');
    this.columnDefs = [
      {
        headerName: '', valueGetter: function (params: any) {
          return { LinkValue: params.data.ChemigationPermitNumber, LinkDisplay: "View", CssClasses: "btn-sm btn-zybach" };
        }, 
        cellRendererFramework: LinkRendererComponent,
        cellRendererParams: { inRouterLink: "/chemigation-permits/" },
        comparator: function (id1: any, id2: any) {
          let link1 = id1.LinkValue;
          let link2 = id2.LinkValue;
          if (link1 < link2) {
            return -1;
          }
          if (link1 > link2) {
            return 1;
          }
          return 0;
        },
        width: 80,
        resizable: true,
        sort: 'asc'
      },
      {
        headerName: 'Permit #',
        field: 'ChemigationPermitNumber',
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
        width: 120,
        filter: true,
        resizable: true,
        sort: 'asc',
      },
      { 
        headerName: 'Status', field: 'ChemigationPermitStatus.ChemigationPermitStatusDisplayName',
        filterFramework: CustomDropdownFilterComponent,
        filterParams: {
          field: 'ChemigationPermitStatus.ChemigationPermitStatusDisplayName'
        },
        width: 80,
        resizable: true,
        sortable: true
      },
      this.createDateColumnDef('Created', 'DateCreated', 'M/d/yyyy'),
      { headerName: 'Township-Range-Section', field: 'TownshipRangeSection', filter: true, resizable: true, sortable: true },
      {
        headerName: 'Latest Annual Record',
        children: [
          
          { headerName: 'Year', field: 'LatestAnnualRecord.RecordYear', width: 80, filter: true, resizable: true, sortable: true },
          { headerName: 'Status', field: 'LatestAnnualRecord.ChemigationPermitAnnualRecordStatusName', 
            filterFramework: CustomDropdownFilterComponent,
            filterParams: {
              field: 'LatestAnnualRecord.ChemigationPermitAnnualRecordStatusName'
            },
            width: 140, resizable: true, sortable: true },
          { headerName: 'Pivot', field: 'LatestAnnualRecord.PivotName', width: 100, filter: true, resizable: true, sortable: true },
          {
            headerName: 'Received', valueGetter: function (params: any) {
              return datePipe.transform(params.data.LatestAnnualRecord.DateReceived, 'M/d/yyyy');
            },
            comparator: this.dateFilterComparator,
            filter: 'agDateColumnFilter',
            filterParams: {
              filterOptions: ['inRange'],
              comparator: this.dateFilterComparator
            }, 
            width: 120,
            resizable: true,
            sortable: true
          },
          {
            headerName: 'Paid', valueGetter: function (params: any) {
              return datePipe.transform(params.data.LatestAnnualRecord.DatePaid, 'M/d/yyyy');
            },
            comparator: this.dateFilterComparator,
            filter: 'agDateColumnFilter',
            filterParams: {
              filterOptions: ['inRange'],
              comparator: this.dateFilterComparator
            }, 
            width: 120,
            resizable: true,
            sortable: true
          },
          { headerName: 'Applicant', 
            valueGetter: function (params) {
              return params.data.LatestAnnualRecord.ApplicantFirstName + " " + params.data.LatestAnnualRecord.ApplicantLastName;
            }
            , filter: true, resizable: true, sortable: true },
          { 
            headerName: 'Applied Chemicals', 
            valueGetter: function (params) {
              const applicators = params.data.LatestAnnualRecord.ChemicalFormulations.map(x => x.ChemicalFormulationName);
              if (applicators.length > 0) {
                return `${applicators.join(', ')}`;
              } else {
                return "-";
              }
            }, 
            filter: true, resizable: true, sortable: true 
          },
          { 
            headerName: 'Applicators', 
            valueGetter: function (params) {
              const applicators = params.data.LatestAnnualRecord.Applicators.map(x => x.ApplicatorName);
              if (applicators.length > 0) {
                return `${applicators.join(', ')}`;
              } else {
                return "-";
              }
            }, 
            filter: true, resizable: true, sortable: true 
          },
          { 
            headerName: 'Wells', 
            valueGetter: function (params) {
              const wells = params.data.LatestAnnualRecord.Wells.map(x => x.WellRegistrationID);
              if (wells.length > 0) {
                return `${wells.join(', ')}`;
              } else {
                return "-";
              }
            }, 
            filter: true, resizable: true, sortable: true 
          },
        ]
      }

    ];

  }

  private dateFilterComparator(filterLocalDateAtMidnight, cellValue) {
    const cellDate = Date.parse(cellValue);
    if (cellDate == filterLocalDateAtMidnight) {
      return 0;
    }
    return (cellDate < filterLocalDateAtMidnight) ? -1 : 1;
  }
  
  private createDateColumnDef(headerName: string, fieldName: string, dateFormat: string): ColDef {
    let datePipe = new DatePipe('en-US');
  
    return {
      headerName: headerName, valueGetter: function (params: any) {
        return datePipe.transform(params.data[fieldName], dateFormat);
      },
      comparator: this.dateFilterComparator,
      filter: 'agDateColumnFilter',
      filterParams: {
        filterOptions: ['inRange'],
        comparator: this.dateFilterComparator
      }, 
      width: 120,
      resizable: true,
      sortable: true
    };
  }
  
  public onFirstDataRendered(params): void {
    this.gridApi = params.api;
    this.gridApi.sizeColumnsToFit();
  }

  public exportToCsv() {
    this.utilityFunctionsService.exportGridToCsv(this.permitGrid, 'chemigation-permits.csv', null);
  }

  ngOnDestroy(): void {
    this.watchUserChangeSubscription.unsubscribe();
    this.authenticationService.dispose();
    this.cdr.detach();
  }
}


