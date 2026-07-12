import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SellerPromoCodes } from './seller-promo-codes';

describe('SellerPromoCodes', () => {
  let component: SellerPromoCodes;
  let fixture: ComponentFixture<SellerPromoCodes>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SellerPromoCodes],
    }).compileComponents();

    fixture = TestBed.createComponent(SellerPromoCodes);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
