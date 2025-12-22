import { Component, HostListener } from '@angular/core';
import { SidebarItem } from './sidebar.model';
import { SIDEBAR_ITEMS } from './sidebar.config';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.css'
})
export class SidebarComponent {
  items: SidebarItem[] = SIDEBAR_ITEMS;
  expanded: SidebarItem | null = null

  toggle(item: SidebarItem) {
    this.expanded = this.expanded === item ? null : item
  }
  @HostListener('document:click', ['$event'])
  closeOnOutsideClick(e: Event) {
    if (!(e.target as HTMLElement).closest('.icon-btn')) {
      this.expanded = null;
    }
  }
}
