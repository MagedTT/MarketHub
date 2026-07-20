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
import { ReviewDto } from '../../models/review-dto.interface';
import { PaginationMetadata } from '../../../../shared/models/paginationMetadata.interface';
import { ReviewsListRequest } from '../../models/ReviewsListRequest.interface';
import { ProductReviews } from "../../components/product-reviews/product-reviews";

@Component({
  selector: 'app-product-details',
  imports: [CurrencyPipe, KeyValuePipe, FormsModule, ProductReviews],
  templateUrl: './product-details.html',
  styleUrl: './product-details.css',
})
export class ProductDetails implements OnInit, OnDestroy {
  private destroy$ = new Subject<void>();
  private productsService = inject(ProductsService);
  private activatedRoute = inject(ActivatedRoute);
  private cartService = inject(CartService);
  private cartStore = inject(CartStore);
  session = inject(SessionStoreService);

  private timeoutId: any = null;
  onSale: WritableSignal<boolean> = signal(false);
  productDetails: WritableSignal<ProductDetailsDto | null> = signal(null);
  reviews: WritableSignal<ReviewDto[]> = signal([]);
  reviewsMetaData: WritableSignal<PaginationMetadata | null> = signal(null);
  selectedImageUrl: WritableSignal<string | null> = signal(null);
  selectedQuantity: number = 1;
  alertMessage: WritableSignal<string | null> = signal(null);
  isSuccess: WritableSignal<boolean> = signal(false);


  reviewsRequest: ReviewsListRequest = {
    productId: '',
    requestParameters: {
      pageNumber: 1,
      pageSize: 10
    },
    trackChanges: false
  };


  ngOnInit(): void {
    this.activatedRoute.paramMap.pipe(
      takeUntil(this.destroy$)
    ).subscribe(params => {
      const productId = params.get('id');

      if (productId) {
        this.getProductDetails(productId);

        this.reviewsRequest.productId = productId;
        this.getProductReviews(this.reviewsRequest);
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

  getProductReviews(request: ReviewsListRequest) {
    this.productsService.getProductReviews(request).pipe(
      takeUntil(this.destroy$)
    ).subscribe(response => {
      this.reviews.set(response.items);
      this.reviewsMetaData.set(response.metadata);
      console.log(this.reviews());
      console.log(this.reviewsMetaData());
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

  onReviewDeleted(reviewId: string) {
    this.productsService.deleteReview(reviewId).pipe(
      takeUntil(this.destroy$),
      catchError((error: HttpErrorResponse) => {
        return throwError(() => error);
      })
    ).subscribe(() => {
      this.reviews.update(reviews =>
        reviews.filter(review => review.id !== reviewId)
      )
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
