import { Permission } from "../../../../../core/auth/permissions/permission.enum";

export interface UpdateUserFullCommand {
    userId: string;
    fullName: string;
    email: string;
    isActive: boolean;
    newPassword?: string;
    role?: string;
    permissions?: Record<Permission, boolean | null>;
}
