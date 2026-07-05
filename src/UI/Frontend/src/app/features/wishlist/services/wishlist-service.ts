import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { wishlistDto } from '../models/wishlist-dto.interface';

@Injectable({
  providedIn: 'root',
})
export class WishlistService {
  constructor(private httpClient: HttpClient) { }

  getWishlist(userId: string): Observable<wishlistDto> {
    return this.httpClient.get<wishlistDto>(`https://localhost:5001/api/users/${userId}/wishlists`);
  }

  removeWishlistItem(obj: { userId: string, wishlistId: string, productId: string }) {
    return this.httpClient.delete(`https://localhost:5001/api/users/${obj.userId}/wishlists`, { body: obj });
  }
}
