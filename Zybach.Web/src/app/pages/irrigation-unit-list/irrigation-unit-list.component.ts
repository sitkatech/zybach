import { Component, OnInit, ViewChild } from '@angular/core';
import { AgGridAngular } from 'ag-grid-angular';
import { ColDef } from 'ag-grid-community';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { IrrigationUnitService } from 'src/app/shared/generated/api/irrigation-unit.service';
import { UtilityFunctionsService } from 'src/app/services/utility-functions.service';
import { LinkRendererComponent } from 'src/app/shared/components/ag-grid/link-renderer/link-renderer.component';
import { MultiLinkRendererComponent } from 'src/app/shared/components/ag-grid/multi-link-renderer/multi-link-renderer.component';
import { AgHubIrrigationUnitSimpleDto } from 'src/app/shared/generated/model/ag-hub-irrigation-unit-simple-dto';
import { UserDto } from 'src/app/shared/generated/model/user-dto';
import { CustomRichTextTypeEnum } from 'src/app/shared/generated/enum/custom-rich-text-type-enum';
import { AlertService } from 'src/app/shared/services/alert.service';
import { FieldDefinitionGridHeaderComponent } from 'src/app/shared/components/field-definition-grid-header/field-definition-grid-header.component';
import { FieldDefinitionTypeEnum } from 'src/app/shared/generated/enum/field-definition-type-enum';

@Component({
  selector: 'zybach-irrigation-unit-list',
  templateUrl: './irrigation-unit-list.component.html',
  styleUrls: ['./irrigation-unit-list.component.scss']
})
export class IrrigationUnitListComponent implements OnInit {
  @ViewChild('irrigationUnitGrid') irrigationUnitGrid: AgGridAngular;

  private currentUser: UserDto;

  public richTextTypeID : number = CustomRichTextTypeEnum.IrrigationUnitIndex;

  public columnDefs: any[];
  public defaultColDef: ColDef;
  public irrigationUnits: Array<AgHubIrrigationUnitSimpleDto>;

  public gridApi: any;

  constructor(
    private alertService: AlertService,
    private authenticationService: AuthenticationService,
    private irrigationUnitService: IrrigationUnitService,
    private utilityFunctionsService: UtilityFunctionsService
  ) { }

  ngOnInit(): void {
    this.authenticationService.getCurrentUser().subscribe(currentUser => {
      this.currentUser = currentUser;
      this.initializeGrid();
      this.irrigationUnitGrid?.api.showLoadingOverlay();

      this.irrigationUnitService.irrigationUnitsGet().subscribe(irrigationUnits => {
        this.irrigationUnits = irrigationUnits;
        this.irrigationUnitGrid?.api.hideOverlay();
      });
    });
  }

  private initializeGrid(): void {
    this.columnDefs = [
      {
        headerName: 'Irrigation Unit ID',
        valueGetter: function (params: any) {
          return { LinkValue: params.data.AgHubIrrigationUnitID, LinkDisplay: params.data.WellTPID };
        }, 
        cellRendererFramework: LinkRendererComponent,
        cellRendererParams: { inRouterLink: "/irrigation-units/" },
        comparator: this.utilityFunctionsService.linkRendererComparator,
        filterValueGetter: function (params: any) {
          return params.data.WellTPID;
        },
        headerComponentFramework: FieldDefinitionGridHeaderComponent, headerComponentParams: {fieldDefinitionTypeID: FieldDefinitionTypeEnum.IrrigationUnitID},
        filter: true,
        resizable: true,
        sortable: true
      },
      {
        headerName: 'Associated Well(s)',
        valueGetter: function (params) {
          let names = params.data.AssociatedWells?.map(x => {
            return { LinkValue: x.WellID, LinkDisplay: x.WellRegistrationID }
          });
          const downloadDisplay = names?.map(x => x.LinkDisplay).join(", ");

          return { links: names, DownloadDisplay: downloadDisplay ?? "" };
        },
        filterValueGetter: function (params: any) {
          let names = params.data.AssociatedWells?.map(x => {
            return { LinkValue: x.WellID, LinkDisplay: x.WellRegistrationID }
          });
          const downloadDisplay = names?.map(x => x.LinkDisplay).join(", ");

          return downloadDisplay ?? "";
        },
        comparator: this.utilityFunctionsService.linkRendererComparator, 
        resizable: true, sortable: true, filter: true,
        cellRendererParams: { inRouterLink: "/wells/" },
        cellRendererFramework: MultiLinkRendererComponent
      },
      {
        headerName: "Associated Well Count",
        valueGetter: function (params: any) {
          return params.data.AssociatedWells.length;
        },
        sortable: true, filter: 'agNumberColumnFilter', resizable: true
      },
      this.utilityFunctionsService.createDecimalColumnDef('Area (ac)', 'IrrigationUnitAreaInAcres', 130, 2, null, FieldDefinitionTypeEnum.IrrigationUnitAcres)
    ]
  }

  public onFirstDataRendered(params): void {
    this.gridApi = params.api;
    this.gridApi.sizeColumnsToFit();
  }

  public exportToCsv() {
    this.utilityFunctionsService.exportGridToCsv(this.irrigationUnitGrid, 'irrigation-units.csv', null);
  }

}