import { ChangeDetectorRef, Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { UserDetailedDto } from 'src/app/shared/models';
import { Alert } from 'src/app/shared/models/alert';
import { AlertContext } from 'src/app/shared/models/enums/alert-context.enum';
import { ReportTemplateModelDto } from 'src/app/shared/models/generated/report-template-model-dto';
import { ReportTemplateNewDto } from 'src/app/shared/models/report-template-new-dto';
import { AlertService } from 'src/app/shared/services/alert.service';
import { ReportTemplateService } from 'src/app/shared/services/report-template-service';

@Component({
  selector: 'zybach-report-template-new',
  templateUrl: './report-template-new.component.html',
  styleUrls: ['./report-template-new.component.scss']
})
export class ReportTemplateNewComponent implements OnInit, OnDestroy {
  @ViewChild('fileUpload') fileUpload: any;


  private watchUserChangeSubscription: any;
  private currentUser: UserDetailedDto;

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

      this.reportTemplateService.getReportTemplateModels().subscribe(reportTemplateModels => {
        this.reportTemplateModels = reportTemplateModels.sort((a: ReportTemplateModelDto, b: ReportTemplateModelDto) => {
          if (a.ReportTemplateModelDisplayName > b.ReportTemplateModelDisplayName)
            return 1;
          if (a.ReportTemplateModelDisplayName < b.ReportTemplateModelDisplayName)
            return -1;
          return 0;
        });
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
    this.reportTemplateService.newReportTemplate(this.model)
      .subscribe(response => {
        this.isLoadingSubmit = false;
        newReportTemplateForm.reset();
        this.router.navigateByUrl("/reports").then(x => {
          this.alertService.pushAlert(new Alert("Report Template '" + response.DisplayName + "' successfully created.", AlertContext.Success));
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
