import { Component, input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Order } from '../../../shared/models/order.models';
import { OrderItemComponent } from '../order-item/order-item.component';

@Component({
  selector: 'app-order',
  standalone: true,
  imports: [CommonModule, OrderItemComponent],
  templateUrl: './order.component.html',
  styleUrl: './order.component.scss'
})
export class OrderComponent{
  order = input.required<Order>();
}
