import { Injectable } from "@angular/core";
import { UsersApi } from "./users.api";
import { UpdateUserDto } from "../../core/auth/dto/Requests/update-user.dto";
import { Permission } from "../../core/auth/permissions/permission.enum";
import { UsersQueryParameters } from "../../core/models/users/users-query-parameters.model";
import { forkJoin } from "rxjs";
import { UpdateUserFullCommand } from "../../features/pages/users/components/edit-user-modal/update-user-command.dto";

@Injectable({ providedIn: 'root' })
export class UsersService {

    constructor(private api: UsersApi) { }
    getPaged(params: UsersQueryParameters) {
        return this.api.getPaged(params);
    }

    getUser(id: string) {
        return this.api.getById(id);
    }

    updateUser(id: string, dto: UpdateUserDto) {
        return this.api.update(id, dto);
    }
    assignRole(userId: string, roleName: string) {
        return this.api.assignRole({ userId, roleName });
    }


    setPermission(
        userId: string,
        permission: Permission,
        isAllowed: boolean | null
    ) {
        return this.api.setPermission(
            userId,
            permission,
            isAllowed
        );
    }

    getPermissions(userId: string) {
        return this.api.getPermissions(userId);
    }
    savePermissions(
        userId: string,
        perms?: Record<Permission, boolean | null>,
        onDone?: () => void,
        onError?: () => void
    ) {
        if (!perms || Object.keys(perms).length === 0) {
            onDone?.();
            return;
        }

        let pending = Object.keys(perms).length;

        for (const [permission, isAllowed] of Object.entries(perms)) {
            this.setPermission(
                userId,
                permission as Permission,
                isAllowed
            ).subscribe({
                next: () => {
                    pending--;
                    if (pending === 0) {
                        onDone?.();
                    }
                },
                error: () => {
                    pending--;
                    if (pending === 0) {
                        onError?.();
                    }
                }
            });
        }
    }

    // updateUserFull()  command style service method hides orchestration and it will leave entire userpage component just UI the logis is here.
    updateUserFull(
        cmd: UpdateUserFullCommand,
        onSuccess: () => void,
        onError?: () => void
    ) {
        this.updateUser(cmd.userId, {
            fullName: cmd.fullName,
            email: cmd.email,
            isActive: cmd.isActive,
            newPassword: cmd.newPassword
        }).subscribe({
            next: () => {
                this.afterUserUpdated(cmd, onSuccess, onError);
            },
            error: () => onError?.()
        });
    }
    private afterUserUpdated(
        cmd: UpdateUserFullCommand,
        onSuccess: () => void,
        onError?: () => void
    ) {
        if (cmd.role) {
            this.assignRole(cmd.userId, cmd.role).subscribe({
                next: () => this.savePerms(cmd, onSuccess, onError),
                error: () => onError?.()
            });
        } else {
            this.savePerms(cmd, onSuccess, onError);
        }
    }

    private savePerms(
        cmd: UpdateUserFullCommand,
        onSuccess: () => void,
        onError?: () => void
    ) {
        this.savePermissions(
            cmd.userId,
            cmd.permissions,
            onSuccess,
            onError
        );
    }





    deleteUser(id: string) {
        return this.api.delete(id);
    }
}
