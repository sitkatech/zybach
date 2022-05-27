import { ChangeDetectorRef, Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { AgGridAngular } from 'ag-grid-angular';
import { ColDef } from 'ag-grid-community';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { LinkRendererComponent } from 'src/app/shared/components/ag-grid/link-renderer/link-renderer.component';
import { CustomDropdownFilterComponent } from 'src/app/shared/components/custom-dropdown-filter/custom-dropdown-filter.component';
import { GenerateReportsDto } from 'src/app/shared/generated/model/generate-reports-dto';
import { ReportTemplateDto } from 'src/app/shared/generated/model/report-template-dto';
import { UserDto } from 'src/app/shared/generated/model/user-dto';
import { Alert } from 'src/app/shared/models/alert';
import { AlertContext } from 'src/app/shared/models/enums/alert-context.enum';
import { CustomRichTextType } from 'src/app/shared/models/enums/custom-rich-text-type.enum';
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
  
  private currentUser: UserDto;

  public richTextTypeID : number = CustomRichTextType.ReportsList;

  public rowData: Array<ReportTemplateDto>;
  public columnDefs: ColDef[];

  constructor(
    private reportTemplateService: ReportTemplateService,
    private authenticationService: AuthenticationService,
    private router: Router,
    private alertService: AlertService,
    private cdr: ChangeDetectorRef) { }

  ngOnInit() {
    this.authenticationService.getCurrentUser().subscribe(currentUser => {
      this.currentUser = currentUser;
      this.reportTemplatesGrid?.api.showLoadingOverlay();
      this.reportTemplateService.listAllReportTemplates().subscribe(reportTemplates => {
        this.rowData = reportTemplates;
        this.reportTemplatesGrid.api.sizeColumnsToFit();
        this.reportTemplatesGrid.api.hideOverlay();
        this.cdr.detectChanges();
      });
      const mainAppApiUrl = environment.mainAppApiUrl
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
          sortable: true, filter: true, width: 300
        },
        { headerName: 'Description', field: 'Description', sortable: true, filter: true, width: 400 },
        { headerName: 'Model', field: 'ReportTemplateModel.ReportTemplateModelDisplayName', 
          filterFramework: CustomDropdownFilterComponent,
          filterParams: {
            field: 'ReportTemplateModel.ReportTemplateModelDisplayName'
          },
          sortable: true, width: 200 },
        {
          headerName: 'Template File', valueGetter: function (params: any) {
            return { LinkValue: `${mainAppApiUrl}/FileResource/${params.data.FileResource.FileResourceGUID}` , LinkDisplay: params.data.FileResource.OriginalBaseFilename };
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
          sortable: true, filter: true, width: 400
        },
      ];

      this.columnDefs.forEach(x => {
        x.resizable = true;
      });
    });
  }

  ngOnDestroy(): void {
    
       this.cdr.detach();
  }

}
