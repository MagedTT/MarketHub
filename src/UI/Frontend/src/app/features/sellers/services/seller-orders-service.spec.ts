import { TestBed } from '@angular/core/testing';

import { SellerOrdersService } from './seller-orders-service';

describe('SellerOrdersService', () => {
  let service: SellerOrdersService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(SellerOrdersService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
