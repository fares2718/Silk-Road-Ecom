import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Page } from '../shared/models/pagination.model';
import { ProductParams } from '../shared/models/product-params.model';

@Injectable({
  providedIn: 'root',
})
export class ProductService {
  private http = inject(HttpClient);

  private baseUrl: string = 'https://localhost:7041/api';

  getProductsPage(productParams?:ProductParams) {
    let params = new HttpParams();
    if (productParams.categoryID)
      params = params.append('categoryID', productParams.categoryID);
    if(productParams.sortBy)
      params = params.append('sortBy', productParams.sortBy);
    if(productParams.search)
      params = params.append('search',productParams.search);
    params = params.append('isDescending',productParams.isDescending);
    params = params.append('pageNumber',productParams.pageNumber);
    params = params.append('pageSize',productParams.pageSize);
    return this.http.get<Page>(`${this.baseUrl}/Product/all-products`, {
      params: params,
    });
  }
}
