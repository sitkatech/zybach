import { DatePipe, DecimalPipe } from '@angular/common';
import { ChangeDetectorRef, Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { AgGridAngular } from 'ag-grid-angular';
import { ColDef } from 'ag-grid-community';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { UtilityFunctionsService } from 'src/app/services/utility-functions.service';
import { WaterQualityInspectionService } from 'src/app/services/water-quality-inspection.service';
import { LinkRendererComponent } from 'src/app/shared/components/ag-grid/link-renderer/link-renderer.component';
import { CustomDropdownFilterComponent } from 'src/app/shared/components/custom-dropdown-filter/custom-dropdown-filter.component';
import { UserDto } from 'src/app/shared/generated/model/user-dto';
import { WaterQualityInspectionSimpleDto } from 'src/app/shared/generated/model/water-quality-inspection-simple-dto';
import { CustomRichTextType } from 'src/app/shared/models/enums/custom-rich-text-type.enum';

@Component({
  selector: 'zybach-water-quality-inspection-list',
  templateUrl: './water-quality-inspection-list.component.html',
  styleUrls: ['./water-quality-inspection-list.component.scss']
})
export class WaterQualityInspectionListComponent implements OnInit, OnDestroy {
  @ViewChild('waterQualityInspectionsGrid') waterQualityInspectionsGrid: AgGridAngular;
  @ViewChild('createAnnualRenewalsModal') renewalEntity: any;

  private watchUserChangeSubscription: any;
  private currentUser: UserDto;

  public richTextTypeID : number = CustomRichTextType.WaterQualityInspections;

  public columnDefs: any[];
  public defaultColDef: ColDef;
  public waterQualityInspections: Array<WaterQualityInspectionSimpleDto>;
  public currentYear: number;
  public countOfActivePermitsWithoutRenewalRecordsForCurrentYear: number;

  public gridApi: any;

  public modalReference: NgbModalRef;
  public isPerformingAction: boolean = false;
  public closeResult: string;

  constructor(
    private authenticationService: AuthenticationService,
    private waterQualityInspectionService: WaterQualityInspectionService,
    private utilityFunctionsService: UtilityFunctionsService,
    private datePipe: DatePipe,
    private decimalPipe: DecimalPipe,
    private cdr: ChangeDetectorRef
  ) { }

  ngOnInit(): void {
    this.watchUserChangeSubscription = this.authenticationService.currentUserSetObservable.subscribe(currentUser => {
      this.currentUser = currentUser;
      this.initializeGrid();

      this.currentYear = new Date().getFullYear();
      this.waterQualityInspectionsGrid.api.showLoadingOverlay();
      this.updateGridData();
    });
  }
  
  private initializeGrid(): void {
    let datePipe = this.datePipe;
    let decimalPipe = this.decimalPipe;
    this.columnDefs = [
      {
        headerName: "Date", 
        valueGetter: function (params: any) {
          return { LinkValue: params.data.WaterQualityInspectionID, LinkDisplay: datePipe.transform(params.data.InspectionDate, "M/dd/yyyy") };
        },
        cellRendererFramework: LinkRendererComponent,
        cellRendererParams: { inRouterLink: "/water-quality-inspections/" },
        comparator: function (id1: any, id2: any) {
          const date1 = Date.parse(id1.LinkDisplay);
          const date2 = Date.parse(id2.LinkDisplay);
          if (date1 < date2) {
            return -1;
          }
          return (date1 > date2)  ?  1 : 0;
        },
        filter: 'agDateColumnFilter',
        filterParams: {
          filterOptions: ['inRange'],
          comparator: this.dateFilterComparator
        }, 
        width: 110,
        resizable: true,
        sortable: true
      },
      {
        headerName: 'Well', valueGetter: function (params: any) {
          if(params.data.Well)
          {
            return { LinkValue: params.data.Well.WellRegistrationID, LinkDisplay: params.data.Well.WellRegistrationID };
          }
          else
          {
            return { LinkValue: null, LinkDisplay: null };
          }
        }, 
        cellRendererFramework: LinkRendererComponent,
        cellRendererParams: { inRouterLink: "/wells/" },
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
        filterValueGetter: function (params: any) {
          return params.data.Well?.WellRegistrationID;
        },
        filter: true,
        width: 100,
        resizable: true,
        sortable: true
      },
      {
        headerName: 'TRS', 
        field: 'Well.TownshipRangeSection',
        filter: true,
        width: 100,
        resizable: true,
        sortable: true
      },
      {
        headerName: 'Inspected By', valueGetter: function (params: any) {
          if(params.data.Inspector)
          {
            return { LinkValue: params.data.Inspector.UserID, LinkDisplay: params.data.Inspector.FullNameLastFirst };
          }
          else
          {
            return { LinkValue: null, LinkDisplay: null };
          }
        }, 
        cellRendererFramework: LinkRendererComponent,
        cellRendererParams: { inRouterLink: "/users/" },
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
        filterValueGetter: function (params: any) {
          return params.data.Inspector.FullNameLastFirst;
        },
        filter: true,
        width: 130,
        resizable: true,
        sortable: true
      },
      { headerName: 'Type', field: 'WaterQualityInspectionTypeName',
        filterFramework: CustomDropdownFilterComponent,
        filterParams: {
          field: 'WaterQualityInspectionTypeName'
        }, 
        width: 100,
        resizable: true, sortable: true 
      },
      { headerName: 'Crop Type', field: 'CropTypeName',
        filterFramework: CustomDropdownFilterComponent,
        filterParams: {
          field: 'CropTypeName'
        }, 
        width: 100,
        resizable: true, sortable: true 
      },
      {
        headerName: 'Temperature (C)', 
        field: 'Temperature',
        filter: 'agNumberColumnFilter',
        type: 'rightAligned',
        width: 130,
        resizable: true,
        sortable: true
      },
      { 
        headerName: 'PH', 
        field: 'PH',
        filter: 'agNumberColumnFilter',
        type: 'rightAligned',
        width: 60,
        resizable: true,
        sortable: true
      },
      { 
        headerName: 'Conductivity', 
        field: 'Conductivity',
        filter: 'agNumberColumnFilter',
        type: 'rightAligned',
        width: 120,
        resizable: true,
        sortable: true
      },
      { 
        headerName: 'Field Alkilinity', 
        field: 'FieldAlkilinity',
        filter: 'agNumberColumnFilter',
        type: 'rightAligned',
        width: 130,
        resizable: true,
        sortable: true
      },
      { 
        headerName: 'Field Nitrates', 
        field: 'FieldNitrates',
        filter: 'agNumberColumnFilter',
        type: 'rightAligned',
        width: 130,
        resizable: true,
        sortable: true
      },
      { 
        headerName: 'Lab Nitrates', 
        field: 'LabNitrates',
        filter: 'agNumberColumnFilter',
        type: 'rightAligned',
        width: 110,
        resizable: true,
        sortable: true
      },
      { 
        headerName: 'Salinity', 
        field: 'Salinity',
        filter: 'agNumberColumnFilter',
        type: 'rightAligned',
        width: 110,
        resizable: true,
        sortable: true
      },
      { 
        headerName: 'MV', 
        field: 'MV',
        filter: 'agNumberColumnFilter',
        type: 'rightAligned',
        width: 60,
        resizable: true,
        sortable: true
      },
      { 
        headerName: 'Na', 
        field: 'Sodium',
        filter: 'agNumberColumnFilter',
        type: 'rightAligned',
        width: 60,
        resizable: true,
        sortable: true
      },
      { 
        headerName: 'Ca', 
        field: 'Calcium',
        filter: 'agNumberColumnFilter',
        type: 'rightAligned',
        width: 60,
        resizable: true,
        sortable: true
      },
      { 
        headerName: 'Mg', 
        field: 'Magnesium',
        filter: 'agNumberColumnFilter',
        type: 'rightAligned',
        width: 60,
        resizable: true,
        sortable: true
      },
      { 
        headerName: 'K', 
        field: 'Potassium',
        filter: 'agNumberColumnFilter',
        type: 'rightAligned',
        width: 60,
        resizable: true,
        sortable: true
      },
      { 
        headerName: 'HCO3-', 
        field: 'HydrogenCarbonate',
        filter: 'agNumberColumnFilter',
        type: 'rightAligned',
        width: 90,
        resizable: true,
        sortable: true
      },
      { 
        headerName: 'CaCO3-', 
        field: 'CalciumCarbonate',
        filter: 'agNumberColumnFilter',
        type: 'rightAligned',
        width: 90,
        resizable: true,
        sortable: true
      },
      { 
        headerName: 'SO42-', 
        field: 'Sulfate',
        filter: 'agNumberColumnFilter',
        type: 'rightAligned',
        width: 90,
        resizable: true,
        sortable: true
      },
      { 
        headerName: 'Cl', 
        field: 'Chloride',
        filter: 'agNumberColumnFilter',
        type: 'rightAligned',
        width: 60,
        resizable: true,
        sortable: true
      },
      { 
        headerName: 'SiO2', 
        field: 'SiliconDioxide',
        filter: 'agNumberColumnFilter',
        type: 'rightAligned',
        width: 90,
        resizable: true,
        sortable: true
      },
      { 
        headerName: 'Pre Water Level', 
        field: 'PreWaterLevel',
        filter: 'agNumberColumnFilter',
        type: 'rightAligned',
        width: 130,
        resizable: true,
        sortable: true
      },
      { 
        headerName: 'Post Water Level', 
        field: 'PostWaterLevel',
        filter: 'agNumberColumnFilter',
        type: 'rightAligned',
        width: 140,
        resizable: true,
        sortable: true
      },
      { 
        headerName: 'Notes', 
        field: 'InspectionNotes',
        filterable: true,
        resizable: true,
        sortable: true
      },
      {
        headerName: 'Site Name', 
        field: 'Well.SiteName',
        filter: true,
        resizable: true,
        sortable: true
      },
      {
        headerName: 'Site Number', 
        field: 'Well.SiteNumber',
        filter: true,
        resizable: true,
        sortable: true
      },
      {
        headerName: 'Page #', 
        field: 'Well.PageNumber',
        filter: true,
        width: 80,
        resizable: true,
        sortable: true
      },
      {
        headerName: 'Replacement Well?', 
        valueGetter: function (params) {
          if (params.data.IsReplacement) {
            return "Yes";
          } else {
            return "No";
          }
        },
        filterFramework: CustomDropdownFilterComponent,
        filterParams: {
          field: 'params.data.IsReplacement'
        },
        sortable: true, resizable: true
      },
    ];
  }

  private dateFilterComparator(filterLocalDate, cellValue) {
    const cellDate = Date.parse(cellValue.LinkDisplay);
    const filterLocalDateAtMidnight = filterLocalDate.getTime();
    if (cellDate == filterLocalDateAtMidnight) {
      return 0;
    }
    return (cellDate < filterLocalDateAtMidnight) ? -1 : 1;
  }
  
  public onFirstDataRendered(params): void {
    this.gridApi = params.api;
    this.gridApi.sizeColumnsToFit();
  }

  public updateGridData(): void {
    this.waterQualityInspectionService.getWaterQualityInspections().subscribe(waterQualityInspections => {
      this.waterQualityInspections = waterQualityInspections;
      this.waterQualityInspectionsGrid ? this.waterQualityInspectionsGrid.api.setRowData(waterQualityInspections) : null;
    });
  }

  public exportToCsv() {
    this.utilityFunctionsService.exportGridToCsv(this.waterQualityInspectionsGrid, 'waterQualityInspections.csv', null);
  }

  ngOnDestroy(): void {
    this.watchUserChangeSubscription.unsubscribe();
    this.authenticationService.dispose();
    this.cdr.detach();
  }
}

