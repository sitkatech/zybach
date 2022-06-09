import { ChangeDetectorRef, Component, OnInit, ViewChild } from '@angular/core';
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

@Component({
  selector: 'zybach-irrigation-unit-detail',
  templateUrl: './irrigation-unit-detail.component.html',
  styleUrls: ['./irrigation-unit-detail.component.scss']
})
export class IrrigationUnitDetailComponent implements OnInit {
  public watchUserChangeSubscription: any;
  @ViewChild("irrigationUnitMap") irrigationUnitMap: IrrigationUnitMapComponent;
  @ViewChild('openETDataGrid') openETDataGrid: AgGridAngular;
 
  public currentUser: UserDto;
  public irrigationUnit: AgHubIrrigationUnitDetailDto;
  public irrigationUnitID: number;

  public columnDefs: any[];
  public defaultColDef: ColDef;
  public openETData: Array<AgHubIrrigationUnitWaterYearMonthETAndPrecipDatumDto>;

  public gridApi: any;

  constructor(
    private irrigationUnitService: IrrigationUnitService,
    private authenticationService: AuthenticationService,
    private route: ActivatedRoute,
    private router: Router,
    private cdr: ChangeDetectorRef,
    private utilityFunctionsService: UtilityFunctionsService
  ) { }

  ngOnInit(): void {
    this.irrigationUnitID = parseInt(this.route.snapshot.paramMap.get("id"));
    this.authenticationService.getCurrentUser().subscribe(currentUser => {
      this.currentUser = currentUser;
      this.initializeGrid();
      this.openETDataGrid?.api.showLoadingOverlay();
      this.getIrrigationUnitDetails();
      
    })
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
    this.columnDefs = [
      {
        headerName: "Month",
        valueGetter: function (params: any) {
          return params.data.WaterYearMonth.Month;
        },
        sortable: true, filter: 'agNumberColumnFilter', resizable: true
      },
      {
        headerName: "Year",
        valueGetter: function (params: any) {
          return params.data.WaterYearMonth.Year;
        },
        sortable: true, filter: 'agNumberColumnFilter', resizable: true
      },
      this.utilityFunctionsService.createDecimalColumnDef('Evapotranspiration (ac-ft)', 'EvapotranspirationAcreFeet'),
      this.utilityFunctionsService.createDecimalColumnDef('Evapotranspiration (in)', 'EvapotranspirationInches'),
      this.utilityFunctionsService.createDecimalColumnDef('Precipitation (ac-ft)', 'PrecipitationAcreFeet'),
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

}
