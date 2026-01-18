import { HttpClient, HttpParams } from "@angular/common/http";
import { API_ENDPOINTS } from "../../core/config/endpoints";
import { Injectable } from "@angular/core";
import { UserDto } from "../../core/auth/dto/Responses/user.dto";
import { UpdateUserDto } from "../../core/auth/dto/Requests/update-user.dto";
import { AssignRoleDto } from "../../core/auth/dto/Requests/assign-role.dto";
import { Permission } from "../../core/auth/permissions/permission.enum";
import { UserPermissionStateDto } from "../../core/auth/dto/Responses/user-permision-state.dto";
import { UsersQueryParameters } from "../../core/models/users/users-query-parameters.model";
import { PagedResult } from "../../core/models/pagination/paged-result.model";
import { UserListItem } from "../../core/models/users/user-list-item.model";

@Injectable({ providedIn: 'root' })
export class UsersApi {
    private readonly baseUrl = API_ENDPOINTS.users;

    constructor(private http: HttpClient) { }

    getPaged(q: UsersQueryParameters) {
        let params = new HttpParams()
            .set('page', q.page)
            .set('pageSize', q.pageSize);

        if (q.search)
            params = params.set('search', q.search);

        if (q.role)
            params = params.set('role', q.role);

        if (q.isActive === true || q.isActive === false)
            params = params.set('isActive', String(q.isActive));

        if (q.sortBy)
            params = params.set('sortBy', q.sortBy);

        if (q.sortDir)
            params = params.set('sortDir', q.sortDir);

        return this.http.get<PagedResult<UserListItem>>(
            this.baseUrl,
            { params }
        );
    }




    getById(id: string) {
        return this.http.get<UserDto>(`${this.baseUrl}/${id}`);
    }

    update(id: string, dto: UpdateUserDto) {
        return this.http.put<void>(`${this.baseUrl}/${id}`, dto);
    }
    assignRole(dto: AssignRoleDto) {
        return this.http.post<void>(
            `${this.baseUrl}/assign-role`,
            dto
        );
    }
    setPermission(
        userId: string,
        permission: Permission,
        isAllowed: boolean | null
    ) {
        return this.http.post<void>(
            `${this.baseUrl}/${userId}/permissions`,
            { permission, isAllowed }
        );
    }
    getPermissions(userId: string) {
        return this.http.get<UserPermissionStateDto[]>(
            `${this.baseUrl}/${userId}/permissions`
        );
    }


    delete(id: string) {
        return this.http.delete<void>(`${this.baseUrl}/${id}`);
    }
}
