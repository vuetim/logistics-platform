import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, Router } from '@angular/router';
import { TokenStorage } from './infrastructure/token-storage.service';
import { JwtDecoderService } from './services/jwt-decoder.service';
import { AuthFacade } from './auth.facade';


@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate {

    constructor(
        private storage: TokenStorage,
        private router: Router,
        private jwt: JwtDecoderService,
        private auth: AuthFacade
    ) { }

    canActivate(route: ActivatedRouteSnapshot): boolean {
        const session = this.storage.get();

        //  Not logged in
        if (!session) {
            this.router.navigate(['/login']);
            return false;
        }

        //  No roles required → allow
        const allowedRoles: string[] | undefined = route.data['roles'];
        if (!allowedRoles || allowedRoles.length === 0) {
            return true;
        }

        //  Decode JWT (KËTU ËSHTË FIXI)
        const claims = this.jwt.decode(session.accessToken);
        const userRoles = this.auth.getRoles();

        // Check access
        const hasAccess = allowedRoles.some(role =>
            userRoles.includes(role)
        );

        if (!hasAccess) {
            this.router.navigate(['/unauthorized']);
            return false;
        }

        return true;
    }
}
