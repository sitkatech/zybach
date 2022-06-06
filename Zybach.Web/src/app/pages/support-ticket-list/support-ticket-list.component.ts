import { Component, OnInit, ViewChild } from '@angular/core';
import { AgGridAngular } from 'ag-grid-angular';
import { ColDef } from 'ag-grid-community';
import { StatusPanelComponent } from 'ag-grid-community/dist/lib/components/framework/componentTypes';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { UtilityFunctionsService } from 'src/app/services/utility-functions.service';
import { LinkRendererComponent } from 'src/app/shared/components/ag-grid/link-renderer/link-renderer.component';
import { CustomDropdownFilterComponent } from 'src/app/shared/components/custom-dropdown-filter/custom-dropdown-filter.component';
import { SupportTicketService } from 'src/app/shared/generated/api/support-ticket.service';
import { SupportTicketSimpleDto } from 'src/app/shared/generated/model/support-ticket-simple-dto';
import { UserDto } from 'src/app/shared/generated/model/user-dto';
import { AlertService } from 'src/app/shared/services/alert.service';

@Component({
  selector: 'zybach-support-ticket-list',
  templateUrl: './support-ticket-list.component.html',
  styleUrls: ['./support-ticket-list.component.scss']
})
export class SupportTicketListComponent implements OnInit {
  @ViewChild('supportTicketGrid') supportTicketGrid: AgGridAngular;

  private currentUser: UserDto;

  //public richTextTypeID : number = CustomRichTextTypeEnum.IrrigationUnitIndex;

  public columnDefs: any[];
  public defaultColDef: ColDef;
  public supportTickets: Array<SupportTicketSimpleDto>;

  public gridApi: any;

  constructor(
    private alertService: AlertService,
    private authenticationService: AuthenticationService,
    private supportTicketService: SupportTicketService,
    private utilityFunctionsService: UtilityFunctionsService
  ) { }

  ngOnInit(): void {
    this.authenticationService.getCurrentUser().subscribe(currentUser => {
      this.currentUser = currentUser;
      this.initializeGrid();
      this.supportTicketGrid?.api.showLoadingOverlay();
      this.updateGridData();
    });
  }

  private initializeGrid(): void {
    this.columnDefs = [
      {
        headerName: 'Ticket ID',
        valueGetter: function (params: any) {
          return { LinkValue: params.data.SupportTicketID, LinkDisplay: params.data.SupportTicketID };
        }, 
        cellRendererFramework: LinkRendererComponent,
        cellRendererParams: { inRouterLink: "/support-tickets/" },
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
          return params.data.SupportTicketID;
        },
        filter: true,
        resizable: true,
        sortable: true
      },
      {
        headerName: 'Ticket Name',
        valueGetter: function (params: any) {
          return { LinkValue: params.data.SupportTicketID, LinkDisplay: params.data.SupportTicketTitle };
        }, 
        cellRendererFramework: LinkRendererComponent,
        cellRendererParams: { inRouterLink: "/support-tickets/" },
        filterValueGetter: function (params: any) {
          return params.data.SupportTicketTitle;
        },
        filter: true,
        resizable: true,
        sortable: true
      },
      this.utilityFunctionsService.createDateColumnDef('Date Updated', 'DateUpdated', 'M/d/yyyy', 'UTC', 130),
      this.utilityFunctionsService.createDateColumnDef('Date Created', 'DateCreated', 'M/d/yyyy', 'UTC', 130),
      {
        headerName: 'Well',
        valueGetter: function (params: any) {
          return { LinkValue: params.data.Well.WellID, LinkDisplay: params.data.Well.WellRegistrationID };
        }, 
        cellRendererFramework: LinkRendererComponent,
        cellRendererParams: { inRouterLink: "/wells/" },
        filterValueGetter: function (params: any) {
          return params.data.Well.WellRegistrationID;
        },
        filter: true,
        resizable: true,
        sortable: true
      },      
      {
        headerName: 'Sensor',
        valueGetter: function (params: any) {
          return { LinkValue: params.data.Sensor.SensorID, LinkDisplay: params.data.Sensor.SensorName };
        }, 
        cellRendererFramework: LinkRendererComponent,
        cellRendererParams: { inRouterLink: "/sensors/" },
        filterValueGetter: function (params: any) {
          return params.data.Sensor.SensorName;
        },
        filter: true,
        resizable: true,
        sortable: true
      },
      { 
        headerName: 'Status',
        field: 'Status.SupportTicketStatusDisplayName',
        filterFramework: CustomDropdownFilterComponent,
        filterParams: {
          field: 'Status.SupportTicketStatusDisplayName'
        },
        resizable: true,
        sortable: true
      },
      { 
        headerName: 'Priority',
        field: 'Priority.SupportTicketPriorityDisplayName',
        filterFramework: CustomDropdownFilterComponent,
        filterParams: {
          field: 'Priority.SupportTicketPriorityDisplayName'
        },
        resizable: true,
        sortable: true
      },
      { 
        headerName: 'Created By',
        field: 'CreatorUser.FullName',
        filter: true,
        resizable: true,
        sortable: true
      },
      { 
        headerName: 'Assigned To',
        field: 'AssigneeUser.FullName',
        filter: true,
        resizable: true,
        sortable: true
      }
    ]
  }

  public updateGridData(): void {
    this.supportTicketService.supportTicketsGet().subscribe(supportTickets => {
      this.supportTickets = supportTickets;
      this.supportTicketGrid ? this.supportTicketGrid.api.setRowData(supportTickets) : null;
    });
  }

  public onFirstDataRendered(params): void {
    this.gridApi = params.api;
    this.gridApi.sizeColumnsToFit();
  }

  public exportToCsv() {
    this.utilityFunctionsService.exportGridToCsv(this.supportTicketGrid, 'support-tickets.csv', null);
  }

}
