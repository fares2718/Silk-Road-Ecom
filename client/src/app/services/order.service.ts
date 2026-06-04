import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { DeliveryMethod, Order, PlaceOrder } from '../shared/models/order.models';

@Injectable({
  providedIn: 'root'
})
export class OrderService {
  private http = inject(HttpClient);

  private baseUrl = `${environment.baseUrl}/Order`;

  getDeliveryMethods(searchTerm: string = ''){
    return this.http.get<DeliveryMethod[]>(`${this.baseUrl}/delivery-methodes?searchTerm=${searchTerm}`);
  }

  getUserOrders(){
    return this.http.get<Order>(`${this.baseUrl}/user-orders`)
  }

  placeOrder(order:PlaceOrder){
    return this.http.post(`${this.baseUrl}/place-order`,order);
  }
}
