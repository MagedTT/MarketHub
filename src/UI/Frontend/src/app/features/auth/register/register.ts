import { Component, Inject, signal, WritableSignal } from '@angular/core';
import { Router } from '@angular/router';
import { AUTH_CONFIG, AuthConfig } from '../../../core/models/auth.config';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { AuthService } from '../../../core/services/auth-service';
import { catchError, throwError } from 'rxjs';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-register',
  imports: [ReactiveFormsModule],
  templateUrl: './register.html',
  styleUrl: './register.css',
})
export class Register {
  registerForm!: FormGroup;
  success: WritableSignal<string | null> = signal(null);
  serverErrorMessage: WritableSignal<string | null> = signal(null);
  private timeoutId: any = null;
  private timeoutId2: any = null;

  constructor(
    private authService: AuthService,
    private router: Router,
    private fb: FormBuilder,
    @Inject(AUTH_CONFIG) private config: AuthConfig) {
    this.registerForm = this.fb.group({
      firstName: ['', [Validators.required]],
      lastName: ['', [Validators.required]],
      userName: ['', [Validators.required]],
      email: ['', [Validators.required]],
      permission: ['buyer', [Validators.required]],
      role: ['', [Validators.required]],
      password: ['', [Validators.required]],
      phoneNumber: ['', [Validators.required]],
      confirmPassword: ['', [Validators.required]]
    });
  }

  navigateToSignIn() {
    this.router.navigateByUrl(this.config.loginPath);
  }

  clearErrorMessage(): void {
    if (this.timeoutId) {
      clearTimeout(this.timeoutId);
      this.timeoutId = null;
    }

    this.serverErrorMessage.set(null);
  }

  clearSuccessMessage(): void {
    if (this.timeoutId2) {
      clearTimeout(this.timeoutId2);
      this.timeoutId2 = null;
    }

    this.success.set(null);
  }

  onSubmit() {
    if (this.registerForm.invalid) return;

    this.clearErrorMessage();

    const formValue = this.registerForm.getRawValue();

    this.authService.register(this.registerForm.value).pipe(
      catchError((error: HttpErrorResponse) => {
        const userMessage = error.error?.message || error.error || 'An unexpected error occurred. Please try again.';

        this.serverErrorMessage.set(userMessage);

        this.timeoutId = setTimeout(() => {
          this.clearErrorMessage();
        }, 10000);

        return throwError(() => error);
      })
    ).subscribe(result => {
      this.success.set(result.message);
    });
  }

  setValues() {
    this.registerForm.setValue({
      firstName: 'ahmed',
      lastName: 'ail',
      userName: 'ahmed394',
      email: 'magedt922001@gmail.com',
      role: 'buyer',
      password: 'Admin@123',
      phoneNumber: '0155000002',
      permission: 'seller',
      confirmPassword: 'Admin@123'
    });
  }
}
