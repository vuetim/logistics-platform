import { Permission } from '../../permissions/permission.enum';

export interface UserPermissionStateDto {
    permission: Permission;
    isAllowed: boolean | null;
}
