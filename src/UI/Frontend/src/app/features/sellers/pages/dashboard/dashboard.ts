import { Component, inject, OnDestroy, OnInit, signal, WritableSignal } from '@angular/core';
import { OrderStatusChart } from '../../components/order-status-chart/order-status-chart';
import { PieChart } from '../../components/pie-chart/pie-chart';
import { RatingChart } from '../../components/rating-chart/rating-chart';
import { TopNSellingBrands } from '../../components/top-n-selling-brands/top-n-selling-brands';
import { TotalX } from '../../components/total-x/total-x';
import { Subject, takeUntil } from 'rxjs';
import { SellerOrdersService } from '../../services/seller-orders-service';
import { SessionStoreService } from '../../../../core/services/session-store-service';

@Component({
  selector: 'app-dashboard',
  imports: [TopNSellingBrands, OrderStatusChart, PieChart, RatingChart, TotalX],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css',
})
export class Dashboard implements OnInit, OnDestroy {
  private destroy$ = new Subject<void>();
  private sellerService = inject(SellerOrdersService);
  private session = inject(SessionStoreService);

  totalBrands: WritableSignal<number> = signal(0);
  totalProducts: WritableSignal<number> = signal(0);
  totalReviews: WritableSignal<number> = signal(0);
  totalPromoCodes: WritableSignal<number> = signal(0);
  totalSales: WritableSignal<number> = signal(0);

  ngOnInit(): void {
    const storeId = this.session.user()?.storeId ?? '';

    this.getTotalBrands(storeId);
    this.getTotalProducts(storeId);
    this.getTotalReviews(storeId);
    this.getTotalPromoCodes(storeId);
    this.getTotalSales(storeId);
  }

  getTotalBrands(storeId: string) {
    this.sellerService.getTotalBrands(storeId).pipe(
      takeUntil(this.destroy$)
    ).subscribe(response => this.totalBrands.set(response));
  }

  getTotalProducts(storeId: string) {
    this.sellerService.getTotalProducts(storeId).pipe(
      takeUntil(this.destroy$)
    ).subscribe(response => this.totalProducts.set(response));
  }

  getTotalReviews(storeId: string) {
    this.sellerService.getTotalReviews(storeId).pipe(
      takeUntil(this.destroy$)
    ).subscribe(response => this.totalReviews.set(response));
  }

  getTotalPromoCodes(storeId: string) {
    this.sellerService.getTotalPromoCodes(storeId).pipe(
      takeUntil(this.destroy$)
    ).subscribe(response => this.totalPromoCodes.set(response));
  }

  getTotalSales(storeId: string) {
    this.sellerService.getTotalPromoCodes(storeId).pipe(
      takeUntil(this.destroy$)
    ).subscribe(response => this.totalSales.set(response));
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
