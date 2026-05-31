import { HttpClient } from '@angular/common/http';
import { DestroyRef, inject, Injectable } from '@angular/core';
import { Basket, IBasket } from '../shared/models/basket.model';
import { BasketItem } from '../shared/models/basket-item.model';
import { BehaviorSubject, map } from 'rxjs';
import { Product } from '../shared/models/product.model';
import { BasketTotal } from '../shared/models/basket-total.model';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { environment } from '../../environments/environment.development';

@Injectable({
  providedIn: 'root',
})
export class BasketService {
  private http = inject(HttpClient);
  private destroyRef = inject(DestroyRef);

  private baseUrl: string = `${environment.baseUrl}/Basket`;

  private basketSource = new BehaviorSubject<IBasket>(null);
  private basketTotalSource = new BehaviorSubject<BasketTotal>(null);
  basket$ = this.basketSource.asObservable();
  basketTotal$ = this.basketTotalSource.asObservable();
 

  addUpdateBasket(basket: Basket) {
    return this.http
      .post(`${this.baseUrl}/add-update-basket`, basket)
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: (res: IBasket) => {
          this.basketSource.next(res);
          this.calculateBasketTotal();
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
    const itemIndex = basketItems.findIndex(bi => bi.itemID == basketItem.itemID);
    if(itemIndex==-1)
      basketItems.push(basketItem);
    else
      basketItems[itemIndex].quantity+=basketItem.quantity;
    return basketItems;
  }

  calculateBasketTotal(){
    const basket = this.currentValue;
    const shipping = 0;
    const subTotal = basket.basketItems.reduce(
      (accumulator, currentItem) => {
        return accumulator + currentItem.price * currentItem.quantity;
      },
      0,
    );
    const tax = 0;
    const total = shipping+tax+subTotal;
    this.basketTotalSource.next({
      subTotal:subTotal,
      shipping:shipping,
      tax:tax,
      Total:total
    });
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
      .delete(`${this.baseUrl}/delete-basket/${basketID}`)
      .pipe(
        map(() => {
          this.basketSource.next(null);
        }),
      );
  }

  getBasketById(basketID: string) {
    return this.http
      .get<IBasket>(`${this.baseUrl}/basket/${basketID}`)
      .pipe(
        map((res: IBasket) => {
          this.basketSource.next(res);
          this.calculateBasketTotal();
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
