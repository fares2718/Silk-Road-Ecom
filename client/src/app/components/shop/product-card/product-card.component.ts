import { Component, input, signal } from '@angular/core';
import { Product } from '../../../shared/models/product.model';
import { CurrencyPipe } from '@angular/common';
import { RouterLink } from "@angular/router";
import { Utiles } from '../../../helpers/utiles';

@Component({
  selector: 'app-product',
  standalone: true,
  imports: [CurrencyPipe, RouterLink],
  templateUrl: './product-card.component.html',
  styleUrl: './product-card.component.scss'
})
export class ProductComponent {
product = input.required<Product>();



get discountPercent(){
  return Utiles.getDiscountPercent(this.product().oldPrice,this.product().newPrice); 
}
}
