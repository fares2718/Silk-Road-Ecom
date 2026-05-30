import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Basket, IBasket } from '../shared/models/basket.model';
import { BasketItem } from '../shared/models/basket-item.model';
import { BehaviorSubject, map } from 'rxjs';
import { Product } from '../shared/models/product.model';

@Injectable({
  providedIn: 'root',
})
export class BasketService {
  private http = inject(HttpClient);

  private baseUrl: string = 'https://localhost:7041/api';

  private basketSource = new BehaviorSubject<IBasket>(null);

  basket = this.basketSource.asObservable();

  private addUpdateBasket(basket: Basket) {
    return this.http
      .post(`${this.baseUrl}/Basket/add-update-basket`, basket)
      .subscribe({
        next: (res: IBasket) => {
          this.basketSource.next(res);
          console.log(res);
        },
        error: (err) => console.log(err),
      });
  }

  addItemToBasket(product: Product, quantity: number = 1) {
    const basketItem: BasketItem = this.mapProductToBasketItem(
      product,
      quantity,
    );
    const basket = this.currentValue ?? this.createBasket();
    basket.basketItems= this.addOrUpdateItemQuantity(basket.basketItems,basketItem);
    return this.addUpdateBasket(basket);
  }

  private addOrUpdateItemQuantity(basketItems:BasketItem[],basketItem:BasketItem){
    const itemIndex = basketItems.indexOf(basketItem);
    if(itemIndex==-1)
      basketItems.push(basketItem);
    else
      basketItems[itemIndex].quantity+=basketItem.quantity;
    return basketItems;
  }

  private createBasket(): IBasket {
    const basket = new Basket();
    localStorage.setItem('basketID',basket.basketID);
    return basket;
  }

  get currentValue() {
    return this.basketSource.value;
  }

  deleteBasket(basketID: string) {
    return this.http
      .delete(`${this.baseUrl}/Basket/delete-basket/${basketID}`)
      .pipe(
        map(() => {
          this.basketSource.next(null);
        }),
      );
  }

  getBasketById(basketID: string) {
    return this.http
      .get<IBasket>(`${this.baseUrl}/Basket/basket/${basketID}`)
      .pipe(
        map((res: IBasket) => {
          this.basketSource.next(res);
          return res;
          //console.log(res);
        }),
      );
  }

  private mapProductToBasketItem(product: Product, quantity: number) {
    const basketItem: BasketItem = {
      itemID: product.productID,
      itemName: product.productName,
      price: product.newPrice,
      imageURL: product.imageURLs[0],
      category: product.categoryName,
      quantity: quantity,
    };
    return basketItem;
  }
}
