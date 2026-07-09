import { computed, inject, Injectable, signal } from '@angular/core';
import { User } from '../models/User.interface';
import { TokenService } from './token-service';
import { DecodedToken } from '../models/DecodedToken.interface';

@Injectable({
  providedIn: 'root',
})
export class SessionStoreService {
  // ── Private writable signals ──────────────────────────────────────────────

  private tokenService = inject(TokenService);

  private readonly _user = signal<User | null>(null);
  private readonly _isLoading = signal<boolean>(false);
  private readonly _isInitialized = signal<boolean>(false);

  // ── Public read-only signals ──────────────────────────────────────────────

  readonly user = this._user.asReadonly();
  readonly isLoading = this._isLoading.asReadonly();
  readonly isInitialized = this._isInitialized.asReadonly();

  // ── Computed signals ──────────────────────────────────────────────────────

  readonly isAuthenticated = computed(() => this._user() !== null);
  readonly userName = computed(() => {
    const u = this._user();
    return u ? u.userName : null;
  });
  readonly roles = computed(() => this._user()?.roles ?? []);

  // ── Mutations ─────────────────────────────────────────────────────────────

  setUser(): void {
    const decodedToken: DecodedToken | null = this.tokenService.getCurrentUserPayload();
    if (decodedToken !== null) {
      const user: User = {
        id: decodedToken?.sub,
        storeId: decodedToken.storeId,
        userName: decodedToken.name,
        email: decodedToken.email,
        roles: decodedToken.roles
      }

      this._user.set(user);
      this._isLoading.set(false);
      this._isInitialized.set(true);
    }
  }

  updateUser(): void {
    const decodedToken: DecodedToken | null = this.tokenService.getCurrentUserPayload();
    if (decodedToken !== null) {
      const user: User = {
        id: decodedToken?.sub,
        storeId: decodedToken.storeId,
        userName: decodedToken.email,
        email: decodedToken.email,
        roles: decodedToken.roles
      }

      this._user.set(user);
      this._isLoading.set(false);
      this._isInitialized.set(true);
    }
  }

  clearUser(): void {
    this._user.set(null);
    this._isInitialized.set(true);
  }

  setLoading(value: boolean): void {
    this._isLoading.set(value);
  }

  markInitialized(): void {
    this._isInitialized.set(true);
  }

  // ── Queries ───────────────────────────────────────────────────────────────

  hasRole(role: string): boolean {
    return this._user()?.roles.includes(role) ?? false;
  }

  hasAllRoles(...roles: string[]): boolean {
    const userRoles = this._user()?.roles ?? [];
    return roles.every(r => userRoles.includes(r));
  }
}