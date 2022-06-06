import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { SupportTicketService } from 'src/app/shared/generated/api/support-ticket.service';
import { SupportTicketDetailDto } from 'src/app/shared/generated/model/support-ticket-detail-dto';
import { UserDto } from 'src/app/shared/generated/model/user-dto';
import { AlertService } from 'src/app/shared/services/alert.service';

@Component({
  selector: 'zybach-support-ticket-detail',
  templateUrl: './support-ticket-detail.component.html',
  styleUrls: ['./support-ticket-detail.component.scss']
})
export class SupportTicketDetailComponent implements OnInit {

  public currentUser: UserDto;
  public supportTicketID: number;
  public supportTicket: SupportTicketDetailDto;

  constructor(
    private authenticationService: AuthenticationService,
    private route: ActivatedRoute,
    private router: Router,
    private cdr: ChangeDetectorRef,
    private alertService: AlertService,
    private supportTicketService: SupportTicketService
  ) { }

  ngOnInit(): void {
    this.authenticationService.getCurrentUser().subscribe(currentUser => {
      this.currentUser = currentUser;
      this.supportTicketID = parseInt(this.route.snapshot.paramMap.get("id"));
      this.supportTicketService.supportTicketsSupportTicketIDGet(this.supportTicketID).subscribe(supportTicket => {
        this.supportTicket = supportTicket;
      });
    })
  }

}
