import { InjectionToken, Provider } from "@angular/core";

export interface AppConfig {
    apiBase: string;
    endpoints: {
        wishlist: {
            getWishlist: string;
            addWishlistItem: string;
        }
    }
}

export const APP_CONFIG = new InjectionToken<AppConfig>('APP_CONFIG');

export function ProvideAppConfig(appConfig: AppConfig): Provider {
    return {
        provide: APP_CONFIG,
        useValue: {
            ...appConfig
        } satisfies AppConfig
    }
};