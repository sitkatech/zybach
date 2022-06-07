import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { SupportTicketCommentUpsertDto } from 'src/app/shared/generated/model/support-ticket-comment-upsert-dto';
import { SupportTicketService } from 'src/app/shared/generated/api/support-ticket.service';
import { UserDto } from 'src/app/shared/generated/model/user-dto';
import { AlertService } from 'src/app/shared/services/alert.service';
import { Alert } from 'src/app/shared/models/alert';
import { AlertContext } from 'src/app/shared/models/enums/alert-context.enum';

@Component({
  selector: 'zybach-support-ticket-comment-new',
  templateUrl: './support-ticket-comment-new.component.html',
  styleUrls: ['./support-ticket-comment-new.component.scss']
})
export class SupportTicketCommentNewComponent implements OnInit {
  
  private currentUser: UserDto;
  public model: SupportTicketCommentUpsertDto;
  public isLoadingSubmit: boolean = false;
  public supportTicketID: number;

  constructor(
    private cdr: ChangeDetectorRef,
    private route: ActivatedRoute,
    private router: Router,
    private supportTicketService: SupportTicketService,
    private authenticationService: AuthenticationService,
    private alertService: AlertService
  ) { }

  ngOnInit(): void {
    this.authenticationService.getCurrentUser().subscribe(currentUser => {
      this.currentUser = currentUser;
      this.supportTicketID = parseInt(this.route.snapshot.paramMap.get("id"));

      this.cdr.detectChanges();

      this.model = new SupportTicketCommentUpsertDto();
      this.model.CreatorUserID = this.currentUser.UserID;
      this.model.SupportTicketID = this.supportTicketID;
    });
  }
  
  public onSubmit(newSupportTicketCommentForm: HTMLFormElement): void {
    this.isLoadingSubmit = true;
    this.alertService.clearAlerts();

    this.supportTicketService.supportTicketCommentsPost(this.model)
      .subscribe(response => {
        this.isLoadingSubmit = false;
        newSupportTicketCommentForm.reset();
        this.router.navigateByUrl("/support-tickets/" + response.SupportTicketID).then(() => {
          this.alertService.pushAlert(new Alert("Support Ticket comment successfully created.", AlertContext.Success));
        });
      },
        error => {
          this.isLoadingSubmit = false;
          this.cdr.detectChanges();
        }
      );
  }

}
