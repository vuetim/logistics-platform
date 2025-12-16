import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, Router } from '@angular/router';
import { TokenStorage } from './infrastructure/token-storage.service';
import { JwtDecoderService } from './services/jwt-decoder.service';

@Injectable({ providedIn: 'root' })
export class RoleGuard implements CanActivate {

    constructor(
        private storage: TokenStorage,
        private router: Router,
        private jwt: JwtDecoderService,

    ) { }

    canActivate(route: ActivatedRouteSnapshot): boolean {
        const session = this.storage.get();
        if (!session) {
            this.router.navigate(['/login']);
            return false;
        }

        const allowedRoles: string[] = route.data['roles'];
        if (!allowedRoles || allowedRoles.length === 0) {
            return true;
        }

        //  Decode JWT 
        const claims = this.jwt.decode(session.accessToken);
        const userRoles = claims.roles?.split(',') ?? [];

        const hasAccess = allowedRoles.some(r => userRoles.includes(r));

        if (!hasAccess) {
            this.router.navigate(['/unauthorized']);
            return false;
        }

        return true;
    }


}
