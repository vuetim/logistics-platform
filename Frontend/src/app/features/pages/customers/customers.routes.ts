import { Routes } from '@angular/router';
import { AuthGuard } from '../../../core/auth/auth.guard';
import { PermissionGuard } from '../../../core/auth/permissions/permission.guard';
import { CustomersPageComponent } from './components/customers-page/customers-page.component';

const routes: Routes = [
    {
        path: '',
        component: CustomersPageComponent,
        canActivate: [AuthGuard, PermissionGuard],
        data: {
            permissions: ['Customer_View', 'User_View_All']
        }
    }
];

export default routes;
