import { Injectable } from '@angular/core';
import { JwtDecoderService } from './jwt-decoder.service';
import { TokenStorage } from '../infrastructure/token-storage.service';

@Injectable({ providedIn: 'root' })
export class PermissionsService {

    constructor(
        private storage: TokenStorage,
        private jwt: JwtDecoderService
    ) { }

    hasRole(role: string): boolean {
        const session = this.storage.get();
        if (!session) return false;

        const roles =
            this.jwt.decode(session.accessToken).roles?.split(',') ?? [];

        return roles.includes(role);
    }
}
