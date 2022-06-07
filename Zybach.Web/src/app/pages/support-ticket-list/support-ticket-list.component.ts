import { Component, OnInit, ViewChild } from '@angular/core';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { AgGridAngular } from 'ag-grid-angular';
import { ColDef } from 'ag-grid-community';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { UtilityFunctionsService } from 'src/app/services/utility-functions.service';
import { FontAwesomeIconLinkRendererComponent } from 'src/app/shared/components/ag-grid/fontawesome-icon-link-renderer/fontawesome-icon-link-renderer.component';
import { LinkRendererComponent } from 'src/app/shared/components/ag-grid/link-renderer/link-renderer.component';
import { CustomDropdownFilterComponent } from 'src/app/shared/components/custom-dropdown-filter/custom-dropdown-filter.component';
import { SupportTicketService } from 'src/app/shared/generated/api/support-ticket.service';
import { CustomRichTextTypeEnum } from 'src/app/shared/generated/enum/custom-rich-text-type-enum';
import { SupportTicketSimpleDto } from 'src/app/shared/generated/model/support-ticket-simple-dto';
import { UserDto } from 'src/app/shared/generated/model/user-dto';
import { Alert } from 'src/app/shared/models/alert';
import { AlertContext } from 'src/app/shared/models/enums/alert-context.enum';
import { AlertService } from 'src/app/shared/services/alert.service';

@Component({
  selector: 'zybach-support-ticket-list',
  templateUrl: './support-ticket-list.component.html',
  styleUrls: ['./support-ticket-list.component.scss']
})
export class SupportTicketListComponent implements OnInit {
  @ViewChild('supportTicketGrid') supportTicketGrid: AgGridAngular;
  @ViewChild('deleteSupportTicketModal') deleteSupportTicketModal;

  private currentUser: UserDto;

  public richTextTypeID : number = CustomRichTextTypeEnum.SupportTicketIndex;

  public columnDefs: any[];
  public defaultColDef: ColDef;
  public supportTickets: Array<SupportTicketSimpleDto>;

  public gridApi: any;
  public isLoadingDelete: boolean = false;
  private modalReference: NgbModalRef;
  public deleteColumnID = 0;
  public supportTicketToDelete: SupportTicketSimpleDto;

  constructor(
    private alertService: AlertService,
    private authenticationService: AuthenticationService,
    private supportTicketService: SupportTicketService,
    private modalService: NgbModal,
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
        cellRendererFramework: FontAwesomeIconLinkRendererComponent,
        cellRendererParams: { isSpan: true, fontawesomeIconName: 'trash', cssClasses: 'text-danger'},
        width: 50, sortable: false, filter: false, cellStyle: {textAlign: 'center'}
      },
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
          return { LinkValue: params.data.Sensor?.SensorID, LinkDisplay: params.data.Sensor?.SensorName };
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
        valueGetter: function (params: any) {
          return params.data.AssigneeUser?.FullName;
        },
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
    var colIDsToExport = this.supportTicketGrid.columnApi.getAllGridColumns().map(x => x.getId()).slice(1);
    this.utilityFunctionsService.exportGridToCsv(this.supportTicketGrid, 'support-tickets.csv', colIDsToExport);
  }

  public onCellClicked(event: any): void {
    if (event.column.colId == this.deleteColumnID) {
      if (this.supportTicketToDelete) {
        this.supportTicketToDelete = null;
      }
      this.supportTicketToDelete = this.supportTickets.find(x => x.SupportTicketID == event.data.SupportTicketID);
      this.launchModal(this.deleteSupportTicketModal, 'deleteSupportTicketModalTitle');
    }
  }

  public currentUserIsTicketOwner(supportTicket: SupportTicketSimpleDto): boolean {
    return this.currentUser.UserID == supportTicket.CreatorUser.UserID || this.currentUser.UserID == supportTicket.AssigneeUser?.UserID;
  }

  private checkIfDeleting(): boolean {
    return this.isLoadingDelete;
  }

  private launchModal(modalContent: any, modalTitle: string): void {
    this.modalReference = this.modalService.open(
      modalContent, 
      { ariaLabelledBy: modalTitle, beforeDismiss: () => this.checkIfDeleting(), backdrop: 'static', keyboard: false }
    );
  }

  public deleteSupportTicket() {
    this.isLoadingDelete = true;

    this.supportTicketService.supportTicketsSupportTicketIDDelete(this.supportTicketToDelete.SupportTicketID).subscribe(() => {
      this.isLoadingDelete = false;
      this.modalReference.close();
      this.alertService.pushAlert(new Alert('Support Ticket was successfully deleted.', AlertContext.Success, true));
      window.scroll(0,0);
      this.updateGridData();
    }, error => {
      this.isLoadingDelete = false;
      this.modalReference.close();
      window.scroll(0,0);
    })
  }

}
