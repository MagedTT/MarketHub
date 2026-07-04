import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PaginationMetadata } from '../../models/paginationMetadata.interface';


@Component({
  selector: 'app-pagination',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './pagination.html',
  styleUrl: './pagination.css'
})
export class Pagination {

  // Makes standard Math functions accessible natively inside your template fields
  protected readonly Math = Math;

  @Input() paginationMetaData: PaginationMetadata | null = null;
  @Output() pageChanged = new EventEmitter<number>();

  onPageChange(targetPage: number): void {
    if (targetPage >= 1 && targetPage <= this.paginationMetaData!.TotalPages && targetPage !== this.paginationMetaData!.CurrentPage) {
      this.pageChanged.emit(targetPage);
    }
  }

  /**
   * Generates a structural pagination map index. 
   * Truncates extra layout fields using a '-1' array integer index flag to represent an ellipsis (...).
   */
  getVisiblePages(): number[] {
    const current = this.paginationMetaData!.CurrentPage;
    const total = this.paginationMetaData!.TotalPages;
    const pages: number[] = [];

    // Fallback block optimization if page scale count is too small to split
    if (total <= 5) {
      for (let i = 1; i <= total; i++) pages.push(i);
      return pages;
    }

    // Always append our anchor point page option 1
    pages.push(1);

    // Compute contextual sliding boundary range maps
    let start = Math.max(2, current - 1);
    let end = Math.min(total - 1, current + 1);

    if (current <= 2) {
      end = 4;
    } else if (current >= total - 1) {
      start = total - 3;
    }

    // Append ellipsis indicators inside split gaps cleanly
    if (start > 2) {
      pages.push(-1); // Flag inserts an element break spacer
    }

    for (let i = start; i <= end; i++) {
      pages.push(i);
    }

    if (end < total - 1) {
      pages.push(-1);
    }

    // Always append final terminal total boundary index choice option
    pages.push(total);

    return pages;
  }
}
