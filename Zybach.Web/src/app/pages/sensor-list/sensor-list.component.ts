import { DatePipe, DecimalPipe } from '@angular/common';
import { ChangeDetectorRef, Component, OnInit, ViewChild } from '@angular/core';
import { AgGridAngular } from 'ag-grid-angular';
import { ColDef } from 'ag-grid-community';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { SensorService } from 'src/app/shared/generated/api/sensor.service';
import { UtilityFunctionsService } from 'src/app/services/utility-functions.service';
import { LinkRendererComponent } from 'src/app/shared/components/ag-grid/link-renderer/link-renderer.component';
import { CustomDropdownFilterComponent } from 'src/app/shared/components/custom-dropdown-filter/custom-dropdown-filter.component';
import { SensorSimpleDto } from 'src/app/shared/generated/model/sensor-simple-dto';
import { UserDto } from 'src/app/shared/generated/model/user-dto';
import { CustomRichTextTypeEnum } from 'src/app/shared/generated/enum/custom-rich-text-type-enum';
import { FieldDefinitionTypeEnum } from 'src/app/shared/generated/enum/field-definition-type-enum';
import { FieldDefinitionGridHeaderComponent } from 'src/app/shared/components/field-definition-grid-header/field-definition-grid-header.component';
import { SensorTypeEnum } from 'src/app/shared/generated/enum/sensor-type-enum';

@Component({
  selector: 'zybach-sensor-list',
  templateUrl: './sensor-list.component.html',
  styleUrls: ['./sensor-list.component.scss']
})
export class SensorListComponent implements OnInit {
  @ViewChild('sensorsGrid') sensorsGrid: AgGridAngular;
  public gridApi: any;

  public currentUser: UserDto;
  
  public sensorColumnDefs: any[];
  public defaultColDef: ColDef;

  public sensors: Array<SensorSimpleDto>;

  public richTextTypeID : number = CustomRichTextTypeEnum.SensorList;

  constructor(
    private authenticationService: AuthenticationService,
    private datePipe: DatePipe,
    private sensorService: SensorService,
    private cdr: ChangeDetectorRef,
    private utilityFunctionsService: UtilityFunctionsService,
    private decimalPipe: DecimalPipe
    ) { }

  ngOnInit(): void {
    this.initializeSensorsGrid();

    this.authenticationService.getCurrentUser().subscribe(currentUser => {
      this.currentUser = currentUser;
      this.sensorService.sensorsGet().subscribe(sensors => {
        this.sensors = sensors;

        this.sensorsGrid?.api.setRowData(sensors);
        this.sensorsGrid.columnApi.autoSizeAllColumns();
      });
    });
  }

  public exportToCsv() {
    this.utilityFunctionsService.exportGridToCsv(this.sensorsGrid, 'sensor-list.csv', null);
  }

  public onGridReady(params) {
    this.gridApi = params.api;
  }

  private initializeSensorsGrid(): void {
    const _decimalPipe = this.decimalPipe;
    let datePipe = this.datePipe;
    this.sensorColumnDefs = [
    {
      valueGetter: params => {
        return { LinkValue: `${params.data.SensorID}/new-support-ticket`, LinkDisplay: "Create Ticket", CssClasses: "btn-sm btn-zybach" };
      },
      cellRenderer: LinkRendererComponent,
      cellRendererParams: { inRouterLink: "/sensors" },
      sortable: false, filter: false, width: 140
    },
    {
      headerName: 'Sensor Name', valueGetter: function (params: any) {
        return { LinkValue: params.data.SensorID, LinkDisplay: params.data.SensorName };
      }, cellRenderer: LinkRendererComponent,
      cellRendererParams: { inRouterLink: "/sensors/" },
      comparator: this.utilityFunctionsService.linkRendererComparator,
      filterValueGetter: function (params: any) {
        return params.data.SensorName;
      },
      width: 120,
      sortable: true, filter: true, resizable: true
    },
    { 
      headerName: 'Sensor Type', field: 'SensorTypeName',
      filter: CustomDropdownFilterComponent,
      filterParams: {
      field: 'SensorTypeName'
      },
      headerComponent: FieldDefinitionGridHeaderComponent, headerComponentParams: { fieldDefinitionTypeID: FieldDefinitionTypeEnum.SensorType },
      resizable: true, sortable: true, width: 120
    },
    { 
      headerName: 'Status', field: 'IsActive',
      valueGetter: (params) => {
        return params.data.IsActive ? "Enabled" : "Disabled";
      },
      filter: CustomDropdownFilterComponent,
      filterParams: {
      field: 'IsActive'
      },
      headerComponent: FieldDefinitionGridHeaderComponent, headerComponentParams: { fieldDefinitionTypeID: FieldDefinitionTypeEnum.SensorStatus },
      resizable: true, sortable: true, width: 120
    },
    { 
      headerName: 'Last Message Age (Hours)', 
      valueGetter: (params) => params.data.LastMessageAgeInHours ? params.data.LastMessageAgeInHours : null,
      valueFormatter: params => params.value != null ? _decimalPipe.transform(params.value, '1.0-0') : '-',
      filter: 'agNumberColumnFilter', cellStyle: { textAlign: 'right' },
      headerComponent: FieldDefinitionGridHeaderComponent, headerComponentParams: { fieldDefinitionTypeID: FieldDefinitionTypeEnum.SensorLastMessageAgeHours },
      sortable: true, resizable: true
    },
    this.createDateColumnDef(datePipe, 'Last Measurement Date', 'LastReadingDate', 'M/d/yyyy', FieldDefinitionTypeEnum.SensorLastReadingDate),
    this.utilityFunctionsService.createDecimalColumnDef('Last Voltage Reading (mV)', 'LastVoltageReading', null, 0, true, FieldDefinitionTypeEnum.SensorLastVoltageReading),
    this.createDateColumnDef(datePipe, 'Last Voltage Reading Date', 'LastVoltageReadingDate', 'M/d/yyyy', FieldDefinitionTypeEnum.SensorLastVoltageReadingDate),
    this.createDateColumnDef(datePipe, 'First Reading Date', 'FirstReadingDate', 'M/d/yyyy', FieldDefinitionTypeEnum.SensorFirstReadingDate),
    {
      headerComponent: FieldDefinitionGridHeaderComponent,
      headerComponentParams: { fieldDefinitionTypeID: FieldDefinitionTypeEnum.ContinuityMeterStatus, labelOverride: 'Always On/Off' },
      valueGetter: params => params.data.SensorTypeID == SensorTypeEnum.ContinuityMeter ?
         params.data.SnoozeStartDate ? 'Snoozed' : params.data.ContinuityMeterStatus?.ContinuityMeterStatusDisplayName : 'N/A',
      sortable: true, resizable: true,
      filter: CustomDropdownFilterComponent,
      filterParams: { field: 'ContinuityMeterStatus?.ContinuityMeter' }
    },
    this.createDateColumnDef(datePipe, 'Retirement Date', 'RetirementDate', 'M/d/yyyy', FieldDefinitionTypeEnum.SensorRetirementDate),
    {
      headerName: 'Well',
      children: [
      {
        headerName: 'Well', valueGetter: function (params: any) {
          if(params.data.WellID)
          {
            return { LinkValue: params.data.WellID, LinkDisplay: params.data.WellRegistrationID };
          }
          else
          {
            return { LinkValue: null, LinkDisplay: null };
          }
        }, 
        cellRenderer: LinkRendererComponent,
        cellRendererParams: { inRouterLink: "/wells/" },
        comparator: this.utilityFunctionsService.linkRendererComparator,
        filterValueGetter: function (params: any) {
          return params.data.WellRegistrationID;
        },
        headerComponent: FieldDefinitionGridHeaderComponent, headerComponentParams: { fieldDefinitionTypeID: FieldDefinitionTypeEnum.WellRegistrationNumber },
        filter: true,
        width: 120,
        resizable: true,
        sortable: true
      },    
      {
        headerName: 'Well Owner Name', 
        field: 'WellOwnerName', 
        headerComponent: FieldDefinitionGridHeaderComponent, headerComponentParams: { fieldDefinitionTypeID: FieldDefinitionTypeEnum.WellOwnerName },
        sortable: true, filter: true, resizable: true
      },
      {
        headerName: 'Well Page #', 
        field: 'WellPageNumber', 
        filter: 'agNumberColumnFilter',
        sortable: true, resizable: true
      },
      {
        headerName: 'Well Legal', 
        field: 'WellTownshipRangeSection', 
        sortable: true, filter: true, resizable: true
      }
      ]
    }
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

  private createDateColumnDef(datePipe: DatePipe, headerName: string, fieldName: string, dateFormat: string, fieldDefinitionTypeID?: number): ColDef { 
    var dateColDef: ColDef =
    {
      headerName: headerName, valueGetter: function (params: any) {
        return datePipe.transform(params.data[fieldName], dateFormat);
      },
      comparator: this.dateSortComparer,
      filter: 'agDateColumnFilter',
      filterParams: {
        filterOptions: ['inRange'],
        comparator: this.dateFilterComparator
      },
      resizable: true,
      sortable: true
    }

    if (fieldDefinitionTypeID) {
      dateColDef.headerComponent = FieldDefinitionGridHeaderComponent;
      dateColDef.headerComponentParams = { fieldDefinitionTypeID: fieldDefinitionTypeID, labelOverride: headerName }
    }
    return dateColDef;
  }

  ngOnDestroy(): void {
    this.cdr.detach();
  }
}
