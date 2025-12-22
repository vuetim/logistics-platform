import { Component, EventEmitter, Input, Output } from '@angular/core';
import { AuthFacade } from '../../../core/auth/auth.facade';

@Component({
  selector: 'app-user-menu',
  standalone: true,
  imports: [],
  templateUrl: './user-menu.component.html',
  styleUrl: './user-menu.component.css'
})
export class UserMenuComponent {
  @Input() name: string | null = '';
  @Input() email: string | null = '';
  @Input() roles: string[] = [];
  @Output() logout = new EventEmitter<void>();
  constructor(public auth: AuthFacade) { }
}
