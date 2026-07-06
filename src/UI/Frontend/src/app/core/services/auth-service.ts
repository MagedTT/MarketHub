import { inject, Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, tap } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { TokenService } from './token-service';
import { LoginRequest } from '../models/LoginRequest.interface';
import { AUTH_CONFIG } from '../models/auth.config';
import { RegisterRequest } from '../models/RegisterRequest.interface';
import { SessionStoreService } from './session-store-service';
import { TokenPair } from '../models/TokenPair.interface';

export interface RegisterResponse {
    message: string;
}

@Injectable({
    providedIn: 'root',
})
export class AuthService {
    private tokenService = inject(TokenService);
    private session = inject(SessionStoreService);
    private httpClient = inject(HttpClient);
    private router = inject(Router);
    private config = inject(AUTH_CONFIG);

    register(registerRequest: RegisterRequest): Observable<RegisterResponse> {
        return this.httpClient.post<RegisterResponse>(`${this.config.apiBase}${this.config.endpoints.register}`, registerRequest);
    }

    login(loginRequest: LoginRequest): Observable<TokenPair> {
        return this.httpClient.post<TokenPair>(`${this.config.apiBase}${this.config.endpoints.login}`, loginRequest).pipe(
            tap(result => {
                this.tokenService.setTokens(result);
                this.session.setUser();
                console.log(result.accessToken);
                console.log(result.refreshToken);
            })
        );
    }

    logout(): void {
        this.session.clearUser();
        this.tokenService.clearTokens();
        this.router.navigateByUrl('auth/login');
    }
}
