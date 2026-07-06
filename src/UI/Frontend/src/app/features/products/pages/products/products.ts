import { Component, computed, effect, inject, OnDestroy, OnInit, signal, WritableSignal } from '@angular/core';
import { ProductsService } from '../../services/products-service';
import { ProductParameters } from '../../models/product-parameters.interface';
import { Subject, takeUntil } from 'rxjs';
import { PaginationMetadata } from '../../../../shared/models/paginationMetadata.interface';
import { ProductCardModel } from '../../models/product-card-model.interface';
import { ProductCard } from '../../components/product-card/product-card';
import { Pagination } from "../../../../shared/components/pagination/pagination";
import { Router } from '@angular/router';
import { ProductSearchStoreService } from '../../../../shared/services/product-search-store-service';

@Component({
  selector: 'app-products',
  imports: [ProductCard, Pagination],
  templateUrl: './products.html',
  styleUrl: './products.css',
})
export class Products implements OnInit, OnDestroy {
  private destory$ = new Subject<void>();
  private router = inject(Router);
  private productSearchStoreService = inject(ProductSearchStoreService);

  productCards: WritableSignal<ProductCardModel[]> = signal([]);
  metaData: WritableSignal<PaginationMetadata | null> = signal(null);

  productParameters: ProductParameters = {
    pageNumber: 1,
    pageSize: 10,
    priceFrom: 0,
    priceTo: 1000,
    ratingFrom: 0,
    ratingTo: 5,
    category: 'phones'
  };

  constructor(private productsService: ProductsService) {
    effect(() => {
      const globalParams = this.productSearchStoreService.productParameters();

      this.productParameters = { ...globalParams };

      this.getProductsCards(this.productParameters);
    });
  }

  ngOnInit(): void {
    // this.getProductsCards(this.productParameters);
  }

  getProductsCards(productParameters: ProductParameters) {
    this.productsService.getProducts(productParameters)
      .pipe(
        takeUntil(this.destory$)
      ).subscribe(response => {
        this.metaData.set(response.metadata);
        this.productCards.set(response.items);
      });
  }

  navigateToProductDetails(productId: string) {
    this.router.navigate(['products', productId]);
  }

  onPageChanged(page: number) {
    this.productParameters.pageNumber = page;
    this.getProductsCards(this.productParameters);
  }

  ngOnDestroy(): void {
    this.destory$.next();
    this.destory$.complete();
  }
}
