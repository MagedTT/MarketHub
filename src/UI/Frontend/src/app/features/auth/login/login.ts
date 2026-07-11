import { Component, Inject, signal, WritableSignal } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../../../core/services/auth-service';
import { Router } from '@angular/router';
import { AUTH_CONFIG, AuthConfig } from '../../../core/models/auth.config';
import { catchError, throwError } from 'rxjs';
import { HttpErrorResponse } from '@angular/common/http';
import { TokenService } from '../../../core/services/token-service';

@Component({
  selector: 'app-login',
  imports: [ReactiveFormsModule, FormsModule],
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class Login {
  loginForm!: FormGroup;
  serverErrorMessage: WritableSignal<string | null> = signal(null);
  private timeoutId: any = null;

  constructor(
    private authService: AuthService,
    private tokenService: TokenService,
    private router: Router,
    private fb: FormBuilder,
    @Inject(AUTH_CONFIG) private config: AuthConfig
  ) {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required]],
      password: ['', [Validators.required]]
    });
  }

  navigateToRegisterPage() {
    this.router.navigateByUrl(this.config.registerPath);
  }

  clearErrorMessage(): void {
    if (this.timeoutId) {
      clearTimeout(this.timeoutId);
      this.timeoutId = null;
    }
    this.serverErrorMessage.set(null);
  }

  setValues() {
    this.loginForm.setValue({
      email: 'maged922001@gmail.com',
      password: 'Admin@123'
    });
  }

  onSubmit() {
    if (this.loginForm.invalid) return;

    this.clearErrorMessage();

    this.authService.login({
      email: this.loginForm.value.email!,
      password: this.loginForm.value.password!
    }).pipe(
      catchError((error: HttpErrorResponse) => {
        const userMessage = error.error?.message || error.error || 'An unexpected error occurred. Please try again.';

        this.serverErrorMessage.set(userMessage);

        this.timeoutId = setTimeout(() => {
          this.clearErrorMessage();
        }, 10000);

        return throwError(() => error);
      })).subscribe(result => {
        // console.log(`Access Token: ${result.accessToken}`);
        // console.log(`Refresh Token: ${result.refreshToken}`);

        const decodedToken = this.tokenService.decodeAccessToken(result.accessToken);

        if (decodedToken?.storeId !== '00000000-0000-0000-0000-000000000000') {
          console.log(`decodedToken?.storeId !== null: ${decodedToken?.storeId}`);
          this.router.navigateByUrl('seller-dashboard');
        } else {
          console.log(`decodedToken?.storeId === null`);
          this.router.navigateByUrl('products');
        }
      });
  }
}
