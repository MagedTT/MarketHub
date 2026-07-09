import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SellerOrdersList } from './seller-orders-list';

describe('SellerOrdersList', () => {
  let component: SellerOrdersList;
  let fixture: ComponentFixture<SellerOrdersList>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SellerOrdersList],
    }).compileComponents();

    fixture = TestBed.createComponent(SellerOrdersList);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
