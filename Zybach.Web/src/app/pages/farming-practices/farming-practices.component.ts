import { Component, OnInit, ViewChild } from '@angular/core';
import { AgGridAngular } from 'ag-grid-angular';
import { ColDef } from 'ag-grid-community';
import { CustomRichTextTypeEnum } from 'src/app/shared/generated/enum/custom-rich-text-type-enum';
import { IrrigationUnitService } from 'src/app/shared/generated/api/irrigation-unit.service';
import { AgHubIrrigationUnitFarmingPracticeDto } from 'src/app/shared/generated/model/ag-hub-irrigation-unit-farming-practice-dto';
import { LinkRendererComponent } from 'src/app/shared/components/ag-grid/link-renderer/link-renderer.component';
import { UtilityFunctionsService } from 'src/app/services/utility-functions.service';
import { FieldDefinitionGridHeaderComponent } from 'src/app/shared/components/field-definition-grid-header/field-definition-grid-header.component';
import { FieldDefinitionTypeEnum } from 'src/app/shared/generated/enum/field-definition-type-enum';
import { MultiLinkRendererComponent } from 'src/app/shared/components/ag-grid/multi-link-renderer/multi-link-renderer.component';

@Component({
  selector: 'farming-practices',
  templateUrl: './farming-practices.component.html',
  styleUrls: ['./farming-practices.component.scss']
})
export class FarmingPracticesComponent implements OnInit {
  @ViewChild('irrigationUnitsGrid') irrigationUnitGrid: AgGridAngular;

  public selectedYear: number;
  public years = [];

  public selectedIrrigationUnitID: number;
  public irrigationUnitFarmingPractices: AgHubIrrigationUnitFarmingPracticeDto[];
  public irrigationUnitFarmingPracticesForSelectedYear: AgHubIrrigationUnitFarmingPracticeDto[];

  public columnDefs: ColDef[];
  public defaultColDef: ColDef;
  public gridOptions = {
    getRowId: params => params.data.AgHubIrrigationUnitID.toString(),
    onGridReady: params => params.api.sizeColumnsToFit()
  };

  public legendColorByCropType: { [cropType: string]: string };
  public irrigationUnitAcresByCropType: { [cropType: string]: number };
  public totalAcres: number;
  public cropTypes: string[];

  public richTextTypeID = CustomRichTextTypeEnum.FarmingPractices;

  constructor(
    private irrigationUnitService: IrrigationUnitService,
    private utilityFunctionsService: UtilityFunctionsService
  ) {}

  ngOnInit(): void {
    this.selectedYear = new Date().getFullYear();
    this.initializeGrid();

    this.irrigationUnitService.irrigationUnitsFarmingPracticesGet().subscribe(irrigationUnitFarmingPractices => {
      this.irrigationUnitFarmingPractices = irrigationUnitFarmingPractices;
      
      const irrigationYears = irrigationUnitFarmingPractices.map(x => x.IrrigationYear)
      const minYear = Math.min(...irrigationYears);
      for (let year = this.selectedYear; year >= minYear; year--) { this.years.push(year) }

      this.updateGridData();
    });
  }

  private createFarmingPracticeOverview() {
    this.irrigationUnitAcresByCropType = {};
    this.legendColorByCropType = {};
    this.totalAcres = 0;

    this.irrigationUnitFarmingPracticesForSelectedYear.forEach(irrigationUnit => {
      const cropType = irrigationUnit.CropTypeLegendDisplayName;

      if (!this.irrigationUnitAcresByCropType[cropType]) {
        this.irrigationUnitAcresByCropType[cropType] = 0;
      }
      this.irrigationUnitAcresByCropType[cropType] += irrigationUnit.Acres;
      this.totalAcres += irrigationUnit.Acres;

      if (!this.legendColorByCropType[cropType]) {
        this.legendColorByCropType[cropType] = irrigationUnit.CropTypeMapColor
      }
    });

    this.cropTypes = Object.keys(this.irrigationUnitAcresByCropType).filter(cropType => cropType != "Not Reported" && cropType != "Other").sort();
    if (this.irrigationUnitAcresByCropType["Other"]) this.cropTypes.push("Other");
    if (this.irrigationUnitAcresByCropType["Not Reported"]) this.cropTypes.push("Not Reported");
  }

  public onGridSelectionChanged(params) {
    if (params.source != "rowClicked") return;

    var selectedRows = this.irrigationUnitGrid.api?.getSelectedRows();
    if (selectedRows.length == 0) return;

    this.selectedIrrigationUnitID = selectedRows[0].AgHubIrrigationUnitID;
  }

  public onMapSelectionChanged(selectedIrrigationUnitID: number) {
    if (selectedIrrigationUnitID == this.selectedIrrigationUnitID) return;
    this.selectedIrrigationUnitID = selectedIrrigationUnitID;

    this.irrigationUnitGrid.api.deselectAll();

    var rowNode = this.irrigationUnitGrid.api.getRowNode(this.selectedIrrigationUnitID.toString());
    rowNode.setSelected(true);
    this.irrigationUnitGrid.api.ensureNodeVisible(rowNode);
  }

  public downloadCsv() {}

  public updateGridData() {
    this.irrigationUnitFarmingPracticesForSelectedYear = this.irrigationUnitFarmingPractices.filter(x => x.IrrigationYear == this.selectedYear);
    this.irrigationUnitGrid.api.setRowData(this.irrigationUnitFarmingPracticesForSelectedYear);

    this.createFarmingPracticeOverview();
  };

  public initializeGrid() {
    this.columnDefs = [
      {
        headerName: 'Irrigation Unit ID',
        valueGetter: params => {
          return { LinkValue: params.data.AgHubIrrigationUnitID, LinkDisplay: params.data.WellTPID };
        }, 
        cellRenderer: LinkRendererComponent,
        cellRendererParams: { inRouterLink: "/irrigation-units/" },
        comparator: this.utilityFunctionsService.linkRendererComparator,
        filterValueGetter: params => params.data.WellTPID,
        headerComponent: FieldDefinitionGridHeaderComponent, 
        headerComponentParams: { fieldDefinitionTypeID: FieldDefinitionTypeEnum.IrrigationUnitID }
      },
      {
        headerName: 'Associated Well(s)',
        valueGetter: params => {
          let names = params.data.Wells?.map(x => {
            return { LinkValue: x.WellID, LinkDisplay: x.WellRegistrationID }
          });
          const downloadDisplay = names?.map(x => x.LinkDisplay).join(", ");

          return { links: names, DownloadDisplay: downloadDisplay ?? "" };
        },
        filterValueGetter: params => {
          let names = params.data.AssociatedWells?.map(x => x.WellRegistrationID);
          return names ? names.join(", ") : "";
        },
        comparator: this.utilityFunctionsService.multiLinkRendererComparator, 
        resizable: true, sortable: true, filter: true,
        cellRendererParams: { inRouterLink: "/wells/" },
        cellRenderer: MultiLinkRendererComponent
      },
      { headerName: "Irrigation Year", field: "IrrigationYear", cellStyle: { textAlign: 'right' }, width: 120 },
      this.utilityFunctionsService.createDecimalColumnDef("Acres", "Acres", 120, 2, true),
      { 
        headerName: "Crop Type", field: "CropTypeLegendDisplayName",
        valueGetter: params => !params.data.CropType ? 'Not Reported' : 
          params.data.CropTypeLegendDisplayName == "Other" ?  `Other (${params.data.CropType})` : params.data.CropType
      },
      { 
        headerName: "Tillage Type", field: "Tillage",
        valueGetter: params => params.data.Tillage ?? "Not Reported"
      }
    ];

    this.defaultColDef = { sortable: true, filter: true, resizable: true };
  }
}
