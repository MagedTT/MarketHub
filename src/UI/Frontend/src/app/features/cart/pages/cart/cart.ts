import { Component } from '@angular/core';
import { CartTotal } from '../../components/cart-total/cart-total';
import { CartItems } from '../../components/cart-items/cart-items';


@Component({
  selector: 'app-cart',
  imports: [CartItems, CartTotal],
  templateUrl: './cart.html',
  styleUrl: './cart.css',
})
export class Cart { }
