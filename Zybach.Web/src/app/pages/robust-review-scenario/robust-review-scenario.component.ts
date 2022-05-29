import { DatePipe } from '@angular/common';
import { ChangeDetectorRef, Component, ElementRef, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { AgGridAngular } from 'ag-grid-angular/lib/ag-grid-angular.component';
import { ColDef } from 'ag-grid-community';
import { Subject, timer } from 'rxjs';
import { switchMap, takeUntil } from 'rxjs/operators';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { RobustReviewScenarioService } from 'src/app/shared/generated/api/robust-review-scenario.service';
import { LinkRendererComponent } from 'src/app/shared/components/ag-grid/link-renderer/link-renderer.component';
import { CustomDropdownFilterComponent } from 'src/app/shared/components/custom-dropdown-filter/custom-dropdown-filter.component';
import { RobustReviewScenarioGETRunHistoryDto } from 'src/app/shared/generated/model/robust-review-scenario-get-run-history-dto';
import { UserDto } from 'src/app/shared/generated/model/user-dto';
import { Alert } from 'src/app/shared/models/alert';
import { AlertContext } from 'src/app/shared/models/enums/alert-context.enum';
import { CustomRichTextType } from 'src/app/shared/models/enums/custom-rich-text-type.enum';
import { AlertService } from 'src/app/shared/services/alert.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'zybach-robust-review-scenario',
  templateUrl: './robust-review-scenario.component.html',
  styleUrls: ['./robust-review-scenario.component.scss']
})
export class RobustReviewScenarioComponent implements OnInit, OnDestroy {
  @ViewChild('robustReviewScenarioGETRunHistoryGrid') robustReviewScenarioGETRunHistoryGrid: AgGridAngular;

  public richTextTypeID: number = CustomRichTextType.RobustReviewScenario;
  public fileDownloading: boolean = false;

  public watchUserChangeSubscription: any;
  public currentUser: UserDto;

  public robustReviewScenarioGETRunHistories: RobustReviewScenarioGETRunHistoryDto[];
  public columnDefs: ColDef[];
  public gridApi: any;
  public isGETAPIResponsive: boolean = false;

  private updateRobustReviewScenarioGETHistoriesSubscription: any;
  private stopUpdatePolling = new Subject();
  overlayLoadingTemplate: string;
  newRunTriggered: boolean;

  constructor(private robustReviewScenarioService: RobustReviewScenarioService,
    private alertService: AlertService,
    private authenticationService: AuthenticationService,
    private cdr: ChangeDetectorRef) { }

  ngOnInit(): void {
    this.authenticationService.getCurrentUser().subscribe(currentUser => {
      this.currentUser = currentUser;
      this.robustReviewScenarioService.robustReviewScenarioCheckGETAPIHealthGet().subscribe(response => {
          this.isGETAPIResponsive = response;
          if (!this.isGETAPIResponsive) {
            this.alertService.pushAlert(new Alert("Unable to establish communication with GET API, ability to trigger new runs and update status of in progress runs will be effected."));
          }
          this.initializeGrid();  
          this.updateRobustReviewScenarioGETHistoriesSubscription = timer(1, 300000).pipe(
            takeUntil(this.stopUpdatePolling)
          ).subscribe(() => {
            this.updateRobustReviewScenarioGETHistories();
          });
      })
    });
  }

  ngOnDestroy(): void {
    
    this.updateRobustReviewScenarioGETHistoriesSubscription?.unsubscribe();
       this.stopUpdatePolling.next();
    this.cdr.detach();
  }

  updateRobustReviewScenarioGETHistories() {
    this.robustReviewScenarioService.robustReviewScenariosGet().subscribe(response => {
      this.robustReviewScenarioGETRunHistoryGrid?.api.showLoadingOverlay();
      this.robustReviewScenarioGETRunHistories = response;
      this.robustReviewScenarioGETRunHistoryGrid.api.sizeColumnsToFit();
    });
  }

  private initializeGrid(): void {
    this.columnDefs = [
      this.createDateColumnDef('Date Created', 'CreateDate', 'M/d/yyyy'),
      {
        headerName: 'Created By User', valueGetter: function (params: any) {
          return { LinkValue: params.data.CreateByUser.UserID, LinkDisplay: params.data.CreateByUser.FullName };
        }, cellRendererFramework: LinkRendererComponent,
        cellRendererParams: { inRouterLink: "/users/" },
        filterValueGetter: function (params: any) {
          return params.data.FullName;
        },
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
        sortable: true, filter: true, width: 170
      },
      {
        headerName: 'Status', field: 'StatusMessage',
        filterFramework: CustomDropdownFilterComponent,
        filterParams: {
          field: 'StatusMessage'
        },
        resizable: true,
        sortable: true
      },
      {
        headerName: 'GET Action Details URL', valueGetter: function (params: any) {
          if (params.data.GETRunID == null) {
            return { LinkDisplay: null};
          }
          return { LinkValue: `${environment.GETEnvironmentUrl}/Action/ActionDetails?actionID=${params.data.GETRunID}`, LinkDisplay: `${environment.GETEnvironmentUrl}/Action/ActionDetails?actionID=${params.data.GETRunID}` };
        }, cellRendererFramework: LinkRendererComponent,
        cellRendererParams: { isExternalUrl: true },
        filterValueGetter: function (params: any) {
          return params.data.GETRunID;
        },
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
        sortable: true, filter: true, width: 500
      },];
      this.overlayLoadingTemplate =
      '<span class="ag-overlay-loading-center">Refreshing History data</span>';
  }

  private dateFilterComparator(filterLocalDateAtMidnight, cellValue) {
    const cellDate = Date.parse(cellValue);
    if (cellDate == filterLocalDateAtMidnight) {
      return 0;
    }
    return (cellDate < filterLocalDateAtMidnight) ? -1 : 1;
  }

  private createDateColumnDef(headerName: string, fieldName: string, dateFormat: string): ColDef {
    let datePipe = new DatePipe('en-US');

    return {
      headerName: headerName, valueGetter: function (params: any) {
        return datePipe.transform(params.data[fieldName], dateFormat);
      },
      comparator: this.dateFilterComparator,
      filter: 'agDateColumnFilter',
      filterParams: {
        filterOptions: ['inRange'],
        comparator: this.dateFilterComparator
      },
      resizable: true,
      sortable: true
    };
  }

  public onFirstDataRendered(params): void {
    this.gridApi = params.api;
    this.gridApi.sizeColumnsToFit();
  }

  public canCreateNewRun(): boolean {
    return this.isGETAPIResponsive && !this.newRunTriggered && (this.robustReviewScenarioGETRunHistories == null || this.robustReviewScenarioGETRunHistories.length ==  0 || this.robustReviewScenarioGETRunHistories.every(x => x.IsTerminal))
  }

  getRobustReviewScenarioJson() {
    this.fileDownloading = true;
    this.robustReviewScenarioService.robustReviewScenarioDownloadRobustReviewScenarioJsonGet().subscribe(x => {
      this.fileDownloading = false;
      //Create a fake object for us to click and download
      var a = document.createElement('a');
      a.href = URL.createObjectURL(x);
      const date = new Date();
      const month = ("0" + (date.getMonth() + 1)).slice(-2);
      const day = ("0" + (date.getDate())).slice(-2);
      a.download = `RobustReviewScenario-${date.getFullYear()}${month}${day}.json`;
      document.body.appendChild(a);
      a.click();
      //Revoke the generated url so the blob doesn't hang in memory https://javascript.info/blob
      URL.revokeObjectURL(a.href);
      document.body.removeChild(a);
    }, (() => {
      this.fileDownloading = false;
      this.alertService.pushAlert(new Alert(`There was an error while downloading the file. Please refresh the page and try again.`, AlertContext.Danger));
    }))
  }

  triggerNewRobustReviewScenarioRun() {
    this.newRunTriggered = true;
    this.robustReviewScenarioService.robustReviewScenarioNewPost().subscribe(x => {
      this.alertService.pushAlert(new Alert(`New Robust Review Scenario run was successfully requested.`, AlertContext.Success));
      this.updateRobustReviewScenarioGETHistories();
    },
    () => {
      this.alertService.pushAlert(new Alert(`Unable to request a new Robust Review Scenario run.`, AlertContext.Danger));
    },
    () => {
      this.newRunTriggered = false;
    })
  }

}
