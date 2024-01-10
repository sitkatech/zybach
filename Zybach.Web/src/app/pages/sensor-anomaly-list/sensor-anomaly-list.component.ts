import { ChangeDetectorRef, Component, OnInit, ViewChild } from '@angular/core';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { AgGridAngular } from 'ag-grid-angular';
import { ColDef } from 'ag-grid-community';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { SensorAnomalyService } from 'src/app/shared/generated/api/sensor-anomaly.service';
import { UtilityFunctionsService } from 'src/app/services/utility-functions.service';
import { FontAwesomeIconLinkRendererComponent } from 'src/app/shared/components/ag-grid/fontawesome-icon-link-renderer/fontawesome-icon-link-renderer.component';
import { SensorAnomalySimpleDto } from 'src/app/shared/generated/model/sensor-anomaly-simple-dto';
import { UserDto } from 'src/app/shared/generated/model/user-dto';
import { CustomRichTextTypeEnum } from 'src/app/shared/generated/enum/custom-rich-text-type-enum';
import { Alert } from 'src/app/shared/models/alert';
import { AlertContext } from 'src/app/shared/models/enums/alert-context.enum';
import { AlertService } from 'src/app/shared/services/alert.service';
import { LinkRendererComponent } from 'src/app/shared/components/ag-grid/link-renderer/link-renderer.component';
import { CustomDropdownFilterComponent } from 'src/app/shared/components/custom-dropdown-filter/custom-dropdown-filter.component';
import { FieldDefinitionGridHeaderComponent } from 'src/app/shared/components/field-definition-grid-header/field-definition-grid-header.component';
import { FieldDefinitionTypeEnum } from 'src/app/shared/generated/enum/field-definition-type-enum';

@Component({
  selector: 'zybach-sensor-anomaly-list',
  templateUrl: './sensor-anomaly-list.component.html',
  styleUrls: ['./sensor-anomaly-list.component.scss']
})
export class SensorAnomalyListComponent implements OnInit {
  @ViewChild('sensorAnomaliesGrid') sensorAnomaliesGrid: AgGridAngular;
  @ViewChild('deleteSensorAnomalyModal') deleteSensorAnomalyModal;


  private currentUser: UserDto;
  
  public sensorAnomalies: Array<SensorAnomalySimpleDto>;
  public columnDefs: any[];
  public defaultColDef: ColDef;
  public richTextTypeID : number = CustomRichTextTypeEnum.AnomalyReportList;
  
  private modalReference: NgbModalRef;
  public deleteColumnID = 1;
  public sensorAnomalyToDelete: SensorAnomalySimpleDto;
  public isLoadingDelete = false;

  constructor(
    private authenticationService: AuthenticationService,
    private cdr: ChangeDetectorRef,
    private utilityFunctionsService: UtilityFunctionsService,
    private sensorAnomalyService: SensorAnomalyService,
    private modalService: NgbModal,
    private alertService: AlertService
  ) { }

  ngOnInit(): void {
    this.authenticationService.getCurrentUser().subscribe(currentUser => {
      this.currentUser = currentUser;

      this.createSensorAnomaliesGridColumnDefs();
      this.updateSensorAnomaliesGridData();
    });
  }
  
  ngOnDestroy(): void {
    this.cdr.detach();
  }

  public exportToCsv() {
    var colIDsToExport = this.sensorAnomaliesGrid.columnApi.getAllGridColumns().map(x => x.getId()).slice(2);
    this.utilityFunctionsService.exportGridToCsv(this.sensorAnomaliesGrid, 'sensor-anomalies.csv', colIDsToExport);
  }

  private createSensorAnomaliesGridColumnDefs(): void {
    this.columnDefs = [
      { 
        valueGetter: params => params.data.SensorAnomalyID, 
        cellRenderer: FontAwesomeIconLinkRendererComponent,
        cellRendererParams: { inRouterLink: '/sensor-anomalies/edit/', fontawesomeIconName: 'edit', cssClasses: 'text-primary'},
        width: 40, sortable: false, filter: false, cellStyle: {textAlign: 'center'}
      },
      {
        cellRenderer: FontAwesomeIconLinkRendererComponent,
        cellRendererParams: { isSpan: true, fontawesomeIconName: 'trash', cssClasses: 'text-danger'},
        width: 40, sortable: false, filter: false, cellStyle: {textAlign: 'center'}
      },
      {
        headerName: 'Sensor Name', valueGetter: function (params: any) {
          return { LinkValue: params.data.Sensor.SensorID, LinkDisplay: params.data.Sensor.SensorName };
        }, 
        cellRenderer: LinkRendererComponent, cellRendererParams: { inRouterLink: "/sensors/" },
        filterValueGetter: params => params.data.Sensor.SensorName,
        comparator: this.utilityFunctionsService.linkRendererComparator,
        width: 180
      },
      {
        headerName: 'Well', valueGetter: function (params: any) {
         return params.data.Sensor.WellID ?  { LinkValue: params.data.Sensor.WellID, LinkDisplay: params.data.Sensor.WellRegistrationID }
            : { LinkValue: null, LinkDisplay: null };
        }, 
        cellRenderer: LinkRendererComponent, cellRendererParams: { inRouterLink: "/wells/" },
        filterValueGetter: params => params.data.Sensor.WellRegistrationID,
        headerComponent: FieldDefinitionGridHeaderComponent, headerComponentParams: { fieldDefinitionTypeID: FieldDefinitionTypeEnum.WellRegistrationNumber },
        comparator: this.utilityFunctionsService.linkRendererComparator,
        width: 180
      },
      {
        headerName: 'Sensor Type', field: 'Sensor.SensorTypeName', width: 180,
        filter: CustomDropdownFilterComponent,
        headerComponent: FieldDefinitionGridHeaderComponent, headerComponentParams: { fieldDefinitionTypeID: FieldDefinitionTypeEnum.SensorType },
        filterParams: {
          field: 'Sensor.SensorTypeName',
        }, 
      },
      this.utilityFunctionsService.createDateColumnDef('Start Date', 'StartDate', 'M/d/yyyy', 'UTC', null, null),
      this.utilityFunctionsService.createDateColumnDef('End Date', 'EndDate', 'M/d/yyyy', 'UTC', null, null),
      this.utilityFunctionsService.createDecimalColumnDef('Number of Days', 'NumberOfAnomalousDays', 120, 0),
      {headerName: 'Notes', field: 'Notes' }
    ];

    this.defaultColDef = { filter: true, sortable: true, resizable: true };
  }

  private updateSensorAnomaliesGridData(): void {
    this.sensorAnomalyService.sensorAnomaliesGet().subscribe(sensorAnomalies => {
      this.sensorAnomalies = sensorAnomalies;

      this.sensorAnomaliesGrid.api.setRowData(sensorAnomalies);
      this.sensorAnomaliesGrid.api.sizeColumnsToFit();
    });
  }

  public onCellClicked(event: any): void {
    if (event.column.colId == this.deleteColumnID) {
      if (this.sensorAnomalyToDelete) {
        this.sensorAnomalyToDelete = null;
      }
      this.sensorAnomalyToDelete = this.sensorAnomalies.find(x => x.SensorAnomalyID == event.data.SensorAnomalyID);
      this.launchModal(this.deleteSensorAnomalyModal, 'deleteSensorAnomalyModalTitle');
    }
  }

  private checkIfDeleting(): boolean {
    return this.isLoadingDelete;
  }

  private launchModal(modalContent: any, modalTitle: string): void {
    this.modalReference = this.modalService.open(
      modalContent, 
      { ariaLabelledBy: modalTitle, beforeDismiss: () => this.checkIfDeleting(), backdrop: 'static', keyboard: false }
    );
  }

  public deleteSensorAnomaly() {
    this.isLoadingDelete = true;

    this.sensorAnomalyService.sensorAnomaliesSensorAnomalyIDDelete(this.sensorAnomalyToDelete.SensorAnomalyID).subscribe(() => {
      this.isLoadingDelete = false;
      this.modalReference.close();
      this.alertService.pushAlert(new Alert('Sensor Anomaly was successfully deleted.', AlertContext.Success, true));
      window.scroll(0,0);
      this.updateSensorAnomaliesGridData();
    }, error => {
      this.isLoadingDelete = false;
      this.modalReference.close();
      window.scroll(0,0);
      this.cdr.detectChanges();
    })
  }
}
