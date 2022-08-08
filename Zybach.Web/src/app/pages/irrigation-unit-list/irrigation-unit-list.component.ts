import { Component, OnInit, ViewChild } from '@angular/core';
import { AgGridAngular } from 'ag-grid-angular';
import { ColDef } from 'ag-grid-community';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { IrrigationUnitService } from 'src/app/shared/generated/api/irrigation-unit.service';
import { UtilityFunctionsService } from 'src/app/services/utility-functions.service';
import { LinkRendererComponent } from 'src/app/shared/components/ag-grid/link-renderer/link-renderer.component';
import { MultiLinkRendererComponent } from 'src/app/shared/components/ag-grid/multi-link-renderer/multi-link-renderer.component';
import { AgHubIrrigationUnitSimpleDto } from 'src/app/shared/generated/model/ag-hub-irrigation-unit-simple-dto';
import { UserDto } from 'src/app/shared/generated/model/user-dto';
import { CustomRichTextTypeEnum } from 'src/app/shared/generated/enum/custom-rich-text-type-enum';
import { AlertService } from 'src/app/shared/services/alert.service';
import { FieldDefinitionGridHeaderComponent } from 'src/app/shared/components/field-definition-grid-header/field-definition-grid-header.component';
import { FieldDefinitionTypeEnum } from 'src/app/shared/generated/enum/field-definition-type-enum';
import { forkJoin } from 'rxjs';
import { AgHubIrrigationUnitSummaryDto } from 'src/app/shared/generated/model/ag-hub-irrigation-unit-summary-dto';
import { WaterYearMonthService } from 'src/app/shared/generated/api/water-year-month.service';
import { array } from 'vega';
import { DateTime } from 'vega-lite/build/src/datetime';
import { DecimalPipe } from '@angular/common';

@Component({
  selector: 'zybach-irrigation-unit-list',
  templateUrl: './irrigation-unit-list.component.html',
  styleUrls: ['./irrigation-unit-list.component.scss']
})
export class IrrigationUnitListComponent implements OnInit {
  @ViewChild('irrigationUnitGrid') irrigationUnitGrid: AgGridAngular;

  private currentUser: UserDto;

  public richTextTypeID : number = CustomRichTextTypeEnum.IrrigationUnitIndex;

  public columnDefs: ColDef[];
  public defaultColDef: ColDef;
  public irrigationUnits: Array<AgHubIrrigationUnitSummaryDto>;

  public years: number[];
  public months = [ 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 ];

  public startDateMonth: number;
  public startDateYear: number;
  public endDateMonth: number;
  public endDateYear: number;

  public gridApi: any;
  public unitsShown: string = "gal"; // gal or in

  constructor(
    private alertService: AlertService,
    private authenticationService: AuthenticationService,
    private irrigationUnitService: IrrigationUnitService,
    private utilityFunctionsService: UtilityFunctionsService,
    private waterYearMonthService: WaterYearMonthService,
    private decimalPipe: DecimalPipe
  ) { }

  ngOnInit(): void {
    this.authenticationService.getCurrentUser().subscribe(currentUser => {
      this.currentUser = currentUser;
      this.initializeGrid();
      this.irrigationUnitGrid?.api.showLoadingOverlay();

      this.waterYearMonthService.waterYearMonthsYearsGet().subscribe(years => {
        this.years = years;
      });

      const currentDate = new Date();
      this.startDateMonth = 1;
      this.endDateMonth = currentDate.getMonth() + 1;
      this.startDateYear = currentDate.getFullYear();
      this.endDateYear = this.startDateYear;
      
      this.updateIrrigationUnits();
    });
  }

  private initializeGrid(): void {
    const _decimalPipe = this.decimalPipe;

    this.columnDefs = [
      {
        headerName: 'Irrigation Unit ID',
        valueGetter: function (params: any) {
          return { LinkValue: params.data.AgHubIrrigationUnitID, LinkDisplay: params.data.WellTPID };
        }, 
        cellRendererFramework: LinkRendererComponent,
        cellRendererParams: { inRouterLink: "/irrigation-units/" },
        comparator: this.utilityFunctionsService.linkRendererComparator,
        filterValueGetter: function (params: any) {
          return params.data.WellTPID;
        },
        headerComponentFramework: FieldDefinitionGridHeaderComponent, headerComponentParams: {fieldDefinitionTypeID: FieldDefinitionTypeEnum.IrrigationUnitID},
        filter: true,
        resizable: true,
        sortable: true
      },
      this.utilityFunctionsService.createDecimalColumnDef('Area (ac)', 'IrrigationUnitAreaInAcres', 130, 2, null, FieldDefinitionTypeEnum.IrrigationUnitAcres),
      {
        headerName: "Associated Well Count",
        valueGetter: function (params: any) {
          return params.data.AssociatedWells.length;
        },
        sortable: true, filter: 'agNumberColumnFilter', resizable: true
      },
      {
        headerName: 'Associated Well(s)',
        valueGetter: function (params) {
          let names = params.data.AssociatedWells?.map(x => {
            return { LinkValue: x.WellID, LinkDisplay: x.WellRegistrationID }
          });
          const downloadDisplay = names?.map(x => x.LinkDisplay).join(", ");

          return { links: names, DownloadDisplay: downloadDisplay ?? "" };
        },
        filterValueGetter: function (params: any) {
          let names = params.data.AssociatedWells?.map(x => x.WellRegistrationID);
          return names ? names.join(", ") : "";
        },
        comparator: this.utilityFunctionsService.multiLinkRendererComparator, 
        resizable: true, sortable: true, filter: true,
        cellRendererParams: { inRouterLink: "/wells/" },
        cellRendererFramework: MultiLinkRendererComponent
      },
      { 
        headerValueGetter: () => `Precipitation (${this.unitsShown})`,
        valueGetter: params => this.unitsShown == 'in' ? params.data.TotalPrecipitationInches : params.data.TotalPrecipitationGallons,
        valueFormatter: params => params.value ? _decimalPipe.transform(params.value,'1.1-1') : '-',
        filter: 'agNumberColumnFilter', cellStyle: { textAlign: 'right' }
      },
      {
        headerValueGetter: () => `Evapotranspiration (${this.unitsShown})`,
        valueGetter: params => this.unitsShown == 'in' ? params.data.TotalEvapotranspirationInches : params.data.TotalEvapotranspirationGallons,
        valueFormatter: params => params.value ? _decimalPipe.transform(params.value,'1.1-1') : '-',
        filter: 'agNumberColumnFilter', cellStyle: { textAlign: 'right' }
      },
      {
        headerValueGetter: () => 'Flow Meter Pumped' + (this.unitsShown == 'gal' ? 'Volume (gal)' : 'Depth (in)'),
        valueGetter: params => this.unitsShown == 'in' ? params.data.FlowMeterPumpedDepthInches : params.data.FlowMeterPumpedVolumeGallons,
        valueFormatter: params => params.value ? _decimalPipe.transform(params.value,'1.1-1') : '-',
        filter: 'agNumberColumnFilter', cellStyle: { textAlign: 'right' }
      },
      {
        headerValueGetter: () => 'Continuity Meter Pumped' + (this.unitsShown == 'gal' ? 'Volume (gal)' : 'Depth (in)'),
        valueGetter: params => this.unitsShown == 'in' ? params.data.ContinuityMeterPumpedDepthInches : params.data.ContinuityMeterPumpedVolumeGallons,
        valueFormatter: params => params.value ? _decimalPipe.transform(params.value,'1.1-1') : '-',
        filter: 'agNumberColumnFilter', cellStyle: { textAlign: 'right' }
      },
      {
        headerValueGetter: () => 'Electrical Usage Pumped' + (this.unitsShown == 'gal' ? 'Volume (gal)' : 'Depth (in)'),
        valueGetter: params => this.unitsShown == 'in' ? params.data.ElectricalUsagePumpedDepthInches : params.data.ElectricalUsagePumpedVolumeGallons,
        valueFormatter: params => params.value ? _decimalPipe.transform(params.value,'1.1-1') : '-',
        filter: 'agNumberColumnFilter', cellStyle: { textAlign: 'right' }
      }
    ];

    this.defaultColDef = { sortable: true, filter: true, resizable: true };
  }

  public onFirstDataRendered(params): void {
    this.gridApi = params.api;
    this.gridApi.sizeColumnsToFit();
  }

  public updateIrrigationUnits() {
    this.irrigationUnitGrid.api.showLoadingOverlay();

    this.irrigationUnitService.irrigationUnitsSummaryStartDateMonthStartDateYearEndDateMonthEndDateYearGet(this.startDateMonth, this.startDateYear, this.endDateMonth, this.endDateYear)
      .subscribe(irrigationUnits => {
        this.irrigationUnits = irrigationUnits;

        this.irrigationUnitGrid.api.setRowData(irrigationUnits);
        this.irrigationUnitGrid.api.hideOverlay();
      });
  }

  public toggleUnitsShown(units : string): void {
    this.unitsShown = units;

    this.irrigationUnitGrid.api.setRowData(this.irrigationUnits);
    this.irrigationUnitGrid.api.refreshHeader();
  }

  public exportToCsv() {
    this.utilityFunctionsService.exportGridToCsv(this.irrigationUnitGrid, 'irrigation-units.csv', null);
  }

}