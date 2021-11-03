import { ChangeDetectorRef, Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { AgGridAngular } from 'ag-grid-angular';
import { ColDef } from 'ag-grid-community';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { LinkRendererComponent } from 'src/app/shared/components/ag-grid/link-renderer/link-renderer.component';
import { UserDetailedDto } from 'src/app/shared/models';
import { Alert } from 'src/app/shared/models/alert';
import { AlertContext } from 'src/app/shared/models/enums/alert-context.enum';
import { CustomRichTextType } from 'src/app/shared/models/enums/custom-rich-text-type.enum';
import { GenerateReportsDto } from 'src/app/shared/models/generate-reports-dto';
import { ReportTemplateDto } from 'src/app/shared/models/generated/report-template-dto';
import { AlertService } from 'src/app/shared/services/alert.service';
import { ReportTemplateService } from 'src/app/shared/services/report-template.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'zybach-reports-list',
  templateUrl: './reports-list.component.html',
  styleUrls: ['./reports-list.component.scss']
})
export class ReportsListComponent implements OnInit, OnDestroy {

  @ViewChild("reportTemplatesGrid") reportTemplatesGrid: AgGridAngular;
  private watchUserChangeSubscription: any;
  private currentUser: UserDetailedDto;

  public reportTemplates: Array<ReportTemplateDto>
  public richTextTypeID : number = CustomRichTextType.ReportsList;

  public rowData = [];
  public columnDefs: ColDef[];

  public selectedReportTemplateID: string;
  public isLoadingSubmit: boolean = false;

  constructor(
    private reportTemplateService: ReportTemplateService,
    private authenticationService: AuthenticationService,
    private router: Router,
    private alertService: AlertService,
    private cdr: ChangeDetectorRef) { }

  ngOnInit() {
    this.watchUserChangeSubscription = this.authenticationService.currentUserSetObservable.subscribe(currentUser => {
      this.currentUser = currentUser;
      this.reportTemplatesGrid.api.showLoadingOverlay();
      this.reportTemplateService.listAllReportTemplates().subscribe(reportTemplates => {
        this.reportTemplates = reportTemplates;
        this.rowData = reportTemplates;
        this.reportTemplatesGrid.api.hideOverlay();
        this.cdr.detectChanges();
      });
      const apiHostName = environment.apiHostName
      this.columnDefs = [
        {
          headerName: 'Name', valueGetter: function (params: any) {
            return { LinkValue: params.data.ReportTemplateID, LinkDisplay: params.data.DisplayName };
          }, cellRendererFramework: LinkRendererComponent,
          cellRendererParams: { inRouterLink: "/reports/" },
          filterValueGetter: function (params: any) {
            return params.data.DisplayName;
          },
          comparator: function (id1: any, id2: any) {
            let link1 = id1.LinkDisplay;
            let link2 = id2.LinkDisplay;
            if (link1 < link2) {
              return -1;
            }
            if (link1 > link2) {
              return 1;
            }
            return 0;
          },
          sortable: true, filter: true, width: 170
        },
        { headerName: 'Description', field: 'Description', sortable: true, filter: true },
        { headerName: 'Model', field: 'ReportTemplateModel.ReportTemplateModelDisplayName', sortable: true, filter: true },
        {
          headerName: 'Template File', valueGetter: function (params: any) {
            return { LinkValue: `https://${apiHostName}/FileResource/${params.data.FileResource.FileResourceGUID}` , LinkDisplay: params.data.FileResource.OriginalBaseFilename };
          }, cellRendererFramework: LinkRendererComponent,
          cellRendererParams: { isExternalUrl: true},
          filterValueGetter: function (params: any) {
            return params.data.FileResource.OriginalBaseFilename;
          },
          comparator: function (id1: any, id2: any) {
            let link1 = id1.LinkDisplay;
            let link2 = id2.LinkDisplay;
            if (link1 < link2) {
              return -1;
            }
            if (link1 > link2) {
              return 1;
            }
            return 0;
          },
          sortable: true, filter: true, width: 170
        },
      ];

      this.columnDefs.forEach(x => {
        x.resizable = true;
      });
    });
  }

  ngOnDestroy(): void {
    this.watchUserChangeSubscription.unsubscribe();
    this.authenticationService.dispose();
    this.cdr.detach();
  }

  public generateReport(): void {
    if(this.selectedReportTemplateID === undefined){
      this.alertService.pushAlert(new Alert("No report template selected.", AlertContext.Warning));
    } else {
      this.isLoadingSubmit = true;
      var reportTemplateID = parseInt(this.selectedReportTemplateID);
      var reportTemplate = this.reportTemplates.find(x => x.ReportTemplateID === reportTemplateID);
      var generateReportsDto = new GenerateReportsDto();
      generateReportsDto.ReportTemplateID = reportTemplateID;
      this.reportTemplateService.generateReport(generateReportsDto)
        .subscribe(response => {
          this.isLoadingSubmit = false;
  
          var a = document.createElement("a");
          a.href = URL.createObjectURL(response);
          a.download = reportTemplate.DisplayName + " Generated Report";
          // start download
          a.click();
  
          this.alertService.pushAlert(new Alert("Report Generated from Report Template '" + reportTemplate.DisplayName + "'", AlertContext.Success));
        }
          ,
          error => {
            this.isLoadingSubmit = false;
            this.cdr.detectChanges();
          }
        );
    }
  }
}
