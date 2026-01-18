import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, Router } from '@angular/router';
import { TokenStorage } from './infrastructure/token-storage.service';
import { JwtDecoderService } from './services/jwt-decoder.service';
import { AuthFacade } from './auth.facade';


@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate {

    constructor(
        private storage: TokenStorage,
        private router: Router
    ) { }

    canActivate(): boolean {
        const session = this.storage.get();

        if (!session) {
            this.router.navigate(['/login']);
            return false;
        }

        return true;
    }
}

