import { ChangeDetectorRef, Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { ReportTemplateDto } from 'src/app/shared/generated/model/report-template-dto';
import { UserDto } from 'src/app/shared/generated/model/user-dto';
import { ReportTemplateService } from 'src/app/shared/services/report-template.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'zybach-report-template-detail',
  templateUrl: './report-template-detail.component.html',
  styleUrls: ['./report-template-detail.component.scss']
})
export class ReportTemplateDetailComponent implements OnInit, OnDestroy {
  private watchUserChangeSubscription: any;
  private currentUser: UserDto;

  public reportTemplate: ReportTemplateDto;
  public reportTemplateFileLinkValue: string;

  constructor(
      private route: ActivatedRoute,
      private router: Router,
      private reportTemplateService: ReportTemplateService,
      private authenticationService: AuthenticationService,
      private cdr: ChangeDetectorRef
  ) {
      // force route reload whenever params change;
      this.router.routeReuseStrategy.shouldReuseRoute = () => false;
  }

  ngOnInit() {
      this.watchUserChangeSubscription = this.authenticationService.currentUserSetObservable.subscribe(currentUser => {
          this.currentUser = currentUser;
          const mainAppApiUrl = environment.mainAppApiUrl

          const id = parseInt(this.route.snapshot.paramMap.get("id"));
          if (id) {
              this.reportTemplateService.getReportTemplate(id).subscribe(reportTemplate => {
                  this.reportTemplate = reportTemplate as ReportTemplateDto;
                  this.reportTemplateFileLinkValue = `${mainAppApiUrl}/FileResource/${this.reportTemplate.FileResource.FileResourceGUID}`;
                  this.cdr.detectChanges();
              });
          }
      });
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
