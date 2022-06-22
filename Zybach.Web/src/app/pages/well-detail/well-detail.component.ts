import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { WellService } from 'src/app/shared/generated/api/well.service';
import { UserDto } from 'src/app/shared/generated/model/user-dto';
import { WellDetailDto } from 'src/app/shared/generated/model/well-detail-dto';

@Component({
  selector: 'zybach-well-detail',
  templateUrl: './well-detail.component.html',
  styleUrls: ['./well-detail.component.scss']
})

export class WellDetailComponent implements OnInit {
  public watchUserChangeSubscription: any;

  currentUser: UserDto;
  well: WellDetailDto;
  wellID: number;
  wellRegistrationID: string;
  
  public isLoadingSubmit: boolean = false;

  constructor(
    private wellService: WellService,
    private authenticationService: AuthenticationService,
    private route: ActivatedRoute,
    private router: Router,
    private cdr: ChangeDetectorRef
  ) {
    // force route reload whenever params change;
    this.router.routeReuseStrategy.shouldReuseRoute = () => false;
  }

  ngOnInit(): void {
    this.wellID = parseInt(this.route.snapshot.paramMap.get("id"));
    this.authenticationService.getCurrentUser().subscribe(currentUser => {
      this.currentUser = currentUser;
      this.getWellDetails();
    })
  }
  
  public isUserReadOnly(): boolean {
    return this.authenticationService.isUserReadOnly(this.currentUser);
  }

  getWellDetails(){
    this.wellService.wellsWellIDGet(this.wellID).subscribe((well: WellDetailDto)=>{
      this.well = well;
      this.wellRegistrationID = well.WellRegistrationID;
      this.cdr.detectChanges();
    })
  }

  resizeWindow(){
    window.dispatchEvent(new Event('resize'));
  }

}
