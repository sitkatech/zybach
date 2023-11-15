import { Component, OnInit, ChangeDetectorRef, OnDestroy, ViewChild } from '@angular/core';
import { UserService } from 'src/app/shared/generated/api/user.service';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { ColDef } from 'ag-grid-community';
import { LinkRendererComponent } from 'src/app/shared/components/ag-grid/link-renderer/link-renderer.component';
import { AgGridAngular } from 'ag-grid-angular';
import { UtilityFunctionsService } from 'src/app/services/utility-functions.service';
import { RoleEnum } from 'src/app/shared/generated/enum/role-enum';
import { UserDto } from 'src/app/shared/generated/model/user-dto';
import { CustomDropdownFilterComponent } from 'src/app/shared/components/custom-dropdown-filter/custom-dropdown-filter.component';

declare var $:any;

@Component({
  selector: 'zybach-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.scss']
})
export class UserListComponent implements OnInit, OnDestroy {
  @ViewChild('usersGrid') usersGrid: AgGridAngular;
  @ViewChild('unassignedUsersGrid') unassignedUsersGrid: AgGridAngular;

  private currentUser: UserDto;

  public users: UserDto[];
  public unassignedUsers: UserDto[];

  public columnDefs: ColDef[];
  public columnDefsUnassigned: ColDef[];
  public defaultColDef: ColDef;

  constructor(
    private cdr: ChangeDetectorRef, 
    private authenticationService: AuthenticationService, 
    private utilityFunctionsService: UtilityFunctionsService, 
    private userService: UserService
  ) { }

  ngOnInit() {
    this.authenticationService.getCurrentUser().subscribe(currentUser => {
      this.currentUser = currentUser;
      this.userService.usersGet().subscribe(users => {
        this.users = users;
        this.unassignedUsers = users.filter(u => u.Role.RoleID === RoleEnum.Unassigned);

        this.cdr.detectChanges();
      });

      this.columnDefs = [
        {
          headerName: 'Name',
          valueGetter: params => {
            return { LinkValue: params.data.UserID, LinkDisplay: params.data.FullName };
          }, 
          cellRendererFramework: LinkRendererComponent,
          cellRendererParams: { inRouterLink: "/users/" },
          filterValueGetter: params => params.data.FullName,
          comparator: this.utilityFunctionsService.linkRendererComparator,
        },
        { headerName: 'Email', field: 'Email' },
        { 
          headerName: 'Role', field: 'Role.RoleDisplayName',
          filterFramework: CustomDropdownFilterComponent,
          filterParams: { field: 'Role.RoleDisplayName' }
        },
        { 
          headerName: 'Receives System Communications?', field: 'ReceiveSupportEmails',
          valueGetter: params => this.utilityFunctionsService.booleanValueGetter(params.data.ReceiveSupportEmails),
          filterFramework: CustomDropdownFilterComponent,
          filterParams: { field: 'ReceiveSupportEmails' }
        },
        { 
          headerName: 'Performs Chemigation Inspections?', field: 'PerformsChemigationInspections',
          valueGetter: params => this.utilityFunctionsService.booleanValueGetter(params.data.PerformsChemigationInspections),
          filterFramework: CustomDropdownFilterComponent,
          filterParams: { field: 'PerformsChemigationInspections' }
        }
      ];
      
      this.defaultColDef = { sortable: true, filter: true, resizable: true };
    });
  }

  ngOnDestroy() {
    this.cdr.detach();
  }

  public onGridReady(params) {
    params.api.sizeColumnsToFit();
  }

  public exportToCsv() {
    this.utilityFunctionsService.exportGridToCsv(this.usersGrid, 'users.csv', null);
  }  
}
