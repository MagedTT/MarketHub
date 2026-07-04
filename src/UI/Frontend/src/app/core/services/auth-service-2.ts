import { computed, inject, Injectable, signal } from '@angular/core';
import { TokenService } from './token-service';
import { SessionStoreService } from './session-store-service';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable, tap } from 'rxjs';
import { LoginRequest } from '../models/LoginRequest.interface';
import { DecodedToken } from '../models/DecodedToken.interface';
import { TokenPair } from '../models/TokenPair.interface';

@Injectable({
  providedIn: 'root',
})
export class AuthService2 {

  private tokenService = inject(TokenService);
  private session = inject(SessionStoreService);

  constructor(private router: Router, private httpClient: HttpClient) { }

  private _currentUser = signal<DecodedToken | null>(this.tokenService.getCurrentUserPayload());

  currentUser = this._currentUser.asReadonly();
  currentUserId = computed(() => this._currentUser()?.sub ?? null);
  isAuthenticated = computed(() => this._currentUser !== null);
  userRoles = computed(() => this._currentUser()?.roles ?? null);

  register(formData: FormData): Observable<any> {
    return this.httpClient.post('https://localhost:7079/api/Authentication/register', formData);
  }

  login(credentials: LoginRequest): Observable<TokenPair> {
    return this.httpClient.post<{ accessToken: string, refreshToken: string }>('https://localhost:7079/api/Authentication/login', credentials).pipe(
      tap((tokenPair: TokenPair) => {
        this.tokenService.setTokens(tokenPair);
        this._currentUser.set(this.tokenService.decodeAccessToken(tokenPair.accessToken));
        this.router.navigateByUrl('discover-home');
      })
    );
  }

  logout() {
    this.tokenService.clearTokens();
    this._currentUser.set(null);
    this.router.navigateByUrl('auth/login');
    // to be done: implement logout in the backend to revoke the refresh token
  }

  syncUserFromAccessToken(): void {
    this._currentUser.set(this.tokenService.getCurrentUserPayload());
  }
}
