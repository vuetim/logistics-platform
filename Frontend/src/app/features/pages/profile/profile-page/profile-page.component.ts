import { Component, OnInit } from '@angular/core';
import { AuthFacade } from '../../../../core/auth/auth.facade';
import { UsersService } from '../../../../data-access/users/users.service';
import { UpdateUserDto } from '../../../../core/auth/dto/Requests/update-user.dto';
import { FormsModule } from '@angular/forms';
import { PageLayoutComponent } from '../../../../layout/app-shell/page-layout/page-layout/page-layout.component';
import { NgIf } from '@angular/common';
import { UiButtonComponent } from "../../../../shared/UI/ui-button/ui-button.component";
import { UiCardComponent } from "../../../../shared/UI/ui-card/ui-card.component";


@Component({
  standalone: true,
  imports: [FormsModule, PageLayoutComponent, NgIf, UiButtonComponent, UiCardComponent, UiCardComponent],
  selector: 'app-profile-page',
  templateUrl: './profile-page.component.html',
  styleUrl: './profile-page.component.css'
})
export class ProfilePageComponent implements OnInit {

  fullName = '';
  email = '';
  roles: string[] = [];
  isActive = true;

  newPassword = '';

  isAdmin = false;
  canEdit = false;

  private userId!: string;

  constructor(
    private auth: AuthFacade,
    private users: UsersService
  ) { }

  ngOnInit(): void {
    const id = this.auth.getCurrentUserId();
    if (!id) return;

    this.userId = id;
    this.isAdmin = this.auth.hasRole('Admin');
    this.canEdit = this.isAdmin;

    this.users.getUser(id).subscribe(user => {
      this.fullName = user.fullName;
      this.email = user.email;
      this.roles = user.roles;
      this.isActive = user.isActive;
    });
  }

  save(): void {
    if (!this.canEdit) return;

    const dto: UpdateUserDto = {
      fullName: this.fullName,
      email: this.email,
      isActive: this.isActive
    };

    if (this.newPassword?.trim()) {
      dto.newPassword = this.newPassword.trim();
    }

    this.users.updateUser(this.userId, dto).subscribe(() => {
      this.auth.refresh().subscribe();
      this.newPassword = '';
      alert('Profile updated');
    });
  }
}

