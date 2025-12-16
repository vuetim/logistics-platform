import { Component } from '@angular/core';
import { AuthFacade } from '../../../../core/auth/auth.facade';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-reset-password',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './reset-password.component.html',
  styleUrl: './reset-password.component.css'
})
export class ResetPasswordComponent {
  password = '';
  confirmPassword = '';
  loading = false;
  error?: string;
  success = false;

  private token: string | null;

  constructor(
    private route: ActivatedRoute,
    private auth: AuthFacade,
    private router: Router
  ) {
    this.token = this.route.snapshot.queryParamMap.get('token');
  }

  submit() {
    if (!this.token) {
      this.error = 'Invalid or expired reset link';
      return;
    }

    if (this.password !== this.confirmPassword) {
      this.error = 'Passwords do not match';
      return;
    }

    this.loading = true;
    this.error = undefined;

    this.auth.resetPassword(this.token, this.password).subscribe({
      next: () => {
        this.success = true;
        this.loading = false;

        // redirect pas pak sekondash
        setTimeout(() => {
          this.router.navigate(['/login']);
        }, 2000);
      },
      error: () => {
        this.error = 'Reset link is invalid or expired';
        this.loading = false;
      }
    });
  }
}
