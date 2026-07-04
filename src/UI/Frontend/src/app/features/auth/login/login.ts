import { Component, Inject, signal, WritableSignal } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../../../core/services/auth-service';
import { Router } from '@angular/router';
import { AUTH_CONFIG, AuthConfig } from '../../../core/models/auth.config';
import { catchError, throwError } from 'rxjs';
import { HttpErrorResponse } from '@angular/common/http';

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
        console.log(`Access Token: ${result.accessToken}`);
        console.log(`Refresh Token: ${result.refreshToken}`);
        this.router.navigateByUrl('products');
      });
  }
}
