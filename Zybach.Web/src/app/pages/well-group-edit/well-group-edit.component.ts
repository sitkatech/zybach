import { ChangeDetectorRef, Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { AgGridAngular } from 'ag-grid-angular';
import { ColDef } from 'ag-grid-community';
import { forkJoin, Subscription } from 'rxjs';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { UtilityFunctionsService } from 'src/app/services/utility-functions.service';
import { CustomDropdownFilterComponent } from 'src/app/shared/components/custom-dropdown-filter/custom-dropdown-filter.component';
import { FieldDefinitionGridHeaderComponent } from 'src/app/shared/components/field-definition-grid-header/field-definition-grid-header.component';
import { WellGroupService } from 'src/app/shared/generated/api/well-group.service';
import { WellService } from 'src/app/shared/generated/api/well.service';
import { CustomRichTextTypeEnum } from 'src/app/shared/generated/enum/custom-rich-text-type-enum';
import { FieldDefinitionTypeEnum } from 'src/app/shared/generated/enum/field-definition-type-enum';
import { UserDto } from 'src/app/shared/generated/model/user-dto';
import { WellGroupDto } from 'src/app/shared/generated/model/well-group-dto';
import { WellGroupWellSimpleDto } from 'src/app/shared/generated/model/well-group-well-simple-dto';
import { WellMinimalDto } from 'src/app/shared/generated/model/well-minimal-dto';
import { Alert } from 'src/app/shared/models/alert';
import { AlertContext } from 'src/app/shared/models/enums/alert-context.enum';
import { AlertService } from 'src/app/shared/services/alert.service';

@Component({
  selector: 'zybach-well-group-edit',
  templateUrl: './well-group-edit.component.html',
  styleUrls: ['./well-group-edit.component.scss']
})
export class WellGroupEditComponent implements OnInit, OnDestroy {
  @ViewChild('selectWellsGrid') selectWellsGrid: AgGridAngular;
  @ViewChild('primaryWellModal') primaryWellModal: NgbModal;

  private currentUser: UserDto;
  private currentUserSubscription: Subscription;

  public wells: WellMinimalDto[];
  public model: WellGroupDto;
  public primaryWellID: number;
  public hasPrimaryWell = false;

  public columnDefs: ColDef[];
  public defaultColDef: ColDef;

  public isCreating: boolean;
  public isLoadingSubmit = false;
  public modalRef: NgbModalRef;
  public richTextTypeID : number = CustomRichTextTypeEnum.WellGroupEdit;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private authenticationService: AuthenticationService,
    private wellGroupService: WellGroupService,
    private wellService: WellService,
    private utilityFunctionsService: UtilityFunctionsService,
    private cdr: ChangeDetectorRef,
    private alertService: AlertService,
    private modalService: NgbModal
  ) { }

  ngOnInit(): void {
    this.currentUserSubscription = this.authenticationService.getCurrentUser().subscribe(currentUser => {
      this.currentUser = currentUser;

      const wellGroupID = parseInt(this.route.snapshot.paramMap.get("id"));
      if (wellGroupID) {
        this.isCreating = false;

        forkJoin({
          wellGroup: this.wellGroupService.wellGroupsWellGroupIDGet(wellGroupID),
          wells: this.wellService.wellsGet()
        }).subscribe(({wellGroup, wells}) => {
          this.model = wellGroup;

          this.primaryWellID = this.model.WellGroupWells.find(x => x.IsPrimary)?.WellID;
          this.hasPrimaryWell = this.primaryWellID ? true : false;

          this.wells = wells;
          this.cdr.detectChanges();
        });

      } else {
        this.isCreating = true;
        this.model = new WellGroupDto();
        this.model.WellGroupWells = new Array<WellGroupWellSimpleDto>();

        this.wellService.wellsGet().subscribe(wells => {
          this.wells = wells;
          this.cdr.detectChanges();
        });
      }

      this.createColumnDefs();
    });
  }

  ngOnDestroy(): void {
    this.currentUserSubscription.unsubscribe();
    this.cdr.detach();
  }

  private createColumnDefs() {
    this.columnDefs = [
      { checkboxSelection: true, filter: false, sortable: false, width: 40 },
      { headerName: "Well Registration #", field: "WellRegistrationID" },
      { headerName: "Well Nickname", field: "WellNickname" },
      { headerName: "Sensors", valueGetter: params => params.data.Sensors.map(x => x.SensorName).join(", ") },
      {
        headerName: "Owner Name", field: "OwnerName",
        headerComponentFramework: FieldDefinitionGridHeaderComponent, 
        headerComponentParams: {fieldDefinitionTypeID: FieldDefinitionTypeEnum.WellOwnerName },
      },
      { headerName: "Legal", field: "TownshipRangeSection" },
      {
        headerName: "Water Quality?",
        valueGetter: params => this.utilityFunctionsService.booleanValueGetter(params.data.HasWaterQualityInspections),
        filterFramework: CustomDropdownFilterComponent,
        filterParams: { field: 'HasWaterQualityInspections' },
        headerComponentFramework: FieldDefinitionGridHeaderComponent, 
        headerComponentParams: { 
          fieldDefinitionTypeID: FieldDefinitionTypeEnum.WellWaterQualityInspectionParticipation, 
          labelOverride: 'Has Water Quality Inspections?' 
        },
      },
      {
        headerName: "Water Level?",
        valueGetter: params => this.utilityFunctionsService.booleanValueGetter(params.data.HasWaterLevelInspections),
        filterFramework: CustomDropdownFilterComponent,
        filterParams: { field: 'HasWaterLevelInspections' },
        headerComponentFramework: FieldDefinitionGridHeaderComponent, 
        headerComponentParams: { fieldDefinitionTypeID: FieldDefinitionTypeEnum.WellWaterLevelInspectionParticipation },
      },
      { headerName: "Notes", field: "Notes", width: 300 }
    ];

    this.defaultColDef = { filter: true, sortable: true, resizable: true };
  }

  public onGridReady() {
    if (!this.isCreating) {
      this.selectWellsGrid.api.forEachNode(node => {
        if (this.model.WellGroupWells.find(x => x.WellID == node.data.WellID)) {
          node.setSelected(true);
        }
      });
    }
  }

  public exportToCsv() {
    this.utilityFunctionsService.exportGridToCsv(this.selectWellsGrid, 'well-groups.csv', null);
  }

  public onSelectionChanged(): void {
    this.model.WellGroupWells = this.selectWellsGrid.api.getSelectedRows().map(x => new WellGroupWellSimpleDto({ 
      WellID: x.WellID, 
      WellRegistrationID: x.WellRegistrationID
    }));
  }

  private validateWellGroup() {
    var isValid = true;
    if (!this.model.WellGroupName) {
      this.alertService.pushAlert(new Alert("The Well Group Name field is required.", AlertContext.Danger));
      isValid = false;
    }
    if (this.model.WellGroupWells.length == 0) {
      this.alertService.pushAlert(new Alert("Please select at least one well to create a well group.", AlertContext.Danger));
      isValid = false;
    }

    return isValid;
  }

  public onSubmit(): void {
    if (!this.validateWellGroup()) return;

    if (!this.hasPrimaryWell) {
      this.saveWellGroup();
      return;
    }

    this.modalRef = this.modalService.open(this.primaryWellModal, { ariaLabelledBy: 'primaryWellModalTitle', backdrop: 'static', keyboard: false });
  }

  public saveWellGroup() {
    this.alertService.clearAlerts();
    this.isLoadingSubmit = true;

    if (this.hasPrimaryWell) {
      this.model.WellGroupWells.find(x => x.WellID == this.primaryWellID).IsPrimary = true;
    }

    if (this.isCreating) {
      this.wellGroupService.wellGroupsPost(this.model).subscribe(
        wellGroupID => this.onRequestSuccess(`well-groups/${wellGroupID}`), error => this.onRequestError());
    } else {
      this.wellGroupService.wellGroupsWellGroupIDPut(this.model.WellGroupID, this.model).subscribe(
        () => this.onRequestSuccess(`well-groups/${this.model.WellGroupID}`), error => this.onRequestError());
    }
  }

  private onRequestSuccess(redirectUrl: string) {
    this.isLoadingSubmit = false;
    this.modalRef.close();

    this.router.navigateByUrl(redirectUrl).then(() => {
      this.alertService.pushAlert(new Alert(`Well group successfully ${this.isCreating ? 'created' : 'updated'}`, AlertContext.Success));
    });
  }

  private onRequestError() {
    this.isLoadingSubmit = false;
    this.modalRef.close();
    this.cdr.detectChanges();
  }
}

