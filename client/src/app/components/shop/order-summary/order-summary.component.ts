import { CurrencyPipe } from '@angular/common';
import { Component, input } from '@angular/core';
import { BasketTotal } from '../../../shared/models/basket-total.model';

@Component({
  selector: 'app-order-summary',
  standalone: true,
  imports: [CurrencyPipe],
  templateUrl: './order-summary.component.html',
  styleUrl: './order-summary.component.scss'
})
export class OrderSummaryComponent {
basketTotal = input.required<BasketTotal>();

}
