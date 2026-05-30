import { Component, DestroyRef, inject, OnInit } from '@angular/core';
import { BasketService } from '../../../services/basket.service';
import { IBasket } from '../../../shared/models/basket.model';
import { CurrencyPipe } from '@angular/common';


@Component({
  selector: 'app-basket',
  standalone: true,
  imports: [CurrencyPipe],
  templateUrl: './basket.component.html',
  styleUrl: './basket.component.scss',
})
export class BasketComponent implements OnInit {
  private basketService = inject(BasketService);
  private destroyRef = inject(DestroyRef);

  basket: IBasket = this.basketService.currentValue;

  ngOnInit(): void {
    // this.basket.basketID = localStorage.getItem('basketID');
    // this.basketService.getBasketById('')
  }

  get totalAmount() {
    return this.basket.basketItems.reduce((accumulator, currentItem) => {
      return accumulator + currentItem.price * currentItem.quantity;
    }, 0);
  }
}
