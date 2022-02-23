import { DatePipe } from '@angular/common';
import { ChangeDetectorRef, Component, Input, OnInit, ViewChild } from '@angular/core';
import { AgGridAngular } from 'ag-grid-angular';
import { ColDef } from 'ag-grid-community';
import { forkJoin } from 'rxjs';
import { SensorStatusService } from 'src/app/services/sensor-status.service';
import { UtilityFunctionsService } from 'src/app/services/utility-functions.service';
import { WellService } from 'src/app/services/well.service';
import { ChemigationPermitDetailedDto } from '../../generated/model/chemigation-permit-detailed-dto';
import { SensorMessageAgeDto } from '../../generated/model/sensor-message-age-dto';
import { WaterLevelInspectionSummaryDto } from '../../generated/model/water-level-inspection-summary-dto';
import { WaterQualityInspectionSummaryDto } from '../../generated/model/water-quality-inspection-summary-dto';
import { WellDetailDto } from '../../generated/model/well-detail-dto';
import { AlertService } from '../../services/alert.service';
import { LinkRendererComponent } from '../ag-grid/link-renderer/link-renderer.component';
import { CustomDropdownFilterComponent } from '../custom-dropdown-filter/custom-dropdown-filter.component';
import * as vega from 'vega';
import { default as vegaEmbed } from 'vega-embed';
import { ReturnStatement } from '@angular/compiler';

@Component({
  selector: 'zybach-well-permits-inspections-tab',
  templateUrl: './well-permits-inspections-tab.component.html',
  styleUrls: ['./well-permits-inspections-tab.component.scss']
})
export class WellPermitsInspectionsTabComponent implements OnInit {
  @Input() well: WellDetailDto;
  
  @ViewChild('waterLevelInspectionsGrid') waterLevelInspectionsGrid: AgGridAngular;
  @ViewChild('waterQualityInspectionsGrid') waterQualityInspectionsGrid: AgGridAngular;
  
  public waterLevelInspectionColumnDefs: any[];
  public waterQualityInspectionColumnDefs: any[];
  public defaultColDef: ColDef;
  public nitrateChartID: string = "nitrateChart";
  public nitrateVegaView: any;
  public hasNitrateChartData: boolean = true;
  
  sensorsWithStatus: SensorMessageAgeDto[];
  public chemigationPermits: Array<ChemigationPermitDetailedDto>;

  public waterLevelInspections: Array<WaterLevelInspectionSummaryDto>;
  public waterQualityInspections: Array<WaterQualityInspectionSummaryDto>;
  
  constructor(
    private wellService: WellService,
    private sensorService: SensorStatusService,
    private cdr: ChangeDetectorRef,
    private datePipe: DatePipe,
    private utilityFunctionsService: UtilityFunctionsService,
    private alertService: AlertService
  ) { }

  ngOnInit(): void {
    this.initializeWaterLevelInspectionsGrid();
    this.initializeWaterQualityInspectionsGrid();
    this.getChemigationPermits();
    this.getSensorsWithAgeMessages();
    this.getInspections();
  }

  
  private initializeWaterLevelInspectionsGrid(): void {
    let datePipe = this.datePipe;
    this.waterLevelInspectionColumnDefs = [
      {
        headerName: "Inspection Date", 
        valueGetter: function (params: any) {
          return { LinkValue: params.data.WaterLevelInspectionID, LinkDisplay: datePipe.transform(params.data.InspectionDate, "M/dd/yyyy h:mm a") };
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
        filterParams: {
          filterOptions: ['inRange'],
          comparator: this.dateFilterComparator
        }, 
        width: 140,
        resizable: true,
        sortable: true
      },
      { 
        headerName: 'Measurement', 
        field: 'Measurement',
        filter: 'agNumberColumnFilter',
        type: 'rightAligned',
        width: 120,
        resizable: true,
        sortable: true
      },
      { 
        headerName: 'Measuring Equipment', field: 'MeasuringEquipment', 
        filterFramework: CustomDropdownFilterComponent,
        filterParams: {
          field: 'MeasuringEquipment'
        }, 
        width: 170,
        resizable: true,
        sortable: true 
      }
    ];
  }

  private initializeWaterQualityInspectionsGrid(): void {
    let datePipe = this.datePipe;
    this.waterQualityInspectionColumnDefs = [
      {
        headerName: "Inspection Date", 
        valueGetter: function (params: any) {
          return { LinkValue: params.data.WaterQualityInspectionID, LinkDisplay: datePipe.transform(params.data.InspectionDate, "M/dd/yyyy") };
        },
        cellRendererFramework: LinkRendererComponent,
        cellRendererParams: { inRouterLink: "/water-quality-inspections/" },
        comparator: function (id1: any, id2: any) {
          const date1 = Date.parse(id1.LinkDisplay);
          const date2 = Date.parse(id2.LinkDisplay);
          if (date1 < date2) {
            return -1;
          }
          return (date1 > date2)  ?  1 : 0;
        },
        filter: 'agDateColumnFilter',
        filterParams: {
          filterOptions: ['inRange'],
          comparator: this.waterQualityDateFilterComparator
        }, 
        width: 130,
        resizable: true,
        sortable: true
      },
      { 
        headerName: 'Inspection Type', field: 'InspectionType', 
        filterFramework: CustomDropdownFilterComponent,
        filterParams: {
          field: 'InspectionType'
        }, 
        width: 170,
        resizable: true,
        sortable: true 
      },
      { 
        headerName: 'Notes', 
        field: 'InspectionNotes',
        width: 180,
        filter: true,
        resizable: true,
        sortable: true
      },
    ];
  }

  
  // using a separate comparator for the LinkDisplay on WQI grid
  private waterQualityDateFilterComparator(filterLocalDate, cellValue) {
    const cellDate = Date.parse(cellValue.LinkDisplay);
    const filterLocalDateAtMidnight = filterLocalDate.getTime();
    if (cellDate == filterLocalDateAtMidnight) {
      return 0;
    }
    return (cellDate < filterLocalDateAtMidnight) ? -1 : 1;
  }

  private dateFilterComparator(filterLocalDate, cellValue) {
    const cellDate = Date.parse(cellValue);
    const filterLocalDateAtMidnight = filterLocalDate.getTime();
    if (cellDate == filterLocalDateAtMidnight) {
      return 0;
    }
    return (cellDate < filterLocalDateAtMidnight) ? -1 : 1;
  }

  getInspections() {
    forkJoin({
      waterLevelInspections: this.wellService.listWaterLevelInspectionsByWellID(this.well.WellID),
      waterQualityInspections: this.wellService.listWaterQualityInspectionsByWellID(this.well.WellID)
    }).subscribe(({ waterLevelInspections, waterQualityInspections }) => {
      this.waterLevelInspections = waterLevelInspections;
      this.waterQualityInspections = waterQualityInspections;

      this.waterLevelInspectionsGrid ? this.waterLevelInspectionsGrid.api.setRowData(waterLevelInspections) : null;
      this.waterQualityInspectionsGrid ? this.waterQualityInspectionsGrid.api.setRowData(waterQualityInspections) : null;

      this.waterLevelInspectionsGrid.api.sizeColumnsToFit();
      this.waterQualityInspectionsGrid.api.sizeColumnsToFit();

      this.cdr.detectChanges();

      if (this.waterQualityInspections != null && this.waterQualityInspections.length > 0) {
        this.wellService.getWellNitrateChartVegaSpec(this.well.WellID).subscribe(result => {
          if (result == null || result == undefined || result == "") {
            this.hasNitrateChartData = false;
            return;
          }

          this.buildNitrateChart(result);
          return;
        })
      } else {
        this.hasNitrateChartData = false;
      }

    });
  }

  public exportWLIToCsv() {
    this.utilityFunctionsService.exportGridToCsv(this.waterLevelInspectionsGrid, `${this.well.WellRegistrationID}-water-level-inspections.csv`, null);
  }

  public exportWQIToCsv() {
    this.utilityFunctionsService.exportGridToCsv(this.waterQualityInspectionsGrid, `${this.well.WellRegistrationID}-water-quality-inspections.csv`, null);
  }

  getChemigationPermits(){
    this.wellService.getChemigationPermits(this.well.WellID).subscribe(chemigationPermits => {
      this.chemigationPermits = chemigationPermits;
    });
  }

  getSensorsWithAgeMessages(){
    this.sensorService.getSensorStatusForWell(this.well.WellID).subscribe(wellWithSensorMessageAge => {
      this.sensorsWithStatus = wellWithSensorMessageAge.Sensors;

      for (var sensor of this.sensorsWithStatus){
        sensor.MessageAge = Math.floor(sensor.MessageAge / 3600)
      }
    });
  }

  hasWellPressureSensor(): boolean{
    return this.sensorsWithStatus?.filter(x => x.SensorType === "Well Pressure").length > 0;
  }

  private buildNitrateChart(nitrateChartVegaSpec : any) {
    var self = this;
    vegaEmbed(`#${this.nitrateChartID}`, nitrateChartVegaSpec, {
      actions: false, tooltip: true, renderer: "svg"
    }).then(function (res) {
      self.nitrateVegaView = res.view;
      setTimeout(() => {
        self.nitrateVegaView.runAsync();
        window.dispatchEvent(new Event('resize'));
      }, 200);
    }).catch(() => this.hasNitrateChartData = false);
  }
  
}
