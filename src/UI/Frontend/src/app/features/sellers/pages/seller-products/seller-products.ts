import { Component, inject, OnDestroy, OnInit, signal, WritableSignal } from '@angular/core';
import { SellerProductsList } from "../../components/seller-products-list/seller-products-list";
import { SessionStoreService } from '../../../../core/services/session-store-service';
import { SellerOrdersService } from '../../services/seller-orders-service';
import { Subject, takeUntil } from 'rxjs';
import { StoreProductsParameters } from '../../models/store-products-parameters.interface';
import { PaginationMetadata } from '../../../../shared/models/paginationMetadata.interface';
import { StoreProductDto } from '../../models/store-product-dto.interface';

@Component({
  selector: 'app-seller-products',
  imports: [SellerProductsList],
  templateUrl: './seller-products.html',
  styleUrl: './seller-products.css',
})
export class SellerProducts implements OnInit, OnDestroy {
  private destroy$ = new Subject<void>();
  private session = inject(SessionStoreService);
  private sellerService = inject(SellerOrdersService);
  private productStatus: WritableSignal<number> = signal(0);

  metaData: WritableSignal<PaginationMetadata | null> = signal(null);
  products: WritableSignal<StoreProductDto[]> = signal([]);

  storeProductParameters: WritableSignal<StoreProductsParameters> = signal({
    pageNumber: 1,
    pageSize: 10,
    orderByProductPrice: true,
    orderByNumberOfReviews: false,
    orderByAverageRating: false,
    orderByNumberOfSoldPieces: false,
    orderByAmountInStock: false,
    descending: true
  });

  ngOnInit(): void {
    const storeId = this.session.user()?.storeId ?? '';

    this.getAllProducts(storeId, this.productStatus(), this.storeProductParameters());
  }

  getAllProducts(storeId: string, productStatus: number, storeProductsParameters: StoreProductsParameters) {
    this.sellerService.getAllProducts(storeId, productStatus, storeProductsParameters).pipe(
      takeUntil(this.destroy$)
    ).subscribe(response => {
      this.products.set(response.items);
      this.metaData.set(response.metadata);
    });
  }

  toggleActivation(event: { productId: string, activate: boolean }) {
    const storeId = this.session.user()?.storeId ?? '';
    if (event.activate) {
      this.sellerService.activateProduct(event.productId, storeId).subscribe(() => {
        this.products.update(products =>
          products.map(product =>
            product.id === event.productId
              ? { ...product, isActive: event.activate }
              : product
          )
        );
      });
    } else {
      this.sellerService.deactivateProduct(event.productId, storeId).subscribe(() => {
        this.products.update(products =>
          products.map(product =>
            product.id === event.productId
              ? { ...product, isActive: event.activate }
              : product
          )
        );
      });
    }
  }

  onProductsStatusChanged(status: number) {
    this.productStatus.set(status);
    const storeId = this.session.user()?.storeId ?? '';

    this.getAllProducts(storeId, this.productStatus(), this.storeProductParameters());
  }

  onPageNumberChanged(pageNumber: number) {
    const storeId = this.session.user()?.storeId ?? '';
    this.storeProductParameters().pageNumber = pageNumber;
    this.getAllProducts(storeId, this.productStatus(), this.storeProductParameters());
  }

  onSortApplied(event: { property: string, descending: boolean }) {
    this.storeProductParameters.set({
      pageNumber: 1,
      pageSize: 10,
      orderByProductPrice: false,
      orderByNumberOfReviews: false,
      orderByAverageRating: false,
      orderByNumberOfSoldPieces: false,
      orderByAmountInStock: false,
      descending: false
    })
    if (event.property === 'price') {
      this.storeProductParameters().orderByProductPrice = true;
    } else if (event.property === 'reviews') {
      this.storeProductParameters().orderByNumberOfReviews = true;
    } else if (event.property === 'rating') {
      this.storeProductParameters().orderByAverageRating = true;
    } else if (event.property === 'AmountInStock') {
      this.storeProductParameters().orderByAmountInStock = true;
    } else if (event.property === 'soldPieces') {
      this.storeProductParameters().orderByNumberOfSoldPieces = true;
    }

    if (event.descending) {
      this.storeProductParameters().descending = true;
    } else {
      this.storeProductParameters().descending = false;
    }

    const storeId = this.session.user()?.storeId ?? '';

    this.getAllProducts(storeId, this.productStatus(), this.storeProductParameters());
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
