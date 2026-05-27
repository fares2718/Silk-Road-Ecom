import { Component, input, signal } from '@angular/core';
import { Product } from '../../../shared/models/product.model';
import { CurrencyPipe } from '@angular/common';

@Component({
  selector: 'app-product',
  standalone: true,
  imports: [CurrencyPipe],
  templateUrl: './product-card.component.html',
  styleUrl: './product-card.component.scss'
})
export class ProductComponent {
product = input.required<Product>();



getDiscountPercent(){
  const oldPrice = this.product().oldPrice;
  const newPrice = this.product().newPrice;

  if (!oldPrice || oldPrice <= 0 || newPrice >= oldPrice) {
    return 0;
  }

  const discount = ((oldPrice - newPrice) / oldPrice) * 100;
  
  return discount.toFixed(2); 
}
}
