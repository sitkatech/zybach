import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { forkJoin } from 'rxjs';
import { CountyService } from 'src/app/shared/generated/api/county.service';
import { WellService } from 'src/app/shared/generated/api/well.service';
import { CountyDto } from 'src/app/shared/generated/model/county-dto';
import { WellContactInfoDto } from 'src/app/shared/generated/model/well-contact-info-dto';
import { Alert } from 'src/app/shared/models/alert';
import { AlertContext } from 'src/app/shared/models/enums/alert-context.enum';
import { States } from 'src/app/shared/models/enums/states.enum';
import { AlertService } from 'src/app/shared/services/alert.service';

@Component({
  selector: 'zybach-well-contact-edit',
  templateUrl: './well-contact-edit.component.html',
  styleUrls: ['./well-contact-edit.component.scss']
})
export class WellContactEditComponent implements OnInit {

  public wellID: number;
  public wellRegistrationID: string;
  public wellContactInfo: WellContactInfoDto;

  public counties: Array<CountyDto>;
  public states: Object;
  public defaultState: string = 'NE';
  
  public isLoadingSubmit: boolean;

  constructor(
    private wellService: WellService,
    private countyService: CountyService,
    private route: ActivatedRoute,
    private router: Router,
    private cdr: ChangeDetectorRef,
    private alertService: AlertService
  ) { }

  ngOnInit(): void {
    this.wellID = parseInt(this.route.snapshot.paramMap.get("id"));
    this.states = States.statesList;

    forkJoin({
      wellContactInfo: this.wellService.wellsWellIDContactInfoGet(this.wellID),
      counties: this.countyService.countiesGet()
    }).subscribe(({ wellContactInfo, counties }) => {
      this.wellContactInfo = wellContactInfo;
      this.wellRegistrationID = wellContactInfo.WellRegistrationID;
      if (!this.wellContactInfo.OwnerState) {
        this.wellContactInfo.OwnerState = this.defaultState;
      }

      this.counties = counties;
      this.cdr.detectChanges();
    });

  }

  public onSubmit(editWellContactForm: HTMLFormElement): void {
    this.isLoadingSubmit = true;

    this.wellService.wellsWellIDContactInfoPut(this.wellID, this.wellContactInfo)
      .subscribe(response => {
        this.isLoadingSubmit = false;
        editWellContactForm.reset();
        this.router.navigateByUrl("/wells/" + this.wellID).then(() => {
          this.alertService.pushAlert(new Alert(`Well contact details updated.`, AlertContext.Success));
        });
      }
        ,
        error => {
          this.isLoadingSubmit = false;
          this.cdr.detectChanges();
        }
      );
}
}