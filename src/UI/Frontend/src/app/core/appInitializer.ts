import { inject, provideAppInitializer } from '@angular/core';
import { SessionStoreService } from './services/session-store-service';
import { TokenService } from './services/token-service';
import { CartService } from '../shared/services/cart-service';
import { CartStore } from './services/stores/cart-store';

export const appInitializer = provideAppInitializer(() => {
    const tokenService = inject(TokenService);
    const sessionStore = inject(SessionStoreService);
    const cartService = inject(CartService);
    const cartStore = inject(CartStore);

    const token = tokenService.getAccessToken();

    if (token && !tokenService.isTokenExpired(token)) {
        sessionStore.setUser();
        // cartService.getAmountInCart(sessionStore.user()?.id!).subscribe(response => {
        //     cartStore.setCartAmount(response);
        // });

    } else if (token && tokenService.isTokenExpired(token)) {
        tokenService.refreshAccessToken();
        sessionStore.setUser();
        // cartService.getAmountInCart(sessionStore.user()?.id!).subscribe({
        //     next: (response) => {
        //         cartStore.setCartAmount(response);
        //     }
        // });
    } else {
        sessionStore.clearUser();
    }

    const userId = sessionStore.user()?.id;

    if (userId) {
        cartService.getAmountInCart(userId).subscribe({
            next: (response) => {
                cartStore.setCartAmount(response);
            }
        });
    }

    sessionStore.markInitialized();
});