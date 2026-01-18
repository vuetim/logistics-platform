import { Component, EventEmitter, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { NgFor, NgIf } from '@angular/common';

import { CreateUserDto } from '../../../../../core/auth/dto/Requests/create-user.dto';
import { AuthFacade } from '../../../../../core/auth/auth.facade';
import { UsersService } from '../../../../../data-access/users/users.service';
import { UiButtonComponent } from '../../../../../shared/UI/ui-button/ui-button.component';

@Component({
  selector: 'app-create-user-modal',
  standalone: true,
  imports: [
    FormsModule,
    NgFor,

    UiButtonComponent
  ],
  templateUrl: './create-user-modal.component.html',
  styleUrl: './create-user-modal.component.css'
})
export class CreateUserModalComponent {

  @Output() close = new EventEmitter<boolean>();

  fullName = '';
  email = '';
  password = '';
  role = 'Operator';

  roles = ['Admin', 'Operator', 'Dispatcher', 'Broker'];
  loading = false;

  constructor(
    private auth: AuthFacade,
    private usersService: UsersService
  ) { }

  save() {
    if (!this.fullName || !this.email || !this.password) return;

    this.loading = true;

    const dto: CreateUserDto = {
      fullName: this.fullName.trim(),
      email: this.email.trim(),
      password: this.password,
      role: this.role
    };

    //  CREATE USER
    this.auth.createUser(dto).subscribe({
      next: () => {
        //  FETCH USER 
        this.usersService.getPaged({
          search: dto.email,
          page: 1,
          pageSize: 1
        }).subscribe(res => {
          const user = res.items[0];
          if (!user) {
            this.loading = false;
            return;
          }

          //  ASSIGN ROLE
          this.usersService.assignRole(user.id, dto.role).subscribe({
            next: () => {
              this.loading = false;
              this.close.emit(true);
            },
            error: () => {
              this.loading = false;
              this.close.emit(true);
            }
          });
        });
      },
      error: err => {
        console.error(err);
        this.loading = false;
        alert('Failed to create user');
      }
    });
  }

  cancel() {
    this.close.emit(false);
  }
}
