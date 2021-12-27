import { ChangeDetectorRef, Component, OnInit, ViewChild } from '@angular/core';
import { AgGridAngular } from 'ag-grid-angular';
import { ColDef } from 'ag-grid-community';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { ChemigationPermitService } from 'src/app/services/chemigation-permit.service';
import { UserDto } from 'src/app/shared/generated/model/user-dto';
import { CustomDropdownFilterComponent } from 'src/app/shared/components/custom-dropdown-filter/custom-dropdown-filter.component';
import { UtilityFunctionsService } from 'src/app/services/utility-functions.service';
import { CustomRichTextType } from 'src/app/shared/models/enums/custom-rich-text-type.enum';
import { LinkRendererComponent } from 'src/app/shared/components/ag-grid/link-renderer/link-renderer.component';
import { AlertService } from 'src/app/shared/services/alert.service';
import { AlertContext } from 'src/app/shared/models/enums/alert-context.enum';
import { Alert } from 'src/app/shared/models/alert';
import { ChemigationInspectionSimpleDto } from 'src/app/shared/generated/model/chemigation-inspection-simple-dto';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'zybach-chemigation-inspections-list',
  templateUrl: './chemigation-inspections-list.component.html',
  styleUrls: ['./chemigation-inspections-list.component.scss']
})
export class ChemigationInspectionsListComponent implements OnInit {
  @ViewChild('chemigationInspectionsGrid') chemigationInspectionsGrid: AgGridAngular;
  
  private watchUserChangeSubscription: any;
  private currentUser: UserDto;
  
  public richTextTypeID : number = CustomRichTextType.ChemigationInspections;
  
  public rowData: Array<ChemigationInspectionSimpleDto>;
  public columnDefs: ColDef[];
  
  public gridApi: any;
  public gridColumnApi: any;
  
  public isLoadingSubmit: boolean;

  constructor(
    private alertService: AlertService,
    private authenticationService: AuthenticationService,
    private chemigationPermitService: ChemigationPermitService,
    private cdr: ChangeDetectorRef,
    private utilityFunctionsService: UtilityFunctionsService
  ) { }

  ngOnInit(): void {
    this.watchUserChangeSubscription = this.authenticationService.currentUserSetObservable.subscribe(currentUser => {
      this.currentUser = currentUser;
      this.chemigationInspectionsGrid.api.showLoadingOverlay();
      this.initializeGrid();
    });
  }

  initializeGrid() {
    this.columnDefs = [
      {
        headerName: 'Permit #',
        valueGetter: function (params: any) {
          return { LinkValue: params.data.ChemigationPermitNumber, LinkDisplay: params.data.ChemigationPermitNumberDisplay };
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
          return params.data.ChemigationPermitNumber;
        },
        filter: 'agNumberColumnFilter',
        resizable: true,
        sortable: true
      },
      { headerName: 'County', field: 'County',
        filterFramework: CustomDropdownFilterComponent,
        filterParams: {
          field: 'County',
        }, 
        resizable: true, sortable: true 
      },
      { headerName: 'Township-Range-Section', field: 'TownshipRangeSection', filter: true, resizable: true, sortable: true },
      this.createDateColumnDef('Inspection Date', 'InspectionDate', 'M/d/yyyy'),
      { headerName: 'Status', field: 'ChemigationInspectionStatusName', 
        filterFramework: CustomDropdownFilterComponent,
        filterParams: {
          field: 'ChemigationInspectionStatusName'
        },
        width: 100, resizable: true, sortable: true 
      },
      { headerName: 'Inspected By', field: "Inspector.FullNameLastFirst",
        filterFramework: CustomDropdownFilterComponent,
        filterParams: {
          field: 'Inspector.FullNameLastFirst'
        },
        width: 130, resizable: true, sortable: true 
      },
      { headerName: 'Mainline Check Valve', field: 'ChemigationMainlineCheckValveName',
        filterFramework: CustomDropdownFilterComponent,
        filterParams: {
          field: 'ChemigationMainlineCheckValveName'
        }, 
       resizable: true, sortable: true },
      { headerName: 'Low Pressure Valve', field: 'ChemigationLowPressureValveName',
        filterFramework: CustomDropdownFilterComponent,
        filterParams: {
          field: 'ChemigationLowPressureValveName'
        }, 
        resizable: true, sortable: true },
      { headerName: 'Injection Valve', field: 'ChemigationInjectionValveName',
        filterFramework: CustomDropdownFilterComponent,
        filterParams: {
          field: 'ChemigationInjectionValveName'
        }, 
        resizable: true, sortable: true },
      { headerName: 'Tillage', field: 'TillageName',
        filterFramework: CustomDropdownFilterComponent,
        filterParams: {
          field: 'TillageName'
        }, 
        resizable: true, sortable: true },
      { headerName: 'Crop Type', field: 'CropTypeName',
        filterFramework: CustomDropdownFilterComponent,
        filterParams: {
          field: 'CropTypeName'
        }, 
        resizable: true, sortable: true }
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
      width: 110,
      resizable: true,
      sortable: true
    };
  }
  
  public exportToCsv() {
    this.utilityFunctionsService.exportGridToCsv(this.chemigationInspectionsGrid, 'chemigation-inspections-report.csv', null);
  }
  
  public onGridReady(gridEvent) {
    this.populateInspectionGrid();
  }
  
  private populateInspectionGrid(): void {
    this.chemigationPermitService.getAllChemigationInspections().subscribe(chemigationInspections => {
      this.rowData = chemigationInspections;
      this.chemigationInspectionsGrid.api.hideOverlay();
      this.chemigationInspectionsGrid.api.sizeColumnsToFit();
    });
  }
  
  ngOnDestroy(): void {
    this.watchUserChangeSubscription.unsubscribe();
    this.authenticationService.dispose();
    this.cdr.detach();
  }

}  