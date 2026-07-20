import { Component, EventEmitter, Input, OnChanges, Output, signal, SimpleChanges } from '@angular/core';
import { PaginationMetadata } from '../../../../shared/models/paginationMetadata.interface';
import { ReviewDto } from '../../models/review-dto.interface';
import { DatePipe, KeyValuePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { CreateReviewCommand } from '../../models/createReviewCommand.interface';
import { ValidationErrors } from '../../../../shared/models/validation-errors.interface';

@Component({
  selector: 'app-product-reviews',
  imports: [DatePipe, KeyValuePipe, FormsModule],
  templateUrl: './product-reviews.html',
  styleUrl: './product-reviews.css',
})
export class ProductReviews implements OnChanges {
  @Input() metaData: PaginationMetadata | null = null;
  @Input() reviews: ReviewDto[] = [];
  @Input() currentUserId: string = '';
  @Input() productId: string = '';
  @Input() serverErrors: ValidationErrors | null = null;
  @Output() reviewDeleted = new EventEmitter<string>();
  @Output() reviewCreated = new EventEmitter<CreateReviewCommand>();

  newReviewRating = signal<number>(0);
  newReviewComment = signal<string>('');
  setRating(stars: number): void {
    this.newReviewRating.set(stars);
  }

  ngOnChanges(changes: SimpleChanges): void {
    // if (changes['serverErrors'])
    // console.log('serverErrors Changed========');
  }

  onSubmitReview(): void {
    if (this.newReviewRating() === 0 || !this.newReviewComment().trim()) return;

    const createReviewCommand: CreateReviewCommand = {
      userId: this.currentUserId,
      productId: this.productId,
      rating: this.newReviewRating(),
      comment: this.newReviewComment()
    };

    this.newReviewRating.set(0);
    this.newReviewComment.set('');

    this.reviewCreated.emit(createReviewCommand);
  }

  onDeleteReview(reviewId: string): void {
    this.reviewDeleted.emit(reviewId);
  }
}
