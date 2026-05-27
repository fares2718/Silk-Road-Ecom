import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Category } from '../shared/models/category.model';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {
private http = inject(HttpClient);

private baseUrl:string = 'https://localhost:7041/api';

getAllCategories(){
  return this.http.get<Category[]>(`${this.baseUrl}/Category/all-categories`);
}

}
