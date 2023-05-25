import { ChangeDetectorRef, Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { AgGridAngular } from 'ag-grid-angular';
import { ColDef } from 'ag-grid-community';
import { Subscription } from 'rxjs';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { ConfirmService } from 'src/app/services/confirm.service';
import { UtilityFunctionsService } from 'src/app/services/utility-functions.service';
import { FontAwesomeIconLinkRendererComponent } from 'src/app/shared/components/ag-grid/fontawesome-icon-link-renderer/fontawesome-icon-link-renderer.component';
import { LinkRendererComponent } from 'src/app/shared/components/ag-grid/link-renderer/link-renderer.component';
import { MultiLinkRendererComponent } from 'src/app/shared/components/ag-grid/multi-link-renderer/multi-link-renderer.component';
import { WellGroupService } from 'src/app/shared/generated/api/well-group.service';
import { CustomRichTextTypeEnum } from 'src/app/shared/generated/enum/custom-rich-text-type-enum';
import { UserDto } from 'src/app/shared/generated/model/user-dto';
import { WellGroupDto } from 'src/app/shared/generated/model/well-group-dto';
import { Alert } from 'src/app/shared/models/alert';
import { AlertContext } from 'src/app/shared/models/enums/alert-context.enum';
import { AlertService } from 'src/app/shared/services/alert.service';

@Component({
  selector: 'zybach-well-group-list',
  templateUrl: './well-group-list.component.html',
  styleUrls: ['./well-group-list.component.scss']
})
export class WellGroupListComponent implements OnInit, OnDestroy {
  @ViewChild('wellGroupsGrid') wellGroupsGrid: AgGridAngular;

  private currentUser: UserDto;
  private currentUserSubscription: Subscription;

  public wellGroups: WellGroupDto[];
  public columnDefs: ColDef[];
  public defaultColDef: ColDef;

  public richTextTypeID : number = CustomRichTextTypeEnum.WellGroupList;

  constructor(
    private authenticationService: AuthenticationService,
    private wellGroupService: WellGroupService,
    private utilityFunctionsService: UtilityFunctionsService,
    private confirmService: ConfirmService,
    private alertService: AlertService,
    private cdr: ChangeDetectorRef
  ) { }

  ngOnInit(): void {
    this.currentUserSubscription = this.authenticationService.getCurrentUser().subscribe(currentUser => {
      
      this.wellGroupService.wellGroupsGet().subscribe(wellGroups => {
        this.wellGroups = wellGroups;
        this.cdr.detectChanges();
      });

      this.createColumnDefs();
    });
  }

  ngOnDestroy(): void {
    this.currentUserSubscription.unsubscribe();
    this.cdr.detach();
  }

  public onGridReady() {
    this.wellGroupsGrid.api.sizeColumnsToFit();
  }

  public exportToCsv() {
    this.utilityFunctionsService.exportGridToCsv(this.wellGroupsGrid, 'well-groups.csv', null);
  }

  private createColumnDefs() {
    this.columnDefs = [
      {
        cellRendererFramework: FontAwesomeIconLinkRendererComponent,
        cellRendererParams: { isSpan: true, fontawesomeIconName: 'trash' },
        width: 30, sortable: false, filter: false, cellStyle: { textAlign: 'center' }
      },
      { 
        headerName: "Well Group Name", 
        valueGetter: params => {
          return { LinkValue: params.data.WellGroupID, LinkDisplay: params.data.WellGroupName}
        },
        cellRendererFramework: LinkRendererComponent,
        cellRendererParams: { inRouterLink: "/well-groups/" },
        comparator: this.utilityFunctionsService.linkRendererComparator,
        filterValueGetter: params => params.data.WellGroupName
      },
      {
        headerName: 'Wells', width: 300,
        valueGetter: function (params) {
          let names = params.data.WellGroupWells.map(x => {
            return { LinkValue: x.WellID, LinkDisplay: x.WellRegistrationID }
          });
          const downloadDisplay = names.map(x => x.LinkDisplay).join(", ");

          return { links: names, DownloadDisplay: downloadDisplay };
        },
        filterValueGetter: params => params.data.WellGroupWells.map(x => x.WellRegistrationID).join(", "),
        comparator: this.utilityFunctionsService.multiLinkRendererComparator, 
        cellRendererParams: { inRouterLink: "/wells/" },
        cellRendererFramework: MultiLinkRendererComponent
      },
      { 
        headerName: "Water Level Reporting Primary Well", 
        valueGetter: params => {
          return { LinkValue: params.data.PrimaryWell?.WellID, LinkDisplay: params.data.PrimaryWell?.WellRegistrationID }
        },
        cellRendererFramework: LinkRendererComponent,
        cellRendererParams: { inRouterLink: "/wells/" },
        comparator: this.utilityFunctionsService.linkRendererComparator,
        filterValueGetter: params => params.data.PrimaryWell?.WellRegistrationID
      }
    ];

    this.defaultColDef = { filter: true, sortable: true, resizable: true };
  }

  public onCellClicked(event: any): void {
    if (event.column.colId == 0) {
      this.deleteWellGroup(event.data.WellGroupID);
    }
  }

  private deleteWellGroup(wellGroupID: number) {
    const wellGroupIndex = this.wellGroups.findIndex(x => x.WellGroupID == wellGroupID);

    const modalMessage = `<p>You are about to delete well group <b>${this.wellGroups[wellGroupIndex].WellGroupName}</b>. Are you sure you wish to proceed?`;
    
    this.confirmService.confirm({ modalSize: "md", buttonClassYes: "btn-danger", buttonTextYes: "Delete", buttonTextNo: "Cancel", title: "Delete Well Group", message: modalMessage })
    .then(confirmed => {

      if (confirmed) {
        this.wellGroupService.wellGroupsWellGroupIDDelete(this.wellGroups[wellGroupIndex].WellGroupID).subscribe(() => {
          this.alertService.pushAlert(new Alert('Well Group successfully deleted', AlertContext.Success));

          this.wellGroups.splice(wellGroupIndex, 1);
          this.wellGroupsGrid.api.setRowData(this.wellGroups);
        });
      }
    });
  }

}
