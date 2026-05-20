import { ActivatedRouteSnapshot, CanActivate, Router } from "@angular/router";
import { AuthFacade } from "../auth.facade";
import { Injectable } from "@angular/core";

@Injectable({ providedIn: 'root' })
export class PermissionGuard implements CanActivate {

    constructor(
        private auth: AuthFacade,
        private router: Router
    ) { }

    canActivate(route: ActivatedRouteSnapshot): boolean {
        const perms = route.data['permissions'] as string[] | undefined;

        if (!perms || perms.length === 0) return true;

        if (this.auth.hasRole('Admin')) return true;

        const hasAny = perms.some(p => this.auth.hasPermission(p));
        if (!hasAny) {
            this.router.navigate(['/unauthorized']);
            return false;
        }
        return true;
    }

}

