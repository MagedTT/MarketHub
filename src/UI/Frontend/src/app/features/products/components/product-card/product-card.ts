import { Component, inject, Input, OnDestroy, signal, WritableSignal } from '@angular/core';
import { ProductCardModel } from '../../models/product-card-model.interface';
import { CurrencyPipe } from '@angular/common';
import { AddToWishListRequest } from '../../models/add-to-wish-list.interface';
import { SessionStoreService } from '../../../../core/services/session-store-service';
import { ProductsService } from '../../services/products-service';
import { FormsModule } from '@angular/forms';
import { CartService } from '../../../../shared/services/cart-service';
import { catchError, Subject, takeUntil, throwError } from 'rxjs';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-product-card',
  standalone: true,
  imports: [CurrencyPipe, FormsModule],
  templateUrl: './product-card.html',
  styleUrl: './product-card.css',
})
export class ProductCard implements OnDestroy {

  @Input({ required: true }) product!: ProductCardModel;
  private cartService = inject(CartService);
  private session = inject(SessionStoreService);
  private productService = inject(ProductsService);
  private destory$ = new Subject<void>();
  selectedQuantity: number = 1;
  alertMessage: WritableSignal<string | null> = signal(null);
  private timeoutId: any = null;
  isSuccess: boolean = false;

  get quantities(): number[] {
    return Array.from(
      { length: this.product.availableAmountInStock - 1 },
      (_, i) => i + 2
    );
  }

  addToWishList(productId: string) {
    const request: AddToWishListRequest = {
      userId: this.session.user()?.id ?? '',
      productId: productId
    };

    this.productService.addProductToWishlist(request).subscribe(response => {
      console.log(response);
    });
  }

  clearAlert(): void {
    if (this.timeoutId) {
      clearTimeout(this.timeoutId);
      this.timeoutId = null;
    }

    this.alertMessage.set(null);
  }

  addToCart(productId: string) {
    const userId = this.session.user()?.id ?? '';

    this.clearAlert();

    this.cartService.addItemToCart(userId, productId, this.selectedQuantity).pipe(
      takeUntil(this.destory$),
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
    this.destory$.next();
    this.destory$.complete();
  }
}
