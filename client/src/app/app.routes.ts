import { Routes } from '@angular/router';
import { HomeComponent } from './components/core/home/home.component';
import { BasketComponent } from './components/shop/basket/basket.component';

export const routes: Routes = [
  { path: 'home', component: HomeComponent },
  
  { 
    path: 'shopping', 
    loadComponent:() => import('./components/shop/shop.component')
    .then(c => c.ShopComponent),
  },
  { 
    path: 'product-details/:id', 
    loadComponent:() => import('./components/shop/product-details/product-details.component')
    .then(c => c.ProductDetailsComponent), 
  },
  {
    path:'basket',
    loadComponent:() => import('./components/shop/basket/basket.component')
    .then(c => c.BasketComponent), 
  },

  { path: '', redirectTo: 'home', pathMatch: 'full' },

  { path: '**', redirectTo: 'home' },
];
