import { Component, EventEmitter, Input, Output } from '@angular/core';
import { PaginationMetadata } from '../../../../shared/models/paginationMetadata.interface';
import { PromoCodeDto } from '../../models/promocode-dto.interface';
import { DatePipe } from '@angular/common';
import { Pagination } from "../../../../shared/components/pagination/pagination";
import { PromoCodeEditModel } from '../../../../shared/models/promo-code-edit.interface';

@Component({
  selector: 'app-seller-promo-codes-list',
  imports: [DatePipe, Pagination],
  templateUrl: './seller-promo-codes-list.html',
  styleUrl: './seller-promo-codes-list.css',
})
export class SellerPromoCodesList {
  @Input() metaData: PaginationMetadata | null = null;
  @Input() promoCodes: PromoCodeDto[] = [];
  @Output() pageChanged = new EventEmitter<number>();
  @Output() sortApplied = new EventEmitter<{ property: string, descending: boolean }>();
  @Output() promoCodeEdited = new EventEmitter<PromoCodeEditModel>();
  @Output() promoCodeActiveStatusChanged = new EventEmitter<{ promoCodeId: string, promoCodeActiveStatus: boolean }>();
  @Output() promoCodeCreated = new EventEmitter<void>();

  onPageChanged(pageNumber: number) {
    this.pageChanged.emit(pageNumber);
  }

  onPromoCodeCreare() {
    this.promoCodeCreated.emit();
  }

  onEditPromoCode(promoCode: PromoCodeDto) {
    const promoCodeToEdit: PromoCodeEditModel = {
      id: promoCode.id,
      code: promoCode.code,
      endDate: promoCode.endDate,
      discountValue: promoCode.discountValue,
      usageLimit: promoCode.usageLimit,
      numberOfTimesUsed: promoCode.numberOfTimesUsed,
      isActive: promoCode.isActive
    }
    this.promoCodeEdited.emit(promoCodeToEdit);
  }

  onSortApplied(property: string, descending: boolean) {
    this.sortApplied.emit({ property, descending });
  }

  onToggleStatus(promoCodeId: string, promoCodeActiveStatus: boolean) {
    this.promoCodeActiveStatusChanged.emit({ promoCodeId, promoCodeActiveStatus });
  }

  navigateToAddPromoCodePage() {

  }
}
