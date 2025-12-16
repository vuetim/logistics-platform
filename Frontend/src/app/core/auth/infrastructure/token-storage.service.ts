import { Injectable } from '@angular/core';
import { AuthSession } from '../models/auth-session';

const STORAGE_KEY = 'auth_session';

@Injectable({ providedIn: 'root' })
export class TokenStorage {

    private storage: Storage = localStorage;

    save(session: AuthSession, rememberMe: boolean = true): void {
        this.storage = rememberMe ? localStorage : sessionStorage;
        this.storage.setItem(STORAGE_KEY, JSON.stringify(session));
    }

    get(): AuthSession | null {
        const raw =
            localStorage.getItem(STORAGE_KEY) ??
            sessionStorage.getItem(STORAGE_KEY);

        return raw ? JSON.parse(raw) : null;
    }

    clear(): void {
        localStorage.removeItem(STORAGE_KEY);
        sessionStorage.removeItem(STORAGE_KEY);
    }
}


