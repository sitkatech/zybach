import { ChangeDetectorRef, Component, OnInit, ViewChild } from '@angular/core';
import { AgGridAngular } from 'ag-grid-angular';
import { ColDef } from 'ag-grid-community';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { UtilityFunctionsService } from 'src/app/services/utility-functions.service';
import { SensorSimpleDto } from 'src/app/shared/generated/model/sensor-simple-dto';
import { UserDto } from 'src/app/shared/generated/model/user-dto';
import { CustomRichTextType } from 'src/app/shared/models/enums/custom-rich-text-type.enum';

@Component({
  selector: 'zybach-anomaly-report-list',
  templateUrl: './anomaly-report-list.component.html',
  styleUrls: ['./anomaly-report-list.component.scss']
})
export class AnomalyReportListComponent implements OnInit {

  @ViewChild('sensorsGrid') sensorsGrid: AgGridAngular;
  public gridApi: any;

  public currentUser: UserDto;
  
  public columnDefs: any[];
  public defaultColDef: ColDef;

  public sensors: Array<SensorSimpleDto>;

  public richTextTypeID : number = CustomRichTextType.SensorList;

  constructor(
    private authenticationService: AuthenticationService,
    private cdr: ChangeDetectorRef,
    private utilityFunctionsService: UtilityFunctionsService,
  ) { }

  ngOnInit(): void {
    this.initializeSensorsGrid();

    this.authenticationService.getCurrentUser().subscribe(currentUser => {
      this.currentUser = currentUser;
    });
  }
  
  ngOnDestroy(): void {
    this.cdr.detach();
  }

  public exportToCsv() {
    this.utilityFunctionsService.exportGridToCsv(this.sensorsGrid, 'reported-anomalies.csv', null);
  }

  private initializeSensorsGrid(): void {
    this.columnDefs = [
      {headerName: "Sensor Name" },
      {headerName: "Well Registration #" },
      this.utilityFunctionsService.createDateColumnDef('Start Date', '', 'M/d/yyyy'),
      this.utilityFunctionsService.createDateColumnDef('End Date', '', 'M/d/yyyy'),
      this.utilityFunctionsService.createDecimalColumnDef('Number of Days', '', 120, 0),
      {headerName: "Notes" }
    ];

    this.defaultColDef = { filter: true, sortable: true, resizable: true };
  }
}
