import { ChangeDetectorRef, Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { AgGridAngular } from 'ag-grid-angular';
import { ColDef, ColGroupDef } from 'ag-grid-community';
import { Subscription } from 'rxjs';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { UtilityFunctionsService } from 'src/app/services/utility-functions.service';
import { LinkRendererComponent } from 'src/app/shared/components/ag-grid/link-renderer/link-renderer.component';
import { FieldDefinitionGridHeaderComponent } from 'src/app/shared/components/field-definition-grid-header/field-definition-grid-header.component';
import { ReportService } from 'src/app/shared/generated/api/report.service';
import { WellGroupService } from 'src/app/shared/generated/api/well-group.service';
import { CustomRichTextTypeEnum } from 'src/app/shared/generated/enum/custom-rich-text-type-enum';
import { ReportTemplateModelEnum } from 'src/app/shared/generated/enum/report-template-model-enum';
import { GenerateReportsDto } from 'src/app/shared/generated/model/generate-reports-dto';
import { UserDto } from 'src/app/shared/generated/model/user-dto';
import { WellGroupDto } from 'src/app/shared/generated/model/well-group-dto';
import { Alert } from 'src/app/shared/models/alert';
import { AlertContext } from 'src/app/shared/models/enums/alert-context.enum';
import { AlertService } from 'src/app/shared/services/alert.service';

@Component({
  selector: 'zybach-water-level-reports',
  templateUrl: './water-level-reports.component.html',
  styleUrls: ['./water-level-reports.component.scss']
})
export class WaterLevelReportsComponent implements OnInit, OnDestroy {
  @ViewChild('waterLevelReportsGrid') waterLevelReportsGrid: AgGridAngular;

  private currentUser: UserDto;
  private currentUserSubscription: Subscription;

  public wellGroups: WellGroupDto[];
  public selectedWellGroupIDs = new Array<number>();

  public columnDefs: (ColDef | ColGroupDef)[];
  public defaultColDef: ColDef;
  public rowsDisplayedCount: number;

  public richTextTypeID = CustomRichTextTypeEnum.WellInspectionReports; // update
  public isLoadingSubmit = false;

  constructor(
    private authenticationService: AuthenticationService,
    private utilityFunctionsService: UtilityFunctionsService,
    private cdr: ChangeDetectorRef,
    private alertService: AlertService,
    private wellGroupService: WellGroupService,
    private reportService: ReportService
  ) { }

  ngOnInit(): void {
    this.currentUserSubscription = this.authenticationService.getCurrentUser().subscribe(currentUser => {
      this.wellGroupService.wellGroupsGet().subscribe(wellGroups => {
        this.wellGroups = wellGroups;
        this.rowsDisplayedCount = wellGroups.length;
      });

      this.initializeGrid();
    });
  }

  ngOnDestroy(): void {
    this.currentUserSubscription.unsubscribe();
    this.cdr.detach();
  }

  public onSelectionChanged() {
    this.waterLevelReportsGrid.api.forEachNodeAfterFilterAndSort(node => {
      if (node.isSelected()) {
        this.selectedWellGroupIDs.push(node.data.WellGroupID);
      }
    })
  }

  public onFilterChanged() {
    this.rowsDisplayedCount = this.waterLevelReportsGrid.api.getDisplayedRowCount();
  }

  public exportToCsv() {
    this.utilityFunctionsService.exportGridToCsv(this.waterLevelReportsGrid, 'water-inspection-summary.csv', null); // rename file
  }

  public generateReport(): void {
    this.alertService.clearAlerts();

    let selectedWellGroupIDs = this.waterLevelReportsGrid.api.getSelectedRows().map(x => x.WellGroupID);

    if (selectedWellGroupIDs.length == 0){
      this.alertService.pushAlert(new Alert("Please select at least one Well Group to generate a Water Level Report.", AlertContext.Danger));
      return;
    } 

    this.isLoadingSubmit = true;

    var generateChemigationPermitReportsDto = new GenerateReportsDto();
    generateChemigationPermitReportsDto.ReportTemplateID = ReportTemplateModelEnum.WellWaterLevelInspection;
    generateChemigationPermitReportsDto.ModelIDList = selectedWellGroupIDs;

    this.reportService.reportTemplatesGenerateReportsPost(generateChemigationPermitReportsDto).subscribe(response => {
      this.isLoadingSubmit = false;

      var a = document.createElement("a");
      a.href = URL.createObjectURL(response);
      a.download = "Well Inspection Reports";
      a.click();

      this.alertService.pushAlert(new Alert("Water Level Report generated for selected Well Groups", AlertContext.Success));

    }, error => {
      this.isLoadingSubmit = false;
      this.cdr.detectChanges();
    });
  }

  private initializeGrid() {
    this.columnDefs = [
      { headerCheckboxSelection: true, checkboxSelection: true, headerCheckboxSelectionFilteredOnly: true, sortable: false, filter: false, width: 40 },
      {
        headerName: 'Well Group',
        valueGetter: function (params: any) {
          return { LinkValue: params.data.WellGroupID, LinkDisplay: params.data.WellGroupName };
        },
        cellRendererFramework: LinkRendererComponent,
        cellRendererParams: { inRouterLink: "/well-groups/" },
        comparator: this.utilityFunctionsService.linkRendererComparator,
        filterValueGetter: params => params.data.WellGroupName
      },
      this.utilityFunctionsService.createDateColumnDef('Last Water Level Inspection', 'LatestWaterLevelInspectionDate', 'M/d/yyyy'),
      {  
        headerName: 'Well Group Owner (from Primary Well)',
        children: [
          { headerName: 'Name', field: "PrimaryWell.OwnerName", filter: true, resizable: true, sortable: true },
          { headerName: 'Address', field: "PrimaryWell.OwnerAddress", filter: true, resizable: true, sortable: true },
          { headerName: 'City', field: "PrimaryWell.OwnerCity", filter: true, resizable: true, sortable: true },
          { headerName: 'State', field: "PrimaryWell.OwnerState", filter: true, resizable: true, sortable: true },
          { headerName: 'Zip', field: "PrimaryWell.OwnerZipCode", filter: true, resizable: true, sortable: true }      
        ]
      },
      {  
        headerName: 'Additional Contact (from Primary Well)',
        children: [
          { headerName: 'Name', field: "PrimaryWell.AdditionalContactName", filter: true, resizable: true, sortable: true },
          { headerName: 'Address', field: "PrimaryWell.AdditionalContactAddress", filter: true, resizable: true, sortable: true },
          { headerName: 'City', field: "PrimaryWell.AdditionalContactCity", filter: true, resizable: true, sortable: true },
          { headerName: 'State', field: "PrimaryWell.AdditionalContactState", filter: true, resizable: true, sortable: true },
          { headerName: 'Zip', field: "PrimaryWell.AdditionalContactZipCode", filter: true, resizable: true, sortable: true }       
        ]
      }
    ];

    this.defaultColDef = { filter: true, sortable: true, resizable: true };
  }
}
