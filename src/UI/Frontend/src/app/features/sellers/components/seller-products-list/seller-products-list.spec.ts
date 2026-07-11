import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SellerProductsList } from './seller-products-list';

describe('SellerProductsList', () => {
  let component: SellerProductsList;
  let fixture: ComponentFixture<SellerProductsList>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SellerProductsList],
    }).compileComponents();

    fixture = TestBed.createComponent(SellerProductsList);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
