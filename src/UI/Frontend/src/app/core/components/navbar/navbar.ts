import { Component, inject, signal, WritableSignal } from '@angular/core';
import { Router } from '@angular/router';
import { AUTH_CONFIG } from '../../models/auth.config';
import { SessionStoreService } from '../../services/session-store-service';
import { CartStore } from '../../services/stores/cart-store';

@Component({
  selector: 'app-navbar',
  imports: [],
  templateUrl: './navbar.html',
  styleUrl: './navbar.css',
})
export class Navbar {
  // constructor(private router: Router, @Inject(AUTH_CONFIG) private config: AuthConfig) { }
  private router = inject(Router);
  private config = inject(AUTH_CONFIG);
  cartStore = inject(CartStore);
  private session = inject(SessionStoreService);

  navigateToSignIn() {
    this.router.navigateByUrl(this.config.loginPath);
  }

  navigateToWishlist() {
    this.router.navigate(['wishlist', this.session.user()?.id]);
  }
}
