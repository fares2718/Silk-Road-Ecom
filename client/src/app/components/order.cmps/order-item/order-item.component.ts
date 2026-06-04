import { Component, input } from '@angular/core';
import { OrderItem } from '../../../shared/models/order.models';
import { CurrencyPipe } from '@angular/common';

@Component({
  selector: 'app-order-item',
  standalone: true,
  imports: [CurrencyPipe],
  templateUrl: './order-item.component.html',
  styleUrl: './order-item.component.scss'
})
export class OrderItemComponent {
  orderItem = input.required<OrderItem>();
}
