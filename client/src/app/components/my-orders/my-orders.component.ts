import { Component, inject, OnInit } from '@angular/core';
import { OrderSummaryComponent } from '../../shared/components/order-summary/order-summary.component';
import { OrderService } from '../../services/order.service';
import { Order } from '../../shared/models/order.models';
import { BasketTotal } from '../../shared/models/basket-total.model';

@Component({
  selector: 'app-my-orders',
  standalone: true,
  imports: [OrderSummaryComponent],
  templateUrl: './my-orders.component.html',
  styleUrl: './my-orders.component.scss'
})
export class MyOrdersComponent implements OnInit {
  private orderService = inject(OrderService);

  userOrders:Order;
  basketTotal:BasketTotal
  ngOnInit(): void {
    this.orderService.getUserOrders().subscribe({
      next:(res) => {
        this.userOrders = res
      }
    })
  }

}
