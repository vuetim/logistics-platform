import { Component, } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { AuthFacade } from '../../../../core/auth/auth.facade';
import { FormsModule, } from '@angular/forms';
import { NgFor, NgIf, SlicePipe } from '@angular/common';
import { ViewChild, ElementRef } from '@angular/core';
import { ToastrService } from 'ngx-toastr';


@Component({
  standalone: true,
  templateUrl: './login-page.component.html',
  imports: [FormsModule, NgIf, SlicePipe],
  styleUrl: './login-page.component.css'
})

export class LoginPageComponent {
  private readonly EMAIL_KEY = 'auth:lastEmail';

  email = '';
  password = '';
  rememberMe = false;
  error?: string;
  step: 'email' | 'password' | 'forgot' = 'email';
  @ViewChild('passwordInput') passwordInput!: ElementRef<HTMLInputElement>;
  time = '';
  date = '';
  emailError?: string;
  passwordError?: string;
  private clockInterval?: number;
  showColon = true;
  isNight = false;


  constructor(
    private auth: AuthFacade,
    private router: Router,

    private toastr: ToastrService,
  ) { }
  ngOnInit() {

    const savedEmail = localStorage.getItem(this.EMAIL_KEY);
    if (savedEmail) {
      this.email = savedEmail;
    }
    this.updateClock();
    this.clockInterval = window.setInterval(() => {
      this.updateClock();
    }, 1000);
  }
  ngOnDestroy() {
    if (this.clockInterval) {
      clearInterval(this.clockInterval);
    }
  }
  private updateClock() {
    const now = new Date(
      new Date().toLocaleString()
    );

    const hours = now.getHours();
    const minutes = now.getMinutes();
    const seconds = now.getSeconds();

    this.showColon = seconds % 2 === 0;

    this.time =
      `${hours.toString().padStart(2, '0')}` +
      `${minutes.toString().padStart(2, '0')}` +
      `${seconds.toString().padStart(2, '0')}`;

    this.date = now.toLocaleDateString([], {
      weekday: 'long',
      day: '2-digit',
      month: 'long',
      year: 'numeric',

    });

    //  night mode
    this.isNight = hours >= 18 || hours < 6;
  }

  saveEmail() {
    localStorage.setItem(this.EMAIL_KEY, this.email);
  }

  continue() {
    if (!this.isValidEmail(this.email)) {
      this.emailError = 'Enter a valid email address';
      return;
    }
    this.saveEmail();
    this.step = 'password';

    setTimeout(() => {
      this.passwordInput?.nativeElement.focus();
    });
  }
  backToEmail() {
    this.step = 'email';
    this.password = '';
    this.error = undefined;

  }
  //FORGOT PASSWORD
  sendResetLink() {
    if (!this.isValidEmail(this.email)) {
      this.emailError = 'Enter a valid email address';
      return;
    }

    this.auth.forgotPassword(this.email).subscribe(() => {
      this.toastr.success('If the email exists, reset instructions were sent to your email');
      this.step = 'email';
    });
  }
  //SUBMIT
  submit() {
    if (!this.password) {
      this.passwordError = 'Password is required';
      return;
    }
    this.auth.login(this.email, this.password, this.rememberMe)
      .subscribe({
        next: () => {
          this.toastr.success('Logged in successfully');
          this.router.navigate(['/']);
        },
        error: () => {
          this.passwordError = 'Incorrect password';
          this.toastr.error('Login Failed')
        }
      });
  }
  public isValidEmail(email: string): boolean {
    return /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email);
  }
}


