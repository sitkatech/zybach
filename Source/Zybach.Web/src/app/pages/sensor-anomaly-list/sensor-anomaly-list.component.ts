import { ChangeDetectorRef, Component, OnInit, ViewChild } from '@angular/core';
import { AgGridAngular } from 'ag-grid-angular';
import { ColDef } from 'ag-grid-community';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { SensorAnomalyService } from 'src/app/services/sensor-anomaly.service';
import { UtilityFunctionsService } from 'src/app/services/utility-functions.service';
import { UserDto } from 'src/app/shared/generated/model/user-dto';
import { CustomRichTextType } from 'src/app/shared/models/enums/custom-rich-text-type.enum';

@Component({
  selector: 'zybach-sensor-anomaly-list',
  templateUrl: './sensor-anomaly-list.component.html',
  styleUrls: ['./sensor-anomaly-list.component.scss']
})
export class SensorAnomalyListComponent implements OnInit {
  @ViewChild('sensorAnomaliesGrid') sensorAnomaliesGrid: AgGridAngular;

  private currentUser: UserDto;
  
  public columnDefs: any[];
  public defaultColDef: ColDef;
  public richTextTypeID : number = CustomRichTextType.AnomalyReportList;

  constructor(
    private authenticationService: AuthenticationService,
    private cdr: ChangeDetectorRef,
    private utilityFunctionsService: UtilityFunctionsService,
    private sensorAnomalyService: SensorAnomalyService
  ) { }

  ngOnInit(): void {
    this.createSensorAnomaliesGridColumnDefs();

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
      {headerName: 'Sensor Name', field: 'SensorName' },
      {headerName: 'Well Registration #', field: 'WellRegistrationID' },
      this.utilityFunctionsService.createDateColumnDef('Start Date', 'StartDate', 'M/d/yyyy'),
      this.utilityFunctionsService.createDateColumnDef('End Date', 'EndDate', 'M/d/yyyy'),
      this.utilityFunctionsService.createDecimalColumnDef('Number of Days', 'NumberOfAnomalousDays', 120, 0),
      {headerName: 'Notes', field: 'Notes' }
    ];

    this.defaultColDef = { filter: true, sortable: true, resizable: true };
  }

  private updateSensorAnomaliesGridData(): void {
    this.sensorAnomalyService.getSensorAnomalies().subscribe(sensorAnomalies => {
      this.sensorAnomaliesGrid.api.setRowData(sensorAnomalies);
    });
  }
}
