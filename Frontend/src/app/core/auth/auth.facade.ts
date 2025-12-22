import { Injectable } from '@angular/core';
import { tap, map } from 'rxjs';
import { AuthApi } from '../../data-access/auth/auth.api';
import { TokenStorage } from './infrastructure/token-storage.service';
import { mapLoginResponseToSession } from './mappers/auth.mapper';
import { JwtDecoderService } from './services/jwt-decoder.service';


@Injectable({ providedIn: 'root' })
export class AuthFacade {
    constructor(
        private api: AuthApi,
        private storage: TokenStorage,
        private jwt: JwtDecoderService

    ) { }

    login(email: string, password: string, rememberMe: boolean) {
        return this.api.login({ email, password, rememberMe }).pipe(
            map(mapLoginResponseToSession),
            tap(session => {
                localStorage.setItem('remember_me', rememberMe ? '1' : '0');

                // save session
                this.storage.save(session, rememberMe);
            })
        );
    }

    refresh() {
        const session = this.storage.get();
        if (!session) {
            throw new Error('No session');
        }
        const rememberMe =
            localStorage.getItem('remember_me') === '1';
        return this.api.refresh(session.refreshToken).pipe(
            map(mapLoginResponseToSession),
            tap(s => this.storage.save(s, rememberMe))

        );
    }

    logout() {
        const session = this.storage.get();

        if (session?.refreshToken) {
            this.api.logout(session.refreshToken).subscribe({
                complete: () => {
                    this.storage.clear();
                    localStorage.removeItem('remember_me');
                },
                error: () => {
                    this.storage.clear();
                    localStorage.removeItem('remember_me');
                }
            });
        } else {
            this.storage.clear();
            localStorage.removeItem('remember_me');
        }
    }


    forgotPassword(email: string) {
        return this.api.forgotPassword({ email });
    }

    resetPassword(token: string, newPassword: string) {
        return this.api.resetPassword({ token, newPassword });
    }

    getCurrentUserId(): string | null {
        const session = this.storage.get();
        if (!session) return null;

        return this.jwt.decode(session.accessToken).sub;
    }

    hasRole(role: string): boolean {
        return this.getRoles().includes(role);
    }
    getRoles(): string[] {
        const session = this.storage.get();
        if (!session) return [];

        return this.jwt
            .decode(session.accessToken)
            .roles?.split(',') ?? [];
    }

    isLoggedIn(): boolean {
        return !!this.storage.get();
    }
    getPermissions(): string[] {
        const session = this.storage.get();
        if (!session) return [];

        return this.jwt
            .decode(session.accessToken)
            .permissions?.split(',') ?? [];
    }

    hasPermission(permission: string): boolean {
        return this.getPermissions().includes(permission);
    }
    getUserName(): string | null {
        const session = this.storage.get();
        if (!session) return null;

        return this.jwt.decode(session.accessToken).name ?? null;
    }
    getUserEmail(): string | null {
        const session = this.storage.get();
        if (!session) return null;
        return this.jwt.decode(session.accessToken).email ?? null;
    }

    getUserInitials(): string {
        const name = this.getUserName();
        if (!name) return '';

        return name
            .split(' ')
            .map(p => p[0])
            .join('')
            .toUpperCase();
    }

}
