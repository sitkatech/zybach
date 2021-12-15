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

  private watchUserChangeSubscription: any;
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
    this.watchUserChangeSubscription = this.authenticationService.currentUserSetObservable.subscribe(currentUser => {
      this.currentUser = currentUser;
      this.initializeGrid();

      this.currentYear = new Date().getFullYear();
      this.permitGrid.api.showLoadingOverlay();
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
        headerName: 'Well', valueGetter: function (params: any) {
          if(params.data.Well)
          {
            return { LinkValue: params.data.Well.WellRegistrationID, LinkDisplay: params.data.Well.WellRegistrationID };
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
      },
      this.createDateColumnDef('Created', 'DateCreated', 'M/d/yyyy'),
      { headerName: 'Township-Range-Section', field: 'TownshipRangeSection', filter: true, resizable: true, sortable: true },
      {
        headerName: 'Latest Annual Record',
        children: [
          
          { headerName: 'Year', field: 'LatestAnnualRecord.RecordYear', 
            filterFramework: CustomDropdownFilterComponent,
            filterParams: {
              field: 'LatestAnnualRecord.RecordYear'
            },
            width: 80, resizable: true, sortable: true },
          { headerName: 'Status', field: 'LatestAnnualRecord.ChemigationPermitAnnualRecordStatusName', 
            filterFramework: CustomDropdownFilterComponent,
            filterParams: {
              field: 'LatestAnnualRecord.ChemigationPermitAnnualRecordStatusName'
            },
            width: 140, resizable: true, sortable: true },
          { headerName: 'Pivot', field: 'LatestAnnualRecord.PivotName', width: 100, filter: true, resizable: true, sortable: true },
          {
            headerName: 'Received', valueGetter: function (params: any) {
              if(params.data.LatestAnnualRecord && params.data.LatestAnnualRecord.DateReceived)
              {
                return datePipe.transform(params.data.LatestAnnualRecord.DateReceived, 'M/d/yyyy');
              }
              else
              {
                return "";
              }
            },
            comparator: this.dateFilterComparator,
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
                return "";
              }
            },
            comparator: this.dateFilterComparator,
            filter: 'agDateColumnFilter',
            filterParams: {
              filterOptions: ['inRange'],
              comparator: this.dateFilterComparator
            }, 
            width: 120,
            resizable: true,
            sortable: true
          },
          { headerName: 'Applicant', field: "LatestAnnualRecord.ApplicantName", filter: true, resizable: true, sortable: true },
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
          { 
            headerName: 'Applicators', 
            valueGetter: function (params) {
              if(params.data.LatestAnnualRecord)
              {
              const applicators = params.data.LatestAnnualRecord.Applicators.map(x => x.ApplicatorName);
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
      }

    ];

  }

  private dateFilterComparator(filterLocalDateAtMidnight, cellValue) {
    const cellDate = Date.parse(cellValue);
    if (cellDate == filterLocalDateAtMidnight) {
      return 0;
    }
    return (cellDate < filterLocalDateAtMidnight) ? -1 : 1;
  }
  
  private createDateColumnDef(headerName: string, fieldName: string, dateFormat: string): ColDef {
    let datePipe = new DatePipe('en-US');
  
    return {
      headerName: headerName, valueGetter: function (params: any) {
        return datePipe.transform(params.data[fieldName], dateFormat);
      },
      comparator: this.dateFilterComparator,
      filter: 'agDateColumnFilter',
      filterParams: {
        filterOptions: ['inRange'],
        comparator: this.dateFilterComparator
      }, 
      width: 120,
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
    this.chemigationPermitService.bulkCreateChemigationPermitAnnualRecordsByRecordYear(this.currentYear).subscribe((responseCount) => {
      this.modalReference.close();
      this.isPerformingAction = false;
      this.alertService.pushAlert(new Alert(`${responseCount} Annual Renewal Applications successfully created`, AlertContext.Success, true));
      this.updateGridData();
    }, error => {
      this.modalReference.close();
      this.isPerformingAction = false;
      this.alertService.pushAlert(new Alert(`There was an error completing the renewal action. Please try again`, AlertContext.Danger, true));
    })
  }

  ngOnDestroy(): void {
    this.watchUserChangeSubscription.unsubscribe();
    this.authenticationService.dispose();
    this.cdr.detach();
  }
}


