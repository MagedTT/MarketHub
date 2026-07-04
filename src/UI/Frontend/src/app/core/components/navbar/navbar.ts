import { Component, Inject } from '@angular/core';
import { Router } from '@angular/router';
import { AUTH_CONFIG, AuthConfig } from '../../models/auth.config';

@Component({
  selector: 'app-navbar',
  imports: [],
  templateUrl: './navbar.html',
  styleUrl: './navbar.css',
})
export class Navbar {
  constructor(private router: Router, @Inject(AUTH_CONFIG) private config: AuthConfig) { }

  navigateToSignIn() {
    this.router.navigateByUrl(this.config.loginPath);
  }
}
