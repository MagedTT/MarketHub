import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { DecodedToken } from '../models/DecodedToken.interface';
import { TokenPair } from '../models/TokenPair.interface';

@Injectable({
  providedIn: 'root',
})
export class TokenService {
  private readonly ACCESS_TOKEN_Key = 'accessToken';
  private readonly REFRESH_TOKEN_KEY = 'refreshToken';
  private httpClient = inject(HttpClient);

  getAccessToken(): string | null {
    return localStorage.getItem(this.ACCESS_TOKEN_Key);
  }

  getRefreshToken(): string | null {
    return localStorage.getItem(this.REFRESH_TOKEN_KEY);
  }

  setTokens(tokenPair: TokenPair): void {
    localStorage.setItem(this.ACCESS_TOKEN_Key, tokenPair.accessToken);
    localStorage.setItem(this.REFRESH_TOKEN_KEY, tokenPair.refreshToken);
  }

  clearTokens(): void {
    localStorage.removeItem(this.ACCESS_TOKEN_Key);
    localStorage.removeItem(this.REFRESH_TOKEN_KEY);
  }

  isTokenExpired(accessToken: string): boolean {
    try {
      const tokenPayload = JSON.parse(atob(accessToken.split('.')[1]));
      return tokenPayload.exp * 1000 < Date.now();
    } catch {
      return false;
    }
  }

  async refreshAccessToken(): Promise<boolean> {
    const accessToken = this.getAccessToken();
    const refreshToken = this.getRefreshToken();

    if (!refreshToken) return false;

    try {
      const result = await firstValueFrom(this.httpClient.post<{ accessToken: string, refreshToken: string }>('https://localhost:5001/api/tokens/refresh', { accessToken, refreshToken }));

      const tokenPair: TokenPair = {
        accessToken: result.accessToken,
        refreshToken: result.refreshToken
      };
      this.setTokens(tokenPair);

      return true;
    } catch {
      this.clearTokens();
      return false;
    }
  }

  decodeAccessToken(accessToken: string): DecodedToken | null {
    try {
      return JSON.parse(atob(accessToken.split('.')[1])) as DecodedToken;
    } catch {
      return null;
    }
  }

  getCurrentUserPayload(): DecodedToken | null {
    try {
      const accessToken = this.getAccessToken();
      if (!accessToken) return null;

      return this.decodeAccessToken(accessToken);
    } catch {
      return null;
    }
  }

  getCurrentUserId(): string | null {
    try {
      const accessToken = this.getAccessToken();

      if (!accessToken) return null;

      return this.decodeAccessToken(accessToken)?.sub ?? null;
    } catch {
      return null;
    }
  }
}
