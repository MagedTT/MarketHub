import { CommonModule } from '@angular/common';
import { Component, EventEmitter, inject, OnDestroy, Output, signal, WritableSignal } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { PromoCodeEditModel } from '../../models/promo-code-edit.interface';
import { catchError, Subject, takeUntil, throwError } from 'rxjs';
import { SellerOrdersService } from '../../../features/sellers/services/seller-orders-service';
import { SessionStoreService } from '../../../core/services/session-store-service';
import { PromoCodeUpate } from '../../../features/sellers/models/promo-code-update.interface';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-edit-promo-code-modal',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './edit-promo-code-modal.html',
  styleUrl: './edit-promo-code-modal.css',
})
export class EditPromoCodeModal implements OnDestroy {
  private destory$ = new Subject<void>();
  private sellerService = inject(SellerOrdersService);
  private session = inject(SessionStoreService);

  isOpen = signal<boolean>(false);
  currentPromo = signal<PromoCodeEditModel | null>(null);
  promoForm!: FormGroup;
  discountValueError: WritableSignal<string | null> = signal(null);
  usageLimitError: WritableSignal<string | null> = signal(null);
  endDateError: WritableSignal<string | null> = signal(null);

  @Output() saved = new EventEmitter<PromoCodeUpate>();
  @Output() cancelled = new EventEmitter<void>();

  constructor(private fb: FormBuilder) {
    this.promoForm = this.fb.group({
      discountValue: [0, [Validators.required, Validators.min(0), Validators.max(100)]],
      numberOfTimesUsed: [{ value: 0, disabled: true }],
      usageLimit: [1, [Validators.required, Validators.min(1)]],
      endDate: ['', [Validators.required]],
      isActive: [false]
    });
  }

  open(promo: PromoCodeEditModel): void {
    this.currentPromo.set(promo);

    this.promoForm.patchValue({
      id: promo.id,
      code: promo.code,
      endDate: new Date(promo.endDate),
      discountValue: promo.discountValue,
      usageLimit: promo.usageLimit,
      numberOfTimesUsed: promo.numberOfTimesUsed,
      isActive: promo.isActive
    });

    this.promoForm.get('usageLimit')?.setValidators([
      Validators.required,
      Validators.min(promo.numberOfTimesUsed)
    ]);

    this.promoForm.get('discountValue')?.setValidators([
      Validators.required,
      Validators.min(0),
      Validators.max(100)
    ]);

    this.promoForm.get('enddDate')?.setValidators([Validators.required]);

    this.isOpen.set(true);
  }

  onSave(): void {
    console.log(this.promoForm.value);

    this.endDateError.set(null);
    this.usageLimitError.set(null);
    this.discountValueError.set(null);

    const dicountValue = this.promoForm.get('discountValue')?.value;
    const usageLimit = this.promoForm.get('usageLimit')?.value;
    const numberOfTimesUsed = this.promoForm.get('numberOfTimesUsed')?.value;
    const endDate = new Date(this.promoForm.get('endDate')?.value);
    const isActive = this.promoForm.get('isActive')?.value;

    if (usageLimit <= numberOfTimesUsed) {
      this.usageLimitError.set('Usage Limit must be greater than number of times used');
    }
    if (endDate.getTime() <= Date.now()) {
      this.endDateError.set('End Date must be greater than now');
    }
    if (endDate.getTime() <= new Date(this.currentPromo()?.endDate!).getTime()) {
      this.endDateError.set('End Date must be greater than old Date');
    }
    if (isActive && usageLimit <= numberOfTimesUsed) {
      this.usageLimitError.set('Promo code must be in active');
    }
    if (!(1 <= dicountValue && dicountValue <= 100)) {
      this.discountValueError.set('Invalid discount value');
    }

    const request: PromoCodeUpate = {
      storeId: this.session.user()?.storeId ?? '',
      promoCodeId: this.currentPromo()?.id ?? '',
      discountValue: dicountValue,
      usageLimit: usageLimit,
      endDate: endDate,
      isActive: isActive
    };

    this.sellerService.updatePromoCode(request).pipe(
      takeUntil(this.destory$),
      catchError((error: HttpErrorResponse) => {
        return throwError(() => error);
      })
    ).subscribe(response => {
      this.isOpen.set(false);
      this.saved.emit(request);
      console.log(response);
    })
  }

  onCancel(): void {
    this.isOpen.set(false);
    this.cancelled.emit();
  }

  ngOnDestroy(): void {
    this.destory$.next();
    this.destory$.complete();
  }
}
