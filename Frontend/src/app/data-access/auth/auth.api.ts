import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { LoginRequestDto } from '../../core/auth/dto/Requests/login-request.dto';
import { LoginResponseDto } from '../../core/auth/dto/Responses/login-response.dto';
import { ResetPasswordRequestDto } from '../../core/auth/dto/Requests/reset-password-request.dto';
import { ForgotPasswordRequestDto } from '../../core/auth/dto/Requests/forgot-password-request.dto';
import { API_ENDPOINTS } from '../../core/config/endpoints';
import { CreateUserDto } from '../../core/auth/dto/Requests/create-user.dto';

@Injectable({ providedIn: 'root' })
export class AuthApi {
    private readonly baseUrl = API_ENDPOINTS.auth;

    constructor(private http: HttpClient) { }

    login(dto: LoginRequestDto) {
        return this.http.post<LoginResponseDto>(
            `${this.baseUrl}/login`,
            dto
        );
    }
    register(dto: CreateUserDto) {
        return this.http.post<void>(
            `${this.baseUrl}/register`,
            {
                fullName: dto.fullName,
                email: dto.email,
                password: dto.password
            }
        );
    }

    forgotPassword(dto: ForgotPasswordRequestDto) {
        return this.http.post<void>(
            `${this.baseUrl}/forgot-password`,
            dto
        );
    }

    resetPassword(dto: ResetPasswordRequestDto) {
        return this.http.post<void>(
            `${this.baseUrl}/reset-password`,
            dto
        );
    }

    refresh(refreshToken: string) {
        return this.http.post<LoginResponseDto>(
            `${this.baseUrl}/refresh`,
            { refreshToken }
        );
    }
    logout(refreshToken: string) {
        return this.http.post<void>(
            `${this.baseUrl}/logout`,
            { refreshToken }
        );
    }

}
