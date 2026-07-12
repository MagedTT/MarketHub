import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SellerPromoCodesList } from './seller-promo-codes-list';

describe('SellerPromoCodesList', () => {
  let component: SellerPromoCodesList;
  let fixture: ComponentFixture<SellerPromoCodesList>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SellerPromoCodesList],
    }).compileComponents();

    fixture = TestBed.createComponent(SellerPromoCodesList);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
