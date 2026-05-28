import { Routes } from '@angular/router';
import { ShopComponent } from './components/shop/shop.component';
import { HomeComponent } from './components/core/home/home.component';

export const routes: Routes = [
  { path: 'home', component: HomeComponent },
  
  { path: 'shopping', component: ShopComponent },

  { path: '', redirectTo: 'home', pathMatch: 'full' },

  { path: '**', redirectTo: 'home' },
];
