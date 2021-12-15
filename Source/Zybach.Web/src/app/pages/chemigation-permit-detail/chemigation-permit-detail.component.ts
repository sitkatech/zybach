import { ChangeDetectorRef, Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { forkJoin } from 'rxjs';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { ChemigationPermitService } from 'src/app/services/chemigation-permit.service';
import { ChemigationPermitAnnualRecordDto } from 'src/app/shared/generated/model/chemigation-permit-annual-record-dto';
import { ChemigationPermitDto } from 'src/app/shared/generated/model/chemigation-permit-dto';
import { UserDto } from 'src/app/shared/generated/model/user-dto';
import { AlertService } from 'src/app/shared/services/alert.service';

@Component({
  selector: 'zybach-chemigation-permit-detail',
  templateUrl: './chemigation-permit-detail.component.html',
  styleUrls: ['./chemigation-permit-detail.component.scss']
})
export class ChemigationPermitDetailComponent implements OnInit, OnDestroy {

  public watchUserChangeSubscription: any;
  public currentUser: UserDto;
  public chemigationPermitNumber: number;
  public chemigationPermit: ChemigationPermitDto;
  public annualRecords: Array<ChemigationPermitAnnualRecordDto>;

  public allYearsSelected: boolean = false;
  public yearToDisplay: number;
  public currentYear: number;
  public currentYearAnnualRecord: ChemigationPermitAnnualRecordDto;

  constructor(
    private chemigationPermitService: ChemigationPermitService,
    private authenticationService: AuthenticationService,
    private route: ActivatedRoute,
    private cdr: ChangeDetectorRef,
    private alertService: AlertService
  ) { }

  ngOnInit(): void {
    this.currentYear = new Date().getFullYear();
    this.yearToDisplay = new Date().getFullYear();    

    this.watchUserChangeSubscription = this.authenticationService.currentUserSetObservable.subscribe(currentUser => {
      this.currentUser = currentUser;
      this.chemigationPermitNumber = parseInt(this.route.snapshot.paramMap.get("permit-number"));
      forkJoin({
        chemigationPermit: this.chemigationPermitService.getChemigationPermitByPermitNumber(this.chemigationPermitNumber),
        annualRecords: this.chemigationPermitService.getChemigationPermitAnnualRecordsByPermitNumber(this.chemigationPermitNumber)
      }).subscribe(({ chemigationPermit, annualRecords}) => {
        this.chemigationPermit = chemigationPermit;
        this.annualRecords = annualRecords;
        this.updateAnnualData();
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

  public permitHasCurrentRecord(): boolean {
    return this.annualRecords?.map(x => x.RecordYear).includes(this.currentYear);
  }
  
  public updateAnnualData(): void {
    this.currentYearAnnualRecord = this.annualRecords?.find(x => x.RecordYear == this.yearToDisplay);
  }
}