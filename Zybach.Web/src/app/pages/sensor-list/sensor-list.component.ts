import { DatePipe } from '@angular/common';
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
    private utilityFunctionsService: UtilityFunctionsService  ) { }

  ngOnInit(): void {
    this.initializeSensorsGrid();

    this.authenticationService.getCurrentUser().subscribe(currentUser => {
      this.currentUser = currentUser;
      this.sensorService.sensorsGet().subscribe(sensors => {
        this.sensors = sensors;

        this.sensorsGrid ? this.sensorsGrid.api.setRowData(sensors) : null;
  
        this.sensorsGrid.api.sizeColumnsToFit();
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
    let datePipe = this.datePipe;
    this.sensorColumnDefs = [
    {
      headerName: 'Sensor Name', valueGetter: function (params: any) {
        return { LinkValue: params.data.SensorID, LinkDisplay: params.data.SensorName };
      }, cellRendererFramework: LinkRendererComponent,
      cellRendererParams: { inRouterLink: "/sensors/" },
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
        return params.data.SensorName;
      },
      width: 120,
      sortable: true, filter: true, resizable: true
    },
    { 
      headerName: 'Sensor Type', field: 'SensorTypeName',
      filterFramework: CustomDropdownFilterComponent,
      filterParams: {
      field: 'SensorTypeName'
      },
      resizable: true, sortable: true, width: 120
    },
    { 
      headerName: 'Status', field: 'IsActive',
      valueGetter: (params) => {
        return params.data.IsActive ? "Enabled" : "Disabled";
      },
      filterFramework: CustomDropdownFilterComponent,
      filterParams: {
      field: 'IsActive'
      },
      resizable: true, sortable: true, width: 120
    },
    { 
      headerName: 'Last Message Age (Hours)',
      valueGetter: (params) => Math.floor(params.data.MessageAge / 3600),
      filter: 'agNumberColumnFilter',
      sortable: true, resizable: true
    },
    this.utilityFunctionsService.createDecimalColumnDef('Last Voltage Reading', 'LastVoltageReading', null, 0),
    this.createDateColumnDef(datePipe, 'First Reading Date', 'FirstReadingDate', 'M/d/yyyy'),
    this.createDateColumnDef(datePipe, 'Last Reading Date', 'LastReadingDate', 'M/d/yyyy'),
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
        cellRendererFramework: LinkRendererComponent,
        cellRendererParams: { inRouterLink: "/wells/" },
        comparator: this.utilityFunctionsService.linkRendererComparator,
        filterValueGetter: function (params: any) {
          return params.data.WellRegistrationID;
        },
        filter: true,
        width: 120,
        resizable: true,
        sortable: true
      },    
      {
        headerName: 'Well Owner Name', 
        field: 'WellOwnerName', 
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

  private createDateColumnDef(datePipe: DatePipe, headerName: string, fieldName: string, dateFormat: string): ColDef {
    return {
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
    };
  }

  ngOnDestroy(): void {
    this.cdr.detach();
  }
}