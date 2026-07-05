import { Component, inject, OnDestroy, OnInit, signal, WritableSignal } from '@angular/core';
import { CartTotal } from '../../components/cart-total/cart-total';
import { CartItems } from '../../components/cart-items/cart-items';
import { SessionStoreService } from '../../../../core/services/session-store-service';
import { CartStore } from '../../../../core/services/stores/cart-store';
import { CartService } from '../../services/cart-service';
import { catchError, Subject, takeUntil, tap, throwError } from 'rxjs';
import { HttpErrorResponse } from '@angular/common/http';
import { CartDto } from '../../models/cart-dto.interface';
import { RemoveCartItemRequest } from '../../models/remove-cart-item-request.interface';
import { UpdateCartItemQuantityRequest } from '../../models/update-cart-item-quantity-request.interface';


@Component({
  selector: 'app-cart',
  imports: [CartItems, CartTotal],
  templateUrl: './cart.html',
  styleUrl: './cart.css',
})
export class Cart implements OnInit, OnDestroy {
  private destroy$ = new Subject<void>();
  private cartService = inject(CartService);
  private cartStore = inject(CartStore);
  private session = inject(SessionStoreService);

  private timeoutId: any = null;
  isSuccess: WritableSignal<boolean> = signal(false);
  message: WritableSignal<string | null> = signal(null);

  total: WritableSignal<number> = signal(0);
  cart: WritableSignal<CartDto | null> = signal(null);

  ngOnInit(): void {
    const userId = this.session.user()?.id ?? '';

    this.getCart(userId);

  }

  getCart(userId: string) {
    this.cartService.getCart(userId).pipe(
      takeUntil(this.destroy$),
      catchError((error: HttpErrorResponse) => {
        return throwError(() => error);
      }),
      tap(cartDto => {
        cartDto.items.forEach(item => this.total.update(value => value + item.subTotal));
      })
    ).subscribe(response => {
      this.cart.set(response);
    });
  }

  removeCartItem(cartItemId: string) {
    const request: RemoveCartItemRequest = {
      userId: this.session.user()?.id ?? '',
      cartId: this.cart()?.cartId ?? '',
      cartItemId: cartItemId,
    }

    this.cartService.removeCartItem(request).pipe(
      takeUntil(this.destroy$),
      catchError((error: HttpErrorResponse) => {
        return throwError(() => error);
      }),
    ).subscribe(response => {
      const cart = this.cart();

      if (cart) {
        this.cart.set({
          ...cart,
          items: cart.items.filter(item => item.cartItemId !== cartItemId)
        });
      }

      this.total.set(0);

      let updatedQuantity: number = 0;

      this.cart()?.items.forEach(item => {
        updatedQuantity += item.quantity;
        this.total.update(value => value + Number(item.subTotal))
      });

      this.cartStore.setCartAmount(updatedQuantity);
    });
  }

  clearAlert() {
    if (this.timeoutId) {
      clearTimeout(this.timeoutId);
      this.timeoutId = null;
    }

    this.message.set(null);
    this.isSuccess.set(false);
  }

  addItemtoCart(productId: string) {
    const userId = this.session.user()?.id ?? '';

    this.cartService.addCartItem(userId, productId, 1).pipe(
      takeUntil(this.destroy$),
      catchError((error: HttpErrorResponse) => {
        this.clearAlert();

        this.isSuccess.set(false);
        this.message.set(error.error?.message || error.error || 'Invalid Quantity');

        this.timeoutId = setTimeout(() => {
          this.clearAlert();
        }, 2500);

        return throwError(() => error);
      })
    ).subscribe({
      next: () => {
        this.clearAlert();

        this.isSuccess.set(true);
        this.message.set('Added Successfully!');

        this.timeoutId = setTimeout(() => {
          this.clearAlert();
        }, 2500);

        this.cartStore.increaseCartAmount(1);
        let price = 0;
        this.cart()?.items.filter(item => {
          if (item.product.productId === productId) {
            price = item.product.productPrice;
          }
        })

        this.cart()?.items.forEach(item => {
          if (item.product.productId === productId) {
            item.quantity += 1;
            item.subTotal += price;
          }
        });

        this.total.update(value => value + price);
      }
    });
  }

  decreaseCartItemQuantity(event: { cartItemId: string, productId: string }) {
    const cartItem = this.cart()?.items.find(item => item.cartItemId === event.cartItemId && item.product.productId === event.productId);
    if (cartItem?.quantity! <= 1) {
      return;
    }

    const request: UpdateCartItemQuantityRequest = {
      cartItemId: event.cartItemId,
      cartId: this.cart()?.cartId ?? '',
      productId: event.productId,
      userId: this.session.user()?.id ?? '',
      quantity: 1
    };

    this.cartService.updateCartItemQuantity(request).pipe(
      takeUntil(this.destroy$),
      catchError((error: HttpErrorResponse) => {
        this.clearAlert();

        this.isSuccess.set(false);
        this.message.set(error.error?.message || error.error || 'Invalid Quantity');

        this.timeoutId = setTimeout(() => {
          this.clearAlert();
        }, 2500);

        return throwError(() => error);
      })
    ).subscribe({
      next: () => {
        this.clearAlert();

        this.isSuccess.set(true);
        this.message.set('Removed Successfully!');

        this.timeoutId = setTimeout(() => {
          this.clearAlert();
        }, 2500);

        this.cartStore.decreaseCartAmount(1);
        let price = 0;
        this.cart()?.items.filter(item => {
          if (item.product.productId === event.productId) {
            price = item.product.productPrice;
          }
        })

        this.total.update(value => value - price);

        this.cart()?.items.forEach(item => {
          if (item.product.productId === event.productId) {
            item.quantity -= 1;
            item.subTotal -= price;
          }
        });
      }
    });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
