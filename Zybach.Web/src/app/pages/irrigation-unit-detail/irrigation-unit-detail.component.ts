import { AfterViewInit, ChangeDetectorRef, Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AgGridAngular } from 'ag-grid-angular';
import { ColDef } from 'ag-grid-community';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { IrrigationUnitService } from 'src/app/shared/generated/api/irrigation-unit.service';
import { UtilityFunctionsService } from 'src/app/services/utility-functions.service';
import { AgHubIrrigationUnitDetailDto } from 'src/app/shared/generated/model/ag-hub-irrigation-unit-detail-dto';
import { AgHubIrrigationUnitWaterYearMonthETAndPrecipDatumDto } from 'src/app/shared/generated/model/ag-hub-irrigation-unit-water-year-month-et-and-precip-datum-dto';
import { UserDto } from 'src/app/shared/generated/model/user-dto';
import { IrrigationUnitMapComponent } from '../irrigation-unit-map/irrigation-unit-map.component';
import { DatePipe } from '@angular/common';
import { AgHubIrrigationUnitRunoffSimpleDto } from 'src/app/shared/generated/model/models';
import { Observable, tap } from 'rxjs';

@Component({
  selector: 'zybach-irrigation-unit-detail',
  templateUrl: './irrigation-unit-detail.component.html',
  styleUrls: ['./irrigation-unit-detail.component.scss']
})
export class IrrigationUnitDetailComponent implements OnInit {
  public watchUserChangeSubscription: any;
  @ViewChild("irrigationUnitMap") irrigationUnitMap: IrrigationUnitMapComponent;
  @ViewChild('openETDataGrid') openETDataGrid: AgGridAngular;
  @ViewChild('runoffGrid') runoffGrid: AgGridAngular;
 
  public currentUser: UserDto;
  public irrigationUnit: AgHubIrrigationUnitDetailDto;
  public irrigationUnitID: number;

  public columnDefs: any[];
  public defaultColDef: ColDef;
  public openETData: Array<AgHubIrrigationUnitWaterYearMonthETAndPrecipDatumDto>;

  public runoffColumnDefs: any[] = [
    { headerName: 'Month', valueGetter: params => {
        const date = new Date();
        date.setMonth(params.data.Month - 1);
        return date.toLocaleString('default', { month: 'long' });
    }, sortable: true, filter: true, resizable: true },
    { headerName: 'Year', field: 'Year', sortable: true, filter: 'agNumberColumnFilter', resizable: true },
    { headerName: 'Day', field: 'Day', sortable: true, filter: true, resizable: true },
    { headerName: 'Precipitation (in)', field: 'Precipitation', sortable: true, filter: 'agNumberColumnFilter', resizable: true },
    { headerName: 'Curve Number', field: 'CurveNumber', sortable: true, filter: 'agNumberColumnFilter', resizable: true },
    { headerName: 'Runoff Depth (in)', field: 'RunoffDepth', sortable: true, filter: 'agNumberColumnFilter', resizable: true },
    { headerName: 'Runoff Volume', field: 'RunoffVolume', sortable: true, filter: 'agNumberColumnFilter', resizable: true },
  ];

  public runoffDefaultColDef: ColDef;
  public runoffData$: Observable<AgHubIrrigationUnitRunoffSimpleDto[]>;

  public gridApi: any;

  constructor(
    private irrigationUnitService: IrrigationUnitService,
    private authenticationService: AuthenticationService,
    private route: ActivatedRoute,
    private cdr: ChangeDetectorRef,
    private utilityFunctionsService: UtilityFunctionsService,
    private datePipe: DatePipe
  ) { }
  ngOnInit(): void {
    this.irrigationUnitID = parseInt(this.route.snapshot.paramMap.get("id"));
    this.authenticationService.getCurrentUser().subscribe(currentUser => {
      this.currentUser = currentUser;
      this.initializeGrid();
      this.openETDataGrid?.api.showLoadingOverlay();
      this.getIrrigationUnitDetails();
    });

    //this.runoffGrid.api.showLoadingOverlay();
    this.runoffData$ = this.irrigationUnitService.irrigationUnitsIrrigationUnitIDRunoffDataGet(this.irrigationUnitID).pipe(
        //tap(() => this.runoffGrid.api.hideOverlay())
    );
  }

  getIrrigationUnitDetails(){
    this.irrigationUnitService.irrigationUnitsIrrigationUnitIDGet(this.irrigationUnitID).subscribe((irrigationUnit: AgHubIrrigationUnitDetailDto) => {
      this.irrigationUnit = irrigationUnit;
      this.irrigationUnitID = irrigationUnit.AgHubIrrigationUnitID;
      this.openETData = irrigationUnit.WaterYearMonthETAndPrecipData;
      this.cdr.detectChanges();
    })
  }

  private initializeGrid(): void {
    const _datePipe = this.datePipe;

    this.columnDefs = [
      {
        headerName: "Month",
        valueGetter: params => _datePipe.transform(params.data.ReportedDate, 'MMMM'),
        sortable: true, filter: true, resizable: true
      },
      {
        headerName: "Year",
        valueGetter: params => parseInt(_datePipe.transform(params.data.ReportedDate, 'yyyy')),
        sortable: true, filter: 'agNumberColumnFilter', resizable: true
      },
      this.utilityFunctionsService.createDecimalColumnDef('Evapotranspiration (ac-in)', 'EvapotranspirationAcreInches'),
      this.utilityFunctionsService.createDecimalColumnDef('Precipitation (ac-in)', 'PrecipitationAcreInches'),
      this.utilityFunctionsService.createDecimalColumnDef('Evapotranspiration (in)', 'EvapotranspirationInches'),
      this.utilityFunctionsService.createDecimalColumnDef('Precipitation (in)', 'PrecipitationInches')
    ];
  }

  public onFirstDataRendered(params): void {
    this.gridApi = params.api;
    this.gridApi.sizeColumnsToFit();
  }

  public exportToCsv() {
    this.utilityFunctionsService.exportGridToCsv(this.openETDataGrid, `${this.irrigationUnit.WellTPID}-openET-data.csv`, null);
  }
  
  public exportRunoffToCsv() {
    this.utilityFunctionsService.exportGridToCsv(this.runoffGrid, `${this.irrigationUnit.WellTPID}-runoff-data.csv`, null);
  }

}
