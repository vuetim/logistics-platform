export interface JwtClaims {
    sub: string;          // userId
    email?: string;
    permissions?: string;

    name: string;
    roles?: string;       // "Admin,Dispatcher"
    exp: number;
    iss: string;
    aud: string;
    'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'?: string;

}
