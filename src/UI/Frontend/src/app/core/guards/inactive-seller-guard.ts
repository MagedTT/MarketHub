import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { SessionStoreService } from '../services/session-store-service';

export const inactiveSellerGuard: CanActivateFn = (route, state) => {
  const session = inject(SessionStoreService);
  const router = inject(Router);

  const user = session.user();

  if (!user)
    return router.createUrlTree(['/auth/login']);

  if (user.isActive === 'True')
    return router.createUrlTree(['/seller-dashboard']);

  return true;
};
