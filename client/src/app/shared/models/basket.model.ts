import { BasketItem } from './basket-item.model';
import { v4 as uuidv4 } from 'uuid';

export interface IBasket {
  basketID: string;
  paymentIntentId: string;
  clientSecret: string;

  basketItems: BasketItem[];
}

export class Basket implements IBasket {
  paymentIntentId: string = '';
  clientSecret: string = '';
  basketID = uuidv4();
  basketItems: BasketItem[] = [];
}
