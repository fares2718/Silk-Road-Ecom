import { Component, inject, OnInit, PLATFORM_ID } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { BasketService } from '../../../services/basket.service';
import { MatBadgeModule } from '@angular/material/badge';
import { MatIconModule } from '@angular/material/icon';
import { AsyncPipe, isPlatformBrowser } from '@angular/common';
import { Observable } from 'rxjs';
import { IBasket } from '../../../shared/models/basket.model';

@Component({
  selector: 'app-nav-bar',
  standalone: true,
  imports: [RouterLink, RouterLinkActive, MatBadgeModule, MatIconModule,AsyncPipe],
  templateUrl: './nav-bar.component.html',
  styleUrl: './nav-bar.component.scss',
})
export class NavBarComponent implements OnInit {
  private basketService = inject(BasketService);
  private platformId = inject(PLATFORM_ID);

  count: Observable<IBasket>;

  ngOnInit(): void {
    let basketID: string = '';
    if (isPlatformBrowser(this.platformId))
      basketID = localStorage.getItem('basketID');
    if (basketID)
      this.basketService.getBasketById(basketID).subscribe({
        next: (res) => {
          console.log(res);
          this.count = this.basketService.basket$;
        },
        error: (err) => console.log(err),
      });
  }

  isDropDownVisible = false;
  toggleDorpDown() {
    this.isDropDownVisible = !this.isDropDownVisible;
  }
}
