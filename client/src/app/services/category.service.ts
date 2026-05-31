import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Category } from '../shared/models/category.model';
import { environment } from '../../environments/environment.development';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {
private http = inject(HttpClient);

private baseUrl:string = `${environment.baseUrl}/Category`;

getAllCategories(){
  return this.http.get<Category[]>(`${this.baseUrl}/all-categories`);
}

}
