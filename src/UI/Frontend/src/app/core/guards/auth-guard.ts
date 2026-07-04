import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { TokenService } from '../services/token-service';
import { AUTH_CONFIG } from '../models/auth.config';

export const authGuard: CanActivateFn = (route, state) => {
  const tokenService = inject(TokenService);
  const router = inject(Router);
  const config = inject(AUTH_CONFIG);

  const accessToken = tokenService.getAccessToken();

  if (!accessToken) {
    router.navigateByUrl(config.loginPath);
    return false;
  }

  return true;
};
