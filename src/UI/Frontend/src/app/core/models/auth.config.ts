import { InjectionToken, Provider } from "@angular/core";

export interface AuthConfig {
    apiBase: string;
    endpoints: {
        register: string;
        login: string;
        refreshToken: string;
    };
    home: string;
    loginPath: string;
    forgetPasswordPath: string;
    registerPath: string;
};

export const AUTH_CONFIG = new InjectionToken<AuthConfig>('AUTH_CONFIG');

export function provideAuthConfig(authConfig: AuthConfig): Provider {
    return {
        provide: AUTH_CONFIG,
        useValue: { ...authConfig } satisfies AuthConfig
    };
}