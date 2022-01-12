import { DatePipe, DecimalPipe } from '@angular/common';
import { ChangeDetectorRef, Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { AgGridAngular } from 'ag-grid-angular';
import { ColDef } from 'ag-grid-community';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { ChemigationPermitService } from 'src/app/services/chemigation-permit.service';
import { LinkRendererComponent } from 'src/app/shared/components/ag-grid/link-renderer/link-renderer.component';
import { CustomRichTextType } from 'src/app/shared/models/enums/custom-rich-text-type.enum';
import { CustomDropdownFilterComponent } from 'src/app/shared/components/custom-dropdown-filter/custom-dropdown-filter.component';
import { UserDto } from 'src/app/shared/generated/model/user-dto';
import { ChemigationPermitDetailedDto } from 'src/app/shared/generated/model/chemigation-permit-detailed-dto';
import { UtilityFunctionsService } from 'src/app/services/utility-functions.service';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { AlertContext } from 'src/app/shared/models/enums/alert-context.enum';
import { Alert } from 'src/app/shared/models/alert';
import { AlertService } from 'src/app/shared/services/alert.service';
import { ChemigationPermitStatusEnum } from 'src/app/shared/models/enums/chemigation-permit-status.enum';

@Component({
  selector: 'zybach-chemigation-permit-list',
  templateUrl: './chemigation-permit-list.component.html',
  styleUrls: ['./chemigation-permit-list.component.scss']
})
export class ChemigationPermitListComponent implements OnInit, OnDestroy {
  @ViewChild('permitGrid') permitGrid: AgGridAngular;
  @ViewChild('createAnnualRenewalsModal') renewalEntity: any;

  
  private currentUser: UserDto;

  public richTextTypeID : number = CustomRichTextType.Chemigation;

  public columnDefs: any[];
  public defaultColDef: ColDef;
  public chemigationPermits: Array<ChemigationPermitDetailedDto>;
  public currentYear: number;
  public countOfActivePermitsWithoutRenewalRecordsForCurrentYear: number;

  public gridApi: any;

  public modalReference: NgbModalRef;
  public isPerformingAction: boolean = false;
  public closeResult: string;

  constructor(
    private alertService: AlertService,
    private authenticationService: AuthenticationService,
    private chemigationPermitService: ChemigationPermitService,
    private modalService: NgbModal,
    private utilityFunctionsService: UtilityFunctionsService,
    private datePipe: DatePipe,
    private decimalPipe: DecimalPipe,
    private cdr: ChangeDetectorRef
  ) { }

  ngOnInit(): void {
    this.authenticationService.getCurrentUser().subscribe(currentUser => {
      this.currentUser = currentUser;
      this.initializeGrid();

      this.currentYear = new Date().getFullYear();
      this.permitGrid?.api.showLoadingOverlay();
      this.updateGridData();
    });
  }
  
  private initializeGrid(): void {
    let datePipe = this.datePipe;
    let decimalPipe = this.decimalPipe;
    this.columnDefs = [
      {
        headerName: '#', valueGetter: function (params: any) {
          return { LinkValue: params.data.ChemigationPermitNumber, LinkDisplay: params.data.ChemigationPermitNumberDisplay };
        }, 
        cellRendererFramework: LinkRendererComponent,
        cellRendererParams: { inRouterLink: "/chemigation-permits/" },
        comparator: function (id1: any, id2: any) {
          let link1 = id1.LinkValue;
          let link2 = id2.LinkValue;
          if (link1 < link2) {
            return -1;
          }
          if (link1 > link2) {
            return 1;
          }
          return 0;
        },
        filterValueGetter: function (params: any) {
          return params.data.ChemigationPermitNumber;
        },
        filter: true,
        width: 80,
        resizable: true,
        sortable: true
      },
      { headerName: 'Permitholder', field: "LatestAnnualRecord.ApplicantName", filter: true, resizable: true, sortable: true },
      { 
        headerName: 'Status', field: 'ChemigationPermitStatus.ChemigationPermitStatusDisplayName',
        filterFramework: CustomDropdownFilterComponent,
        filterParams: {
          field: 'ChemigationPermitStatus.ChemigationPermitStatusDisplayName'
        },
        width: 80,
        resizable: true,
        sortable: true
      },
      { 
        headerName: 'Applicators', 
          valueGetter: function (params) {
            if(params.data.LatestAnnualRecord)
            {
            const applicators = params.data.LatestAnnualRecord.Applicators.map(x => x.ApplicatorName);
            if (applicators.length > 0) {
              return `${applicators.join('; ')}`;
            } else {
              return "-";
            }
          }
          else {
            return "-";
          }
        }, 
        filter: true, resizable: true, sortable: true 
      },
      { headerName: 'TRS', field: 'LatestAnnualRecord.TownshipRangeSection', filter: true, resizable: true, sortable: true },
      { 
        headerName: 'County', field: 'County.CountyDisplayName',
        filterFramework: CustomDropdownFilterComponent,
        filterParams: {
          field: 'County.CountyDisplayName'
        },
        width: 100,
        resizable: true,
        sortable: true
      },
      { headerName: 'Pivot', field: 'LatestAnnualRecord.PivotName', width: 100, filter: true, resizable: true, sortable: true },
      {
        headerName: 'Latest Inspection',
        children: [
          {
            headerName: 'Inspection Date', valueGetter: function (params: any) {
              if(params.data.LatestInspection && params.data.LatestInspection.InspectionDate)
              {
                return datePipe.transform(params.data.LatestInspection.InspectionDate, 'M/d/yyyy');
              }
              else
              {
                return "";
              }
            },
            comparator: this.dateSortComparer,
            filter: 'agDateColumnFilter',
            filterParams: {
              filterOptions: ['inRange'],
              comparator: this.dateFilterComparator
            }, 
            width: 140,
            resizable: true,
            sortable: true
          },          
          { headerName: 'Inspected By', field: "LatestInspection.Inspector.FullNameLastFirst", width: 130, filter: true, resizable: true, sortable: true },
          { headerName: 'Status', field: 'LatestInspection.ChemigationInspectionStatusName', 
            filterFramework: CustomDropdownFilterComponent,
            filterParams: {
              field: 'LatestInspection.ChemigationInspectionStatusName'
            },
            width: 100, resizable: true, sortable: true },
        ]
      },
      {
        headerName: 'Latest Annual Record',
        children: [
          { headerName: 'Year', field: 'LatestAnnualRecord.RecordYear', 
            filterFramework: CustomDropdownFilterComponent,
            filterParams: {
              field: 'LatestAnnualRecord.RecordYear'
            },
            width: 80, resizable: true, sortable: true 
          },
          {
            headerName: 'Received', valueGetter: function (params: any) {
              if(params.data.LatestAnnualRecord && params.data.LatestAnnualRecord.DateReceived)
              {
                return datePipe.transform(params.data.LatestAnnualRecord.DateReceived, 'M/d/yyyy');
              }
              else
              {
                return null;
              }
            },
            comparator: this.dateSortComparer,
            filter: 'agDateColumnFilter',
            filterParams: {
              filterOptions: ['inRange'],
              comparator: this.dateFilterComparator
            }, 
            width: 120,
            resizable: true,
            sortable: true
          },
          {
            headerName: 'Paid', valueGetter: function (params: any) {
              if(params.data.LatestAnnualRecord && params.data.LatestAnnualRecord.DatePaid)
              {
                return datePipe.transform(params.data.LatestAnnualRecord.DatePaid, 'M/d/yyyy');
              }
              else
              {
                return null;
              }
            },
            comparator: this.dateSortComparer,
            filter: 'agDateColumnFilter',
            filterParams: {
              filterOptions: ['inRange'],
              comparator: this.dateFilterComparator
            }, 
            width: 120,
            resizable: true,
            sortable: true
          },
          {
            headerName: 'Approved', valueGetter: function (params: any) {
              if(params.data.LatestAnnualRecord && params.data.LatestAnnualRecord.DateApproved)
              {
                return datePipe.transform(params.data.LatestAnnualRecord.DateApproved, 'M/d/yyyy');
              }
              else
              {
                return null;
              }
            },
            comparator: this.dateSortComparer,
            filter: 'agDateColumnFilter',
            filterParams: {
              filterOptions: ['inRange'],
              comparator: this.dateFilterComparator
            }, 
            width: 120,
            resizable: true,
            sortable: true
          },
          { 
            headerName: 'Status', field: 'LatestAnnualRecord.ChemigationPermitAnnualRecordStatusName', 
            filterFramework: CustomDropdownFilterComponent,
            filterParams: {
              field: 'LatestAnnualRecord.ChemigationPermitAnnualRecordStatusName'
            },
            width: 120, resizable: true, sortable: true 
          },
          { headerName: 'Notes', field: "LatestAnnualRecord.AnnualNotes", filter: true, resizable: true, sortable: true },
          { 
            headerName: 'Applied Chemicals', 
            valueGetter: function (params) {
              if(params.data.LatestAnnualRecord)
              {
                const applicators = params.data.LatestAnnualRecord.ChemicalFormulations.map(x => x.ChemicalFormulationName);
                if (applicators.length > 0) {
                  return `${applicators.join(', ')}`;
                } else {
                  return "-";
                }
              }
              else {
                return "-";
              }
            }, 
            filter: true, resizable: true, sortable: true 
          },
        ]
      },
      {
        headerName: "Nickname",
        field: "Well.WellNickname",
        width: 125,
        sortable: true, filter: true, resizable: true,        
      },
      this.createDateColumnDef(datePipe, 'Created', 'DateCreated', 'M/d/yyyy'),
      {
        headerName: 'Well', valueGetter: function (params: any) {
          if(params.data.Well)
          {
            return { LinkValue: params.data.Well.WellID, LinkDisplay: params.data.Well.WellRegistrationID };
          }
          else
          {
            return { LinkValue: null, LinkDisplay: null };
          }
        }, 
        cellRendererFramework: LinkRendererComponent,
        cellRendererParams: { inRouterLink: "/wells/" },
        comparator: function (id1: any, id2: any) {
          let link1 = id1.LinkValue;
          let link2 = id2.LinkValue;
          if (link1 < link2) {
            return -1;
          }
          if (link1 > link2) {
            return 1;
          }
          return 0;
        },
        filterValueGetter: function (params: any) {
          return params.data.Well?.WellRegistrationID;
        },
        filter: true,
        width: 100,
        resizable: true,
        sortable: true
      }
    ];

  }

  private dateSortComparer (id1: any, id2: any) {
      const date1 = id1 ? Date.parse(id1) : Date.parse("1/1/1900");
      const date2 = id2 ? Date.parse(id2) : Date.parse("1/1/1900");
      if (date1 < date2) {
        return -1;
      }
      return (date1 > date2)  ?  1 : 0;
  }
  
  private dateFilterComparator(filterLocalDateAtMidnight, cellValue) {
    if(cellValue === null) return -1;
    const cellDate = Date.parse(cellValue);
    if (cellDate == filterLocalDateAtMidnight) {
      return 0;
    }
    return (cellDate < filterLocalDateAtMidnight) ? -1 : 1;
  }

  private createDateColumnDef(datePipe: DatePipe, headerName: string, fieldName: string, dateFormat: string): ColDef {
    return {
      headerName: headerName, valueGetter: function (params: any) {
        return datePipe.transform(params.data[fieldName], dateFormat);
      },
      comparator: this.dateSortComparer,
      filter: 'agDateColumnFilter',
      filterParams: {
        filterOptions: ['inRange'],
        comparator: this.dateFilterComparator
      }, 
      width: 110,
      resizable: true,
      sortable: true
    };
  }
  
  public onFirstDataRendered(params): void {
    this.gridApi = params.api;
    this.gridApi.sizeColumnsToFit();
  }

  public updateGridData(): void {
    this.chemigationPermitService.getChemigationPermits().subscribe(chemigationPermits => {
      this.chemigationPermits = chemigationPermits;
      this.countOfActivePermitsWithoutRenewalRecordsForCurrentYear = this.chemigationPermits.filter(x => 
        x.ChemigationPermitStatus.ChemigationPermitStatusID == ChemigationPermitStatusEnum.Active &&
        x.LatestAnnualRecord?.RecordYear == this.currentYear - 1).length;
        this.permitGrid ? this.permitGrid.api.setRowData(chemigationPermits) : null;
      });
  }

  public exportToCsv() {
    this.utilityFunctionsService.exportGridToCsv(this.permitGrid, 'chemigation-permits.csv', null);
  }

  public launchModal(modalContent: any, modalTitle: string): void {
    this.modalReference = this.modalService.open(modalContent, { ariaLabelledBy: modalTitle, beforeDismiss: () => this.checkIfSubmitting(), backdrop: 'static', keyboard: false });
    this.modalReference.result.then((result) => {
      this.closeResult = `Closed with: ${result}`;
    }, (reason) => {
      this.closeResult = `Dismissed`;
    });
  }
  
  public checkIfSubmitting(): boolean {
    return !this.isPerformingAction;
  }

  public bulkCreateAnnualRenewals(): void {
    this.isPerformingAction = true;
    this.chemigationPermitService.bulkCreateChemigationPermitAnnualRecordsByRecordYear(this.currentYear).subscribe((response) => {
      this.modalReference.close();
      this.isPerformingAction = false;
      this.alertService.pushAlert(new Alert(`${response.ChemigationPermitsRenewed} Annual Renewal Applications successfully created, of which ${response.ChemigationInspectionsCreated} have a Chemigation Inspection due and auto-created for them`, AlertContext.Success, true));
      this.updateGridData();
    }, error => {
      this.modalReference.close();
      this.isPerformingAction = false;
      this.alertService.pushAlert(new Alert(`There was an error completing the renewal action. Please try again`, AlertContext.Danger, true));
    })
  }

  ngOnDestroy(): void {
    
    this.cdr.detach();
  }
}


