import { Component, EventEmitter, Input, Output } from '@angular/core';
import { wishlistDto } from '../../models/wishlist-dto.interface';
import { CurrencyPipe } from '@angular/common';

@Component({
  selector: 'app-wishlist-card',
  imports: [CurrencyPipe],
  templateUrl: './wishlist-card.html',
  styleUrl: './wishlist-card.css',
})
export class WishlistCard {
  @Input() wishlist: wishlistDto | null = null;
  @Output() onWishlistItemRemoval = new EventEmitter<{ wishlistId: string, productId: string }>();


  removeItemFromWishlist(wishlistId: string, productId: string) {
    this.onWishlistItemRemoval.emit({ wishlistId, productId });
  }
}
