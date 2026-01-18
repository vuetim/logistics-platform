import { Permission } from "../../../../../core/auth/permissions/permission.enum";

export interface EditUserResultDto {
    saved: boolean;
    userId?: string;
    fullName?: string;
    email?: string;
    role?: string;
    isActive?: boolean;
    permissions?: Record<Permission, boolean | null>;
    newPassword?: string;
}
