import { DatePipe } from '@angular/common';
import { Component, OnInit, Input, ViewChild } from '@angular/core';
import { AgGridAngular } from 'ag-grid-angular';
import { ColDef } from 'ag-grid-community';
import { UtilityFunctionsService } from 'src/app/services/utility-functions.service';
import { LinkRendererComponent } from 'src/app/shared/components/ag-grid/link-renderer/link-renderer.component';
import { CustomDropdownFilterComponent } from 'src/app/shared/components/custom-dropdown-filter/custom-dropdown-filter.component';
import { WaterLevelInspectionSummaryDto } from 'src/app/shared/generated/model/water-level-inspection-summary-dto';
import { WellGroupDto } from 'src/app/shared/generated/model/well-group-dto';
import { WellGroupSummaryDto } from 'src/app/shared/generated/model/well-group-summary-dto';
import { default as vegaEmbed } from 'vega-embed';

@Component({
  selector: 'zybach-well-group-water-levels-tab',
  templateUrl: './well-group-water-levels-tab.component.html',
  styleUrls: ['./well-group-water-levels-tab.component.scss']
})
export class WellGroupWaterLevelsTabComponent implements OnInit {
  @Input() wellGroup: WellGroupSummaryDto;

  @ViewChild('waterLevelInspectionsGrid') waterLevelInspectionsGrid: AgGridAngular;

  public waterLevelVegaView: any;
  public hasWaterLevelChartData: boolean;
  public waterLevelChartID = "waterLevelChart";

  public waterLevelInspections: WaterLevelInspectionSummaryDto[];
  public columnDefs: ColDef[];
  public defaultColDef: ColDef;

  constructor(
    private utilityFunctionsService: UtilityFunctionsService,
    private datePipe: DatePipe
  ) { }

  ngOnInit(): void {

    this.createColumnDefs();
    this.waterLevelInspections = this.wellGroup.WaterLevelInspections;

    if (!this.wellGroup.WaterLevelChartVegaSpec) {
      this.hasWaterLevelChartData = false;
      return;
    }

    const waterLevelChartVegaSpec = JSON.parse(this.wellGroup.WaterLevelChartVegaSpec);
    this.buildWaterLevelChart(waterLevelChartVegaSpec);
    this.hasWaterLevelChartData = true;
  }

  private buildWaterLevelChart(waterLevelChartVegaSpec: {}) {
    var self = this;
    vegaEmbed(`#${this.waterLevelChartID}`, waterLevelChartVegaSpec, {
      actions: false, tooltip: true, renderer: "svg"
    }).then(function (res) {
      self.waterLevelVegaView = res.view;
      setTimeout(() => {
        self.waterLevelVegaView.runAsync();
        window.dispatchEvent(new Event('resize'));
      }, 200);
    }).catch(console.error);
  }

  private createColumnDefs() {
    const _datePipe = this.datePipe;

    this.columnDefs = [
      { headerName: "Well Registration #", field: "WellRegistrationID" },
      {
        headerName: "Inspection Date", 
        valueGetter: function (params: any) {
          return { LinkValue: params.data.WaterLevelInspectionID, LinkDisplay: _datePipe.transform(params.data.InspectionDate, "M/dd/yyyy h:mm a") };
        },
        cellRendererFramework: LinkRendererComponent,
        cellRendererParams: { inRouterLink: "/water-level-inspections/" },
        comparator: function (id1: any, id2: any) {
          const date1 = Date.parse(id1.LinkDisplay);
          const date2 = Date.parse(id2.LinkDisplay);
          if (date1 < date2) {
            return -1;
          }
          return (date1 > date2)  ?  1 : 0;
        },
        filter: 'agDateColumnFilter',
        filterValueGetter: params => _datePipe.transform(params.data.InspectionDate, "M/dd/yyyy h:mm a"),
        filterParams: {
          filterOptions: ['inRange'],
          comparator: this.utilityFunctionsService.dateFilterComparator
        }
      },
      this.utilityFunctionsService.createDecimalColumnDef('Measurement', 'Measurement', 120, 2),
      { 
        headerName: 'Measuring Equipment', field: 'MeasuringEquipment', 
        filterFramework: CustomDropdownFilterComponent,
        filterParams: { field: 'MeasuringEquipment' },
      },
      { headerName: 'Notes', field: 'InspectionNotes' }
    ];

    this.defaultColDef = { filter: true, sortable: true, resizable: true };
  }

  public exportToCsv() {
    this.utilityFunctionsService.exportGridToCsv(this.waterLevelInspectionsGrid, `${this.wellGroup.WellGroupName}-water-level-inspections.csv`, null);
  }
}
