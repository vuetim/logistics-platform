import { Routes } from '@angular/router';
import { AuthGuard } from '../../core/auth/auth.guard';
import { PermissionGuard } from '../../core/auth/permissions/permission.guard';
import { LoadsPageComponent } from './components/loads-page/loads-page.component';

const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    component: LoadsPageComponent,
    canActivate: [AuthGuard, PermissionGuard],
    data: {
      permissions: ['Load_View']
    }
  },
  {
    path: 'carrier-offers',
    canActivate: [AuthGuard, PermissionGuard],
    data: {
      permissions: ['CarrierOffer_View_All']
    },
    loadComponent: () =>
      import('./components/carrier-offers-page/carrier-offers-page.component')
        .then(m => m.CarrierOffersPageComponent)
  },
  {
    path: ':id',
    canActivate: [AuthGuard, PermissionGuard],
    data: {
      permissions: ['Load_View']
    },
    loadComponent: () =>
      import('./components/load-details-page/load-details-page.component')
        .then(m => m.LoadDetailsPageComponent)
  }
];

export default routes;
