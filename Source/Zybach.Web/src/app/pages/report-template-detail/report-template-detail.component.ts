import { ChangeDetectorRef, Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { forkJoin } from 'rxjs';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { UserService } from 'src/app/services/user/user.service';
import { LinkRendererComponent } from 'src/app/shared/components/ag-grid/link-renderer/link-renderer.component';
import { UserDetailedDto } from 'src/app/shared/models';
import { ReportTemplateDto } from 'src/app/shared/models/generated/report-template-dto';
import { ReportTemplateService } from 'src/app/shared/services/report-template-service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'zybach-report-template-detail',
  templateUrl: './report-template-detail.component.html',
  styleUrls: ['./report-template-detail.component.scss']
})
export class ReportTemplateDetailComponent implements OnInit, OnDestroy {
  private watchUserChangeSubscription: any;
  private currentUser: UserDetailedDto;

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
          const apiHostName = environment.apiHostName

          const id = parseInt(this.route.snapshot.paramMap.get("id"));
          if (id) {
              this.reportTemplateService.getReportTemplate(id).subscribe(reportTemplate => {
                  this.reportTemplate = reportTemplate as ReportTemplateDto;
                  this.reportTemplateFileLinkValue = `https://${apiHostName}/FileResource/${this.reportTemplate.FileResource.FileResourceGUID}`;
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
