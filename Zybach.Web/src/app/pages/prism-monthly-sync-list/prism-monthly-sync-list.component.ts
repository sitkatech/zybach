import { ChangeDetectorRef, Component, ViewChild } from '@angular/core';
import { NgbModalRef, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AgGridAngular } from 'ag-grid-angular';
import { ColDef } from 'ag-grid-community';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { ConfirmService } from 'src/app/services/confirm.service';
import { UtilityFunctionsService } from 'src/app/services/utility-functions.service';
import { ContextMenuRendererComponent } from 'src/app/shared/components/ag-grid/context-menu/context-menu-renderer.component';
import { PrismSyncService } from 'src/app/shared/generated/api/prism-sync.service';
import { CustomRichTextTypeEnum } from 'src/app/shared/generated/enum/custom-rich-text-type-enum';
import { PrismMonthlySyncSimpleDto, UserDto } from 'src/app/shared/generated/model/models';
import { Alert } from 'src/app/shared/models/alert';
import { AlertContext } from 'src/app/shared/models/enums/alert-context.enum';
import { AlertService } from 'src/app/shared/services/alert.service';
import { saveAs } from 'file-saver';
import { PrismSyncStatusEnum } from 'src/app/shared/generated/enum/prism-sync-status-enum';
import { DatePipe } from '@angular/common';
import { RunoffCalculationStatusEnum } from 'src/app/shared/generated/enum/runoff-calculation-status-enum';

@Component({
    selector: 'prism-monthly-sync-list',
    templateUrl: './prism-monthly-sync-list.component.html',
    styleUrls: ['./prism-monthly-sync-list.component.scss']
})
export class PrismMonthlySyncListComponent {

    @ViewChild('prismMonthlySyncGrid') prismMonthlySyncGrid: AgGridAngular;
  
    public currentUser: UserDto;
    public richTextTypeID: number = CustomRichTextTypeEnum.OpenETIntegration;
    public modalReference: NgbModalRef;
  
    public prismMonthlySyncs: Array<PrismMonthlySyncSimpleDto>;

    public loadingPage: boolean = true;
    public isPerformingAction: boolean = false;
  
    public columnDefs: ColDef[];
    public defaultColDef: ColDef;

    public allowedDataTypes = ["ppt", "tmin", "tmax"];
    public selectedDataType = "ppt";

    public currentYear: number = new Date().getFullYear();
    public startingYear: number = 2020;
    public allowedYears: number[]; 
    public selectedYear: number = this.currentYear;

    public currentMonth: number = new Date().getMonth() + 1;

    public showDownloadButtons: boolean = false;
  
    constructor(
      private authenticationService: AuthenticationService,
      private prismSyncService: PrismSyncService,
      private alertService: AlertService,
      private utilityFunctionsService: UtilityFunctionsService,
      private confirmService: ConfirmService, 
      private datePipe: DatePipe,
      private cdr: ChangeDetectorRef
    ) { }
  
    ngOnInit() {
        this.loadingPage = true;
        this.authenticationService.getCurrentUser().subscribe(currentUser => {
            this.currentUser = currentUser;
            this.refreshData();
        });

        this.allowedYears = Array.from({length: this.currentYear - this.startingYear + 1}, (v, k) => k + this.startingYear);
    }

    refreshData(){
        this.loadingPage = true;
        this.prismSyncService.prismMonthlySyncYearsYearDataTypesPrismDataTypeNameGet(this.selectedYear, this.selectedDataType).subscribe(data => {
            this.initializeGrid();
            this.prismMonthlySyncs = data.filter(x => {
                if(this.selectedYear == this.currentYear){
                    return x.Month <= this.currentMonth;
                }   
                return true;
            });

            this.loadingPage = false;   
            this.showDownloadButtons = this.selectedDataType == "ppt" && this.prismMonthlySyncs.filter(x =>{
                return x.RunoffCalculationStatusID == RunoffCalculationStatusEnum.Succeeded;
            }).length > 0;
            this.cdr.markForCheck();
        });
    }

    initializeGrid() {
        this.columnDefs = [];

        this.columnDefs.push(this.createActionColumn());

        let datePipe = this.datePipe;
        
        this.columnDefs.push(
            {
                headerName: 'Year', 
                field: 'Year',
                filter: 'agNumberColumnFilter',
                width: 100
            },
            {
                headerName: 'Month',
                valueGetter: (params) => this.utilityFunctionsService.getMonthNameByMonthNumber(params.data.Month),
                width: 100
            },
            {
                headerName: 'PRISM Sync Status',
                field: 'PrismSyncStatusDisplayName',
                width: 300
            },
            {
                headerName: "Last Synchronized On", 
                valueGetter: function (params: any) {
                    return datePipe.transform(params.data.LastSynchronizedDate, "M/d/yyyy");
                },
                comparator: function (id1: any, id2: any) {
                    const date1 = Date.parse(id1);
                    const date2 = Date.parse(id2);
                    if (date1 < date2) {
                        return -1;
                    }
                    return (date1 > date2)  ?  1 : 0;
                },
                filterValueGetter: function (params: any) {
                    return datePipe.transform(params.data.LastSynchronizedDate, "M/d/yyyy");
                },
                filter: 'agDateColumnFilter',
                filterParams: {
                    filterOptions: ['inRange'],
                    comparator: this.dateFilterComparator
                }, 
                width: 175,
                resizable: true,
                sortable: true
            },
            {
                headerName: 'Last Synchronized By',
                field: 'LastSynchronizedByUserFullName',
                width: 300
            },
            {
                headerName: 'Runoff Calculation Status',
                field: 'RunoffCalculationStatusDisplayName',
                width: 300
            },
            {
                headerName: "Last Calculated On", 
                valueGetter: function (params: any) {
                    return datePipe.transform(params.data.LastRunoffCalculationDate, "M/d/yyyy");
                },
                comparator: function (id1: any, id2: any) {
                    const date1 = Date.parse(id1);
                    const date2 = Date.parse(id2);
                    if (date1 < date2) {
                        return -1;
                    }
                    return (date1 > date2)  ?  1 : 0;
                },
                filterValueGetter: function (params: any) {
                    return datePipe.transform(params.data.LastRunoffCalculationDate, "M/d/yyyy");
                },
                filter: 'agDateColumnFilter',
                filterParams: {
                    filterOptions: ['inRange'],
                    comparator: this.dateFilterComparator
                }, 
                width: 175,
                resizable: true,
                sortable: true
            },
            {
                headerName: 'Runoff Calculated By',
                field: 'LastRunoffCalculatedByUserFullName',
                width: 300
            },
            {
                headerName: "Finalized On", 
                valueGetter: function (params: any) {
                    return datePipe.transform(params.data.FinalizeDate, "M/d/yyyy");
                },
                comparator: function (id1: any, id2: any) {
                    const date1 = Date.parse(id1);
                    const date2 = Date.parse(id2);
                    if (date1 < date2) {
                        return -1;
                    }
                    return (date1 > date2)  ?  1 : 0;
                },
                filterValueGetter: function (params: any) {
                    return datePipe.transform(params.data.FinalizeDate, "M/d/yyyy");
                },
                filter: 'agDateColumnFilter',
                filterParams: {
                    filterOptions: ['inRange'],
                    comparator: this.dateFilterComparator
                }, 
                width: 175,
                resizable: true,
                sortable: true
            },
            {
                headerName: 'Finalized By',
                field: 'FinalizedByUserFullName',
                width: 300
            }
        );
  
        this.defaultColDef = { filter: true, sortable: true, resizable: true };
    }

    public getRunoffData(outputFormat: string) {
        
        this.alertService.pushAlert(new Alert(`Downloading data.`, AlertContext.Info));
        this.prismSyncService.runoffDataYearsYearGet(this.selectedYear).subscribe(result => {
            // If the output format is csv, we need to convert the result to a csv file and download it.
            if (outputFormat === 'csv') {
                const csvData = this.convertToCSV(result);
                this.downloadCSV(csvData, `runoff_data_${this.selectedYear}.csv`);
                this.alertService.clearAlerts();
                return;
            }

            this.downloadJSON(result, `runoff_data_${this.selectedYear}.json`);
            this.alertService.clearAlerts();
        });
    }

    private convertToCSV(data: any[]): string {
        const array = [Object.keys(data[0])].concat(data);

        return array.map(it => {
            return Object.values(it).toString();
        }).join('\n');
    }

    private downloadCSV(csvData: string, filename: string) {
        const blob = new Blob([csvData], { type: 'text/csv' });
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.setAttribute('href', url);
        a.setAttribute('download', filename);
        a.click();
        window.URL.revokeObjectURL(url);
    }
    
    private downloadJSON(jsonData: any, filename: string) {
        const blob = new Blob([JSON.stringify(jsonData, null, 2)], { type: 'application/json' });
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.setAttribute('href', url);
        a.setAttribute('download', filename);
        a.click();
        window.URL.revokeObjectURL(url);
    }
    
    public createActionColumn(): ColDef {
        var actionColDef: ColDef = {
            headerName: "Actions",
            valueGetter: (params: any) => {
                let actions = [];
                if(params.data.FinalizeDate == null) {
                    actions.push({ 
                            ActionName: "Sync",  
                            ActionHandler: () => {
                                this.confirmService.confirm({
                                    title: 'Sync Now',
                                    message: `Are you sure you want sync ${this.utilityFunctionsService.getMonthNameByMonthNumber(params.data.Month)} ${ params.data.Year }'s ${ params.data.PrismDataTypeDisplayName } data from PRISM? Note: This may take some time to return data, please check in again in a few minutes.`,
                                    buttonTextYes: 'Sync',
                                    buttonClassYes: 'btn-primary',
                                    buttonTextNo: 'Cancel',
                                    modalSize: 'small'
                                }).then(confirmed => {
                                    if (confirmed) {
                                        this.isPerformingAction = true;
                                        this.prismSyncService.prismMonthlySyncYearsYearMonthsMonthDataTypesPrismDataTypeNameSyncPut(params.data.Year, params.data.Month, this.selectedDataType).subscribe(() => {  
                                            this.refreshData();
                                            this.alertService.pushAlert(new Alert(`${this.utilityFunctionsService.getMonthNameByMonthNumber(params.data.Month)} ${ params.data.Year }'s ${ params.data.PrismDataTypeDisplayName } data is being synchronized, please check in again in a few minutes.`, AlertContext.Success));
                                        });
                                    }
                                });
                            }
                        });

                    if(params.data.PrismSyncStatusID == PrismSyncStatusEnum.Succeeded){
                        actions.push({ 
                                ActionName: "Download Zip", 
                                ActionHandler: () => {
                                    this.alertService.pushAlert(new Alert(`Downloading data.`, AlertContext.Info));
                                    this.prismSyncService.prismMonthlySyncYearsYearMonthsMonthDataTypesPrismDataTypeNameDownloadGet(params.data.Year, params.data.Month, this.selectedDataType).subscribe((blob: Blob) => {
                                        this.alertService.clearAlerts();
                                        saveAs(blob, `${params.data.Year}-${params.data.Month}-${params.data.PrismDataTypeName}-prism-data.zip`);
                                    }, error => {
                                        this.alertService.pushAlert(new Alert(`An error occurred while downloading the zip file.`, AlertContext.Danger));
                                    });
                                }
                            });
                            
                        actions.push({ 
                                ActionName: "Calculate Runoff",  
                                ActionHandler: () => {
                                    this.confirmService.confirm({
                                        title: 'Calculate Runoff',
                                        message: `Are you sure you calculate runoff for${this.utilityFunctionsService.getMonthNameByMonthNumber(params.data.Month)} ${ params.data.Year }? Note: This may take some time to return data, please check in again in a few minutes.`,
                                        buttonTextYes: 'Sync',
                                        buttonClassYes: 'btn-primary',
                                        buttonTextNo: 'Cancel',
                                        modalSize: 'small'
                                    }).then(confirmed => {
                                        if (confirmed) {
                                            this.isPerformingAction = true;
                                            this.prismSyncService.prismMonthlySyncYearsYearMonthsMonthCalculateRunoffPut(params.data.Year, params.data.Month).subscribe(() => {  
                                                this.refreshData();
                                                this.alertService.pushAlert(new Alert(`${this.utilityFunctionsService.getMonthNameByMonthNumber(params.data.Month)} ${ params.data.Year }'s runoff data is being calculated, please check in again in a few minutes.`, AlertContext.Success));
                                            });
                                        }
                                    });
                                }
                            });
                    }
                
                    
                    if(params.data.PrismSyncStatusID == PrismSyncStatusEnum.Succeeded && params.data.RunoffCalculationStatusID == RunoffCalculationStatusEnum.Succeeded) {
                        actions.push({ 
                                ActionName: "Finalize", 
                                ActionHandler: () => {
                                    this.confirmService.confirm({
                                        title: 'Finalize',
                                        message: `Are you sure you would like to finalize ${this.utilityFunctionsService.getMonthNameByMonthNumber(params.data.Month)} ${params.data.Year}? You will not be able to resync after this action. Please reach out to support if you run into issues.`,
                                        buttonTextYes: 'Finalize',
                                        buttonClassYes: 'btn-primary',
                                        buttonTextNo: 'Cancel',
                                        modalSize: 'small'
                                    }).then(confirmed => {
                                        if (confirmed) {
                                            this.isPerformingAction = true;
                                            this.prismSyncService.prismMonthlySyncYearsYearMonthsMonthDataTypesPrismDataTypeNameFinalizePut(params.data.Year, params.data.Month, this.selectedDataType).subscribe((result) => {
                                                this.refreshData();
                                                this.alertService.pushAlert(new Alert(`${this.utilityFunctionsService.getMonthNameByMonthNumber(params.data.Month)} ${ params.data.Year }'s ${ params.data.PrismDataTypeDisplayName } data was succesfully finalized.`, AlertContext.Success));
                                            });
                                        }
                                    });
                                }
                            });
                    }
                }

                return actions; 
            },
            cellRenderer: ContextMenuRendererComponent,
            cellClass: "context-menu-container",
            sortable: false, filter: false, width: 100 
        }
  
        return actionColDef;
    }


    private dateFilterComparator(filterLocalDate, cellValue) {
        const cellDate = Date.parse(cellValue);
        const filterLocalDateAtMidnight = filterLocalDate.getTime();
        if (cellDate == filterLocalDateAtMidnight) {
          return 0;
        }

        return (cellDate < filterLocalDateAtMidnight) ? -1 : 1;
    }
        
    public exportToCsv() {
        this.utilityFunctionsService.exportGridToCsv(this.prismMonthlySyncGrid, 'prism-monthly-syncs.csv', null);
    }
  
    public isCurrentUserAdministrator(): boolean {
        return this.authenticationService.isCurrentUserAnAdministrator();
    }

    public selectedYearChanged(event){
        this.selectedYear = event;
        this.refreshData();
    }

    public selectedDataTypeChanged(event){
        this.selectedDataType = event;
        this.refreshData();
    }
}
