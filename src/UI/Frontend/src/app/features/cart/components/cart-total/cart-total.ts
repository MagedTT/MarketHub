import { CurrencyPipe } from '@angular/common';
import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-cart-total',
  imports: [CurrencyPipe],
  templateUrl: './cart-total.html',
  styleUrl: './cart-total.css',
})
export class CartTotal {
  @Input() total: number = 0;
}
