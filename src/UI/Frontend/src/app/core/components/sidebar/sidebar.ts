import { Component, inject, OnDestroy, signal, WritableSignal } from '@angular/core';
import { SessionStoreService } from '../../services/session-store-service';
import { ProductFilterationOptions } from '../../models/product-filteration-options.interface';
import { FormsModule } from '@angular/forms';
import { Subject } from 'rxjs';
import { AuthService } from '../../services/auth-service';
import { ProductSearchStoreService } from '../../../shared/services/product-search-store-service';
import { ProductParameters } from '../../../features/products/models/product-parameters.interface';

@Component({
  selector: 'app-sidebar',
  imports: [FormsModule],
  templateUrl: './sidebar.html',
  styleUrl: './sidebar.css',
})
export class Sidebar implements OnDestroy {
  private destroy$ = new Subject<void>();
  private authService = inject(AuthService);
  private productSearchStoreService = inject(ProductSearchStoreService);
  session = inject(SessionStoreService);

  filters: WritableSignal<ProductFilterationOptions> = signal({
    priceFrom: 0,
    priceTo: 1000,
    ratingFrom: 0,
    ratingTo: 5,
    categories: {
      electronics: false,
      phones: false,
      tvs: false
    }
  });

  applyFilters() {
    let category = '';

    if (this.filters().categories.electronics)
      category = 'electronics';
    else if (this.filters().categories.phones)
      category = 'phones';
    else if (this.filters().categories.tvs)
      category = 'tvs';

    const productParameters: ProductParameters = {
      pageNumber: 1,
      pageSize: 10,
      priceFrom: this.filters().priceFrom,
      priceTo: this.filters().priceTo,
      ratingFrom: this.filters().ratingFrom,
      ratingTo: this.filters().ratingTo,
      category: category
    };

    this.productSearchStoreService.setProductParameters(productParameters);
  }

  resetFilters() {
    this.filters.set({
      priceFrom: 0,
      priceTo: 1000,
      ratingFrom: 0,
      ratingTo: 5,
      categories: {
        electronics: false,
        phones: false,
        tvs: false
      }
    });
  }

  logout() {
    this.authService.logout();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
