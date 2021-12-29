import { DatePipe, DecimalPipe } from '@angular/common';
import { ChangeDetectorRef, Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { AgGridAngular } from 'ag-grid-angular';
import { ColDef } from 'ag-grid-community';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { UtilityFunctionsService } from 'src/app/services/utility-functions.service';
import { WaterLevelInspectionService } from 'src/app/services/water-level-inspection.service';
import { LinkRendererComponent } from 'src/app/shared/components/ag-grid/link-renderer/link-renderer.component';
import { CustomDropdownFilterComponent } from 'src/app/shared/components/custom-dropdown-filter/custom-dropdown-filter.component';
import { UserDto } from 'src/app/shared/generated/model/user-dto';
import { WaterLevelInspectionSimpleDto } from 'src/app/shared/generated/model/water-level-inspection-simple-dto';
import { CustomRichTextType } from 'src/app/shared/models/enums/custom-rich-text-type.enum';

@Component({
  selector: 'zybach-water-level-inspection-list',
  templateUrl: './water-level-inspection-list.component.html',
  styleUrls: ['./water-level-inspection-list.component.scss']
})
export class WaterLevelInspectionListComponent  implements OnInit, OnDestroy {
  @ViewChild('waterLevelInspectionsGrid') waterLevelInspectionsGrid: AgGridAngular;

  private watchUserChangeSubscription: any;
  private currentUser: UserDto;

  public richTextTypeID : number = CustomRichTextType.WaterLevelInspections;

  public columnDefs: any[];
  public defaultColDef: ColDef;
  public waterLevelInspections: Array<WaterLevelInspectionSimpleDto>;
  public currentYear: number;
  public countOfActivePermitsWithoutRenewalRecordsForCurrentYear: number;

  public gridApi: any;

  public modalReference: NgbModalRef;
  public isPerformingAction: boolean = false;
  public closeResult: string;

  constructor(
    private authenticationService: AuthenticationService,
    private waterLevelInspectionService: WaterLevelInspectionService,
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
      this.waterLevelInspectionsGrid.api.showLoadingOverlay();
      this.updateGridData();
    });
  }
  
  private initializeGrid(): void {
    let datePipe = this.datePipe;
    this.columnDefs = [
      {
        headerName: "Date", 
        valueGetter: function (params: any) {
          return datePipe.transform(params.data.InspectionDate, "M/dd/yyyy");
        },
        comparator: function (id1: any, id2: any) {
          const date1 = Date.parse(id1);
          const date2 = Date.parse(id2);
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
        headerName: 'Status', field: 'WaterLevelInspectionStatus', 
        filterFramework: CustomDropdownFilterComponent,
        filterParams: {
          field: 'WaterLevelInspectionStatus'
        }, resizable: true, sortable: true 
      },
      { 
        headerName: 'Measuring Equipment', field: 'MeasuringEquipment', 
        filterFramework: CustomDropdownFilterComponent,
        filterParams: {
          field: 'MeasuringEquipment'
        }, resizable: true, sortable: true 
      },
      { 
        headerName: 'Accuracy', field: 'Accuracy', 
        filterFramework: CustomDropdownFilterComponent,
        filterParams: {
          field: 'Accuracy'
        }, resizable: true, sortable: true 
      },
      { 
        headerName: 'Method', field: 'Method', 
        filterFramework: CustomDropdownFilterComponent,
        filterParams: {
          field: 'Method'
        }, resizable: true, sortable: true 
      },
      { 
        headerName: 'Party', field: 'Party', 
        filterFramework: CustomDropdownFilterComponent,
        filterParams: {
          field: 'Party'
        }, resizable: true, sortable: true 
      },
      { 
        headerName: 'Source Agency', field: 'SourceAgency', 
        filterFramework: CustomDropdownFilterComponent,
        filterParams: {
          field: 'SourceAgency'
        }, resizable: true, sortable: true 
      },
      { 
        headerName: 'Source Code', field: 'SourceCode', 
        filterFramework: CustomDropdownFilterComponent,
        filterParams: {
          field: 'SourceCode'
        }, resizable: true, sortable: true 
      },
      { 
        headerName: 'Time Datum Code', field: 'TimeDatumCode', 
        filterFramework: CustomDropdownFilterComponent,
        filterParams: {
          field: 'TimeDatumCode'
        }, resizable: true, sortable: true 
      },
      { 
        headerName: 'Time Datum Reliability', field: 'TimeDatumReliability', 
        filterFramework: CustomDropdownFilterComponent,
        filterParams: {
          field: 'TimeDatumReliability'
        }, resizable: true, sortable: true 
      },
      { 
        headerName: 'Level Type Code', field: 'LevelTypeCode', 
        filterFramework: CustomDropdownFilterComponent,
        filterParams: {
          field: 'LevelTypeCode'
        }, resizable: true, sortable: true 
      },
      { 
        headerName: 'Agency Code', field: 'AgencyCode', 
        filterFramework: CustomDropdownFilterComponent,
        filterParams: {
          field: 'AgencyCode'
        }, resizable: true, sortable: true 
      },
      { 
        headerName: 'Access', 
        field: 'Access',
        filter: 'agNumberColumnFilter',
        type: 'rightAligned',
        resizable: true,
        sortable: true
      },
      { 
        headerName: 'Hold', 
        field: 'Hold',
        filter: 'agNumberColumnFilter',
        type: 'rightAligned',
        resizable: true,
        sortable: true
      },
      { 
        headerName: 'Cut', 
        field: 'Cut',
        filter: 'agNumberColumnFilter',
        type: 'rightAligned',
        resizable: true,
        sortable: true
      },
      { 
        headerName: 'MP', 
        field: 'MP',
        filter: 'agNumberColumnFilter',
        type: 'rightAligned',
        resizable: true,
        sortable: true
      },
      { 
        headerName: 'Measurement', 
        field: 'Measurement',
        filter: 'agNumberColumnFilter',
        type: 'rightAligned',
        resizable: true,
        sortable: true
      },
      { 
        headerName: 'Water Level', 
        field: 'WaterLevel',
        filter: 'agNumberColumnFilter',
        type: 'rightAligned',
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
    this.waterLevelInspectionService.getWaterLevelInspections().subscribe(waterLevelInspections => {
      this.waterLevelInspections = waterLevelInspections;
      this.waterLevelInspectionsGrid ? this.waterLevelInspectionsGrid.api.setRowData(waterLevelInspections) : null;
    });
  }

  public exportToCsv() {
    this.utilityFunctionsService.exportGridToCsv(this.waterLevelInspectionsGrid, 'waterLevelInspections.csv', null);
  }

  ngOnDestroy(): void {
    this.watchUserChangeSubscription.unsubscribe();
    this.authenticationService.dispose();
    this.cdr.detach();
  }
}


