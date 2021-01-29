import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { WellService } from 'src/app/services/well.service';
import { UserDetailedDto } from 'src/app/shared/models';
import { WellWithSensorSummaryDto } from 'src/app/shared/models/well-with-sensor-summary-dto';

@Component({
  selector: 'zybach-well-detail',
  templateUrl: './well-detail.component.html',
  styleUrls: ['./well-detail.component.scss']
})
export class WellDetailComponent implements OnInit, OnDestroy {
  public watchUserChangeSubscription: any;
  currentUser: UserDetailedDto;
  wellSubscription: any;
  well: WellWithSensorSummaryDto;
  rawResults: string;
  constructor(
    private wellService: WellService,
    private authenticationService: AuthenticationService,
    private route: ActivatedRoute,
  ) { }

  ngOnInit(): void {
    this.watchUserChangeSubscription = this.authenticationService.currentUserSetObservable.subscribe(currentUser => {
      this.currentUser = currentUser;
      const id = this.route.snapshot.paramMap.get("wellRegistrationID");
      this.wellSubscription = this.wellService.getWell(id).subscribe(response=>{
        this.well = response.result;
        this.rawResults = JSON.stringify(response);
      })
    })
  }

  ngOnDestroy(){
    this.watchUserChangeSubscription.unsubscribe();
    this.wellSubscription.unsubscribe();
  }

}
