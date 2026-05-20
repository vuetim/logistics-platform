import { Component, HostListener, OnDestroy, OnInit } from '@angular/core';
import { AuthFacade } from '../../../core/auth/auth.facade';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { UserMenuComponent } from '../../../shared/UI/user-menu/user-menu.component';
import { NotificationsService } from '../../../data-access/notifications/notifications.service';
import { NotificationDto } from '../../../core/models/notifications/notification.dto';
import { GlobalSearchService } from '../../../data-access/search/global-search.service';
import { GlobalSearchResultDto } from '../../../core/models/search/global-search-result.dto';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [CommonModule, FormsModule, UserMenuComponent],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css'
})
export class HeaderComponent implements OnInit, OnDestroy {
  userMenuOpen = false;
  notificationsOpen = false;
  searchOpen = false;
  unreadCount = 0;
  notifications: NotificationDto[] = [];
  searchQuery = '';
  searchResults: GlobalSearchResultDto[] = [];
  searchLoading = false;
  private pollId?: number;
  private searchTimer?: number;

  constructor(
    public auth: AuthFacade,
    private router: Router,
    private notificationsService: NotificationsService,
    private globalSearch: GlobalSearchService
  ) { }

  ngOnInit() {
    this.refreshNotifications();
    this.pollId = window.setInterval(() => this.refreshNotificationCount(), 60000);
  }

  ngOnDestroy() {
    if (this.pollId) window.clearInterval(this.pollId);
    if (this.searchTimer) window.clearTimeout(this.searchTimer);
  }

  toggle() {
    this.userMenuOpen = !this.userMenuOpen;
    this.notificationsOpen = false;
    this.searchOpen = false;
  }

  toggleNotifications() {
    this.notificationsOpen = !this.notificationsOpen;
    this.userMenuOpen = false;
    this.searchOpen = false;
    if (this.notificationsOpen) {
      this.refreshNotifications();
    }
  }

  toggleSearch() {
    this.searchOpen = !this.searchOpen;
    this.userMenuOpen = false;
    this.notificationsOpen = false;
    if (!this.searchOpen) {
      this.searchQuery = '';
      this.searchResults = [];
    }
  }

  @HostListener('document:click', ['$event'])
  closeOnOutsideClick(e: Event) {
    const target = e.target as HTMLElement;
    if (!target.closest('.avatar')) {
      this.userMenuOpen = false;
    }
    if (!target.closest('.notifications-wrap')) {
      this.notificationsOpen = false;
    }
    if (!target.closest('.search-wrap')) {
      this.searchOpen = false;
    }
  }

  logout() {
    this.auth.logout();
    this.router.navigate(['/auth.login'])
  }

  onSearchChanged() {
    if (this.searchTimer) window.clearTimeout(this.searchTimer);

    const query = this.searchQuery.trim();
    if (query.length < 2) {
      this.searchResults = [];
      this.searchLoading = false;
      return;
    }

    this.searchLoading = true;
    this.searchTimer = window.setTimeout(() => {
      this.globalSearch.search(query).subscribe({
        next: results => {
          this.searchResults = results;
          this.searchLoading = false;
        },
        error: () => {
          this.searchResults = [];
          this.searchLoading = false;
        }
      });
    }, 250);
  }

  openSearchResult(result: GlobalSearchResultDto) {
    this.searchOpen = false;
    this.searchQuery = '';
    this.searchResults = [];
    this.router.navigateByUrl(result.route);
  }

  openNotification(notification: NotificationDto) {
    this.notificationsService.markRead(notification.id).subscribe({
      next: () => {
        notification.isRead = true;
        this.refreshNotificationCount();
        this.notificationsOpen = false;
        this.router.navigateByUrl(notification.route);
      }
    });
  }

  markAllRead(event: Event) {
    event.stopPropagation();
    this.notificationsService.markAllRead().subscribe({
      next: () => this.refreshNotifications()
    });
  }

  private refreshNotifications() {
    this.notificationsService.getRecent().subscribe({
      next: items => {
        this.notifications = items;
        this.unreadCount = items.filter(x => !x.isRead).length;
      },
      error: () => {
        this.notifications = [];
        this.unreadCount = 0;
      }
    });
  }

  private refreshNotificationCount() {
    this.notificationsService.getUnreadCount().subscribe({
      next: res => this.unreadCount = res.count,
      error: () => this.unreadCount = 0
    });
  }
}
