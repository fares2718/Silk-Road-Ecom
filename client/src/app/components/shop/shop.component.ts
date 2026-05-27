import { Component, DestroyRef, inject, OnInit } from '@angular/core';
import { ProductService } from '../../services/product.service';
import { Product } from '../../shared/models/product.model';
import { ProductComponent } from "./product-card/product-card.component";
import { CategoryService } from '../../services/category.service';
import { Category } from '../../shared/models/category.model';

@Component({
  selector: 'app-shop',
  standalone: true,
  imports: [ProductComponent],
  templateUrl: './shop.component.html',
  styleUrls: ['./shop.component.scss']
})
export class ShopComponent implements OnInit{
private productService = inject(ProductService);
private categoryService = inject(CategoryService);
private destroyRef = inject(DestroyRef);

Products:Product[] = [];
Categories:Category[] = [];

selectedCategoryId:number=0;

ngOnInit(): void {
  const productSubscription = this.productService.getProductsPage()
  .subscribe({
    next: (page) => {
      this.Products = page.data;
    }
  });

  const categorySubscription = this.categoryService.getAllCategories()
  .subscribe({
    next:(categories) => {
      this.Categories = categories;
    }
  });

  this.destroyRef.onDestroy(() => {
    productSubscription.unsubscribe();
    categorySubscription.unsubscribe();
  });
}

}
