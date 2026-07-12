import { CommonModule } from '@angular/common';
import { Component, EventEmitter, inject, OnDestroy, Output, signal, WritableSignal } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { catchError, Subject, takeUntil, throwError } from 'rxjs';
import { SessionStoreService } from '../../../core/services/session-store-service';
import { SellerOrdersService } from '../../../features/sellers/services/seller-orders-service';
import { CreatePromoCode } from '../../models/promo-code-create.interface';
import { HttpErrorResponse } from '@angular/common/http';


export interface PromoCreateInput {
  code: string;
  discountType: number;
  discountValue: number;
  endDate: Date;
  usageLimit: number;
  isActive: boolean;
}

@Component({
  selector: 'app-create-promo-code-modal',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './create-promo-code-modal.html',
  styleUrl: './create-promo-code-modal.css',
})
export class CreatePromoCodeModal implements OnDestroy {
  private destroy$ = new Subject<void>();
  private session = inject(SessionStoreService);
  private sellerService = inject(SellerOrdersService);

  isOpen = signal<boolean>(false);

  codeError: WritableSignal<string | null> = signal(null);
  discountTypeError: WritableSignal<string | null> = signal(null);
  discountValueError: WritableSignal<string | null> = signal(null);
  endDateError: WritableSignal<string | null> = signal(null);
  usageLimitError: WritableSignal<string | null> = signal(null);

  createForm!: FormGroup;

  @Output() created = new EventEmitter<void>();

  constructor(private fb: FormBuilder) {
    this.createForm = this.fb.group({
      code: ['', [Validators.required, Validators.minLength(8), Validators.maxLength(8), Validators.pattern('^[a-zA-Z0-9]*$')]],
      discountType: [1, [Validators.required]],
      discountValue: [1, [Validators.required, Validators.min(1), Validators.max(100)]],
      endDate: ['', [Validators.required]],
      usageLimit: [100, [Validators.required, Validators.min(1)]],
      isActive: [true]
    });
  }

  open(): void {
    this.createForm.reset({
      code: '',
      discountType: 1,
      discountValue: 0,
      endDate: new Date(Date.now() + 86400000).toISOString().split('T')[0],
      usageLimit: 100,
      isActive: true
    });

    this.isOpen.set(true);
  }

  onSave(): void {
    this.codeError.set(null);
    this.discountTypeError.set(null);
    this.discountValueError.set(null);
    this.endDateError.set(null);
    this.usageLimitError.set(null);

    const code = this.createForm.get('code')?.value;
    const discountType = this.createForm.get('discountType')?.value;
    const discountValue = this.createForm.get('discountValue')?.value;
    const endDate = new Date(this.createForm.get('endDate')?.value);
    const usageLimit = this.createForm.get('usageLimit')?.value;
    const isActive = this.createForm.get('isActive')?.value;


    if (!(1 <= usageLimit && usageLimit <= 100)) {
      this.usageLimitError.set('Invalid usage limit');
    }

    if (endDate.getTime() <= new Date().getTime()) {
      this.endDateError.set('Invalid expiration date');
    }

    if (!(1 <= discountValue && discountValue <= 100)) {
      this.usageLimitError.set('Invalid discount value');
    }

    const request: CreatePromoCode = {
      storeId: this.session.user()?.storeId ?? '',
      code: code,
      discountType: discountType,
      discountValue: discountValue,
      endDate: endDate,
      usageLimit: usageLimit,
      isActive: isActive
    }

    this.sellerService.createPromoCode(request).pipe(
      takeUntil(this.destroy$),
      catchError((error: HttpErrorResponse) => {
        return throwError(() => error);
      })
    ).subscribe(response => {
      this.isOpen.set(false);
      this.created.emit();
    });
  }

  onCancel(): void {
    this.isOpen.set(false);
  }

  generateRandomCode(): void {
    const chars = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789';

    let result = '';

    for (let i = 0; i < 8; i++) {
      result += chars.charAt(Math.floor(Math.random() * chars.length));
    }

    this.createForm.patchValue({ code: result });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
