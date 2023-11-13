import { Component, OnInit, ViewChild } from '@angular/core';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { OpenETService } from 'src/app/shared/generated/api/open-et.service';
import { Alert } from 'src/app/shared/models/alert';
import { AlertContext } from 'src/app/shared/models/enums/alert-context.enum';
import { AlertService } from 'src/app/shared/services/alert.service';
import { environment } from 'src/environments/environment';
import { finalize } from 'rxjs/operators';
import { CustomRichTextTypeEnum } from 'src/app/shared/generated/enum/custom-rich-text-type-enum';
import { UserDto } from 'src/app/shared/generated/model/user-dto';
import { OpenETSyncDto } from 'src/app/shared/generated/model/open-et-sync-dto';
import { OpenETRunDto } from 'src/app/shared/generated/model/open-et-run-dto';
import { ColDef } from 'ag-grid-community';
import { CustomDropdownFilterComponent } from 'src/app/shared/components/custom-dropdown-filter/custom-dropdown-filter.component';
import { UtilityFunctionsService } from 'src/app/services/utility-functions.service';
import { ConfirmService } from 'src/app/services/confirm.service';
import { ContextMenuRendererComponent } from 'src/app/shared/components/ag-grid/context-menu/context-menu-renderer.component';
import { AgGridAngular } from 'ag-grid-angular';

@Component({
  selector: 'zybach-openet-sync-water-year-month-status-list',
  templateUrl: './openet-sync-water-year-month-status-list.component.html',
  styleUrls: ['./openet-sync-water-year-month-status-list.component.scss']
})
export class OpenetSyncWaterYearMonthStatusListComponent implements OnInit {
  @ViewChild('openETGrid') openETGrid: AgGridAngular;
  
  public currentUser: UserDto;
  public richTextTypeID: number = CustomRichTextTypeEnum.OpenETIntegration;
  public modalReference: NgbModalRef;

  public openETSyncs: Array<OpenETSyncDto>;
  public syncsInProgress: OpenETSyncDto[];
  public selectedOpenETSync: OpenETSyncDto;
  public selectedOpenETSyncName: string;
  public isPerformingAction: boolean = false;

  public dateFormatString: string = "M/dd/yyyy hh:mm a";
  public monthNameFormatter: any = new Intl.DateTimeFormat('en-us', { month: 'long' });
  public isOpenETAPIKeyValid: boolean;
  public loadingPage: boolean = true;

  public columnDefs: ColDef[];
  public defaultColDef: ColDef;

  constructor(
    private authenticationService: AuthenticationService,
    private openETService: OpenETService,
    private modalService: NgbModal,
    private alertService: AlertService,
    private utilityFunctionsService: UtilityFunctionsService,
    private confirmService: ConfirmService
  ) { }

  ngOnInit() {
    this.authenticationService.getCurrentUser().subscribe(currentUser => {
      this.loadingPage = true;
      this.currentUser = currentUser;
      
      this.openETService.openetIsApiKeyValidGet().subscribe(isValid => {
        this.isOpenETAPIKeyValid = isValid;
        this.initializeGrid();
        this.refreshOpenETSyncsAndOpenETSyncData();
        this.loadingPage = false;
      })
    });
  }

  private refreshOpenETSyncsAndOpenETSyncData() {
    this.isPerformingAction = true;
    
    this.openETService.openetSyncGet().subscribe(openETSyncs => {
        this.isPerformingAction = false;
        this.openETSyncs = openETSyncs;
        this.syncsInProgress = this.openETSyncs.filter(x => x.HasInProgressSync);
      });
  }

  initializeGrid() {
    this.columnDefs = [];

    if (this.openETSyncEnabled()) {
      this.columnDefs.push(
        this.createActionColumn()
      )
    }

    this.columnDefs.push(
      {
        headerName: 'Year', 
        field: 'Year',
        filter: 'agNumberColumnFilter',
        width: 100
      },
      {
        headerName: 'Month',
        valueGetter: (params) => this.utilityFunctionsService.getMonthNameByMonthNumber(params.data.Month),
        width: 100
      },
      {
        headerName: 'Variable',
        field: 'OpenETDataType.OpenETDataTypeDisplayName',
        width: 150,
        filterFramework: CustomDropdownFilterComponent,
        filterParams: {
          field: 'OpenETDataType.OpenETDataTypeDisplayName',
        },
      },
      this.utilityFunctionsService.createDateColumnDef('Last Synced', 'LastSuccessfulSyncDate', 'M/d/yyyy', 'Last Imported', 150),
      this.utilityFunctionsService.createDateColumnDef('Date Finalized', 'FinalizeDate', 'M/d/yyyy', 'Date Finalized', 150),
      {
        headerName: 'Last Message',
        field: 'LastSyncMessage',
        flex: 1,
      },
      this.utilityFunctionsService.createDateColumnDef('Last Checked', 'LastSyncDate', 'M/d/yyyy', 'Last Checked', 150),
    );

    this.defaultColDef = { filter: true, sortable: true, resizable: true }
  }

  public createActionColumn(): ColDef {
    var actionColDef: ColDef = {
      headerName: "Actions",
      valueGetter: (params: any) => {
        if (params.data.FinalizeDate == null){
          return [
            { ActionName: "Sync",  ActionHandler: () => {
              this.confirmService.confirm({
                title: 'Sync Now',
                message: `Are you sure you want to query OpenET for data updates for ${this.utilityFunctionsService.getMonthNameByMonthNumber(params.data.Month)} ${params.data.Year}? Note: This may take some time to return data.`,
                buttonTextYes: 'Sync',
                buttonClassYes: 'btn-primary',
                buttonTextNo: 'Cancel',
                modalSize: 'small'
              }).then(confirmed => {
                if (confirmed) {
                  this.isPerformingAction = true;

                  let openETRunDto = new OpenETRunDto();
                  openETRunDto.Year = params.data.Year;
                  openETRunDto.Month = params.data.Month;
                  openETRunDto.OpenETDataTypeID = params.data.OpenETDataType.OpenETDataTypeID;
              
                  this.openETService.openetSyncHistoryTriggerOpenetGoogleBucketRefreshPost(openETRunDto).subscribe(() => {
                    this.isPerformingAction = false;
                    this.alertService.pushAlert(new Alert(`Sync successful for ${this.utilityFunctionsService.getMonthNameByMonthNumber(params.data.Month)} ${params.data.Year} ${params.data.OpenETDataType.OpenETDataTypeDisplayName}`, AlertContext.Success, true));
                    this.refreshOpenETSyncsAndOpenETSyncData();
                  }, error => {
                    this.isPerformingAction = false;
                  });
                }
              });
            }},
            { ActionName: "Finalize", ActionHandler: () => {
              this.confirmService.confirm({
                title: 'Finalize',
                message: `Are you sure you would like to finalize ${this.utilityFunctionsService.getMonthNameByMonthNumber(params.data.Month)} ${params.data.Year}?  The system will no longer query OpenET for updates once marked as final.`,
                buttonTextYes: 'Finalize',
                buttonClassYes: 'btn-primary',
                buttonTextNo: 'Cancel',
                modalSize: 'small'
              }).then(confirmed => {
                if (confirmed) {
                  this.isPerformingAction = true;

                  this.openETService.openetSyncOpenETSyncIDFinalizePut(params.data.OpenETSyncID).subscribe(() => {
                    this.isPerformingAction = false;
                    this.alertService.pushAlert(new Alert(`OpenET data successfully finalized`, AlertContext.Success, true));
                    this.refreshOpenETSyncsAndOpenETSyncData();
                  }, error => {
                    this.isPerformingAction = false;
                  });
                }
              });
            }}
          ];
        }
        return null;
      },
      cellRendererFramework: ContextMenuRendererComponent,
      cellClass: "context-menu-container",
      sortable: false, filter: false,width: 100
    }

    return actionColDef;
  }

  public exportToCsv() {
    this.utilityFunctionsService.exportGridToCsv(this.openETGrid, 'openet-syncs.csv', null);
  }

  public isCurrentUserAdministrator(): boolean {
    return this.authenticationService.isCurrentUserAnAdministrator();
  }

  public openETSyncEnabled() {
    return true;
    return environment.allowOpenETSync;
  }
}
