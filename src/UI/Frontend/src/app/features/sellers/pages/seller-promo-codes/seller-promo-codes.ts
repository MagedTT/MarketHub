import { Component, inject, OnDestroy, OnInit, signal, WritableSignal } from '@angular/core';
import { catchError, Subject, takeUntil, throwError } from 'rxjs';
import { SessionStoreService } from '../../../../core/services/session-store-service';
import { SellerOrdersService } from '../../services/seller-orders-service';
import { PromoCodeParameters } from '../../models/promocode-parameters.interface';
import { PaginationMetadata } from '../../../../shared/models/paginationMetadata.interface';
import { PromoCodeDto } from '../../models/promocode-dto.interface';
import { HttpErrorResponse } from '@angular/common/http';
import { SellerPromoCodesList } from "../../components/seller-promo-codes-list/seller-promo-codes-list";

@Component({
  selector: 'app-seller-promo-codes',
  imports: [SellerPromoCodesList],
  templateUrl: './seller-promo-codes.html',
  styleUrl: './seller-promo-codes.css',
})
export class SellerPromoCodes implements OnInit, OnDestroy {
  private destroy$ = new Subject<void>();
  private session = inject(SessionStoreService);
  private sellerService = inject(SellerOrdersService);

  metaData: WritableSignal<PaginationMetadata | null> = signal(null);
  promoCodes: WritableSignal<PromoCodeDto[]> = signal([]);

  promoCodeParameters: PromoCodeParameters = {
    pageNumber: 1,
    pageSize: 10,
    usageLimitMin: 0,
    usageLimitMax: 1000,
    orderByDiscountValue: false,
    orderByStartDate: true,
    orderByEndDate: false,
    orderByUsageLimit: false,
    orderByNumberOfTimesUsed: false,
    descending: false,
  }

  ngOnInit(): void {
    this.getAllPromoCodes();
  }

  getAllPromoCodes() {
    const storeId = this.session.user()?.storeId ?? '';

    this.promoCodeParameters.storeId = storeId;

    this.sellerService.getPromoCodes(this.promoCodeParameters).pipe(
      takeUntil(this.destroy$),
      catchError((error: HttpErrorResponse) => {
        return throwError(() => error);
      })
    ).subscribe(response => {
      this.metaData.set(response.metadata);
      this.promoCodes.set(response.items);
    })
  }

  onPageChanged(pageNumber: number) {
    this.promoCodeParameters.pageNumber = pageNumber;
    this.getAllPromoCodes();
  }

  onSortApplied(event: { property: string, descending: boolean }) {
    this.promoCodeParameters.descending = event.descending;
    this.promoCodeParameters.orderByDiscountValue = false;
    this.promoCodeParameters.orderByStartDate = false
    this.promoCodeParameters.orderByEndDate = false;
    this.promoCodeParameters.orderByUsageLimit = false;
    this.promoCodeParameters.orderByNumberOfTimesUsed = false;
    this.promoCodeParameters.descending = event.descending;

    if (event.property === 'value') {
      this.promoCodeParameters.orderByDiscountValue = true;
    } else if (event.property === 'enddate') {
      this.promoCodeParameters.orderByEndDate = true;
    } else if (event.property === 'usageLimit') {
      this.promoCodeParameters.orderByUsageLimit = true;
    } else if (event.property === 'timesUsed') {
      this.promoCodeParameters.orderByNumberOfTimesUsed = true;
    }

    this.getAllPromoCodes();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
