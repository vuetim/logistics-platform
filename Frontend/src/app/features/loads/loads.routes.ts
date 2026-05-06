import { AuthGuard } from "../../core/auth/auth.guard";
import { PermissionGuard } from "../../core/auth/permissions/permission.guard";
import { LoadsPageComponent } from "./components/loads-page/loads-page.component";

export default [
  {
    path: '',
    component: LoadsPageComponent,
    canActivate: [AuthGuard, PermissionGuard],
    data: {
      permissions: ['Load_View']
    }
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
