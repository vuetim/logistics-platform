import { Routes } from '@angular/router';
import { AuthGuard } from '../../core/auth/auth.guard';
import { PermissionGuard } from '../../core/auth/permissions/permission.guard';
import { FinancialsPageComponent } from './financials-page.component';

const routes: Routes = [
  {
    path: '',
    component: FinancialsPageComponent,
    canActivate: [AuthGuard, PermissionGuard],
    data: {
      permissions: ['Financial_View']
    }
  }
];

export default routes;
