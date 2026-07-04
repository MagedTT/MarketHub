import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { TokenService } from '../services/token-service';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { SessionStoreService } from '../services/session-store-service';
import { AuthService } from '../services/auth-service';
import { catchError, from, switchMap, throwError } from 'rxjs';
import { AUTH_CONFIG } from '../models/auth.config';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const tokenService = inject(TokenService);
  const session = inject(SessionStoreService);
  const router = inject(Router);
  const config = inject(AUTH_CONFIG);

  const addToken = (request: typeof req) => {
    const accessToken = tokenService.getAccessToken();

    return accessToken ? req.clone({ setHeaders: { Authorization: `Bearer ${accessToken}` } }) : request;
  };

  return next(addToken(req)).pipe(
    catchError((errorResponse: HttpErrorResponse) => {
      if (errorResponse.status === 401) {
        return from(tokenService.refreshAccessToken()).pipe(
          switchMap(success => {
            if (success) {
              session.updateUser();
              return next(addToken(req));
            }

            tokenService.clearTokens();
            session.clearUser();
            router.navigateByUrl(config.home);
            return throwError(() => errorResponse);
          })
        )
      }

      return throwError(() => errorResponse);
    })
  );
};
