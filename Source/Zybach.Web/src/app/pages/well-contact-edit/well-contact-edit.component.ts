import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { forkJoin } from 'rxjs';
import { WellService } from 'src/app/services/well.service';
import { CountyDto } from 'src/app/shared/generated/model/county-dto';
import { WellContactInfoDto } from 'src/app/shared/generated/model/well-contact-info-dto';
import { AlertService } from 'src/app/shared/services/alert.service';

@Component({
  selector: 'zybach-well-contact-edit',
  templateUrl: './well-contact-edit.component.html',
  styleUrls: ['./well-contact-edit.component.scss']
})
export class WellContactEditComponent implements OnInit {

  public wellContactInfo: WellContactInfoDto;
  public counties: Array<CountyDto>;
  public wellRegistrationID: string;

  constructor(
    private wellService: WellService,
    private route: ActivatedRoute,
    private router: Router,
    private cdr: ChangeDetectorRef,
    private alertService: AlertService
  ) { }

  ngOnInit(): void {
    this.wellRegistrationID = this.route.snapshot.paramMap.get("wellRegistrationID");

    forkJoin({
      wellContactInfo: this.wellService.getWellContactDetails(this.wellRegistrationID),
      counties: this.wellService.getCounties()
    }).subscribe(({ wellContactInfo, counties }) => {
      this.wellContactInfo = wellContactInfo;
      this.counties = counties;
      this.cdr.detectChanges();
    });

  }
}