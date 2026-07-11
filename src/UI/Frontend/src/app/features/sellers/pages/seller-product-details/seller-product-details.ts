import { HttpErrorResponse } from '@angular/common/http';
import { Component, inject, OnDestroy, OnInit, signal, WritableSignal } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { catchError, Subject, take, takeUntil, throwError } from 'rxjs';
import { SessionStoreService } from '../../../../core/services/session-store-service';
import { CurrencyPipe, JsonPipe, KeyValuePipe } from '@angular/common';
import { SellerOrdersService } from '../../services/seller-orders-service';
import { StoreProductDetailsDto } from '../../models/store-product-details-dto.interface';

@Component({
  selector: 'app-seller-product-details',
  imports: [CurrencyPipe, KeyValuePipe, JsonPipe],
  templateUrl: './seller-product-details.html',
  styleUrl: './seller-product-details.css',
})
export class SellerProductDetails implements OnInit, OnDestroy {
  private destroy$ = new Subject<void>();
  private activatedRoute = inject(ActivatedRoute);
  private session = inject(SessionStoreService);
  private sellerProductsService = inject(SellerOrdersService);

  private timeoutId: any = null;
  onSale: WritableSignal<boolean> = signal(false);
  productDetails: WritableSignal<StoreProductDetailsDto | null> = signal(null);
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
    const storeId = this.session.user()?.storeId ?? '';
    this.sellerProductsService.getProductDetails(storeId, productId).pipe(
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

  toggleActivation() {
    const storeId = this.session.user()?.storeId;
    if (!this.productDetails()?.isActive) {
      this.sellerProductsService.activateProduct(this.productDetails()?.id ?? '', storeId ?? '').pipe(
        takeUntil(this.destroy$),
        catchError((error: HttpErrorResponse) => {
          return throwError(() => error);
        })
      ).subscribe(response => {
      });
      this.productDetails.update(value => ({
        ...value!,
        isActive: true
      }));
    } else {
      this.sellerProductsService.deactivateProduct(this.productDetails()?.id ?? '', storeId ?? '').pipe(
        takeUntil(this.destroy$),
        catchError((error: HttpErrorResponse) => {
          return throwError(() => error);
        })
      ).subscribe(response => {
      });;
      this.productDetails.update(value => ({
        ...value!,
        isActive: false
      }));
    }
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
