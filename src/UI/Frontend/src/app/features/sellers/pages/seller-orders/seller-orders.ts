import { Component, inject, OnDestroy, OnInit, signal, WritableSignal } from '@angular/core';
import { OrderStatusChart } from "../../components/order-status-chart/order-status-chart";
import { SessionStoreService } from '../../../../core/services/session-store-service';
import { SellerOrdersService } from '../../services/seller-orders-service';
import { Subject, takeUntil } from 'rxjs';
import { StoreOrdersParameters } from '../../models/store-orders-parameters.interface';
import { PaginationMetadata } from '../../../../shared/models/paginationMetadata.interface';
import { StoreOrderDto } from '../../models/store-orders-dto.interface';
import { CurrencyPipe, DatePipe } from '@angular/common';
import { OrderStatus } from '../../../orders/models/order-parameters.interface';
import { SellerOrdersList } from "../../components/seller-orders-list/seller-orders-list";

@Component({
  selector: 'app-seller-orders',
  imports: [OrderStatusChart, SellerOrdersList],
  templateUrl: './seller-orders.html',
  styleUrl: './seller-orders.css',
})
export class SellerOrders implements OnInit, OnDestroy {
  private destroy$ = new Subject<void>();
  private session = inject(SessionStoreService);
  private sellerOrdersService = inject(SellerOrdersService);

  metaData: WritableSignal<PaginationMetadata | null> = signal(null);
  orders: WritableSignal<StoreOrderDto[]> = signal([]);

  storeOrdersParametes: StoreOrdersParameters = {
    pageNumber: 1,
    pageSize: 10,
    OrderStatus: null
  };

  ngOnInit(): void {
    const storeId = this.session.user()?.storeId ?? '';
    // console.log(storeId);
    this.getRecentOrders(storeId, this.storeOrdersParametes);
  }

  getRecentOrders(storeId: string, storeOrdersParameters: StoreOrdersParameters) {
    this.sellerOrdersService.getRecentOrders(storeId, storeOrdersParameters).pipe(
      takeUntil(this.destroy$)
    ).subscribe(response => {
      this.metaData.set(response.metadata);
      this.orders.set(response.items);
    });
  }

  onOrderStatusChanged(status: number) {
    const storeId = this.session.user()?.storeId ?? '';
    this.storeOrdersParametes.OrderStatus = status == 0 ? null : status;
    this.getRecentOrders(storeId, this.storeOrdersParametes);
  }

  onPageChanged(page: number) {
    const storeId = this.session.user()?.storeId ?? '';
    this.storeOrdersParametes.pageNumber = page
    this.getRecentOrders(storeId, this.storeOrdersParametes);

  }

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

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
