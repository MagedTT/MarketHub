import { CurrencyPipe } from '@angular/common';
import { Component } from '@angular/core';

@Component({
  selector: 'app-cart-items',
  imports: [CurrencyPipe],
  templateUrl: './cart-items.html',
  styleUrl: './cart-items.css',
})
export class CartItems { }
