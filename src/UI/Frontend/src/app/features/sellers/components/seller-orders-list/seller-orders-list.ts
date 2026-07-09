import { Component, EventEmitter, inject, Input, Output } from '@angular/core';
import { StoreOrderDto } from '../../models/store-orders-dto.interface';
import { CurrencyPipe, DatePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { OrderStatus } from '../../../orders/models/order-parameters.interface';
import { Pagination } from '../../../../shared/components/pagination/pagination';
import { PaginationMetadata } from '../../../../shared/models/paginationMetadata.interface';
import { OrdersFilterationHeader } from "../../../orders/components/orders-filteration-header/orders-filteration-header";
import { Router } from '@angular/router';

@Component({
  selector: 'app-seller-orders-list',
  imports: [CurrencyPipe, FormsModule, DatePipe, Pagination, OrdersFilterationHeader],
  templateUrl: './seller-orders-list.html',
  styleUrl: './seller-orders-list.css',
})
export class SellerOrdersList {
  @Input() orders: StoreOrderDto[] = [];
  @Input() peginatinoMetaData: PaginationMetadata | null = null;
  @Output() pageChanged = new EventEmitter<number>();
  @Output() orderStatusChanged = new EventEmitter<number>();
  searchQuery: string = '';

  private router = inject(Router);

  convertOrderStatusFromEnumToString(orderStatus: OrderStatus): string {
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

  navigateToOrderDetails(orderId: string) {
    this.router.navigate(['seller-order-details', orderId]);
  }

  onOrderStatusChanged(status: number) {
    this.orderStatusChanged.emit(status);
  }

  onPageChanged(page: number) {
    this.pageChanged.emit(page);
  }

}
