import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { StoreOrdersParameters } from '../models/store-orders-parameters.interface';
import { map, Observable, tap } from 'rxjs';
import { PagedResult } from '../../../shared/models/pagedResult.interface';
import { StoreOrderDto } from '../models/store-orders-dto.interface';

@Injectable({
  providedIn: 'root',
})
export class SellerOrdersService {
  private httpClient = inject(HttpClient);

  getRecentOrders(storeId: string, orderStatusParameters: StoreOrdersParameters): Observable<PagedResult<StoreOrderDto>> {
    return this.httpClient.post<StoreOrderDto[]>(
      `https://localhost:5001/api/stores/recentorders/${storeId}`,
      orderStatusParameters,
      {
        observe: 'response'
      }).pipe(
        map(response => ({
          items: response.body ?? [],
          metadata: JSON.parse(response.headers.get('X-Pagination') ?? '{}')
        }))
      );
  }

  getOrderDetails(orderId: string, storeId: string): Observable<StoreOrderDto> {
    return this.httpClient.get<StoreOrderDto>(`https://localhost:5001/api/stores/order/${storeId}/${orderId}`);
  }
}
