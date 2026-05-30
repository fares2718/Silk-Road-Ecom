import { Component, DestroyRef, inject, OnInit } from '@angular/core';
import { BasketService } from '../../../services/basket.service';
import { IBasket } from '../../../shared/models/basket.model';
import { CurrencyPipe } from '@angular/common';
import { RouterLink } from '@angular/router';
import { OrderSummaryComponent } from '../order-summary/order-summary.component';
import { BasketTotal } from '../../../shared/models/basket-total.model';

@Component({
  selector: 'app-basket',
  standalone: true,
  imports: [CurrencyPipe, RouterLink, OrderSummaryComponent],
  templateUrl: './basket.component.html',
  styleUrl: './basket.component.scss',
})
export class BasketComponent implements OnInit {
  private basketService = inject(BasketService);
  private destroyRef = inject(DestroyRef);

  basket: IBasket;

  ngOnInit(): void {
    this.basketService.basket$.subscribe({
      next: (value) => (this.basket = value),
      error: (err) => console.log(err),
    });
  }

  changeQuantity(amount: 1 | -1, itemId: number) {
    const index = this.basket.basketItems.findIndex((i) => i.itemID === itemId);
    if (index !== -1) {
      if (!(this.basket.basketItems[index].quantity == 0 && amount == -1)) {
        this.basket.basketItems[index].quantity += amount;
        this.basketService.addUpdateBasket(this.basket);
      }
    }
    //
  }

  deleteItem(itemId: number) {
    this.basket.basketItems = this.basket.basketItems.filter(
      (bi) => bi.itemID !== itemId,
    );
    this.basketService.addUpdateBasket(this.basket);
  }

  get basketTotal() {
    const basketTotal = new BasketTotal();
    
    basketTotal.subTotal = this.basket.basketItems.reduce(
      (accumulator, currentItem) => {
        return accumulator + currentItem.price * currentItem.quantity;
      },
      0,
    );
    basketTotal.shipping = 3;
    basketTotal.tax = 25
    return basketTotal;
  }
}
