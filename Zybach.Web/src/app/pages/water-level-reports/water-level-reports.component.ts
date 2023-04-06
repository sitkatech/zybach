import { ChangeDetectorRef, Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { AgGridAngular } from 'ag-grid-angular';
import { ColDef, ColGroupDef } from 'ag-grid-community';
import { Subscription } from 'rxjs';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { UtilityFunctionsService } from 'src/app/services/utility-functions.service';
import { LinkRendererComponent } from 'src/app/shared/components/ag-grid/link-renderer/link-renderer.component';
import { MultiLinkRendererComponent } from 'src/app/shared/components/ag-grid/multi-link-renderer/multi-link-renderer.component';
import { CustomDropdownFilterComponent } from 'src/app/shared/components/custom-dropdown-filter/custom-dropdown-filter.component';
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
  @ViewChild('wellGroupsGrid') wellGroupsGrid: AgGridAngular;

  private currentUser: UserDto;
  private currentUserSubscription: Subscription;

  public wellGroups: WellGroupDto[];
  public columnDefs: (ColDef | ColGroupDef)[];
  public defaultColDef: ColDef;
  public selectedWellGroupsCount = 0;
  public rowsDisplayedCount: number;
  public reportTemplateID: number;

  public richTextTypeID = CustomRichTextTypeEnum.WaterLevelsReport;
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
      this.reportService.reportTemplatesGet().subscribe(reportTemplates => {
        this.reportTemplateID = reportTemplates.find(x => x.ReportTemplateModel.ReportTemplateModelID == 
          ReportTemplateModelEnum.WellGroupWaterLevelInspection)?.ReportTemplateID;
      });

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
    var selectedWellGroupsCount = 0;
    this.wellGroupsGrid.api.forEachNodeAfterFilterAndSort(node => {
      if (node.isSelected()) selectedWellGroupsCount++;
    });

    this.selectedWellGroupsCount = selectedWellGroupsCount;
  }

  public onFilterChanged() {
    this.rowsDisplayedCount = this.wellGroupsGrid.api.getDisplayedRowCount();
  }

  public exportToCsv() {
    this.utilityFunctionsService.exportGridToCsv(this.wellGroupsGrid, 'water-levels-summary.csv', null);
  }

  public generateReport(): void {
    this.alertService.clearAlerts();

    let selectedWellGroupIDs = [];
    this.wellGroupsGrid.api.forEachNodeAfterFilterAndSort(node => {
      if (node.isSelected()) {
        selectedWellGroupIDs.push(node.data.WellGroupID);
      }
    });

    if (selectedWellGroupIDs.length == 0){
      this.alertService.pushAlert(new Alert("No Well Groups selected", AlertContext.Danger));
      return;
    } 

    this.isLoadingSubmit = true;

    var generateChemigationPermitReportsDto = new GenerateReportsDto();
    generateChemigationPermitReportsDto.ReportTemplateID = this.reportTemplateID;
    generateChemigationPermitReportsDto.ModelIDList = selectedWellGroupIDs;

    this.reportService.reportTemplatesGenerateReportsPost(generateChemigationPermitReportsDto).subscribe(response => {
      this.isLoadingSubmit = false;

      var a = document.createElement("a");
      a.href = URL.createObjectURL(response);
      a.download = "Water Level Report";
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
        headerName: 'Well Group Name',
        valueGetter: function (params: any) {
          return { LinkValue: params.data.WellGroupID, LinkDisplay: params.data.WellGroupName };
        },
        cellRendererFramework: LinkRendererComponent,
        cellRendererParams: { inRouterLink: "/well-groups/" },
        comparator: this.utilityFunctionsService.linkRendererComparator,
        filterValueGetter: params => params.data.WellGroupName
      },
      {
        headerName: 'Wells', valueGetter: params => params.data.WellGroupWells.map(x => x.WellRegistrationID).join(", ")
      },
      { headerName: "Primary Well", field: "PrimaryWell.WellRegistrationID" },
      { 
        headerName: "Has Water Level Inspections?", 
        valueGetter: params => this.utilityFunctionsService.booleanValueGetter(params.data.HasWaterLevelInspections),
        filterFramework: CustomDropdownFilterComponent, filterParams: {} 
      },
      this.utilityFunctionsService.createDateColumnDef('Last Water Level Inspection', 'LatestWaterLevelInspectionDate', 'M/d/yyyy'),
      {  
        headerName: 'Well Group Owner (from Primary Well)',
        children: [
          { headerName: 'Name', field: "PrimaryWell.OwnerName" },
          { headerName: 'Address', field: "PrimaryWell.OwnerAddress" },
          { headerName: 'City', field: "PrimaryWell.OwnerCity" },
          { headerName: 'State', field: "PrimaryWell.OwnerState" },
          { headerName: 'Zip', field: "PrimaryWell.OwnerZipCode" }      
        ]
      },
      {  
        headerName: 'Additional Contact (from Primary Well)',
        children: [
          { headerName: 'Name', field: "PrimaryWell.AdditionalContactName" },
          { headerName: 'Address', field: "PrimaryWell.AdditionalContactAddress" },
          { headerName: 'City', field: "PrimaryWell.AdditionalContactCity" },
          { headerName: 'State', field: "PrimaryWell.AdditionalContactState" },
          { headerName: 'Zip', field: "PrimaryWell.AdditionalContactZipCode" }       
        ]
      }
    ];

    this.defaultColDef = { filter: true, sortable: true, resizable: true };
  }
}
