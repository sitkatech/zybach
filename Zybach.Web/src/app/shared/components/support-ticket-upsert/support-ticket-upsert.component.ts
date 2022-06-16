import { ChangeDetectorRef, Component, Input, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { forkJoin, Observable, of } from 'rxjs';
import { catchError, debounceTime, distinctUntilChanged, switchMap, tap } from 'rxjs/operators';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { SensorService } from '../../generated/api/sensor.service';
import { SupportTicketService } from '../../generated/api/support-ticket.service';
import { UserService } from '../../generated/api/user.service';
import { WellService } from '../../generated/api/well.service';
import { SupportTicketPriorityDto } from '../../generated/model/support-ticket-priority-dto';
import { SupportTicketStatusDto } from '../../generated/model/support-ticket-status-dto';
import { SupportTicketUpsertDto } from '../../generated/model/support-ticket-upsert-dto';
import { UserDto } from '../../generated/model/user-dto';
import { RoleEnum } from '../../generated/enum/role-enum';
import { UserSimpleDto } from '../../generated/model/user-simple-dto';

@Component({
  selector: 'zybach-support-ticket-upsert',
  templateUrl: './support-ticket-upsert.component.html',
  styleUrls: ['./support-ticket-upsert.component.scss']
})
export class SupportTicketUpsertComponent implements OnInit {
  @Input() model: SupportTicketUpsertDto;
  @Input() creatorUser?: UserSimpleDto
  @ViewChild('supportTicketForm',  {static: true}) public supportTicketForm: NgForm;

  public supportTicketStatuses: Array<SupportTicketStatusDto>;
  public supportTicketPriorities: Array<SupportTicketPriorityDto>;
  public users: UserDto[];
  public currentUser: UserDto;

  public sensorSearchFailed : boolean = false;
  public wellSearchFailed : boolean = false;

  constructor(
    private authenticationService: AuthenticationService,
    private supportTicketService: SupportTicketService,
    private userService: UserService,
    private wellService: WellService,
    private sensorService: SensorService,
    private cdr: ChangeDetectorRef
  ) { }

  ngOnInit(): void {
    forkJoin({
      supportTicketStatuses: this.supportTicketService.supportTicketsStatusesGet(),
      supportTicketPriorities: this.supportTicketService.supportTicketsPrioritiesGet(),
      users: this.userService.usersNotUnassignedOrDisabledGet(),
      currentUser: this.authenticationService.getCurrentUser(),
    }).subscribe(({ supportTicketStatuses, supportTicketPriorities, users, currentUser }) => {
      this.supportTicketStatuses = supportTicketStatuses;
      this.supportTicketPriorities = supportTicketPriorities;
      this.users = users;
      this.currentUser = currentUser;
      this.cdr.detectChanges();
    });
  }

  wellSearchApi = (text$: Observable<string>) => {
    return text$.pipe(      
        debounceTime(200), 
        distinctUntilChanged(),
        tap(() => this.wellSearchFailed = false),
        switchMap(searchText => searchText.length > 2 ? this.wellService.wellsSearchWellRegistrationIDGet(searchText) : ([])), 
        catchError(() => {
          this.wellSearchFailed = true;
          return of([]);
        })     
      );                 
  }

  sensorSearchApi = (text$: Observable<string>) => {
    return text$.pipe(      
        debounceTime(200), 
        distinctUntilChanged(),
        tap(() => this.sensorSearchFailed = false),
        switchMap(searchText => searchText.length > 2 ? this.sensorService.sensorsSearchSensorNameGet(searchText) : ([])), 
        catchError(() => {
          this.sensorSearchFailed = true;
          return of([]);
        })     
      );   
  }

}
