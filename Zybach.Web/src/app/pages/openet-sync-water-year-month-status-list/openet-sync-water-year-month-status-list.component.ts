import { Component, OnInit } from '@angular/core';
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

@Component({
  selector: 'zybach-openet-sync-water-year-month-status-list',
  templateUrl: './openet-sync-water-year-month-status-list.component.html',
  styleUrls: ['./openet-sync-water-year-month-status-list.component.scss']
})
export class OpenetSyncWaterYearMonthStatusListComponent implements OnInit {
  
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

  constructor(
    private authenticationService: AuthenticationService,
    private openETService: OpenETService,
    private modalService: NgbModal,
    private alertService: AlertService
  ) { }

  ngOnInit() {
    this.authenticationService.getCurrentUser().subscribe(currentUser => {
      this.loadingPage = true;
      this.currentUser = currentUser;
      
      this.openETService.openetIsApiKeyValidGet().subscribe(isValid => {
        this.isOpenETAPIKeyValid = isValid;
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

  public isCurrentUserAdministrator(): boolean {
    return this.authenticationService.isCurrentUserAnAdministrator();
  }

  public showActionButtonsForOpenETSync(openETSync: OpenETSyncDto): boolean {
    var currentDate = new Date();
    return (new Date(openETSync.Year, openETSync.Month - 1) <= new Date(currentDate.getFullYear(), currentDate.getMonth())) && openETSync.FinalizeDate == null;
  }

  public setSelectedOpenETSyncAndLaunchModal(modalContent: any, openETSync: OpenETSyncDto) {
    this.selectedOpenETSync = openETSync;
    this.selectedOpenETSyncName = this.monthNameFormatter.format(new Date(openETSync.Year, openETSync.Month - 1));
    this.launchModal(modalContent);
  }

  public launchModal(modalContent: any) {
    this.modalReference = this.modalService.open(modalContent, { windowClass: 'modal-size', backdrop: 'static', keyboard: false });
  }

  public resetOpenETSyncToFinalizeAndCloseModal(modal: any) {
    this.selectedOpenETSync = null;
    this.selectedOpenETSyncName = null;
    this.closeModal(modal, "Cancel click")
  }

  public closeModal(modal: any, reason: string) {
    modal.close(reason);
  }

  public syncOpenETSync() {
    if (this.modalReference) {
      this.modalReference.close();
      this.modalReference = null;
    }

    this.isPerformingAction = true;

    const openETRunDto = new OpenETRunDto({
      Year: this.selectedOpenETSync.Year,
      Month: this.selectedOpenETSync.Month,
      OpenETDataTypeID: this.selectedOpenETSync.OpenETDataType.OpenETDataTypeID
    });

    this.openETService.openetSyncHistoryTriggerOpenetGoogleBucketRefreshPost(openETRunDto).pipe(
      finalize(() => {
        this.isPerformingAction = false;
        this.selectedOpenETSync = null;
        this.selectedOpenETSyncName = null;
        this.refreshOpenETSyncsAndOpenETSyncData();
      }),
    ).subscribe(response => {
      this.alertService.pushAlert(new Alert(`The request to sync data for ${this.selectedOpenETSyncName} ${this.selectedOpenETSync.Year} was successfully submitted. The update may take a while, but will continue in the background.`, AlertContext.Success));
    });
  }

  public openETSyncEnabled() {
    return true;
    return environment.allowOpenETSync;
  }

  public finalizeOpenETSync() {
    if (this.modalReference) {
      this.modalReference.close();
      this.modalReference = null;
    }
    this.isPerformingAction = true;
    this.openETService.openetSyncOpenETSyncIDFinalizePut(this.selectedOpenETSync.OpenETSyncID).pipe(
      finalize(() => {
        this.isPerformingAction = false;
        this.selectedOpenETSync = null;
        this.selectedOpenETSyncName = null;
        this.refreshOpenETSyncsAndOpenETSyncData();
      }),
    ).subscribe(response => {
      this.alertService.pushAlert(new Alert(`The Evapotranspiration Data for ${this.selectedOpenETSyncName} ${this.selectedOpenETSync.Year} was successfully finalized`, AlertContext.Success));
    })
  }

  public syncInProgressForOpenETSync(openETSyncID : number): boolean {
    return this.syncsInProgress.length > 0 && this.syncsInProgress.some(x => x.OpenETSyncID == openETSyncID);
  }

  public getInProgressDates(): string {
    if (this.syncsInProgress.length == 0) {
      return "";
    }

    var allYearsInProgressUniqueInString = this.syncsInProgress.sort((x, y) => {
      //this should technically be an error case, we should never have two updates for the same month running at the same time
      if (x.Year == y.Year && x.Month == y.Month) {
        return 0;
      }

      if (x.Year > y.Year ||
        (x.Year == y.Year && x.Month > y.Month)) {
        return 1;
      }

      return -1;
    }).map(x => {
      let monthName = this.monthNameFormatter.format(new Date(x.Year, x.Month - 1));
      return `${monthName} ${x.Year}`
    }).join(", ");

    return allYearsInProgressUniqueInString;
  }
}
