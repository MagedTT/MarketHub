import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CartDto } from '../models/cart-dto.interface';
import { RemoveCartItemRequest } from '../models/remove-cart-item-request.interface';
import { UpdateCartItemQuantityRequest } from '../models/update-cart-item-quantity-request.interface';

@Injectable({
  providedIn: 'root',
})
export class CartService {
  private httpClient = inject(HttpClient);

  getCart(userId: string): Observable<CartDto> {
    return this.httpClient.get<CartDto>(`https://localhost:5001/api/users/${userId}/carts`);
  }

  addCartItem(userId: string, productId: string, quantity: number): Observable<any> {
    return this.httpClient.post<any>(`https://localhost:5001/api/users/${userId}/carts/addcartitem/${productId}?quantity=${quantity}`, {});
  }

  removeCartItem(request: RemoveCartItemRequest): Observable<any> {
    return this.httpClient.delete<any>(`https://localhost:5001/api/users/${request.userId}/carts/removecartitem`, { body: request });
  }

  updateCartItemQuantity(request: UpdateCartItemQuantityRequest): Observable<any> {
    return this.httpClient.put<any>(`https://localhost:5001/api/users/${request.userId}/carts/updateCartItemQuantity`, request);
  }

  checkout(userId: string): Observable<any> {
    return this.httpClient.post<any>(`https://localhost:5001/api/users/${userId}/orders/checkout`, {});
  }
}
