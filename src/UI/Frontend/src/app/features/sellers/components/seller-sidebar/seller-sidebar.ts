import { Component, inject } from '@angular/core';
import { Router, RouterLink, RouterLinkActive, RouterLinkWithHref } from '@angular/router';
import { AuthService } from '../../../../core/services/auth-service';

@Component({
  selector: 'app-seller-sidebar',
  imports: [RouterLinkActive, RouterLink],
  templateUrl: './seller-sidebar.html',
  styleUrl: './seller-sidebar.css',
})
export class SellerSidebar {
  private authService = inject(AuthService);

  logout() {
    this.authService.logout();
  }
}
