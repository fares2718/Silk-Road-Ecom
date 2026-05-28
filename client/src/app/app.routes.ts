import { Routes } from '@angular/router';
import { ShopComponent } from './components/shop/shop.component';
import { HomeComponent } from './components/core/home/home.component';
import { ProductComponent } from './components/shop/product-card/product-card.component';
import { ProductDetailsComponent } from './components/shop/product-details/product-details.component';

export const routes: Routes = [
  { path: 'home', component: HomeComponent },
  
  { path: 'shopping', component: ShopComponent },

  { path: 'product-details/:id', component: ProductDetailsComponent },

  { path: '', redirectTo: 'home', pathMatch: 'full' },

  { path: '**', redirectTo: 'home' },
];
