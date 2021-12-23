import { ChangeDetectorRef, Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { forkJoin } from 'rxjs';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { WaterQualityInspectionService } from 'src/app/services/water-quality-inspection.service';
import { UserDto } from 'src/app/shared/generated/model/user-dto';
import { WaterQualityInspectionSimpleDto } from 'src/app/shared/generated/model/water-quality-inspection-simple-dto';
import { AlertService } from 'src/app/shared/services/alert.service';

@Component({
  selector: 'zybach-water-quality-inspection-detail',
  templateUrl: './water-quality-inspection-detail.component.html',
  styleUrls: ['./water-quality-inspection-detail.component.scss']
})
export class WaterQualityInspectionDetailComponent implements OnInit, OnDestroy {

  public watchUserChangeSubscription: any;
  public currentUser: UserDto;
  public waterQualityInspectionID: number;
  public waterQualityInspection: WaterQualityInspectionSimpleDto;


  constructor(
    private waterQualityInspectionService: WaterQualityInspectionService,
    private authenticationService: AuthenticationService,
    private route: ActivatedRoute,
    private cdr: ChangeDetectorRef,
    private alertService: AlertService
  ) { }

  ngOnInit(): void {
    this.watchUserChangeSubscription = this.authenticationService.currentUserSetObservable.subscribe(currentUser => {
      this.currentUser = currentUser;
      this.waterQualityInspectionID = parseInt(this.route.snapshot.paramMap.get("id"));
      forkJoin({
        waterQualityInspection: this.waterQualityInspectionService.getByID(this.waterQualityInspectionID)
      }).subscribe(({ waterQualityInspection}) => {
        this.waterQualityInspection = waterQualityInspection;
        this.cdr.detectChanges();
      });
    })
  }

  ngOnDestroy() {
    this.watchUserChangeSubscription.unsubscribe();
    this.authenticationService.dispose();
    this.cdr.detach();
  }

  public currentUserIsAdmin(): boolean {
    return this.authenticationService.isUserAnAdministrator(this.currentUser);
  }
}