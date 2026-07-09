import { Component, inject, OnDestroy, OnInit, signal, WritableSignal } from '@angular/core';
import { SessionStoreService } from '../../../../core/services/session-store-service';
import { SellerOrdersService } from '../../services/seller-orders-service';
import { ActivatedRoute } from '@angular/router';
import { catchError, Subject, take, takeUntil, throwError } from 'rxjs';
import { StoreOrderDto } from '../../models/store-orders-dto.interface';
import { HttpErrorResponse } from '@angular/common/http';
import { OrderStatus } from '../../../orders/models/order-parameters.interface';
import { CurrencyPipe, DatePipe } from '@angular/common';

@Component({
  selector: 'app-seller-order-details',
  imports: [DatePipe, CurrencyPipe],
  templateUrl: './seller-order-details.html',
  styleUrl: './seller-order-details.css',
})
export class SellerOrderDetails implements OnInit, OnDestroy {
  private destroy$ = new Subject<void>();
  private session = inject(SessionStoreService);
  private storeOrdersService = inject(SellerOrdersService);
  private activatedRoute = inject(ActivatedRoute);
  private orderId: WritableSignal<string> = signal('');

  order: WritableSignal<StoreOrderDto | null> = signal(null);


  ngOnInit(): void {
    const storeId = this.session.user()?.storeId ?? '';

    this.activatedRoute.paramMap.subscribe(params => this.orderId.set(params.get('id') ?? ''));

    this.getOrderDetails(this.orderId(), storeId);
  }

  getOrderDetails(orderId: string, storeId: string) {
    this.storeOrdersService.getOrderDetails(orderId, storeId).pipe(
      takeUntil(this.destroy$),
      catchError((error: HttpErrorResponse) => {
        return throwError(() => error);
      })
    ).subscribe(response => {
      this.order.set(response);
    });
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
