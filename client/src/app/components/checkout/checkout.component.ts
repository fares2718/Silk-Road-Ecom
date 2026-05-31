import { Component } from '@angular/core';
import { OrderSummaryComponent } from "../order-summary/order-summary.component";

@Component({
  selector: 'app-checkout',
  standalone: true,
  imports: [OrderSummaryComponent],
  templateUrl: './checkout.component.html',
  styleUrl: './checkout.component.scss'
})
export class CheckoutComponent {

}
