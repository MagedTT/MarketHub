import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OrdersFilterationHeader } from './orders-filteration-header';

describe('OrdersFilterationHeader', () => {
  let component: OrdersFilterationHeader;
  let fixture: ComponentFixture<OrdersFilterationHeader>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [OrdersFilterationHeader],
    }).compileComponents();

    fixture = TestBed.createComponent(OrdersFilterationHeader);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
