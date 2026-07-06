import { TestBed } from '@angular/core/testing';

import { ProductSearchStoreService } from './product-search-store-service';

describe('ProductSearchStoreService', () => {
  let service: ProductSearchStoreService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ProductSearchStoreService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
