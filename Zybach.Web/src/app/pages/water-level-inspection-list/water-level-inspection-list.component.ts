import { DatePipe, DecimalPipe } from '@angular/common';
import { ChangeDetectorRef, Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { AgGridAngular } from 'ag-grid-angular';
import { ColDef } from 'ag-grid-community';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { UtilityFunctionsService } from 'src/app/services/utility-functions.service';
import { WaterLevelInspectionService } from 'src/app/shared/generated/api/water-level-inspection.service';
import { LinkRendererComponent } from 'src/app/shared/components/ag-grid/link-renderer/link-renderer.component';
import { CustomDropdownFilterComponent } from 'src/app/shared/components/custom-dropdown-filter/custom-dropdown-filter.component';
import { UserDto } from 'src/app/shared/generated/model/user-dto';
import { WaterLevelInspectionSimpleDto } from 'src/app/shared/generated/model/water-level-inspection-simple-dto';
import { CustomRichTextTypeEnum } from 'src/app/shared/generated/enum/custom-rich-text-type-enum';
import { FieldDefinitionGridHeaderComponent } from 'src/app/shared/components/field-definition-grid-header/field-definition-grid-header.component';
import { FieldDefinitionTypeEnum } from 'src/app/shared/generated/enum/field-definition-type-enum';

@Component({
  selector: 'zybach-water-level-inspection-list',
  templateUrl: './water-level-inspection-list.component.html',
  styleUrls: ['./water-level-inspection-list.component.scss']
})
export class WaterLevelInspectionListComponent  implements OnInit, OnDestroy {
  @ViewChild('waterLevelInspectionsGrid') waterLevelInspectionsGrid: AgGridAngular;

  
  private currentUser: UserDto;

  public richTextTypeID : number = CustomRichTextTypeEnum.WaterLevelInspections;

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
    this.authenticationService.getCurrentUser().subscribe(currentUser => {
      this.currentUser = currentUser;
      this.initializeGrid();

      this.currentYear = new Date().getFullYear();
      this.waterLevelInspectionsGrid?.api.showLoadingOverlay();
      this.updateGridData();
    });
  }
  
  private initializeGrid(): void {
    let datePipe = this.datePipe;
    this.columnDefs = [
      {
        headerName: "Year", field: "InspectionYear",
        filterFramework: CustomDropdownFilterComponent,
        filterParams: {
          field: 'InspectionYear'
        },
        width: 80,
        resizable: true,
        sortable: true
      },
      {
        headerName: "Date", 
        valueGetter: function (params: any) {
          return { LinkValue: params.data.WaterLevelInspectionID, LinkDisplay: datePipe.transform(params.data.InspectionDate, "M/dd/yyyy, h:mm a") };
        },
        cellRendererFramework: LinkRendererComponent,
        cellRendererParams: { inRouterLink: "/water-level-inspections/" },
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
        width: 140,
        resizable: true,
        sortable: true
      },
      {
        headerName: 'Well', valueGetter: function (params: any) {
          if(params.data.Well)
          {
            return { LinkValue: params.data.Well.WellID, LinkDisplay: params.data.Well.WellRegistrationID };
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
        headerComponentFramework: FieldDefinitionGridHeaderComponent, headerComponentParams: { fieldDefinitionTypeID: FieldDefinitionTypeEnum.WellRegistrationNumber },
        filter: true,
        width: 100,
        resizable: true,
        sortable: true
      },
      {
        headerName: "Well Nickname",
        field: "Well.WellNickname",
        headerComponentFramework: FieldDefinitionGridHeaderComponent, headerComponentParams: {fieldDefinitionTypeID: FieldDefinitionTypeEnum.WellNickname},
        width: 140,
        sortable: true, filter: true, resizable: true,        
      },
      {
        headerName: "Nickname",
        field: "InspectionNickname",
        width: 125,
        sortable: true, filter: true, resizable: true,        
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
        headerName: 'Page #', 
        field: 'Well.PageNumber',
        filter: true,
        type: 'rightAligned',
        width: 80,
        resizable: true,
        sortable: true
      },
      { 
        headerName: 'Hold', 
        field: 'Hold',
        filter: 'agNumberColumnFilter',
        type: 'rightAligned',
        width: 70,
        resizable: true,
        sortable: true
      },
      { 
        headerName: 'Cut', 
        field: 'Cut',
        filter: 'agNumberColumnFilter',
        type: 'rightAligned',
        width: 70,
        resizable: true,
        sortable: true
      },
      { 
        headerName: 'MP', 
        field: 'MP',
        filter: 'agNumberColumnFilter',
        type: 'rightAligned',
        width: 60,
        resizable: true,
        sortable: true
      },
      { 
        headerName: 'Measurement', 
        field: 'Measurement',
        filter: 'agNumberColumnFilter',
        type: 'rightAligned',
        width: 120,
        resizable: true,
        sortable: true
      },
      { 
        headerName: 'Measuring Equipment', field: 'MeasuringEquipment', 
        filterFramework: CustomDropdownFilterComponent,
        filterParams: {
          field: 'MeasuringEquipment'
        }, 
        width: 170,
        resizable: true,
         sortable: true 
      },
      { 
        headerName: 'Accuracy', field: 'Accuracy', 
        filterFramework: CustomDropdownFilterComponent,
        filterParams: {
          field: 'Accuracy'
        }, 
        width: 100,
        resizable: true, 
        sortable: true 
      },
      { 
        headerName: 'Method', field: 'Method', 
        filterFramework: CustomDropdownFilterComponent,
        filterParams: {
          field: 'Method'
        }, 
        width: 90,
        resizable: true, 
        sortable: true 
      },      
      { 
        headerName: 'Participation', field: 'Well.WellParticipationName',
        filterFramework: CustomDropdownFilterComponent,
        filterParams: {
          field: 'Well.WellParticipationName'
        }, 
        width: 120,
        resizable: true, sortable: true 
      },
      { 
        headerName: 'Party', field: 'Party', 
        filterFramework: CustomDropdownFilterComponent,
        filterParams: {
          field: 'Party'
        }, 
        width: 70,
        resizable: true, 
        sortable: true 
      },
      { 
        headerName: 'Source Agency', field: 'SourceAgency', 
        filterFramework: CustomDropdownFilterComponent,
        filterParams: {
          field: 'SourceAgency'
        }, 
        width: 130,
        resizable: true, 
        sortable: true 
      },
      { 
        headerName: 'Source Code', field: 'SourceCode', 
        filterFramework: CustomDropdownFilterComponent,
        filterParams: {
          field: 'SourceCode'
        }, 
        width: 120,
        resizable: true, 
        sortable: true 
      },
      { 
        headerName: 'Time Datum Code', field: 'TimeDatumCode', 
        filterFramework: CustomDropdownFilterComponent,
        filterParams: {
          field: 'TimeDatumCode'
        }, 
        width: 140,
        resizable: true, 
        sortable: true 
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
        }, 
        width: 130,
        resizable: true, 
        sortable: true 
      },
      { 
        headerName: 'Agency Code', field: 'AgencyCode', 
        filterFramework: CustomDropdownFilterComponent,
        filterParams: {
          field: 'AgencyCode'
        }, 
        width: 120,
        resizable: true, 
        sortable: true 
      },
      { 
        headerName: 'Notes', 
        field: 'InspectionNotes',
        filter: true,
        resizable: true,
        sortable: true
      },
    ];
  }

  private dateFilterComparator(filterLocalDate, cellValue) {
    const cellDate = Date.parse(cellValue);
    const filterLocalDateAtMidnight = filterLocalDate.getTime();
    if (cellDate == filterLocalDateAtMidnight) {
      return 0;
    }
    return (cellDate < filterLocalDateAtMidnight) ? -1 : 1;
  }
  
  public onFirstDataRendered(params): void {
    this.gridApi = params.api;
    this.gridApi.autoSizeAllColumns();
  }

  public updateGridData(): void {
    this.waterLevelInspectionService.waterLevelInspectionsGet().subscribe(waterLevelInspections => {
      this.waterLevelInspections = waterLevelInspections;
      this.waterLevelInspectionsGrid ? this.waterLevelInspectionsGrid.api.setRowData(waterLevelInspections) : null;
    });
  }

  public exportToCsv() {
    this.utilityFunctionsService.exportGridToCsv(this.waterLevelInspectionsGrid, 'waterLevelInspections.csv', null);
  }

  ngOnDestroy(): void {
    
       this.cdr.detach();
  }
}


