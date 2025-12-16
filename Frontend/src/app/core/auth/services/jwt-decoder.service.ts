import { Injectable } from '@angular/core';
import { JwtClaims } from '../models/jwt-claims';

@Injectable({ providedIn: 'root' })
export class JwtDecoderService {

    decode(token: string): JwtClaims {
        try {
            const payload = token.split('.')[1];
            return JSON.parse(atob(payload)) as JwtClaims;
        } catch {
            throw new Error('Invalid JWT token');
        }
    }

}
