import { Component, HostListener } from '@angular/core';
import { SidebarItem } from './sidebar.model';
import { SIDEBAR_ITEMS } from './sidebar.config';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { AuthFacade } from '../../../core/auth/auth.facade';

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.css'
})
export class SidebarComponent {
  items: SidebarItem[] = [];
  expanded: SidebarItem | null = null

  constructor(private auth: AuthFacade) {
    this.items = SIDEBAR_ITEMS
      .map(item => ({
        ...item,
        children: item.children?.filter(child => this.canShow(child))
      }))
      .filter(item => this.canShow(item) && (!item.children || item.children.length > 0));
  }

  toggle(item: SidebarItem) {
    this.expanded = this.expanded === item ? null : item
  }
  @HostListener('document:click', ['$event'])
  closeOnOutsideClick(e: Event) {
    if (!(e.target as HTMLElement).closest('.icon-btn')) {
      this.expanded = null;
    }
  }

  private canShow(item: SidebarItem) {
    return !item.permission || this.auth.hasRole('Admin') || this.auth.hasPermission(item.permission);
  }
}
