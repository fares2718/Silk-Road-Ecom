import { Component, DestroyRef, inject, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Product } from '../../../shared/models/product.model';
import { ProductService } from '../../../services/product.service';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { CurrencyPipe } from '@angular/common';
import { Utiles } from '../../../helpers/utiles';

@Component({
  selector: 'app-product-details',
  standalone: true,
  imports: [CurrencyPipe],
  templateUrl: './product-details.component.html',
  styleUrl: './product-details.component.scss'
})
export class ProductDetailsComponent implements OnInit{
private route = inject(ActivatedRoute);
private productService = inject(ProductService);
private destroyRef = inject(DestroyRef);

product:Product = {
  productID:0,
  categoryName:'',
  description:'',
  imageURLs:[],
  newPrice:0,
  oldPrice:0,
  productName:''
};

ngOnInit(): void {
    this.product.productID =parseInt( this.route.snapshot.paramMap.get('id'));
    this.getProduct();
}


getProduct(){
  this.productService.getProductById(this.product.productID)
  .pipe(takeUntilDestroyed(this.destroyRef))
  .subscribe({
    next:(productData) => {
      this.product = productData;
    }
  })
}

get discountPercent(){
  return Utiles.getDiscountPercent(this.product.oldPrice,this.product.newPrice); 
}

}
