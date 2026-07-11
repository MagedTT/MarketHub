import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SellerProductsFilterationHeader } from './seller-products-filteration-header';

describe('SellerProductsFilterationHeader', () => {
  let component: SellerProductsFilterationHeader;
  let fixture: ComponentFixture<SellerProductsFilterationHeader>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SellerProductsFilterationHeader],
    }).compileComponents();

    fixture = TestBed.createComponent(SellerProductsFilterationHeader);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
