import { Component, inject, OnDestroy, OnInit, signal, WritableSignal } from '@angular/core';
import { OrdersFilterationHeader } from "../../components/orders-filteration-header/orders-filteration-header";
import { OrderDetails } from "../../components/order-details/order-details";
import { catchError, Subject, takeUntil, throwError } from 'rxjs';
import { OrderParameters, OrderStatus } from '../../models/order-parameters.interface';
import { OrdersService } from '../../services/orders-service';
import { SessionStoreService } from '../../../../core/services/session-store-service';
import { HttpErrorResponse } from '@angular/common/http';
import { PaginationMetadata } from '../../../../shared/models/paginationMetadata.interface';
import { OrderDto } from '../../models/order-dto.interface';
import { Pagination } from '../../../../shared/components/pagination/pagination';

@Component({
  selector: 'app-orders-list',
  imports: [OrdersFilterationHeader, OrderDetails, Pagination],
  templateUrl: './orders-list.html',
  styleUrl: './orders-list.css',
})
export class OrdersList implements OnInit, OnDestroy {
  private destroy$ = new Subject<void>();

  private ordersService = inject(OrdersService);
  private session = inject(SessionStoreService);

  orders: WritableSignal<OrderDto[]> = signal([]);
  metaData: WritableSignal<PaginationMetadata | null> = signal(null);

  orderParameters: OrderParameters = {
    pageNumber: 1,
    pageSize: 10,
    orderByCreationTimeDescending: true,
    orderMinTotalPrice: 0,
    orderMaxTotalPrice: 100_000
  };

  ngOnInit(): void {
    this.getOrders(this.orderParameters);
  }

  getOrders(orderParameters: OrderParameters) {
    orderParameters.userId = this.session.user()?.id;

    this.ordersService.getOrders(orderParameters).pipe(
      takeUntil(this.destroy$),
      catchError((error: HttpErrorResponse) => {
        return throwError(() => error);
      })
    ).subscribe(response => {
      console.log("response.items: ", response.items);
      this.metaData.set(response.metadata);
      this.orders.set(response.items);
    });
  }

  onPageChanged(pageNumber: number) {
    this.orderParameters.pageNumber = pageNumber;
    this.getOrders(this.orderParameters);
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
