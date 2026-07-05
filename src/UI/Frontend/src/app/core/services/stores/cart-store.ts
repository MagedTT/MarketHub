import { computed, Injectable, Signal, signal, WritableSignal } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class CartStore {
  private readonly _cartAmount: WritableSignal<number> = signal(0);

  readonly cartAmount: Signal<number> = computed(() => this._cartAmount());

  decreaseCartAmount(amount: number): void {
    if (this._cartAmount() < amount) {
      this._cartAmount.set(0);
      return;
    }

    this._cartAmount.update(value => value - amount);
  }

  increaseCartAmount(amount: number): void {
    this._cartAmount.update(value => value + Number(amount));
  }

  setCartAmount(amount: number): void {
    this._cartAmount.set(amount);
  }
}
