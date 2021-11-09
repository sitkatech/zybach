import { ChangeDetectorRef, Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { ChemigationPermitService } from 'src/app/services/chemigation-permit.service';
import { UserDetailedDto } from 'src/app/shared/models';
import { ChemigationPermitDto } from 'src/app/shared/models/generated/chemigation-permit-dto';
import { AlertService } from 'src/app/shared/services/alert.service';

@Component({
  selector: 'zybach-chemigation-permit-detail',
  templateUrl: './chemigation-permit-detail.component.html',
  styleUrls: ['./chemigation-permit-detail.component.scss']
})
export class ChemigationPermitDetailComponent implements OnInit, OnDestroy {

  public watchUserChangeSubscription: any;
  public currentUser: UserDetailedDto;
  public chemigationPermitNumber: number;
  public chemigationPermit: ChemigationPermitDto;

  constructor(
    private chemigationPermitService: ChemigationPermitService,
    private authenticationService: AuthenticationService,
    private route: ActivatedRoute,
    private cdr: ChangeDetectorRef,
    private alertService: AlertService
  ) { }

  ngOnInit(): void {
    this.watchUserChangeSubscription = this.authenticationService.currentUserSetObservable.subscribe(currentUser => {
      this.currentUser = currentUser;
      this.chemigationPermitNumber = parseInt(this.route.snapshot.paramMap.get("permit-number"));
      this.getPermitDetails();
    })
  }

  ngOnDestroy() {
    this.watchUserChangeSubscription.unsubscribe();
  }

  public currentUserIsAdmin(): boolean {
    return this.authenticationService.isUserAnAdministrator(this.currentUser);
}

  private getPermitDetails(): void {
    this.chemigationPermitService.getChemigationPermitByPermitNumber(this.chemigationPermitNumber).subscribe((permit: ChemigationPermitDto) => {
      this.chemigationPermit = permit;
      
      this.cdr.detectChanges();
    });
  }
}