import { BasketItem } from './basket-item.model';
import { v4 as uuidv4 } from 'uuid';

export interface IBasket {
  basketID: string;
  basketItems: BasketItem[];
}

export class Basket implements IBasket {
  basketID = uuidv4();
  basketItems: BasketItem[] = [];
}
