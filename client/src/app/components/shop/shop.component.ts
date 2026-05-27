import { Component, DestroyRef, inject, OnInit, signal } from '@angular/core';
import { ProductService } from '../../services/product.service';
import { Product } from '../../shared/models/product.model';
import { ProductComponent } from "./product-card/product-card.component";

@Component({
  selector: 'app-shop',
  standalone: true,
  imports: [ProductComponent],
  templateUrl: './shop.component.html',
  styleUrl: './shop.component.scss'
})
export class ShopComponent implements OnInit{
private productService = inject(ProductService);
private destroyRef = inject(DestroyRef);

Products:Product[] = [];

ngOnInit(): void {
  this.productService.getProductsPage().subscribe({
    next: (page) => {
      this.Products = page.data;
    }
  });
}

}
