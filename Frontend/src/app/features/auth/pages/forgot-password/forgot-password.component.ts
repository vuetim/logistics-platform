import { Component } from '@angular/core';
import { AuthFacade } from '../../../../core/auth/auth.facade';
import { FormsModule } from '@angular/forms';
import { NgIf } from '@angular/common';

@Component({
  selector: 'app-forgot-password',
  standalone: true,
  imports: [FormsModule, NgIf],
  templateUrl: './forgot-password.component.html',
  styleUrl: './forgot-password.component.css'
})
export class ForgotPasswordComponent {
  email = '';
  submitted = false;
  loading = false;
  error?: string;

  constructor(private auth: AuthFacade) { }

  submit() {
    if (!this.email) return;

    this.loading = true;
    this.error = undefined;

    this.auth.forgotPassword(this.email).subscribe({
      next: () => {
        this.submitted = true;
        this.loading = false;
      },
      error: () => {
        // ❗ MOS trego a ekziston email-i
        this.submitted = true;
        this.loading = false;
      }
    });
  }
}
