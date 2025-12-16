import { Routes } from '@angular/router';
import { AuthGuard } from './core/auth/auth.guard';

export const routes: Routes = [
    {
        path: '',
        canActivate: [AuthGuard],
        loadComponent: () =>
            import('./features/pages/dashboard/dashboard.component')
                .then(m => m.DashboardComponent)
    },
    {
        path: '',
        loadChildren: () =>
            import('./features/auth/auth.routes')
                .then(m => m.authRoutes)
    }
];