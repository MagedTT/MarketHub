import { CurrencyPipe } from '@angular/common';
import { Component, inject, Input, OnDestroy, signal, WritableSignal } from '@angular/core';
import { Router, RouterLink } from "@angular/router";
import { SessionStoreService } from '../../../../core/services/session-store-service';
import { CartService } from '../../services/cart-service';
import { catchError, Subject, takeUntil, throwError } from 'rxjs';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-cart-total',
  imports: [CurrencyPipe, RouterLink],
  templateUrl: './cart-total.html',
  styleUrl: './cart-total.css',
})
export class CartTotal implements OnDestroy {
  @Input() total: number = 0;
  @Input() cartId: string = '';
  private router = inject(Router);
  private session = inject(SessionStoreService);
  private cartService = inject(CartService);
  private destroy$ = new Subject<void>();

  errorMessage = signal<any>(null);

  navigateToCheckout() {
    this.clearError();
    const userId = this.session.user()?.id ?? '';

    this.cartService.checkout(userId).pipe(
      takeUntil(this.destroy$),
      catchError((error: HttpErrorResponse) => {
        this.handleErrorResponse(error);
        return throwError(() => error);
      })
    ).subscribe({
      next: (response) => {
        console.log(response);
        this.router.navigate(['/checkout', this.cartId]);
      }
    });
  }

  private handleErrorResponse(error: HttpErrorResponse): void {
    if (error.status === 400 && error.error?.errors) {
      this.errorMessage.set(error.error.errors);
    }
    else if (error.error && typeof error.error === 'string') {
      this.errorMessage.set(error.error);
    } else if (error.error?.message) {
      this.errorMessage.set(error.error.message);
    }
    else {
      this.errorMessage.set('An unexpected transactional processing failure has occurred. Please retry.');
    }
  }

  isValidationErrorArray(value: any): boolean {
    return value && typeof value === 'object' && !Array.isArray(value);
  }

  getValidationErrorArray(value: any): Array<{ key: string; value: string }> {
    if (!value) return [];
    return Object.keys(value).map(key => ({
      key: key,
      value: Array.isArray(value[key]) ? value[key].join(', ') : value[key]
    }));
  }

  clearError(): void {
    this.errorMessage.set(null);
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
