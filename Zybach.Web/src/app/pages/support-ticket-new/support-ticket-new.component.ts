import { ChangeDetectorRef, Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, ActivationEnd, Router } from '@angular/router';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { SupportTicketUpsertComponent } from 'src/app/shared/components/support-ticket-upsert/support-ticket-upsert.component';
import { SensorService } from 'src/app/shared/generated/api/sensor.service';
import { SupportTicketService } from 'src/app/shared/generated/api/support-ticket.service';
import { WellService } from 'src/app/shared/generated/api/well.service';
import { SupportTicketPriorityEnum } from 'src/app/shared/generated/enum/support-ticket-priority-enum';
import { SupportTicketStatusEnum } from 'src/app/shared/generated/enum/support-ticket-status-enum';
import { SupportTicketUpsertDto } from 'src/app/shared/generated/model/support-ticket-upsert-dto';
import { UserDto } from 'src/app/shared/generated/model/user-dto';
import { WellSimpleDto } from 'src/app/shared/generated/model/well-simple-dto';
import { Alert } from 'src/app/shared/models/alert';
import { AlertContext } from 'src/app/shared/models/enums/alert-context.enum';
import { AlertService } from 'src/app/shared/services/alert.service';

@Component({
  selector: 'zybach-support-ticket-new',
  templateUrl: './support-ticket-new.component.html',
  styleUrls: ['./support-ticket-new.component.scss']
})
export class SupportTicketNewComponent implements OnInit, OnDestroy {
  @ViewChild('supportTicketForm') private supportTicketUpsertComponent: SupportTicketUpsertComponent;
  
  private currentUser: UserDto;
  public model: SupportTicketUpsertDto;
  public isLoadingSubmit: boolean = false;

  constructor(
    private cdr: ChangeDetectorRef,
    private router: Router,
    private route: ActivatedRoute,
    private supportTicketService: SupportTicketService,
    private wellService: WellService,
    private sensorService: SensorService,
    private authenticationService: AuthenticationService,
    private alertService: AlertService
  ) { }

  ngOnInit(): void {   
    this.authenticationService.getCurrentUser().subscribe(currentUser => {
      this.currentUser = currentUser;

      this.model = new SupportTicketUpsertDto();
      this.model.CreatorUserID = this.currentUser.UserID;
      this.model.SupportTicketPriorityID = SupportTicketPriorityEnum.High;
      this.model.SupportTicketStatusID = SupportTicketStatusEnum.Open;

      const wellID = parseInt(this.route.snapshot.paramMap.get("well-id"));
      const sensorID = parseInt(this.route.snapshot.paramMap.get("sensor-id"));

      if (wellID) {
        this.wellService.wellsWellIDGet(wellID).subscribe(well => {
          this.model.WellRegistrationID = well.WellRegistrationID;
        });
      } else if (sensorID) {
        this.sensorService.sensorsSensorIDGet(sensorID).subscribe(sensor => {
          this.model.SensorID = sensor.SensorID;
          this.model.SensorName = sensor.SensorName;
          this.model.WellRegistrationID = sensor.WellRegistrationID;
        });
      }

      this.cdr.detectChanges();
    });
  }

  ngOnDestroy(): void {
    this.cdr.detach();
  }

  public onSubmit(newSupportTicketForm: HTMLFormElement): void {
    this.isLoadingSubmit = true;
    this.alertService.clearAlerts();

    this.supportTicketService.supportTicketsPost(this.model)
      .subscribe(response => {
        this.isLoadingSubmit = false;
        newSupportTicketForm.reset();
        this.router.navigateByUrl("/support-tickets/" + response.SupportTicketID).then(() => {
          this.alertService.pushAlert(new Alert("Support Ticket '" + response.SupportTicketTitle + "' successfully created.", AlertContext.Success));
        });
      },
        error => {
          this.isLoadingSubmit = false;
          this.cdr.detectChanges();
        }
      );
  }

}
