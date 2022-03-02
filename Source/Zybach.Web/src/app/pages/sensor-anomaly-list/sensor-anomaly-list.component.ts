import { ChangeDetectorRef, Component, OnInit, ViewChild } from '@angular/core';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { AgGridAngular } from 'ag-grid-angular';
import { ColDef } from 'ag-grid-community';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { SensorAnomalyService } from 'src/app/services/sensor-anomaly.service';
import { UtilityFunctionsService } from 'src/app/services/utility-functions.service';
import { FontAwesomeIconLinkRendererComponent } from 'src/app/shared/components/ag-grid/fontawesome-icon-link-renderer/fontawesome-icon-link-renderer.component';
import { SensorAnomalySimpleDto } from 'src/app/shared/generated/model/sensor-anomaly-simple-dto';
import { UserDto } from 'src/app/shared/generated/model/user-dto';
import { CustomRichTextType } from 'src/app/shared/models/enums/custom-rich-text-type.enum';
import { Alert } from 'src/app/shared/models/alert';
import { AlertContext } from 'src/app/shared/models/enums/alert-context.enum';
import { AlertService } from 'src/app/shared/services/alert.service';

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
  public richTextTypeID : number = CustomRichTextType.AnomalyReportList;
  
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
    this.utilityFunctionsService.exportGridToCsv(this.sensorAnomaliesGrid, 'reported-anomalies.csv', null);
  }

  private createSensorAnomaliesGridColumnDefs(): void {
    this.columnDefs = [
      { 
        valueGetter: params => params.data.SensorAnomalyID, 
        cellRendererFramework: FontAwesomeIconLinkRendererComponent,
        cellRendererParams: { inRouterLink: '/sensor-anomalies/edit/', fontawesomeIconName: 'edit', cssClasses: 'text-primary'},
        width: 30, sortable: false, filter: false
      },
      {
        cellRendererFramework: FontAwesomeIconLinkRendererComponent,
        cellRendererParams: { isSpan: true, fontawesomeIconName: 'trash', cssClasses: 'text-danger'},
        width: 30, sortable: false, filter: false
      },
      {headerName: 'Sensor Name', field: 'SensorName' },
      {headerName: 'Well Registration #', field: 'WellRegistrationID' },
      this.utilityFunctionsService.createDateColumnDef('Start Date', 'StartDate', 'M/d/yyyy', 'UTC'),
      this.utilityFunctionsService.createDateColumnDef('End Date', 'EndDate', 'M/d/yyyy', 'UTC'),
      this.utilityFunctionsService.createDecimalColumnDef('Number of Days', 'NumberOfAnomalousDays', 120, 0),
      {headerName: 'Notes', field: 'Notes' }
    ];

    this.defaultColDef = { filter: true, sortable: true, resizable: true };
  }

  private updateSensorAnomaliesGridData(): void {
    this.sensorAnomalyService.getSensorAnomalies().subscribe(sensorAnomalies => {
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

    this.sensorAnomalyService.deleteSensorAnomaly(this.sensorAnomalyToDelete.SensorAnomalyID).subscribe(() => {
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
