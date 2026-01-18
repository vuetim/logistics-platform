import { AuthGuard } from "../../../core/auth/auth.guard";
import { PermissionGuard } from "../../../core/auth/permissions/permission.guard";
import { UsersPageComponent } from "./users-page/users-page.component";


export default [
    {
        path: '',
        component: UsersPageComponent,
        canActivate: [AuthGuard, PermissionGuard],
        data: {
            permissions: ['User_View_All']
        }
    }
];
