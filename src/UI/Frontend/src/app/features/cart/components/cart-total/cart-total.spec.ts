import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CartTotal } from './cart-total';

describe('CartTotal', () => {
  let component: CartTotal;
  let fixture: ComponentFixture<CartTotal>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CartTotal],
    }).compileComponents();

    fixture = TestBed.createComponent(CartTotal);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
