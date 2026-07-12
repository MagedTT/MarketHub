import { Component, EventEmitter, Input, Output } from '@angular/core';
import { PaginationMetadata } from '../../../../shared/models/paginationMetadata.interface';
import { PromoCodeDto } from '../../models/promocode-dto.interface';
import { DatePipe } from '@angular/common';
import { Pagination } from "../../../../shared/components/pagination/pagination";

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

  onPageChanged(pageNumber: number) {
    this.pageChanged.emit(pageNumber);
  }

  onSortApplied(property: string, descending: boolean) {
    this.sortApplied.emit({ property, descending });
  }

  onToggleStatus(promoCodeStatus: boolean) {

  }

  navigateToAddPromoCodePage() {

  }
}
