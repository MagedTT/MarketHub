import { Component, EventEmitter, inject, Input, output, Output } from '@angular/core';
import { StoreProductDto } from '../../models/store-product-dto.interface';
import { PaginationMetadata } from '../../../../shared/models/paginationMetadata.interface';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Pagination } from "../../../../shared/components/pagination/pagination";
import { TrimPipe } from '../../../../shared/pipes/trim-pipe';
import { Router } from '@angular/router';
import { SellerProductsFilterationHeader } from '../seller-products-filteration-header/seller-products-filteration-header';
import { SessionStoreService } from '../../../../core/services/session-store-service';

@Component({
  selector: 'app-seller-products-list',
  imports: [FormsModule, CommonModule, Pagination, TrimPipe, SellerProductsFilterationHeader],
  templateUrl: './seller-products-list.html',
  styleUrl: './seller-products-list.css',
})
export class SellerProductsList {
  @Input() products: StoreProductDto[] = [];
  @Input() metaData: PaginationMetadata | null = null;
  @Output() pageNumberChanged = new EventEmitter<number>();
  @Output() sortApplied = new EventEmitter<{ property: string, descending: boolean }>();
  @Output() toggleProductActivation = new EventEmitter<{ productId: string, activate: boolean }>();
  @Output() productsStatusChanged = new EventEmitter<number>();
  private router = inject(Router);
  private session = inject(SessionStoreService);

  searchQuery: string = '';

  navigateToProductDetails(productId: string) {
    this.router.navigate(['seller-product-details', productId]);
  }

  navigateToAddProductPage() {
    this.router.navigate(['add-product', this.session.user()?.storeId]);
  }

  onProductsStatusChanged(status: number) {
    this.productsStatusChanged.emit(status);
  }

  onPageChanged(pageNumber: number) {
    this.pageNumberChanged.emit(pageNumber);
  }

  activate(productId: string) {
    this.toggleProductActivation.emit({ productId, activate: true });
  }

  deactivate(productId: string) {
    this.toggleProductActivation.emit({ productId, activate: false });
  }

  sortBy(property: string = 'price', descending: boolean = false) {
    this.sortApplied.emit({ property: property, descending: descending });
  }
}
