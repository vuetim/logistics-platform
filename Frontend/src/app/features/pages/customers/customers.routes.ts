import { AuthGuard } from "../../../core/auth/auth.guard";
import { PermissionGuard } from "../../../core/auth/permissions/permission.guard";
import { CustomersPageComponent } from "./components/customers-page/customers-page.component";

export default [
    {
        path: '',
        component: CustomersPageComponent,
        canActivate: [AuthGuard, PermissionGuard],
        data: {
            permissions: ['Customer_View', 'User_View_All']
        }
    }
];