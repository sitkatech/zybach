import { ChangeDetectorRef, Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ColDef } from 'ag-grid-community';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { UtilityFunctionsService } from 'src/app/services/utility-functions.service';
import { LinkRendererComponent } from 'src/app/shared/components/ag-grid/link-renderer/link-renderer.component';
import { CustomDropdownFilterComponent } from 'src/app/shared/components/custom-dropdown-filter/custom-dropdown-filter.component';
import { FieldDefinitionGridHeaderComponent } from 'src/app/shared/components/field-definition-grid-header/field-definition-grid-header.component';
import { WellService } from 'src/app/shared/generated/api/well.service';
import { FieldDefinitionTypeEnum } from 'src/app/shared/generated/enum/field-definition-type-enum';
import { UserDto } from 'src/app/shared/generated/model/user-dto';
import { WellPumpingSummaryDto } from 'src/app/shared/generated/model/well-pumping-summary-dto';

@Component({
  selector: 'zybach-well-pumping-summary',
  templateUrl: './well-pumping-summary.component.html',
  styleUrls: ['./well-pumping-summary.component.scss']
})
export class WellPumpingSummaryComponent implements OnInit, OnDestroy {
  @ViewChild("wellPumpingSummariesGrid") wellPumpingSummariesGrid: any;

  private currentUser: UserDto;

  public wellPumpingSummaries: WellPumpingSummaryDto[];
  public columnDefs: ColDef[];
  public defaultColDef: ColDef;


  constructor(
    private authenticationService: AuthenticationService,
    private cdr: ChangeDetectorRef,
    private utilityFunctionsService: UtilityFunctionsService,
    private wellService: WellService
  ) { }

  ngOnInit(): void {
    this.authenticationService.getCurrentUser().subscribe(currentUser => {
      this.currentUser = currentUser;

      this.createColumnDefs();

      const startDate = new Date(new Date().getFullYear(), 0, 1);
      const endDate = new Date();
      this.wellService.wellsWellPumpingSummaryStartDateEndDateGet(startDate, endDate).subscribe(wellPumpingSummaries => {
        console.log(wellPumpingSummaries);
        this.wellPumpingSummaries = wellPumpingSummaries;
      })
      this.cdr.detectChanges;
    });
  }

  ngOnDestroy(): void {
    this.cdr.detach();
  }

  private createColumnDefs() {
    this.columnDefs = [
      {
        valueGetter: params => {
          return { LinkValue: `${params.data.WellID}/new-support-ticket`, LinkDisplay: "Create Ticket" };
        },
        cellRendererFramework: LinkRendererComponent,
        cellRendererParams: { inRouterLink: "/wells", CssClasses: "btn btn-primary"},
        sortable: false, filter: false, width: 60
      },
      { 
        headerComponentFramework: FieldDefinitionGridHeaderComponent, 
        headerComponentParams: {fieldDefinitionTypeID: FieldDefinitionTypeEnum.WellRegistrationNumber},
        valueGetter: params => {
          return { LinkValue: params.data.WellID, LinkDisplay: params.data.WellRegistrationID };
        }, 
        cellRendererFramework: LinkRendererComponent,
        cellRendererParams: { inRouterLink: "/wells/" },
        comparator: this.utilityFunctionsService.linkRendererComparator,
        filterValueGetter: params => params.data.WellRegistrationID
      },
      {
        headerName: "Owner",
        field: "OwnerName",
        headerComponentFramework: FieldDefinitionGridHeaderComponent, 
        headerComponentParams: {fieldDefinitionTypeID: FieldDefinitionTypeEnum.WellOwnerName},
      },
      {
        headerName: "Most Recent Support Ticket",
        valueGetter: params => {
          return { LinkValue: params.data.MostRecentSupportTicket.SupportTicketID, LinkDisplay: params.data.MostRecentSupportTicket.SupportTicketName }
        },
        cellRendererFramework: LinkRendererComponent,
        cellRendererParams: { inRouterLink: "/support-tickets/"},
        comparator: this.utilityFunctionsService.linkRendererComparator,
        filterValueGetter: params => params.data.MostRecentSupportTicket.SupportTicketName
      },
      { 
        headerName: "Has Flow Meter?", field: "HasFlowMeter",
        filterFramework: CustomDropdownFilterComponent,
        filterParams: { field: "HasFlowMeter" }
      },
      { 
        headerName: "Has Continuity Meter?", field: "HasContinuityMeter",
        filterFramework: CustomDropdownFilterComponent,
        filterParams: { field: "HasContinuityMeter" }
      },
      { 
        headerName: "Has Electrical Usage?", field: "HasElectricalUsage",
        filterFramework: CustomDropdownFilterComponent,
        filterParams: { field: "HasElectricalUsage" }
      },
      this.utilityFunctionsService.createDecimalColumnDef("Flow Meter Pumped Volume (gal)", "FlowMeterPumpedVolume"),
      this.utilityFunctionsService.createDecimalColumnDef("Continuity Meter Pumped Volume (gal)", "ContinuityMeterPumpedVolume"),
      this.utilityFunctionsService.createDecimalColumnDef("Electrical Usage Pumped Volume (gal)", "ElectricalUsagePumpedVolume"),
      this.utilityFunctionsService.createDecimalColumnDef("Flowmeter - Continuity Meter (gal)", "FlowMeterContinuityMeterDifference"),
      this.utilityFunctionsService.createDecimalColumnDef("Flowmeter - Electrical Usage (gal)", "FlowMeterElectricalUsageDifference")
    ];

    this.defaultColDef = { sortable: true, filter: true, resizable: true };
  }

  public downloadCsv(){
    var colIDsToExport = this.wellPumpingSummariesGrid.columnApi.getAllGridColumns().map(x => x.getId()).slice(1);
    this.utilityFunctionsService.exportGridToCsv(this.wellPumpingSummariesGrid, 'well-pumping-summary.csv', colIDsToExport);
  }
}
