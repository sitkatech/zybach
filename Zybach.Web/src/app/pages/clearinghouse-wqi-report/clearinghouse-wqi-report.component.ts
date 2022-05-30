import { ChangeDetectorRef, Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { DatePipe, DecimalPipe } from '@angular/common';
import { AgGridAngular } from 'ag-grid-angular';
import { ColDef } from 'ag-grid-community';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { UtilityFunctionsService } from 'src/app/services/utility-functions.service';
import { WaterQualityInspectionService } from 'src/app/shared/generated/api/water-quality-inspection.service';
import { ClearinghouseWaterQualityInspectionDto } from 'src/app/shared/generated/model/clearinghouse-water-quality-inspection-dto';
import { UserDto } from 'src/app/shared/generated/model/user-dto';
import { CustomRichTextTypeEnum } from 'src/app/shared/generated/enum/custom-rich-text-type-enum';
import { CustomDropdownFilterComponent } from 'src/app/shared/components/custom-dropdown-filter/custom-dropdown-filter.component';

@Component({
  selector: 'zybach-clearinghouse-wqi-report',
  templateUrl: './clearinghouse-wqi-report.component.html',
  styleUrls: ['./clearinghouse-wqi-report.component.scss']
})

export class ClearinghouseWqiReportComponent implements OnInit, OnDestroy {
  @ViewChild('clearinghouseReportGrid') clearinghouseReportGrid: AgGridAngular;
    
  private currentUser: UserDto;

  public richTextTypeID : number = CustomRichTextTypeEnum.ClearinghouseReport;
  
  public rowData: Array<ClearinghouseWaterQualityInspectionDto>;
  public columnDefs: ColDef[];

  public gridApi: any;

  constructor(
    private authenticationService: AuthenticationService,
    private waterQualityInspectionService: WaterQualityInspectionService,
    private cdr: ChangeDetectorRef,
    private utilityFunctionsService: UtilityFunctionsService,
    private datePipe: DatePipe,
    private decimalPipe: DecimalPipe
  ) { }

  ngOnInit(): void {
    this.authenticationService.getCurrentUser().subscribe(currentUser => {
      this.currentUser = currentUser;
      this.clearinghouseReportGrid?.api.showLoadingOverlay();
      this.initializeGrid();
    });
  }

  initializeGrid() {
    let datePipe = this.datePipe;
    this.columnDefs = [
      { 
        headerName: 'Year', 
        valueGetter: function (params) {
          if (params.data.SamplingDate) {
            return datePipe.transform(params.data.SamplingDate, 'yyyy');
          } else {
            return "-";
          }
        },
        filterFramework: CustomDropdownFilterComponent,
        filterParams: {
          field: 'Year'
        },
        resizable: true,
        sortable: true,
        width: 100
      },
      {
        headerName: 'Well Name',
        field: 'WellNickname',
        filter: true,
        resizable: true,
        sortable: true,
        width: 150
      },
      {
        headerName: 'Clearinghouse',
        field: 'Clearinghouse',
        filter: true,
        resizable: true,
        sortable: true,
        width: 120
      },
      {
        headerName: 'Contaminant',
        field: 'Contaminant',
        filterFramework: CustomDropdownFilterComponent,
        filterParams: {
          field: 'Contaminant'
        },
        resizable: true,
        sortable: true,
        width: 120
      },
      {
        headerName: 'Sampling Date', valueGetter: function (params: any) {
            return datePipe.transform(params.data.SamplingDate, 'M/d/yyyy');
        },
        comparator: this.dateSortComparer,
        filter: 'agDateColumnFilter',
        filterParams: {
          filterOptions: ['inRange'],
          comparator: this.dateFilterComparator
        },
        resizable: true,
        sortable: true,
        width: 140
      },
      { 
        headerName: 'Concentration', 
        field: 'LabConcentration',
        filter: 'agNumberColumnFilter',
        resizable: true,
        sortable: true,
        width: 120
      },
      { 
        headerName: 'Field Concentration', 
        field: 'FieldConcentration',
        filter: 'agNumberColumnFilter',
        resizable: true,
        sortable: true,
        width: 150
      },
      { 
        headerName: 'Reporting Limit', 
        field: 'ReportingLimit',
        filter: 'agNumberColumnFilter',
        resizable: true,
        sortable: true,
        width: 130
      },
      {
        headerName: 'Laboratory',
        field: 'Laboratory',
        filterFramework: CustomDropdownFilterComponent,
        filterParams: {
          field: 'Laboratory'
        },
        resizable: true,
        sortable: true,
        width: 110
      },
      {
        headerName: 'Well Registration #',
        field: 'WellRegistrationID',
        filter: true,
        resizable: true,
        sortable: true,
        width: 150
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
          field: 'IsReplacement'
        },
        sortable: true, 
        resizable: true,
        width: 130
      },
      { 
        headerName: 'TRS',
        field: 'TownshipRangeSection', 
        filter: true, 
        resizable: true,
        sortable: true,
        width: 120
      },
      { 
        headerName: 'Well Depth', 
        field: 'WellDepth',
        filter: 'agNumberColumnFilter',
        resizable: true,
        sortable: true,
        width: 110
      },
      { 
        headerName: 'Participation', 
        field: 'WellParticipation',
        filterFramework: CustomDropdownFilterComponent,
        filterParams: {
          field: 'WellParticipation'
        },
        resizable: true, 
        sortable: true,
        width: 120 
      },
      {
        headerName: 'Chemigation?', 
        valueGetter: function (params) {
          if (params.data.RequiresChemigationInspection) {
            return "Yes";
          } else {
            return "No";
          }
        },
        filterFramework: CustomDropdownFilterComponent,
        filterParams: {
          field: 'RequiresChemigationInspection'
        },
        sortable: true, 
        resizable: true,
        width: 120 
      },
      {
        headerName: 'Inspection Type', 
        field: 'WaterQualityInspectionType',
        filterFramework: CustomDropdownFilterComponent,
        filterParams: {
          field: 'WaterQualityInspectionType'
        },
        resizable: true,
        sortable: true,
        width: 130 
      },
      { 
        headerName: 'Well Use',
        field: 'WellUse',
        filterFramework: CustomDropdownFilterComponent,
        filterParams: {
          field: 'WellUse'
        },
        resizable: true,
        sortable: true,
        width: 100 
      },
      { 
        headerName: 'Pre Level', 
        field: 'WellPreLevel',
        filter: 'agNumberColumnFilter',
        resizable: true,
        sortable: true,
        width: 100 
      },
      { 
        headerName: 'Post Level', 
        field: 'WellPostLevel',
        filter: 'agNumberColumnFilter',
        resizable: true,
        sortable: true,
        width: 100 
      },
      { 
        headerName: 'Screen Interval', 
        field: 'ScreenInterval',
        filter: true,
        resizable: true,
        sortable: true,
        width: 130 
      },
    ];

  }

  private dateSortComparer (id1: any, id2: any) {
    const date1 = id1 ? Date.parse(id1) : Date.parse("1/1/1900");
    const date2 = id2 ? Date.parse(id2) : Date.parse("1/1/1900");
    if (date1 < date2) {
      return -1;
    }
    return (date1 > date2)  ?  1 : 0;
  }

  private dateFilterComparator(filterLocalDateAtMidnight, cellValue) {
    if(cellValue === null) return -1;
    const cellDate = Date.parse(cellValue);
    if (cellDate == filterLocalDateAtMidnight) {
      return 0;
    }
    return (cellDate < filterLocalDateAtMidnight) ? -1 : 1;
  }

  public exportToCsv() {
    this.utilityFunctionsService.exportGridToCsv(this.clearinghouseReportGrid, 'clearinghouse-wqi-report.csv', null);
  }

  public onGridReady(params) {
    this.waterQualityInspectionService.clearinghouseWaterQualityInspectionsGet().subscribe(clearinghouseWaterQualityInspections => {
      this.rowData = clearinghouseWaterQualityInspections;
      this.clearinghouseReportGrid.api.hideOverlay();
    });

  }

  ngOnDestroy(): void {
    
      this.cdr.detach();
  }
}