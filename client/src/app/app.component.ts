import { Component } from '@angular/core';
import { NavBarComponent } from './components/core/nav-bar/nav-bar.component';
import { ShopComponent } from "./components/shop/shop.component";
import { FooterComponent } from './components/core/footer/footer.component';

@Component({
  selector: 'app-root',
  standalone: true,
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
  imports: [NavBarComponent, ShopComponent,FooterComponent]
})
export class AppComponent {
  title = 'client';
}
