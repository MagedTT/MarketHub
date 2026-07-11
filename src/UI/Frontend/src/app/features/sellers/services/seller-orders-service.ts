import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { StoreOrdersParameters } from '../models/store-orders-parameters.interface';
import { map, Observable, tap } from 'rxjs';
import { PagedResult } from '../../../shared/models/pagedResult.interface';
import { StoreOrderDto } from '../models/store-orders-dto.interface';
import { StoreProductsParameters } from '../models/store-products-parameters.interface';
import { StoreProductDto } from '../models/store-product-dto.interface';
import { StoreProductDetailsDto } from '../models/store-product-details-dto.interface';
import { BrandDto } from '../models/brand-dto.interface';
import { CategoryDto } from '../models/category-dto.interface';

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

  getAllProducts(storeId: string, productStatus: number, storeProductsParameters: StoreProductsParameters): Observable<PagedResult<StoreProductDto>> {
    return this.httpClient.post<StoreProductDto[]>(
      `https://localhost:5001/api/stores/allproducts/${storeId}?productStatus=${productStatus}`,
      storeProductsParameters,
      {
        observe: 'response'
      }
    ).pipe(
      map(response => ({
        items: response.body ?? [],
        metadata: JSON.parse(response.headers.get('X-Pagination') ?? '{}')
      }))
    )
  }

  getProductDetails(storeId: string, productId: string): Observable<StoreProductDetailsDto> {
    console.log(`https://localhost:5001/api/stores/productDetails/${storeId}/${productId}`);
    return this.httpClient.get<StoreProductDetailsDto>(`https://localhost:5001/api/stores/productDetails/${storeId}/${productId}`);
  }

  activateProduct(productId: string, storeId: string): Observable<any> {
    return this.httpClient.post<any>(`https://localhost:5001/api/products/activate/${productId}/${storeId}`, {});
  }

  deactivateProduct(productId: string, storeId: string): Observable<any> {
    return this.httpClient.post<any>(`https://localhost:5001/api/products/deactivate/${productId}/${storeId}`, {});
  }

  getBrandsWithIdsAndNames(): Observable<BrandDto[]> {
    return this.httpClient.get<BrandDto[]>('https://localhost:5001/api/brands/brandsWithIdsAndNames');
  }

  getCategories(): Observable<CategoryDto[]> {
    return this.httpClient.get<CategoryDto[]>('https://localhost:5001/api/products/allCategories');
  }

  createProduct(formData: FormData): Observable<any> {
    return this.httpClient.post<any>(`https://localhost:5001/api/products`, formData);
  }
}
