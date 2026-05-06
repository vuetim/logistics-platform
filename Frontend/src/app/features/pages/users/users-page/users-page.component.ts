import { Component, OnInit } from '@angular/core';
import { KeyValuePipe, NgFor, NgIf } from '@angular/common';

import { UsersService } from '../../../../data-access/users/users.service';
import { UserDto } from '../../../../core/auth/dto/Responses/user.dto';
import { PageLayoutComponent } from '../../../../layout/app-shell/page-layout/page-layout/page-layout.component';
import { EditUserModalComponent } from '../components/edit-user-modal/edit-user-modal.component';
import { Permission } from '../../../../core/auth/permissions/permission.enum';
import { EditUserResultDto } from '../components/edit-user-modal/edit-user-result.dto';
import { AuthFacade } from '../../../../core/auth/auth.facade';
import { ToastrService } from 'ngx-toastr';
import { EntityTableComponent } from '../../../../shared/UI/entity-table/entity-table.component';
import { PaginationComponent } from '../../../../shared/UI/pagination/pagination.component';
import { UiCardComponent } from '../../../../shared/UI/ui-card/ui-card.component';
import { UserListItem } from '../../../../core/models/users/user-list-item.model';

import { UiButtonComponent } from '../../../../shared/UI/ui-button/ui-button.component';
import { TableAction } from '../../../../shared/UI/entity-table/entity-table.models';
import { CreateUserModalComponent } from '../components/create-user-modal/create-user-modal.component';
import { USER_FILTERS } from '../filters/users.filters';
import { FilterBuilderComponent } from '../../../../shared/filters/filter-builder/filter-builder.component';
import { GenericListPage } from '../../../../shared/list/generic-list-page';
import { UsersQueryParameters } from '../../../../core/models/users/users-query-parameters.model';
import { of } from 'rxjs';
import { USER_STATUS_MAP } from '../../../../shared/status/user-status.map';

@Component({
  selector: 'app-users-page',
  standalone: true,
  imports: [
    KeyValuePipe,
    NgFor,
    NgIf,
    PageLayoutComponent,
    EditUserModalComponent,
    EntityTableComponent,
    UiCardComponent,
    PaginationComponent,
    UiButtonComponent,
    CreateUserModalComponent,
    FilterBuilderComponent
  ],
  templateUrl: './users-page.component.html',
  styleUrl: './users-page.component.css'
})
export class UsersPageComponent
  extends GenericListPage<UsersQueryParameters>
  implements OnInit {

  users: UserListItem[] = [];
  selectedUser?: UserDto;

  canEditUser = false;
  canDeleteUser = false;
  creatingUser = false;
  filtersConfig = USER_FILTERS;

  columns = [
    { key: 'fullName', label: 'Name', sortable: true },
    { key: 'email', label: 'Email', sortable: true },
    {
      key: 'roles',
      label: 'Role',
      formatter: (u: UserListItem) => u.roles.join(', ')
    },
    {
      key: 'isActive',
      label: 'Status',
      formatter: (u: UserListItem) => {
        const key: 'true' | 'false' = u.isActive ? 'true' : 'false';
        return USER_STATUS_MAP[key].label;
      },
      classFn: (u: UserListItem) => {
        const key: 'true' | 'false' = u.isActive ? 'true' : 'false';
        return USER_STATUS_MAP[key].class;
      }
    }
  ];


  actions: TableAction<UserListItem>[] = [
    {
      label: 'Edit',
      variant: 'primary',
      visible: u => this.canEdit(u),
      handler: u => this.edit(u)
    },
    {
      label: 'Delete',
      variant: 'secondary',
      visible: u => this.canDelete(u),
      handler: u => this.delete(u.id)
    }
  ];

  constructor(
    private usersService: UsersService,
    public auth: AuthFacade,
    private toastr: ToastrService
  ) {
    super(); // only for GenericListPage
  }

  ngOnInit() {
    this.canEditUser = this.auth.hasPermission('User_Update');
    this.canDeleteUser = this.auth.hasPermission('User_Delete');
    this.reload();
  }

  protected loadData(query: UsersQueryParameters) {
    this.usersService.getPaged(query).subscribe(res => {
      this.users = res.items;
      this.totalCount = res.total;
      this.page = res.page;
      this.pageSize = res.pageSize;
    });
  }

  /*  FILTER CHIP LABEL  */

  formatFilterValue(key: string, value: any): string {
    const cfg = this.filtersConfig.find(f => f.key === key);
    if (!cfg) return String(value);

    if (key === 'isActive') return value ? 'Active' : 'Inactive';

    if (cfg.options) {
      const opt = cfg.options.find(o => o.value === value);
      return opt ? opt.label : String(value);
    }

    return String(value);
  }

  /*  MODALS   */

  edit(user: UserListItem) {
    if (!this.canEdit(user)) return;

    this.usersService.getUser(user.id).subscribe(userDto => {
      this.selectedUser = userDto;
    });
  }

  openCreateUser() {
    this.creatingUser = true;
  }

  onCreateUserClose(created: boolean) {
    this.creatingUser = false;

    if (created) {
      this.toastr.success('User created successfully');
      this.reload();
    }
  }

  onModalClose(result: EditUserResultDto) {
    this.selectedUser = undefined;

    if (!result.saved || !result.userId) return;

    this.usersService.updateUserFull(
      {
        userId: result.userId,
        fullName: result.fullName!,
        email: result.email!,
        isActive: result.isActive!,
        newPassword: result.newPassword,
        role: result.role,
        permissions: result.permissions
      },
      () => {
        this.toastr.success('User updated successfully');
        this.reload();
      },
      () => {
        this.toastr.error('Failed to update user');
      }
    );
  }

  delete(id: string) {
    if (!confirm('Delete user?')) return;

    this.usersService.deleteUser(id).subscribe(() => {
      this.toastr.success('User deleted');
      this.reload();
    });
  }

  canEdit(user: UserListItem): boolean {
    if (!this.auth.hasPermission('User_Update')) return false;
    if (user.roles.includes('Admin') && !this.auth.hasRole('Admin')) return false;
    return true;
  }

  canDelete(user: UserListItem): boolean {
    if (!this.auth.hasPermission('User_Delete')) return false;
    if (user.roles.includes('Admin') && !this.auth.hasRole('Admin')) return false;
    return true;
  }
}

