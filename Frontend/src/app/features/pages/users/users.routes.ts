import { Routes } from '@angular/router';
import { AuthGuard } from '../../../core/auth/auth.guard';
import { PermissionGuard } from '../../../core/auth/permissions/permission.guard';
import { UsersPageComponent } from './users-page/users-page.component';


const routes: Routes = [
    {
        path: '',
        component: UsersPageComponent,
        canActivate: [AuthGuard, PermissionGuard],
        data: {
            permissions: ['User_View_All']
        }
    }
];

export default routes;
