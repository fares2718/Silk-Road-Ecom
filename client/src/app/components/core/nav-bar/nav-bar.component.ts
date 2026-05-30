import { Component, inject, OnInit } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { BasketService } from '../../../services/basket.service';
import {MatBadgeModule} from '@angular/material/badge';


@Component({
  selector: 'app-nav-bar',
  standalone: true,
  imports: [RouterLink, RouterLinkActive,MatBadgeModule],
  templateUrl: './nav-bar.component.html',
  styleUrl: './nav-bar.component.scss',
})
export class NavBarComponent implements OnInit {
  private basketService = inject(BasketService);

  ngOnInit(): void {
    const basketID = localStorage.getItem('basketID');
    this.basketService.getBasketById(basketID)
    .subscribe({
      next:(res)=> console.log(res),
      error:(err) => console.log(err),
    })
  }

  isDropDownVisible = false;
  toggleDorpDown() {
    this.isDropDownVisible = !this.isDropDownVisible;
  }
}
