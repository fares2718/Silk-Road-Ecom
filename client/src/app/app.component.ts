import { Component } from '@angular/core';
import { NavBarComponent } from './components/core/nav-bar/nav-bar.component';
import { ShopComponent } from "./components/shop/shop.component";

@Component({
  selector: 'app-root',
  standalone: true,
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
  imports: [NavBarComponent, ShopComponent]
})
export class AppComponent {
  title = 'client';
}
