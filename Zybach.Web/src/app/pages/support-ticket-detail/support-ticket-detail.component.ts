import { ChangeDetectorRef, Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { SupportTicketService } from 'src/app/shared/generated/api/support-ticket.service';
import { SupportTicketDetailDto } from 'src/app/shared/generated/model/support-ticket-detail-dto';
import { SupportTicketNotificationSimpleDto } from 'src/app/shared/generated/model/support-ticket-notification-simple-dto';
import { UserDto } from 'src/app/shared/generated/model/user-dto';
import { Alert } from 'src/app/shared/models/alert';
import { AlertContext } from 'src/app/shared/models/enums/alert-context.enum';
import { AlertService } from 'src/app/shared/services/alert.service';
import { forkJoin } from 'rxjs';

@Component({
  selector: 'zybach-support-ticket-detail',
  templateUrl: './support-ticket-detail.component.html',
  styleUrls: ['./support-ticket-detail.component.scss']
})
export class SupportTicketDetailComponent implements OnInit {
  @ViewChild('deleteCommentModal') deleteCommentModal;
  
  public currentUser: UserDto;
  public supportTicketID: number;
  public supportTicket: SupportTicketDetailDto;
  public notifications: SupportTicketNotificationSimpleDto[];

  public isLoadingDelete: boolean = false;
  private modalReference: NgbModalRef;
  public commentIDToRemove: number;

  constructor(
    private authenticationService: AuthenticationService,
    private route: ActivatedRoute,
    private router: Router,
    private cdr: ChangeDetectorRef,
    private alertService: AlertService,
    private modalService: NgbModal,
    private supportTicketService: SupportTicketService
  ) { }

  ngOnInit(): void {
    this.authenticationService.getCurrentUser().subscribe(currentUser => {
      this.currentUser = currentUser;
      this.supportTicketID = parseInt(this.route.snapshot.paramMap.get("id"));
      forkJoin({
        supportTicket: this.supportTicketService.supportTicketsSupportTicketIDGet(this.supportTicketID),
        notifications: this.supportTicketService.supportTicketsSupportTicketIDNotificationsGet(this.supportTicketID)
      }).subscribe(({supportTicket, notifications}) => {
        this.supportTicket = supportTicket;
        this.notifications = notifications;
      });
    })
  }

  public currentUserIsTicketOwner(): boolean {
    return this.currentUser.UserID == this.supportTicket.CreatorUser.UserID || this.currentUser.UserID == this.supportTicket.AssigneeUser?.UserID;
  }

  private checkIfDeleting(): boolean {
    return this.isLoadingDelete;
  }
  
  public launchDeleteModal(modalContent: any, commentIDToRemove: number): void {
    this.commentIDToRemove = commentIDToRemove;
    this.modalReference = this.modalService.open(
      modalContent, 
      { 
        ariaLabelledBy: 'deleteCommentModal', beforeDismiss: () => this.checkIfDeleting(), backdrop: 'static', keyboard: false
      });
  }

  private refreshTicket() {
    this.supportTicketService.supportTicketsSupportTicketIDGet(this.supportTicketID).subscribe(supportTicket => {
      this.supportTicket = supportTicket;
    });
  }
  
  public deleteComment() {
    this.isLoadingDelete = true;

    this.supportTicketService.supportTicketCommentsSupportTicketCommentIDDelete(this.commentIDToRemove).subscribe(() => {
      this.isLoadingDelete = false;
      this.modalReference.close();
      this.alertService.pushAlert(new Alert('Commment was successfully deleted.', AlertContext.Success, true));
      this.refreshTicket();
      window.scroll(0,0);
      this.cdr.detectChanges();
    }, error => {
      this.isLoadingDelete = false;
      this.modalReference.close();
      window.scroll(0,0);
      this.cdr.detectChanges();
    })
  }

}
