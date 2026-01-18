import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { NgFor, NgIf } from '@angular/common';

import { UserDto } from '../../../../../core/auth/dto/Responses/user.dto';
import { Permission } from '../../../../../core/auth/permissions/permission.enum';
import { EditUserResultDto } from './edit-user-result.dto';
import { UsersService } from '../../../../../data-access/users/users.service';
import { AuthFacade } from '../../../../../core/auth/auth.facade';
import { UiButtonComponent } from "../../../../../shared/UI/ui-button/ui-button.component";

@Component({
  selector: 'app-edit-user-modal',
  standalone: true,
  imports: [FormsModule, NgFor, UiButtonComponent, UiButtonComponent, NgIf],
  templateUrl: './edit-user-modal.component.html',
  styleUrl: './edit-user-modal.component.css'
})
export class EditUserModalComponent implements OnInit {
  @Input() user!: UserDto;
  @Output() close = new EventEmitter<EditUserResultDto>();

  fullName = '';
  email = '';
  role = '';
  isActive = true;
  newPassword = '';

  roles = ['Admin', 'Operator', 'Dispatcher', 'Broker'];
  permissions = Object.values(Permission);
  permissionState: Record<Permission, boolean | null> = {} as any;

  canUpdate = false;
  canAssignRole = false;

  constructor(
    private usersService: UsersService,
    private auth: AuthFacade
  ) { }

  ngOnInit() {


    if (!this.user) return;
    this.canUpdate =
      this.auth.hasPermission('User_Update') &&
      (!this.user.roles.includes('Admin') || this.auth.hasRole('Admin'));

    this.canAssignRole =
      this.auth.hasPermission('User_AssignRole') &&
      (!this.user.roles.includes('Admin') || this.auth.hasRole('Admin'));

    this.fullName = this.user.fullName;
    this.email = this.user.email;
    this.role = this.user.roles[0];
    this.isActive = this.user.isActive;
    this.newPassword = '';


    this.usersService.getPermissions(this.user.id).subscribe(perms => {
      perms.forEach(p => {
        this.permissionState[p.permission] = p.isAllowed;
      });
    });
  }

  save() {
    this.close.emit({
      saved: true,
      userId: this.user.id,
      fullName: this.fullName,
      email: this.email,
      role: this.canAssignRole ? this.role : undefined,
      isActive: this.isActive,
      permissions: this.permissionState,
      newPassword: this.newPassword?.trim() || undefined
    });
  }
  cancel() {
    this.close.emit({ saved: false });
  }
}
