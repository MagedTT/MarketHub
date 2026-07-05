import { inject, Injectable } from '@angular/core';
import { CartStore } from '../../core/services/stores/cart-store';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class CartService {
  private httpClient = inject(HttpClient);

  addItemToCart(userId: string, productId: string, quantity: number): Observable<any> {
    return this.httpClient.post(`https://localhost:5001/api/users/${userId}/carts/addcartitem/${productId}?quantity=${quantity}`, {});
  }

  getAmountInCart(userId: string): Observable<number> {
    return this.httpClient.get<number>(`https://localhost:5001/api/users/${userId}/carts/amount`);
  }
}
