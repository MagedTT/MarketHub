import { Component, inject, OnDestroy, OnInit, signal, ViewChild, WritableSignal } from '@angular/core';
import { catchError, Subject, takeUntil, throwError } from 'rxjs';
import { SessionStoreService } from '../../../../core/services/session-store-service';
import { SellerOrdersService } from '../../services/seller-orders-service';
import { PromoCodeParameters } from '../../models/promocode-parameters.interface';
import { PaginationMetadata } from '../../../../shared/models/paginationMetadata.interface';
import { PromoCodeDto } from '../../models/promocode-dto.interface';
import { HttpErrorResponse } from '@angular/common/http';
import { SellerPromoCodesList } from "../../components/seller-promo-codes-list/seller-promo-codes-list";
import { ConfirmationModal } from "../../../../shared/components/confirmation-modal/confirmation-modal";
import { EditPromoCodeModal } from '../../../../shared/components/edit-promo-code-modal/edit-promo-code-modal';
import { PromoCodeEditModel } from '../../../../shared/models/promo-code-edit.interface';
import { PromoCodeUpate } from '../../models/promo-code-update.interface';
import { isActive } from '@angular/router';
import { CreatePromoCodeModal } from "../../../../shared/components/create-promo-code-modal/create-promo-code-modal";
import { CreatePromoCode } from '../../../../shared/models/promo-code-create.interface';

@Component({
  selector: 'app-seller-promo-codes',
  imports: [SellerPromoCodesList, ConfirmationModal, EditPromoCodeModal, CreatePromoCodeModal],
  templateUrl: './seller-promo-codes.html',
  styleUrl: './seller-promo-codes.css',
})
export class SellerPromoCodes implements OnInit, OnDestroy {
  private destroy$ = new Subject<void>();
  private session = inject(SessionStoreService);
  private sellerService = inject(SellerOrdersService);
  private promoCodeIdToToggleActivation: WritableSignal<string> = signal('');

  @ViewChild('confirmationModal') confirmationModal!: ConfirmationModal;
  @ViewChild('editPromoModal') editPromoModal!: EditPromoCodeModal;

  @ViewChild('createPromoModal') createPromoModal!: CreatePromoCodeModal;

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

  onPromoCodeActiveStatusChaged(event: { promoCodeId: string, promoCodeActiveStatus: boolean }) {
    this.promoCodeIdToToggleActivation.set(event.promoCodeId);
    if (event.promoCodeActiveStatus) {
      this.sellerService.checkPromoCodeValidity(event.promoCodeId).pipe(
        takeUntil(this.destroy$),
        catchError((error: HttpErrorResponse) => {
          return throwError(() => error);
        })
      ).subscribe(response => {
        if (response) {
          this.confirmationModal.open({
            title: 'Confirm Deactivation',
            message: 'This promo code is still valid, are you sure you want to deactivate?',
            confirmText: 'Confirm',
            cancelText: 'Cancell',
            isDanger: true
          });
        } else {
          console.log('no');
        }
      });
    } else {
      this.sellerService.activatePromoCode(this.promoCodeIdToToggleActivation()).pipe(
        takeUntil(this.destroy$),
        catchError((error: HttpErrorResponse) => {
          alert(error.error);
          return throwError(() => error);
        })
      ).subscribe(response => {
        this.promoCodes.update(promocodes =>
          promocodes.map(promocode =>
            promocode.id === this.promoCodeIdToToggleActivation()
              ? { ...promocode, isActive: true }
              : promocode
          )
        );

        this.promoCodeIdToToggleActivation.set('');
      });
    }
  }

  onPromoCodeUpdated(updatedPromoCode: PromoCodeUpate) {
    this.promoCodes.update(promoCodes =>
      promoCodes.map(promoCode =>
        promoCode.id === updatedPromoCode.promoCodeId
          ? {
            ...promoCode,
            endDate: updatedPromoCode.endDate,
            discountValue: updatedPromoCode.discountValue,
            usageLimit: updatedPromoCode.usageLimit,
            isActive: updatedPromoCode.isActive,
          }
          : promoCode
      )
    );
  }
  openCreateModal(): void {
    this.createPromoModal.open();
  }

  onPromoCodeCreated() {
    this.getAllPromoCodes();
  }

  onPromoCodeEdit(promoCode: PromoCodeEditModel) {
    this.editPromoModal.open(promoCode);
  }

  onConfirmed() {
    this.sellerService.deactivatePromoCode(this.promoCodeIdToToggleActivation()).pipe(
      takeUntil(this.destroy$)
    ).subscribe(response => {
      this.promoCodes.update(promocodes =>
        promocodes.map(promocode =>
          promocode.id === this.promoCodeIdToToggleActivation()
            ? { ...promocode, isActive: false }
            : promocode
        )
      );

      this.promoCodeIdToToggleActivation.set('');
    });
  }

  onCancelled() {
    console.log('cancelled');
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
