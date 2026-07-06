import { Component, EventEmitter, Output } from '@angular/core';

@Component({
  selector: 'app-orders-filteration-header',
  imports: [],
  templateUrl: './orders-filteration-header.html',
  styleUrl: './orders-filteration-header.css',
})
export class OrdersFilterationHeader {
  @Output() orderStatusChanged = new EventEmitter<number>();
  selectedFilter: string = 'All';

  orderStatusToFetchChanged(status: number) {
    this.selectedFilter = this.convertStatusNumberToString(status);
    this.orderStatusChanged.emit(status);
  }

  convertStatusNumberToString(status: number): string {
    if (status == 0) return 'All';
    else if (status == 1) return 'Pending';
    else if (status == 2) return 'Confirmed';
    else if (status == 3) return 'Shipped';
    else if (status == 4) return 'Delivered';
    else if (status == 5) return 'Cancelled';
    return 'All';
  }
}