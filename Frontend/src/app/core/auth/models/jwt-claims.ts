export interface JwtClaims {
    sub: string;          // userId
    email?: string;
    permissions?: string;

    name: string;
    roles?: string;       // "Admin,Dispatcher"
    exp: number;
    iss: string;
    aud: string;
}
