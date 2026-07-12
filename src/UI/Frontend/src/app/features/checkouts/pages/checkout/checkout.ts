import { Component, computed, inject, OnDestroy, OnInit, signal, WritableSignal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CheckoutService } from '../../services/checkout-service';
import { catchError, Subject, takeUntil, throwError } from 'rxjs';
import { HttpErrorResponse } from '@angular/common/http';
import { SessionStoreService } from '../../../../core/services/session-store-service';
import { ActivatedRoute } from '@angular/router';
import { CreateOrderCommand } from '../../models/create-order-command.interface';

@Component({
  selector: 'app-checkout',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './checkout.html',
  styleUrl: './checkout.css',
})
export class Checkout implements OnInit, OnDestroy {

  private destroy$ = new Subject<void>();
  private session = inject(SessionStoreService);
  private checkoutService = inject(CheckoutService);
  private activatedRoute = inject(ActivatedRoute);
  promoCode?: string | null = null;
  // 1. Data Model mapping your backend C# ShippingAddress class structure
  address = {
    fullName: '',
    phoneNumber: '',
    country: 'Egypt',
    governorate: '',
    city: '',
    street: '',
    buildingNumber: '',
    floor: '',
    apartment: '',
    postalCode: '',
    isDefault: false
  };

  // 2. Checkout Calculation Signals
  cartTotalPrice: WritableSignal<number> = signal(0);
  subtotal = signal<number>(0);
  shippingCost = signal<number>(15.00);
  promoCodeString = '';

  isPromoApplied = signal<boolean>(false);
  promoMessage = signal<string>('');
  discountValue = signal<number>(0);
  discountType = signal<string>('Percentage'); // 'Percentage' or 'FixedAmount'

  totalDiscount = computed(() => {
    if (!this.isPromoApplied()) return 0;

    const discount =
      this.discountType() === 'Percentage'
        ? (this.subtotal() * this.discountValue()) / 100
        : this.discountValue();

    return Number(discount.toFixed(2));
  });

  grandTotal = computed(() => {
    return this.subtotal() - this.totalDiscount(); // + this.shippingCost();
  });

  ngOnInit(): void {
    const cartId = this.activatedRoute.snapshot.paramMap.get('id') ?? '';
    this.getCartTotalPrice(cartId);
  }

  checkPromoCodeValidity() {
    this.checkoutService.checkPromoCodeValidity(this.promoCode?.trim() ?? '').pipe(
      takeUntil(this.destroy$),
      catchError((error: HttpErrorResponse) => {
        this.isPromoApplied.set(false);
        this.promoMessage.set('Invalid, expired, or structural coupon code limit reached.');
        return throwError(() => error);
      })
    ).subscribe(response => {
      this.discountValue.set(response);
      this.isPromoApplied.set(true);
    });
  }

  getCartTotalPrice(cartId: string) {
    const userId = this.session.user()?.id ?? '';
    this.checkoutService.getTotalPriceInCart(userId, cartId).pipe(
      takeUntil(this.destroy$)
    ).subscribe(response => {
      this.subtotal.set(response);
    });
  }

  removeCoupon(): void {
    this.isPromoApplied.set(false);
    this.promoCodeString = '';
    this.discountValue.set(0);
    this.promoMessage.set('');
  }

  onSubmit(): void {
    const userId = this.session.user()?.id ?? '';
    const request: CreateOrderCommand = {
      userId: userId,
      promoCode: this.promoCode,
      total: this.grandTotal(),
      shippingAddress: {
        userId: userId,
        fullName: this.address.fullName,
        phoneNumber: this.address.phoneNumber,
        country: this.address.country,
        governorate: this.address.governorate,
        city: this.address.city,
        street: this.address.street,
        buildingNumber: this.address.buildingNumber,
        floor: this.address.floor,
        apartment: this.address.apartment,
        postalCode: this.address.postalCode,
        isDefault: this.address.isDefault
      }
    }

    this.checkoutService.createOrder(userId, request).pipe(
      takeUntil(this.destroy$),
      catchError((error: HttpErrorResponse) => {
        return throwError(() => error);
      })
    ).subscribe(response => {
      console.log(response);
    });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
