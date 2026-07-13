import { Component, EventEmitter, inject, Input, Output } from '@angular/core';
import { OrderDto } from '../../models/order-dto.interface';
import { CurrencyPipe, DatePipe, KeyValuePipe, SlicePipe } from '@angular/common';
import { OrderStatus } from '../../models/order-parameters.interface';
import { TrimPipe } from '../../../../shared/pipes/trim-pipe';
import { Router } from '@angular/router';

@Component({
  selector: 'app-order-details',
  imports: [DatePipe, CurrencyPipe, KeyValuePipe, SlicePipe, TrimPipe],
  templateUrl: './order-details.html',
  styleUrl: './order-details.css',
})
export class OrderDetails {
  @Input() orders: OrderDto[] = [];
  @Output() orderCancelled = new EventEmitter<string>();
  private router = inject(Router);

  convertOrderStatusFromEnumToStrign(orderStatus: OrderStatus): string {
    if (orderStatus === OrderStatus.Pending) {
      return 'Pending';
    } else if (orderStatus === OrderStatus.Confirmed) {
      return 'Confirmed';
    } else if (orderStatus === OrderStatus.Cancelled) {
      return 'Cancelled';
    } else if (orderStatus === OrderStatus.Delivered) {
      return 'Delivered';
    } else if (orderStatus === OrderStatus.Shipped) {
      return 'Shipped';
    } return 'Pending';
  }

  onOrderCancelled(orderId: string) {
    this.orderCancelled.emit(orderId);
  }

  navigateToProductDetailsPage(productId: string) {
    this.router.navigate(['products', productId]);
  }
}
