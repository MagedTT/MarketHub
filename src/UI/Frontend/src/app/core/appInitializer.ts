import { inject, provideAppInitializer } from '@angular/core';
import { SessionStoreService } from './services/session-store-service';
import { TokenService } from './services/token-service';

export const appInitializer = provideAppInitializer(() => {
    const tokenService = inject(TokenService);
    const sessionStore = inject(SessionStoreService);

    const token = tokenService.getAccessToken();

    if (token && !tokenService.isTokenExpired(token)) {
        sessionStore.setUser();
    } else if (token && tokenService.isTokenExpired(token)) {
        tokenService.refreshAccessToken();
        sessionStore.setUser();
    } else {
        sessionStore.clearUser();
    }

    sessionStore.markInitialized();
});