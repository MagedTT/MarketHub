import { Component, Input } from '@angular/core';
import { OrderDto } from '../../models/order-dto.interface';
import { CurrencyPipe, DatePipe, KeyValuePipe, SlicePipe } from '@angular/common';
import { OrderStatus } from '../../models/order-parameters.interface';
import { TrimPipe } from '../../../../shared/pipes/trim-pipe';

@Component({
  selector: 'app-order-details',
  imports: [DatePipe, CurrencyPipe, KeyValuePipe, SlicePipe, TrimPipe],
  templateUrl: './order-details.html',
  styleUrl: './order-details.css',
})
export class OrderDetails {
  @Input() orders: OrderDto[] = [];

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
}
