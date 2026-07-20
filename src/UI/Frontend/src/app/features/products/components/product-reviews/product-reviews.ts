import { Component, EventEmitter, Input, OnChanges, Output, SimpleChanges } from '@angular/core';
import { PaginationMetadata } from '../../../../shared/models/paginationMetadata.interface';
import { ReviewDto } from '../../models/review-dto.interface';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-product-reviews',
  imports: [DatePipe],
  templateUrl: './product-reviews.html',
  styleUrl: './product-reviews.css',
})
export class ProductReviews implements OnChanges {
  @Input() metaData: PaginationMetadata | null = null;
  @Input() reviews: ReviewDto[] = [];
  @Input() currentUserId: string = '';
  @Output() reviewDeleted = new EventEmitter<string>();
  ngOnChanges(changes: SimpleChanges): void {
    if (changes['reviews'])
      console.log(this.reviews);

    if (changes['currentUserId'])
      console.log(this.currentUserId);
  }

  onReportReview(review: ReviewDto): void {
    console.log('Reporting structural review validation parameter markers:', review.id);
  }

  onDeleteReview(reviewId: string): void {
    this.reviewDeleted.emit(reviewId);
  }
}
