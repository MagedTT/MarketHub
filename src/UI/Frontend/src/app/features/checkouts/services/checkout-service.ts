import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CreateOrderCommand } from '../models/create-order-command.interface';

@Injectable({
  providedIn: 'root',
})
export class CheckoutService {
  private httpClient = inject(HttpClient);

  checkPromoCodeValidity(promoCode: string): Observable<number> {
    return this.httpClient.post<number>(`https://localhost:5001/api/promoCodes/checkCodeValidity/${promoCode}`, {});
  }

  getTotalPriceInCart(userId: string, cartId: string): Observable<number> {
    return this.httpClient.get<number>(`https://localhost:5001/api/users/${userId}/carts/totalPrice/${cartId}`);
  }

  createOrder(userId: string, request: CreateOrderCommand): Observable<any> {
    return this.httpClient.post<any>(`https://localhost:5001/api/users/${userId}/orders/create`, request);
  }
}
