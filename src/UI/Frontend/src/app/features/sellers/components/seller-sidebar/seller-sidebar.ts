import { Component, inject } from '@angular/core';
import { Router, RouterLink, RouterLinkActive, RouterLinkWithHref } from '@angular/router';
import { AuthService } from '../../../../core/services/auth-service';
import { SessionStoreService } from '../../../../core/services/session-store-service';

@Component({
  selector: 'app-seller-sidebar',
  imports: [RouterLinkActive, RouterLink],
  templateUrl: './seller-sidebar.html',
  styleUrl: './seller-sidebar.css',
})
export class SellerSidebar {
  private authService = inject(AuthService);
  private session = inject(SessionStoreService);
  private router = inject(Router);

  navigateToStoreProfile() {
    const storeId = this.session.user()?.storeId ?? '';

    this.router.navigate(['seller-profile', storeId]);
  }

  logout() {
    this.authService.logout();
  }
}
