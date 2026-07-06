import { computed, Injectable, Signal, signal, WritableSignal } from '@angular/core';
import { ProductParameters } from '../../features/products/models/product-parameters.interface';

@Injectable({
  providedIn: 'root',
})
export class ProductSearchStoreService {
  private readonly _productParameters: WritableSignal<ProductParameters> = signal({
    pageNumber: 1,
    pageSize: 10,
    priceFrom: 0,
    priceTo: 1000,
    ratingFrom: 0,
    ratingTo: 5,
    category: ''
  });

  readonly productParameters: Signal<ProductParameters> = computed(() => this._productParameters());
  applyFilters: WritableSignal<boolean> = signal(false);

  setProductParameters(productParameters: ProductParameters): void {
    this._productParameters.set(productParameters);
    this.applyFilters.set(true);
  }

  resetProductParameters(): void {
    this._productParameters.set({
      pageNumber: 1,
      pageSize: 10,
      priceFrom: 0,
      priceTo: 1000,
      ratingFrom: 0,
      ratingTo: 5,
      category: ''
    });
  }
}
