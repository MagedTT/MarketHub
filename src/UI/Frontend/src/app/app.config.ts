import { ApplicationConfig, provideBrowserGlobalErrorListeners } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideAuthConfig } from './core/models/auth.config';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { authInterceptor } from './core/interceptors/auth-interceptor';
import { appInitializer } from './core/appInitializer';

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideHttpClient(withInterceptors([authInterceptor])),
    appInitializer,
    provideRouter(routes),
    provideAuthConfig({
      apiBase: 'https://localhost:5001',
      endpoints: {
        register: '/api/authentication/register',
        login: '/api/authentication/login',
        refreshToken: '/api/tokens/refresh'
      },
      loginPath: 'auth/login',
      registerPath: 'auth/register',
      forgetPasswordPath: 'auth/forget-password',
      home: 'home'
    })
  ]
};
