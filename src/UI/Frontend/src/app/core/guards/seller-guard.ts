import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { SessionStoreService } from '../services/session-store-service';

export const sellerGuard: CanActivateFn = (route, state) => {
  const session = inject(SessionStoreService);
  const router = inject(Router);

  const user = session.user();

  if (!user) {
    return router.createUrlTree(['/auth/login']);
  }

  if (user.storeId === null) {
    return router.createUrlTree(['/']);
  }

  return true;
};
