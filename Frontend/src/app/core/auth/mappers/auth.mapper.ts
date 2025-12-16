import { LoginResponseDto } from '../dto/Responses/login-response.dto';
import { AuthSession } from '../models/auth-session';

export function mapLoginResponseToSession(
    dto: LoginResponseDto
): AuthSession {

    const payload = JSON.parse(atob(dto.accessToken.split('.')[1]));
    const userId =
        payload[
        'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'
        ];
    return {
        userId,
        accessToken: dto.accessToken,
        refreshToken: dto.refreshToken,
        expiresAt: payload.exp * 1000 // nga JWT
    };
}
