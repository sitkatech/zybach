import { DatePipe } from '@angular/common';
import { ChangeDetectorRef, Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { AgGridAngular } from 'ag-grid-angular';
import { ColDef } from 'ag-grid-community';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { ChemigationPermitService } from 'src/app/services/chemigation-permit.service';
import { LinkRendererComponent } from 'src/app/shared/components/ag-grid/link-renderer/link-renderer.component';
import { CustomRichTextType } from 'src/app/shared/models/enums/custom-rich-text-type.enum';
import { CustomDropdownFilterComponent } from 'src/app/shared/components/custom-dropdown-filter/custom-dropdown-filter.component';
import { ChemigationPermitDto } from 'src/app/shared/generated/model/chemigation-permit-dto';
import { UserDto } from 'src/app/shared/generated/model/user-dto';

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

  public columnDefs: ColDef[];
  public chemigationPermits: Array<ChemigationPermitDto>;
  public users: UserDto[];
  public unassignedUsers: UserDto[];

  public gridApi: any;

  constructor(
    private authenticationService: AuthenticationService,
    private chemigationPermitService: ChemigationPermitService,
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
        width: 60,
        resizable: true,
        sort: 'asc'
      },
      {
        headerName: 'Permit Number',
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
        resizable: true,
        sortable: true
      },
      this.createDateColumnDef('Date Created', 'DateCreated', 'M/d/yyyy'),
      { headerName: 'Township-Range-Section', field: 'TownshipRangeSection', filter: true, resizable: true, sortable: true }
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
      resizable: true,
      sortable: true
    };
  }
  
  public onFirstDataRendered(params): void {
    this.gridApi = params.api;
    this.gridApi.sizeColumnsToFit();
  }

  ngOnDestroy(): void {
    this.watchUserChangeSubscription.unsubscribe();
    this.authenticationService.dispose();
    this.cdr.detach();
  }
}


