import { ChangeDetectorRef, Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { ConfirmService } from 'src/app/services/confirm.service';
import { SupportTicketUpsertComponent } from 'src/app/shared/components/support-ticket-upsert/support-ticket-upsert.component';
import { SupportTicketService } from 'src/app/shared/generated/api/support-ticket.service';
import { SupportTicketStatusEnum } from 'src/app/shared/generated/enum/support-ticket-status-enum';
import { SupportTicketDetailDto } from 'src/app/shared/generated/model/support-ticket-detail-dto';
import { SupportTicketUpsertDto } from 'src/app/shared/generated/model/support-ticket-upsert-dto';
import { UserDto } from 'src/app/shared/generated/model/user-dto';
import { UserSimpleDto } from 'src/app/shared/generated/model/user-simple-dto';
import { Alert } from 'src/app/shared/models/alert';
import { AlertContext } from 'src/app/shared/models/enums/alert-context.enum';
import { AlertService } from 'src/app/shared/services/alert.service';
import internal from 'stream';

@Component({
  selector: 'zybach-support-ticket-edit',
  templateUrl: './support-ticket-edit.component.html',
  styleUrls: ['./support-ticket-edit.component.scss']
})
export class SupportTicketEditComponent implements OnInit {
  @ViewChild('supportTicketForm') private supportTicketUpsertComponent: SupportTicketUpsertComponent;
  
  private currentUser: UserDto;
  public model: SupportTicketUpsertDto;
  public isLoadingSubmit: boolean = false;
  public supportTicketID: number;
  public creatorUser: UserSimpleDto;
  private initialStatusID: number;

  constructor(
    private cdr: ChangeDetectorRef,
    private route: ActivatedRoute,
    private router: Router,
    private supportTicketService: SupportTicketService,
    private authenticationService: AuthenticationService,
    private alertService: AlertService,
    private confirmService: ConfirmService
  ) { }

  ngOnInit(): void {
    this.authenticationService.getCurrentUser().subscribe(currentUser => {
      this.currentUser = currentUser;
      this.supportTicketID = parseInt(this.route.snapshot.paramMap.get("id"));

      this.supportTicketService.supportTicketsSupportTicketIDGet(this.supportTicketID).subscribe(supportTicketDetailDto => {
        this.creatorUser = supportTicketDetailDto.CreatorUser;
        this.initialStatusID = supportTicketDetailDto.Status.SupportTicketStatusID;

        this.initializeSupportTicketModel(supportTicketDetailDto);
      });

      this.cdr.detectChanges();
    });
  }

  private initializeSupportTicketModel(supportTicketDetailDto: SupportTicketDetailDto) : void {
    var supportTicketUpsertDto = new SupportTicketUpsertDto();

    supportTicketUpsertDto.WellRegistrationID = supportTicketDetailDto.Well.WellRegistrationID;
    supportTicketUpsertDto.WellID = supportTicketDetailDto.Well.WellID;
    supportTicketUpsertDto.SensorName = supportTicketDetailDto.Sensor?.SensorName;
    supportTicketUpsertDto.SensorID = supportTicketDetailDto.Sensor?.SensorID;
    supportTicketUpsertDto.CreatorUserID = supportTicketDetailDto.CreatorUser.UserID;
    supportTicketUpsertDto.AssigneeUserID = supportTicketDetailDto.AssigneeUser?.UserID;
    supportTicketUpsertDto.SupportTicketPriorityID = supportTicketDetailDto.Priority.SupportTicketPriorityID;
    supportTicketUpsertDto.SupportTicketStatusID = supportTicketDetailDto.Status.SupportTicketStatusID;
    supportTicketUpsertDto.SupportTicketTitle = supportTicketDetailDto.SupportTicketTitle;
    supportTicketUpsertDto.SupportTicketDescription = supportTicketDetailDto.SupportTicketDescription;
  
    this.model = supportTicketUpsertDto;
  }

  public onSubmit(editSupportTicketForm: HTMLFormElement): void {
    if (this.model.SupportTicketStatusID == SupportTicketStatusEnum.Resolved && this.model.SupportTicketStatusID != this.initialStatusID) {
      
      const modalContents = "Reminder: Please review the data and create anomaly reports if needed before resolving this ticket.";
      this.confirmService.confirm({ modalSize: "md", buttonClassYes: "btn-zybach", buttonTextYes: "Save", buttonTextNo: "Cancel", title: "Resolve Ticket", message: modalContents }).then(confirmed => {
        if (confirmed) {
          this.SaveSupportTicket(editSupportTicketForm);
        }
      });

    } else {
      this.SaveSupportTicket(editSupportTicketForm);
    }
  }

  private SaveSupportTicket(editSupportTicketForm: HTMLFormElement) {
    this.isLoadingSubmit = true;
    this.alertService.clearAlerts();
  
    this.supportTicketService.supportTicketsSupportTicketIDPut(this.supportTicketID, this.model).subscribe(() => {
      this.isLoadingSubmit = false;
      editSupportTicketForm.reset();
      this.router.navigateByUrl("/support-tickets/" + this.supportTicketID).then(() => {
        this.alertService.pushAlert(new Alert(`Support ticket updated.`, AlertContext.Success));
      });
    }, error => {
        this.isLoadingSubmit = false;
        this.cdr.detectChanges();
      }
    );
  }
}
