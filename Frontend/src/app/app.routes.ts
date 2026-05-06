import { Routes } from '@angular/router';
import { AuthGuard } from './core/auth/auth.guard';
import { AppShellComponent } from './layout/app-shell/app-shell.component';
import { CustomerDetailsPageComponent } from './features/pages/customers/components/customer-details-page/customer-details-page.component';

export const routes: Routes = [

    //  AUTH
    {
        path: 'auth',
        loadChildren: () =>
            import('./features/auth/auth.routes')
                .then(m => m.authRoutes)
    },
    //users


    // APP SHELL
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
            },
            {
                path: 'profile',
                loadChildren: () =>
                    import('./features/pages/profile/profile.routes')
                        .then(m => m.default),
                data: {
                    breadcrumb: ['Profile']
                }
            },
            {
                path: 'admin/users',
                loadChildren: () =>
                    import('./features/pages/users/users.routes')
                        .then(m => m.default),
                data: {
                    breadcrumb: ['Users']
                }
            },
            {
                path: 'admin/customers',
                loadChildren: () =>
                    import('./features/pages/customers/customers.routes')
                        .then(m => m.default),
                data: {
                    breadcrumb: ['Customers']
                }
            },
            {
                path: 'customers/:id',
                component: CustomerDetailsPageComponent
            },
            {
                path: 'orders',
                loadChildren: () =>
                    import('./features/orders/orders.routes')
                        .then(m => m.default),
                data: {
                    breadcrumb: ['Orders']
                }
            },
            {
                path: 'loads',
                loadChildren: () =>
                    import('./features/loads/loads.routes')
                        .then(m => m.default),
                data: {
                    breadcrumb: ['Loads']
                }
            },
            {
                path: 'financials',
                loadChildren: () =>
                    import('./features/financials/financials.routes')
                        .then(m => m.default),
                data: {
                    breadcrumb: ['Financials']
                }
            },
            {
                path: 'documents',
                loadComponent: () =>
                    import('./features/documents/documents-page.component')
                        .then(m => m.DocumentsPageComponent),
                data: {
                    breadcrumb: ['Documents']
                }
            }

        ]
    },

    { path: '**', redirectTo: 'auth/login' }
];
