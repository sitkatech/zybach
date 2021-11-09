import { ChangeDetectorRef, Component, OnInit, ViewChild } from '@angular/core';
import { AgGridAngular } from 'ag-grid-angular';
import { ColDef } from 'ag-grid-community';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { ChemigationPermitService } from 'src/app/services/chemigation-permit.service';
import { LinkRendererComponent } from 'src/app/shared/components/ag-grid/link-renderer/link-renderer.component';
import { UserDetailedDto } from 'src/app/shared/models';
import { CustomRichTextType } from 'src/app/shared/models/enums/custom-rich-text-type.enum';
import { ChemigationPermitDto } from 'src/app/shared/models/generated/chemigation-permit-dto';
import agGridDateFormatter from 'src/app/util/agGridDateFormatter';

@Component({
  selector: 'zybach-chemigation-permit-list',
  templateUrl: './chemigation-permit-list.component.html',
  styleUrls: ['./chemigation-permit-list.component.scss']
})
export class ChemigationPermitListComponent implements OnInit {
  @ViewChild('permitGrid') permitGrid: AgGridAngular;

  private watchUserChangeSubscription: any;
  private currentUser: UserDetailedDto;

  public richTextTypeID : number = CustomRichTextType.Chemigation;

  public columnDefs: ColDef[];
  public chemigationPermits: Array<ChemigationPermitDto>;
  public users: UserDetailedDto[];
  public unassignedUsers: UserDetailedDto[];

  public gridApi: any;

  constructor(
    private authenticationService: AuthenticationService,
    private chemigationPermitService: ChemigationPermitService,
    private cdr: ChangeDetectorRef
  ) { }

  ngOnInit(): void {
    this.watchUserChangeSubscription = this.authenticationService.currentUserSetObservable.subscribe(currentUser => {
      this.currentUser = currentUser;
      this.permitGrid.api.showLoadingOverlay();
      this.initializeGrid();
      this.chemigationPermitService.getAllChemigationPermits().subscribe(chemigationPermits => {
        this.chemigationPermits = chemigationPermits;
        this.permitGrid.api.hideOverlay();
        this.cdr.detectChanges();
      });
      this.permitGrid.api.hideOverlay();
    });
  }
  
  private initializeGrid(): void {
    this.columnDefs = [
      {
        headerName: 'Permit Number', valueGetter: function (params: any) {
          return { LinkValue: params.data.ChemigationPermitNumber, LinkDisplay: params.data.ChemigationPermitNumber };
        },
        cellRendererFramework: LinkRendererComponent,
        cellRendererParams: { inRouterLink: "/chemigation-permits/" },
        comparator: function (id1: any, id2: any) {
          let link1 = id1.LinkDisplay;
          let link2 = id2.LinkDisplay;
          if (link1 < link2) {
            return -1;
          }
          if (link1 > link2) {
            return 1;
          }
          return 0;
        },
        filter: true,
        filterValueGetter: function (params: any) {
          return params.data.ChemigationPermitNumber;
        }
      },
      { headerName: 'Status', field: 'ChemigationPermitStatus.ChemigationPermitStatusDisplayName' },
      { headerName: 'Date Received', field: 'DateReceived', valueFormatter: agGridDateFormatter },
      { headerName: 'Township-Range-Section', field: 'TownshipRangeSection' }
    ];

    this.columnDefs.forEach(x => {
      x.filter = true;
      x.resizable = true;
      x.sortable = true;
    });
  }

  public onFirstDataRendered(params): void {
    this.gridApi = params.api;
    this.gridApi.sizeColumnsToFit();
    this.updateGridData();
  }

  public updateGridData(): void {
    this.chemigationPermitService.getAllChemigationPermits().subscribe(chemigationPermits => {
      this.chemigationPermits = chemigationPermits;
      this.cdr.detectChanges();
    });
    this.permitGrid.api.hideOverlay();
  }

  ngOnDestroy(): void {
    this.watchUserChangeSubscription.unsubscribe();
    this.authenticationService.dispose();
    this.cdr.detach();
  }
}


