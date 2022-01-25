import { ChangeDetectorRef, Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AgGridAngular } from 'ag-grid-angular';
import { ColDef } from 'ag-grid-community';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { SensorService } from 'src/app/services/sensor.service';
import { UtilityFunctionsService } from 'src/app/services/utility-functions.service';
import { LinkRendererComponent } from 'src/app/shared/components/ag-grid/link-renderer/link-renderer.component';
import { CustomDropdownFilterComponent } from 'src/app/shared/components/custom-dropdown-filter/custom-dropdown-filter.component';
import { SensorSimpleDto } from 'src/app/shared/generated/model/sensor-simple-dto';
import { UserDto } from 'src/app/shared/generated/model/user-dto';
import { CustomRichTextType } from 'src/app/shared/models/enums/custom-rich-text-type.enum';
import { AlertService } from 'src/app/shared/services/alert.service';

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

  public richTextTypeID : number = CustomRichTextType.SensorList;

  constructor(
    private authenticationService: AuthenticationService,
    private sensorService: SensorService,
    private route: ActivatedRoute,
    private router: Router,
    private cdr: ChangeDetectorRef,
    private utilityFunctionsService: UtilityFunctionsService,
    private alertService: AlertService
  ) { }

  ngOnInit(): void {
    this.initializeSensorsGrid();

    this.authenticationService.getCurrentUser().subscribe(currentUser => {
      this.currentUser = currentUser;
      this.sensorService.listSensors().subscribe(sensors => {
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
    this.sensorColumnDefs = [
    {
      headerName: '', valueGetter: function (params: any) {
        return { LinkValue: params.data.SensorID, LinkDisplay: "View Sensor", CssClasses: "btn-sm btn-zybach" };
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
      width: 120,
      resizable: true
    },
    {
      headerName: 'Sensor Name', 
      field: 'SensorName', 
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
      headerName: 'Last Message Age (Hours)',
      valueGetter: (params) => Math.floor(params.data.MessageAge / 3600),
      filter: 'agNumberColumnFilter',
      sortable: true, resizable: true
    },
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
        return params.data.WellRegistrationID;
      },
      filter: true,
      width: 120,
      resizable: true,
      sortable: true
    }
    ];
  }

  ngOnDestroy(): void {
    this.cdr.detach();
  }
}
