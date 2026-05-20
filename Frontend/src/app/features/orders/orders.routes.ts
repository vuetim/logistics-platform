import { Routes } from '@angular/router';
import { AuthGuard } from '../../core/auth/auth.guard';
import { PermissionGuard } from '../../core/auth/permissions/permission.guard';
import { OrdersPageComponent } from './components/orders-page/orders-page.component';

const routes: Routes = [
    {
        path: '',
        component: OrdersPageComponent,
        canActivate: [AuthGuard, PermissionGuard],
        data: {
            permissions: ['Load_View']
        }
    },
    {
        path: 'create',
        component: OrdersPageComponent,
        canActivate: [AuthGuard, PermissionGuard],
        data: {
            permissions: ['Load_Create']
        }
    },
    {
        path: ':id',
        canActivate: [AuthGuard, PermissionGuard],
        data: {
            permissions: ['Load_View']
        },
        loadComponent: () =>
            import('./components/order-details-page/order-details-page.component')
                .then(m => m.OrderDetailsPageComponent)
    }
];

export default routes;
