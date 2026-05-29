import { Component, DestroyRef, ElementRef, inject, OnInit, ViewChild } from '@angular/core';
import { ProductService } from '../../services/product.service';
import { Product } from '../../shared/models/product.model';
import { ProductComponent } from './product-card/product-card.component';
import { CategoryService } from '../../services/category.service';
import { Category } from '../../shared/models/category.model';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { ProductParams } from '../../shared/models/product-params.model';
import { CustomPaginationComponent } from "../../shared/components/custom-pagination/custom-pagination.component";
import { Router } from '@angular/router';

@Component({
  selector: 'app-shop',
  standalone: true,
  imports: [ProductComponent, PaginationModule, CustomPaginationComponent],
  templateUrl: './shop.component.html',
  styleUrls: ['./shop.component.scss'],
})
export class ShopComponent implements OnInit {
  private productService = inject(ProductService);
  private categoryService = inject(CategoryService);
  private destroyRef = inject(DestroyRef);
  private router = inject(Router)
  @ViewChild('search') SearchBox:ElementRef;
  @ViewChild('sort') SortSelected:ElementRef;
  @ViewChild('isDesc') IsDesc:ElementRef;
  @ViewChild('selectedCategory') selectedCtegory:ElementRef;


  Products: Product[] = [];
  Categories: Category[] = [];
  totalCount:number;
  productParams: ProductParams = new ProductParams();

  ngOnInit(): void {
    this.getProducts();

    this.categoryService
      .getAllCategories()
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: (categories) => {
          this.Categories = categories;
        },
      });
  }

  onSelectCategory(categoruId:number){
    this.productParams.categoryID = categoruId;
    this.getProducts()
  }

  onSortSelected(event:Event){
    this.productParams.sortBy = (event.target as HTMLInputElement).value;
    this.getProducts();
  }

  onOrderChange(event:Event){
    this.productParams.isDescending = (event.target as HTMLInputElement).checked;
    this.getProducts();
  }

  onSearch(search:string){
    this.productParams.search = search;
    this.getProducts();
  }


  private getProducts(){
    this.productService
      .getProductsPage(this.productParams)
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: (page) => {
          this.Products = page.data;
          this.totalCount = page.totalCount;
          this.productParams.pageNumber= page.pageNumber
          this.productParams.pageSize= page.pageSize
        },
      });
  }

  resetFilters(){
    this.productParams.sortBy = '';
    this.productParams.isDescending = false;
    this.productParams.search = '';
    this.productParams.categoryID = 0;
    this.SearchBox.nativeElement.value = '';
    this.SortSelected.nativeElement.selectedIndex = 0;
    this.selectedCtegory.nativeElement.selectedIndex = 0;
    this.IsDesc.nativeElement.checked = false;
    this.getProducts();
  }

  onPageChanged(pageNum:number){
    this.productParams.pageNumber = pageNum;
    this.getProducts();
  }

}
