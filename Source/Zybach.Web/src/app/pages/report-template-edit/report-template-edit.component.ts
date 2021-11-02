import { ChangeDetectorRef, Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { forkJoin } from 'rxjs';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { UserDetailedDto } from 'src/app/shared/models';
import { Alert } from 'src/app/shared/models/alert';
import { AlertContext } from 'src/app/shared/models/enums/alert-context.enum';
import { ReportTemplateDto } from 'src/app/shared/models/generated/report-template-dto';
import { ReportTemplateModelDto } from 'src/app/shared/models/generated/report-template-model-dto';
import { ReportTemplateNewDto } from 'src/app/shared/models/report-template-new-dto';
import { AlertService } from 'src/app/shared/services/alert.service';
import { ReportTemplateService } from 'src/app/shared/services/report-template.service';

@Component({
  selector: 'zybach-report-template-edit',
  templateUrl: './report-template-edit.component.html',
  styleUrls: ['./report-template-edit.component.scss']
})
export class ReportTemplateEditComponent implements OnInit, OnDestroy {
  @ViewChild('fileUpload') fileUpload: any;


  private watchUserChangeSubscription: any;
  private currentUser: UserDetailedDto;

  public reportTemplateID: number;
  public reportTemplate: ReportTemplateDto;
  public model: ReportTemplateNewDto;
  public reportTemplateModels: Array<ReportTemplateModelDto>;
  public isLoadingSubmit: boolean = false;

  public displayErrors: any = {};
  public displayFileErrors: any = {};

  public fileName: string;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private authenticationService: AuthenticationService,
    private reportTemplateService: ReportTemplateService,
    private cdr: ChangeDetectorRef,
    private alertService: AlertService
  ) {
  }

  ngOnInit() {
    this.model = new ReportTemplateNewDto();

    this.watchUserChangeSubscription = this.authenticationService.currentUserSetObservable.subscribe(currentUser => {
      this.currentUser = currentUser;
      if (!this.authenticationService.isUserAnAdministrator(this.currentUser)) {
        this.router.navigateByUrl("/not-found")
          .then();
        return;
      }

      this.reportTemplateID = parseInt(this.route.snapshot.paramMap.get("id"));

      forkJoin(
        this.reportTemplateService.getReportTemplate(this.reportTemplateID),
        this.reportTemplateService.getReportTemplateModels()
      ).subscribe(([reportTemplate, reportTemplateModels]) => {
        this.reportTemplate = reportTemplate as ReportTemplateDto;
        this.reportTemplateModels = reportTemplateModels.sort((a: ReportTemplateModelDto, b: ReportTemplateModelDto) => {
          if (a.ReportTemplateModelDisplayName > b.ReportTemplateModelDisplayName)
            return 1;
          if (a.ReportTemplateModelDisplayName < b.ReportTemplateModelDisplayName)
            return -1;
          return 0;
        });

        this.model = new ReportTemplateNewDto();
        this.model.ReportTemplateID = reportTemplate.ReportTemplateID;
        this.model.DisplayName = reportTemplate.DisplayName;
        this.model.Description = reportTemplate.Description;
        this.model.ReportTemplateModelID = reportTemplate.ReportTemplateModel.ReportTemplateModelID;
        this.cdr.detectChanges();
      });
    });
  }

  ngOnDestroy() {
    this.watchUserChangeSubscription.unsubscribe();
    this.authenticationService.dispose();
    this.cdr.detach();
  }

  fileEvent() {
    let file = this.getFile();
    this.model.FileResource = file;
    this.displayErrors = false;

    // if (file && file.name.split(".").pop().toUpperCase() != "DOCX") {
    //   this.displayFileErrors = true;
    // } else {
    //   this.displayFileErrors = false;
    // }

    this.cdr.detectChanges();
  }

  public getFile(): File {
    if (!this.fileUpload) {
      return null;
    }
    console.log(this.fileUpload);
    return this.fileUpload.nativeElement.files[0];
  }

  public getFileName(): string {
    let file = this.getFile();
    if (!file) {
      return ""
    }

    return file.name;
  }

  public openFileUpload() {
    this.fileUpload.nativeElement.click();
  }

  public onSubmit(newReportTemplateForm: HTMLFormElement): void {
    this.isLoadingSubmit = true;
    this.reportTemplateService.updateReportTemplate(this.reportTemplateID, this.model)
      .subscribe(response => {
        this.isLoadingSubmit = false;
        this.router.navigateByUrl("/reports/" + this.reportTemplateID).then(x => {
          this.alertService.pushAlert(new Alert("Report Template '" + response.DisplayName + "' successfully updated.", AlertContext.Success));
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
