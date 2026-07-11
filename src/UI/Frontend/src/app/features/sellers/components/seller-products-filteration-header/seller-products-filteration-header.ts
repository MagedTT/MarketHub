import { Component, EventEmitter, Output } from '@angular/core';

@Component({
  selector: 'app-seller-products-filteration-header',
  imports: [],
  templateUrl: './seller-products-filteration-header.html',
  styleUrl: './seller-products-filteration-header.css',
})
export class SellerProductsFilterationHeader {
  @Output() productsStatusChanged = new EventEmitter<number>();
  selectedFilter: string = 'all';

  onProductsStatusChanged(status: number) {
    this.selectedFilter = this.convertStatusNumberToString(status);
    this.productsStatusChanged.emit(status);
  }

  convertStatusNumberToString(status: number): string {
    if (status == 0) return 'all';
    else if (status == 1) return 'active';
    else if (status == 2) return 'inactive';
    return 'all';
  }
}
