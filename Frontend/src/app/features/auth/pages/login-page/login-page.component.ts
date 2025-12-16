import { Component, NgModule } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { AuthFacade } from '../../../../core/auth/auth.facade';
import { FormsModule, NgModel } from '@angular/forms';

@Component({
  standalone: true,
  templateUrl: './login-page.component.html',
  imports: [FormsModule, RouterLink],

})
export class LoginPageComponent {

  email = '';
  password = '';
  rememberMe = false;
  error?: string;

  constructor(
    private auth: AuthFacade,
    private router: Router
  ) { }

  submit() {
    this.auth.login(this.email, this.password, this.rememberMe)
      .subscribe({
        next: () => this.router.navigate(['/']),
        error: (err) => {
          this.error =
            err.status === 401
              ? 'Invalid email or password'
              : 'Server error. Try again later.';
        }
      });
  }

}
