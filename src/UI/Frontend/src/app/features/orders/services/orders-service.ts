import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { SessionStoreService } from '../../../core/services/session-store-service';
import { OrderParameters } from '../models/order-parameters.interface';
import { map, Observable } from 'rxjs';
import { PagedResult } from '../../../shared/models/pagedResult.interface';
import { OrderDto } from '../models/order-dto.interface';

@Injectable({
  providedIn: 'root',
})
export class OrdersService {
  private httpClient = inject(HttpClient);
  private session = inject(SessionStoreService);

  getOrders(orderParameters: OrderParameters): Observable<PagedResult<OrderDto>> {
    return this.httpClient.post<OrderDto[]>(`https://localhost:5001/api/users/orders`,
      orderParameters,
      {
        observe: 'response'
      }).pipe(
        map(response => ({
          items: response.body ?? [],
          metadata: JSON.parse(response.headers.get('X-Pagination') ?? '{}')
        }))
      );
  }

  cancelOrder(obj: { userId: string, orderId: string }): Observable<any> {
    return this.httpClient.post<any>(`https://localhost:5001/api/users/${obj.userId}/orders/cancel`, obj);
  }
}
