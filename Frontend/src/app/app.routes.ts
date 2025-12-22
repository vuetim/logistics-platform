import { Routes } from '@angular/router';
import { AuthGuard } from './core/auth/auth.guard';
import { AppShellComponent } from './layout/app-shell/app-shell.component';

export const routes: Routes = [

    //  AUTH
    {
        path: 'auth',
        loadChildren: () =>
            import('./features/auth/auth.routes')
                .then(m => m.authRoutes)
    },

    //  APP SHELL
    {
        path: '',
        component: AppShellComponent,
        canActivate: [AuthGuard],
        children: [
            {
                path: '',
                redirectTo: 'dashboard',
                pathMatch: 'full'
            },
            {
                path: 'dashboard',
                loadComponent: () =>
                    import('./features/pages/dashboard/dashboard.component')
                        .then(m => m.DashboardComponent),
                data: {
                    breadcrumb: ['Dashboard']
                }
            }

        ]
    },

    { path: '**', redirectTo: 'auth/login' }
];
