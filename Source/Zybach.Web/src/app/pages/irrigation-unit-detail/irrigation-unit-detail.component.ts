import { ChangeDetectorRef, Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { IrrigationUnitService } from 'src/app/services/irrigation-unit.service';
import { AgHubIrrigationUnitDetailDto } from 'src/app/shared/generated/model/ag-hub-irrigation-unit-detail-dto';
import { UserDto } from 'src/app/shared/generated/model/user-dto';
import { IrrigationUnitMapComponent } from '../irrigation-unit-map/irrigation-unit-map.component';

@Component({
  selector: 'zybach-irrigation-unit-detail',
  templateUrl: './irrigation-unit-detail.component.html',
  styleUrls: ['./irrigation-unit-detail.component.scss']
})
export class IrrigationUnitDetailComponent implements OnInit {
  public watchUserChangeSubscription: any;
  @ViewChild("irrigationUnitMap") irrigationUnitMap: IrrigationUnitMapComponent;
 
  currentUser: UserDto;
  public irrigationUnit: AgHubIrrigationUnitDetailDto;
  public irrigationUnitID: number;

  constructor(
    private irrigationUnitService: IrrigationUnitService,
    private authenticationService: AuthenticationService,
    private route: ActivatedRoute,
    private router: Router,
    private cdr: ChangeDetectorRef
  ) { }

  ngOnInit(): void {
    this.irrigationUnitID = parseInt(this.route.snapshot.paramMap.get("id"));
    this.authenticationService.getCurrentUser().subscribe(currentUser => {
      this.currentUser = currentUser;
      this.getIrrigationUnitDetails();
    })
  }

  getIrrigationUnitDetails(){
    this.irrigationUnitService.getIrrigationUnitDetailsByID(this.irrigationUnitID).subscribe((irrigationUnit: AgHubIrrigationUnitDetailDto) => {
      this.irrigationUnit = irrigationUnit;
      this.irrigationUnitID = irrigationUnit.AgHubIrrigationUnitID;
      this.cdr.detectChanges();
    })
  }

}
