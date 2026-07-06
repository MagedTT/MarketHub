import { Component, EventEmitter, inject, OnDestroy, OnInit, Output, signal, WritableSignal } from '@angular/core';
import { ProductsService } from '../../services/products-service';
import { ActivatedRoute } from '@angular/router';
import { catchError, Subject, takeUntil, throwError } from 'rxjs';
import { ProductDetailsDto } from '../../models/product-details-dto.interface';
import { HttpErrorResponse } from '@angular/common/http';
import { CurrencyPipe, JsonPipe, KeyValuePipe } from '@angular/common';
import { SessionStoreService } from '../../../../core/services/session-store-service';
import { CartService } from '../../../../shared/services/cart-service';
import { CartStore } from '../../../../core/services/stores/cart-store';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-product-details',
  imports: [CurrencyPipe, KeyValuePipe, FormsModule],
  templateUrl: './product-details.html',
  styleUrl: './product-details.css',
})
export class ProductDetails implements OnInit, OnDestroy {
  private destroy$ = new Subject<void>();
  private productsService = inject(ProductsService);
  private activatedRoute = inject(ActivatedRoute);
  private session = inject(SessionStoreService);
  private cartService = inject(CartService);
  private cartStore = inject(CartStore);

  private timeoutId: any = null;
  onSale: WritableSignal<boolean> = signal(false);
  productDetails: WritableSignal<ProductDetailsDto | null> = signal(null);
  selectedImageUrl: WritableSignal<string | null> = signal(null);
  selectedQuantity: number = 1;
  alertMessage: WritableSignal<string | null> = signal(null);
  isSuccess: WritableSignal<boolean> = signal(false);


  ngOnInit(): void {
    this.activatedRoute.paramMap.pipe(
      takeUntil(this.destroy$)
    ).subscribe(params => {
      const productId = params.get('id');

      if (productId) {
        this.getProductDetails(productId);
      }
    });
  }

  getProductDetails(productId: string) {
    this.productsService.getProductDetails(productId).pipe(
      takeUntil(this.destroy$),
      catchError((error: HttpErrorResponse) => {
        return throwError(() => error);
      })
    ).subscribe(response => {
      this.productDetails.set(response);
      this.selectedImageUrl.set(this.productDetails()?.imagesUrls.length ? this.productDetails()?.imagesUrls[0]! : null);
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
      takeUntil(this.destroy$),
      catchError((error: HttpErrorResponse) => {
        const message = error.error?.message || error.error || 'Invalid Quantity';

        this.alertMessage.set(message);

        this.isSuccess.set(false);

        this.timeoutId = setTimeout(() => {
          this.clearAlert();
        }, 2000);

        return throwError(() => error);
      })
    ).subscribe({
      next: () => {
        this.cartStore.increaseCartAmount(this.selectedQuantity);
        this.clearAlert();
        this.alertMessage.set('Added Successfully!');
        this.isSuccess.set(true);
        this.timeoutId = setTimeout(() => {
          this.clearAlert();
        }, 2000);
      }
    });
  }

  increaseSelectedQuantity() {
    this.selectedQuantity += 1;
  }

  decreaseSelectedQuantity() {
    if (this.selectedQuantity > 1) {
      this.selectedQuantity -= 1;
    }
  }

  setActiveImage(imageUrl: string): void {
    this.selectedImageUrl.set(imageUrl);
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
