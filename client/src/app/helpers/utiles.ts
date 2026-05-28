import { Product } from "../shared/models/product.model";

export class Utiles {
static getDiscountPercent(oldPrice:number,newPrice:number){


  if (!oldPrice || oldPrice <= 0 || newPrice >= oldPrice) {
    return 0;
  }

  const discount = ((oldPrice - newPrice) / oldPrice) * 100;
  
  return discount.toFixed(2); 
}
}