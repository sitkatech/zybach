import { ChangeDetectorRef, Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { NgbDateAdapter } from '@ng-bootstrap/ng-bootstrap';
import { ColDef } from 'ag-grid-community';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { UtilityFunctionsService } from 'src/app/services/utility-functions.service';
import { LinkRendererComponent } from 'src/app/shared/components/ag-grid/link-renderer/link-renderer.component';
import { CustomDropdownFilterComponent } from 'src/app/shared/components/custom-dropdown-filter/custom-dropdown-filter.component';
import { FieldDefinitionGridHeaderComponent } from 'src/app/shared/components/field-definition-grid-header/field-definition-grid-header.component';
import { NgbDateAdapterFromString } from 'src/app/shared/components/ngb-date-adapter-from-string';
import { WellService } from 'src/app/shared/generated/api/well.service';
import { CustomRichTextTypeEnum } from 'src/app/shared/generated/enum/custom-rich-text-type-enum';
import { FieldDefinitionTypeEnum } from 'src/app/shared/generated/enum/field-definition-type-enum';
import { UserDto } from 'src/app/shared/generated/model/user-dto';
import { WellPumpingSummaryDto } from 'src/app/shared/generated/model/well-pumping-summary-dto';

@Component({
  selector: 'zybach-well-pumping-summary',
  templateUrl: './well-pumping-summary.component.html',
  styleUrls: ['./well-pumping-summary.component.scss'],
  providers: [{ provide: NgbDateAdapter, useClass: NgbDateAdapterFromString }]
})
export class WellPumpingSummaryComponent implements OnInit, OnDestroy {
  @ViewChild("wellPumpingSummariesGrid") wellPumpingSummariesGrid: any;

  private currentUser: UserDto;

  public startDate: string;
  public endDate: string;

  public wellPumpingSummaries: WellPumpingSummaryDto[];
  public columnDefs: ColDef[];
  public defaultColDef: ColDef;

  public richTextTypeID = CustomRichTextTypeEnum.WellPumpingSummary;

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

      const today = new Date();
      const yesterday = new Date().setDate(today.getDate() - 1);
      
      this.startDate = new Date(today.getFullYear(), 0, 1).toISOString().split('T')[0];
      this.endDate = new Date(yesterday).toISOString().split('T')[0];

      this.updateWellPumpingSummaries();

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
          return { LinkValue: `${params.data.WellID}/new-support-ticket`, LinkDisplay: "Create Ticket", CssClasses: "btn-sm btn-zybach" };
        },
        cellRendererFramework: LinkRendererComponent,
        cellRendererParams: { inRouterLink: "/wells" },
        sortable: false, filter: false, width: 140
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
          return { 
            LinkValue: params.data.MostRecentSupportTicketID, 
            LinkDisplay: params.data.MostRecentSupportTicketID ? `#${params.data.MostRecentSupportTicketID}: ${params.data.MostRecentSupportTicketTitle}` : ''
          }
        },
        cellRendererFramework: LinkRendererComponent,
        cellRendererParams: { inRouterLink: "/support-tickets/"},
        comparator: this.utilityFunctionsService.linkRendererComparator,
        filterValueGetter: params => params.data.MostRecentSupportTicketID ? `#${params.data.MostRecentSupportTicketID}: ${params.data.MostRecentSupportTicketTitle}` : ''
      },
      { 
        headerName: "Has Flow Meter?",
        valueGetter: params => params.data.FlowMeters ? `Yes (${params.data.FlowMeters})` : 'No',
        width: 170
      },
      { 
        headerName: "Has Continuity Meter?",
        valueGetter: params => params.data.ContinuityMeters ? `Yes (${params.data.ContinuityMeters})` : 'No',
        width: 170
      },
      { 
        headerName: "Has Electrical Usage?",
        valueGetter: params => params.data.ElectricalUsage ? `Yes (${params.data.ElectricalUsage})` : 'No',
        width: 170
      },
      this.utilityFunctionsService.createDecimalColumnDef("Flow Meter Pumped Volume (gal)", "FlowMeterPumpedVolume", 220, 1, true),
      this.utilityFunctionsService.createDecimalColumnDef("Continuity Meter Pumped Volume (gal)", "ContinuityMeterPumpedVolume", 220, 1, true),
      this.utilityFunctionsService.createDecimalColumnDef("Electrical Usage Pumped Volume (gal)", "ElectricalUsagePumpedVolume", 220, 1, true),
      this.utilityFunctionsService.createDecimalColumnDef("Flowmeter - Continuity Meter (gal)", "FlowMeterContinuityMeterDifference", 220, 1, true),
      this.utilityFunctionsService.createDecimalColumnDef("Flowmeter - Electrical Usage (gal)", "FlowMeterElectricalUsageDifference", 220, 1, true)
    ];

    this.defaultColDef = { sortable: true, filter: true, resizable: true };
  }

  public updateWellPumpingSummaries() {
    this.wellPumpingSummaries = null;
    this.wellService.wellsWellPumpingSummaryStartDateEndDateGet(this.startDate, this.endDate).subscribe(wellPumpingSummaries => {
      this.wellPumpingSummaries = wellPumpingSummaries;
    });
  }

  public downloadCsv(){
    var colIDsToExport = this.wellPumpingSummariesGrid.columnApi.getAllGridColumns().map(x => x.getId()).slice(1);
    this.utilityFunctionsService.exportGridToCsv(this.wellPumpingSummariesGrid, 'well-pumping-summary.csv', colIDsToExport);
  }
}