import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ProductParameters } from '../models/product-parameters.interface';
import { map, Observable } from 'rxjs';
import { PagedResult } from '../../../shared/models/pagedResult.interface';
import { ProductCardModel } from '../models/product-card-model.interface';
import { AddToWishListRequest } from '../models/add-to-wish-list.interface';
import { ProductDetailsDto } from '../models/product-details-dto.interface';

@Injectable({
  providedIn: 'root',
})
export class ProductsService {
  constructor(private httpClient: HttpClient) { }

  getProducts(productParameters: ProductParameters): Observable<PagedResult<ProductCardModel>> {
    return this.httpClient.post<ProductCardModel[]>('https://localhost:5001/api/products/productCards',
      productParameters,
      {
        observe: 'response'
      }).pipe(
        map(response => ({
          items: response.body ?? [],
          metadata: JSON.parse(response.headers.get('X-Pagination') ?? '{}')
        }))
      )
  }

  getProductDetails(productId: string): Observable<ProductDetailsDto> {
    return this.httpClient.get<ProductDetailsDto>(`https://localhost:5001/api/products/${productId}`);
  }

  addProductToWishlist(request: AddToWishListRequest): Observable<any> {
    return this.httpClient.post(`https://localhost:5001/api/users/${request.userId}/wishlists`, request);
  }
}
