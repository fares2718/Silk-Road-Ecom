import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { BasketService } from './basket.service';
import { map } from 'rxjs';
import { IBasket } from '../shared/models/basket.model';

@Injectable({
  providedIn: 'root',
})
export class PaymentService {
  private http = inject(HttpClient);
  private basketService = inject(BasketService);

  private baseUrl: string = `${environment.baseUrl}/Payment`;

  createOrUpdatePayment(deliveryMethodId: number) {
    return this.http.post(
      `${this.baseUrl}/create-or-update?basketId=${this.basketService.currentValue.basketID}&deliveryId=${deliveryMethodId}`,
      {},
      { withCredentials: true },
    ).pipe(
      map((res:IBasket)=>{
        this.basketService.basketSource.next(res);
      })
    );
  }
}
