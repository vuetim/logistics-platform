import { Routes } from '@angular/router';

export const authRoutes: Routes = [
    {
        path: 'login',
        loadComponent: () =>
            import('./pages/login-page/login-page.component')
                .then(m => m.LoginPageComponent)
    },

    {
        path: 'reset-password',
        loadComponent: () =>
            import('./pages/reset-password/reset-password.component')
                .then(m => m.ResetPasswordComponent)
    }
];
