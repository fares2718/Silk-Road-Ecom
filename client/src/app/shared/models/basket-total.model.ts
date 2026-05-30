export class BasketTotal {
  subTotal: number;
  tax: number;
  shipping: number;

get Total(){
    return this.subTotal+this.tax+this.shipping;
}
}
