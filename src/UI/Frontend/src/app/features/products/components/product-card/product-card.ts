import { Component, inject, Input } from '@angular/core';
import { ProductCardModel } from '../../models/product-card-model.interface';
import { CurrencyPipe } from '@angular/common';
import { AddToWishListRequest } from '../../models/add-to-wish-list.interface';
import { SessionStoreService } from '../../../../core/services/session-store-service';
import { ProductsService } from '../../services/products-service';

@Component({
  selector: 'app-product-card',
  standalone: true,
  imports: [CurrencyPipe],
  templateUrl: './product-card.html',
  styleUrl: './product-card.css',
})
export class ProductCard {

  @Input({ required: true }) product!: ProductCardModel;
  private session = inject(SessionStoreService);
  private productService = inject(ProductsService);

  get quantities(): number[] {
    return Array.from(
      { length: this.product.availableAmountInStock },
      (_, i) => i + 1
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
}
