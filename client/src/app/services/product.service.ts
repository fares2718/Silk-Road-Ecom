import { Injectable,inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Page } from '../shared/models/pagination.model';

@Injectable({
  providedIn: 'root'
})
export class ProductService {
private http = inject(HttpClient);

private baseUrl:string = 'https://localhost:7041/api';

getProductsPage(){
  return this.http.get<Page>(`${this.baseUrl}/Product/all-products`);
}

}
