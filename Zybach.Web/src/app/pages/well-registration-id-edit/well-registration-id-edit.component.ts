import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { forkJoin } from 'rxjs';
import { WellService } from 'src/app/shared/generated/api/well.service';
import { WellRegistrationIDDto } from 'src/app/shared/generated/model/well-registration-id-dto';
import { Alert } from 'src/app/shared/models/alert';
import { AlertContext } from 'src/app/shared/models/enums/alert-context.enum';
import { CustomRichTextTypeEnum } from 'src/app/shared/generated/enum/custom-rich-text-type-enum';
import { AlertService } from 'src/app/shared/services/alert.service';

@Component({
  selector: 'zybach-well-registration-id-edit',
  templateUrl: './well-registration-id-edit.component.html',
  styleUrls: ['./well-registration-id-edit.component.scss']
})
export class WellRegistrationIdEditComponent implements OnInit {
  public wellID: number;
  public wellRegistrationID: string;
  public newWellRegistrationIDDto: WellRegistrationIDDto;
  
  public richTextTypeID : number = CustomRichTextTypeEnum.WellRegistrationIDChangeHelpText;
  public isLoadingSubmit: boolean;

  constructor(
    private wellService: WellService,
    private route: ActivatedRoute,
    private router: Router,
    private cdr: ChangeDetectorRef,
    private alertService: AlertService
  ) { }

  ngOnInit(): void {
    
    this.wellID = parseInt(this.route.snapshot.paramMap.get("id"));
    forkJoin({
      well: this.wellService.wellsWellIDGet(this.wellID),
    }).subscribe(({ well }) => {
      this.wellRegistrationID = well.WellRegistrationID;
      this.newWellRegistrationIDDto = new WellRegistrationIDDto()
      this.newWellRegistrationIDDto.WellRegistrationID = this.wellRegistrationID;
      this.cdr.detectChanges();
    });

  }

  public onSubmit(editWellRegistrationIDForm: HTMLFormElement): void {
    this.isLoadingSubmit = true;
  
    this.wellService.wellsWellIDEditRegistrationIDPut(this.wellID, this.newWellRegistrationIDDto)
      .subscribe(response => {
        this.isLoadingSubmit = false;
        editWellRegistrationIDForm.reset();
        this.router.navigateByUrl("/wells/" + this.wellID).then(() => {
          this.alertService.pushAlert(new Alert(`Well Registration ID updated.`, AlertContext.Success));
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
