import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { AgGridAngular } from 'ag-grid-angular';
import { ColDef } from 'ag-grid-community';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { ChemigationInspectionService } from 'src/app/services/chemigation-inspection.service';
import { UserDetailedDto } from 'src/app/shared/models';

@Component({
  selector: 'zybach-chemigation-landing',
  templateUrl: './chemigation-landing.component.html',
  styleUrls: ['./chemigation-landing.component.scss']
})
export class ChemigationLandingComponent implements OnInit, OnDestroy {

  @ViewChild('inspectionsGrid') inspectionsGrid: AgGridAngular;

  private watchUserChangeSubscription: any;
  private chemigationInspectionSubscription: any;

  private currentUser: UserDetailedDto;

  public inpsectionSummaries = [];
  columnDefs: ColDef[];
  users: UserDetailedDto[];
  unassignedUsers: UserDetailedDto[];
  constructor(
    private authenticationService: AuthenticationService,
    private chemigationInspectionService: ChemigationInspectionService
  ) { }

  ngOnInit(): void {
    this.columnDefs = [
      { headerName: 'Well Number', field: 'wellRegistrationID', sortable: true, filter: true, resizable: true },
      { headerName: 'Last Chemigation Date', field: 'lastChemigationDate', sortable: true, filter: true, resizable: true },
      { headerName: 'Last Water Quality Date', field: 'lastWaterQualityDate', sortable: true, filter: true, resizable: true },
      { headerName: 'Last Nitrates Date', field: 'lastNitratesDate', sortable: true, filter: true, resizable: true },
      { headerName: 'Last Water Level Date', field: 'lastWaterLevelDate', sortable: true, filter: true, resizable: true },
      { headerName: '# Pending', field: 'pendingInspectionCount', sortable: true, filter: true, resizable: true }
    ];

    this.watchUserChangeSubscription = this.authenticationService.currentUserSetObservable.subscribe(currentUser => {
      this.chemigationInspectionSubscription = this.chemigationInspectionService.getInspectionSummaries().subscribe(inspectionSummaries => {
        this.inpsectionSummaries = inspectionSummaries;
        this.inspectionsGrid.api.hideOverlay();
      })
    })
  }

  ngOnDestroy(): void {
    this.watchUserChangeSubscription.unsubscribe();
    this.chemigationInspectionSubscription.unsubscribe();
  }
}
