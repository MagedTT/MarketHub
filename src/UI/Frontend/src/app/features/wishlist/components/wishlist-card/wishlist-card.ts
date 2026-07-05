import { Component, EventEmitter, inject, Input, OnChanges, OnDestroy, Output, signal, SimpleChanges, WritableSignal } from '@angular/core';
import { wishlistDto } from '../../models/wishlist-dto.interface';
import { CurrencyPipe } from '@angular/common';
import { CartService } from '../../../../shared/services/cart-service';
import { SessionStoreService } from '../../../../core/services/session-store-service';
import { catchError, Subject, takeUntil, throwError } from 'rxjs';
import { HttpErrorResponse } from '@angular/common/http';
import { CartStore } from '../../../../core/services/stores/cart-store';

@Component({
  selector: 'app-wishlist-card',
  imports: [CurrencyPipe],
  templateUrl: './wishlist-card.html',
  styleUrl: './wishlist-card.css',
})
export class WishlistCard implements OnChanges, OnDestroy {

  private cartService = inject(CartService);
  private cartStore = inject(CartStore);
  private destroy$ = new Subject<void>();
  private session = inject(SessionStoreService);
  @Input() wishlist: wishlistDto | null = null;
  @Output() onWishlistItemRemoval = new EventEmitter<{ wishlistId: string, productId: string }>();
  alertMessage: WritableSignal<string | null> = signal(null);
  activeItemId: WritableSignal<string | null> = signal(null);
  private timeoutId: any = null;
  isSuccess: boolean = true;

  ngOnChanges(changes: SimpleChanges): void {
    // console.log(changes);
  }

  clearAlert(): void {
    if (this.timeoutId) {
      clearTimeout(this.timeoutId);
      this.timeoutId = null;
    }

    this.alertMessage.set(null);
  }

  removeItemFromWishlist(wishlistId: string, productId: string) {
    this.onWishlistItemRemoval.emit({ wishlistId, productId });
  }

  addToCart(productId: string) {
    this.activeItemId.set(productId);

    const userId = this.session.user()?.id ?? '';

    this.cartService.addItemToCart(userId, productId, 1).pipe(
      takeUntil(this.destroy$),
      catchError((error: HttpErrorResponse) => {
        const message = error.error?.message || error.error || 'Invalid Quantity';

        this.alertMessage.set(message);

        this.isSuccess = false;

        this.timeoutId = setTimeout(() => {
          this.clearAlert();
        }, 2000);

        return throwError(() => error);
      })
    ).subscribe({
      next: () => {
        this.cartStore.increaseCartAmount(1);
        this.clearAlert();
        this.alertMessage.set('Added Successfully!');
        this.isSuccess = true;
        this.timeoutId = setTimeout(() => {
          this.clearAlert();
        }, 2000);
      }
    });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
