import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SellerProductDetails } from './seller-product-details';

describe('SellerProductDetails', () => {
  let component: SellerProductDetails;
  let fixture: ComponentFixture<SellerProductDetails>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SellerProductDetails],
    }).compileComponents();

    fixture = TestBed.createComponent(SellerProductDetails);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
