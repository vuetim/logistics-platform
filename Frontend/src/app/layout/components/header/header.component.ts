import { Component, HostListener } from '@angular/core';
import { AuthFacade } from '../../../core/auth/auth.facade';
import { Router } from '@angular/router';
import { NgIf } from '@angular/common';
import { UserMenuComponent } from '../../../shared/UI/user-menu/user-menu.component';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [NgIf, UserMenuComponent],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css'
})
export class HeaderComponent {
  open = false;

  constructor(public auth: AuthFacade, private router: Router) { }

  toggle() {
    this.open = !this.open
  }
  @HostListener('document:click', ['$event'])
  closeOnOutsideClick(e: Event) {
    if (!(e.target as HTMLElement).closest('.avatar')) {
      this.open = false;
    }
  }
  logout() {
    this.auth.logout();
    this.router.navigate(['/auth.login'])
  }

}
