import { CurrencyPipe } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CartItemDto } from '../../models/cart-item-dto.interface';
import { UpdateCartItemQuantityRequest } from '../../models/update-cart-item-quantity-request.interface';

@Component({
  selector: 'app-cart-items',
  imports: [CurrencyPipe],
  templateUrl: './cart-items.html',
  styleUrl: './cart-items.css',
})
export class CartItems {
  @Input() cartItems: CartItemDto[] = [];
  @Output() cartItemRemoved = new EventEmitter<string>();
  @Output() cartItemAdded = new EventEmitter<string>();
  @Output() cartItemQuantityDecreased = new EventEmitter<{ cartItemId: string, productId: string }>();

  removeCartItem(cartItemId: string) {
    this.cartItemRemoved.emit(cartItemId);
  }

  addItemToCart(productId: string) {
    this.cartItemAdded.emit(productId);
  }

  decreaseCartItemQuantity(cartItemId: string, productId: string) {
    const request = {
      cartItemId,
      productId
    }

    this.cartItemQuantityDecreased.emit(request);
  }
}
