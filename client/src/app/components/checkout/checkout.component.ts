import { Component, DestroyRef, inject, OnInit } from '@angular/core';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { OrderSummaryComponent } from '../../shared/components/order-summary/order-summary.component';
import { OrderService } from '../../services/order.service';
import { DeliveryMethod, PlaceOrder } from '../../shared/models/order.models';
import { CurrencyPipe } from '@angular/common';
import { BasketService } from '../../services/basket.service';
import { Basket } from '../../shared/models/basket.model';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { BasketTotal } from '../../shared/models/basket-total.model';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';

@Component({
  selector: 'app-checkout',
  standalone: true,
  imports: [
    NgSelectModule,
    ReactiveFormsModule,
    CurrencyPipe,
    OrderSummaryComponent,
  ],
  templateUrl: './checkout.component.html',
  styleUrl: './checkout.component.scss',
})
export class CheckoutComponent implements OnInit {
  private orderService = inject(OrderService);
  private basketService = inject(BasketService);
  private destroyRef = inject(DestroyRef);
  private toastrService = inject(ToastrService);
  private router = inject(Router)
  basketTotal: BasketTotal;
  deliveryOptions: DeliveryMethod[] = [];
  deliveryOptionId:number;

  ngOnInit(): void {
    this.orderService.getDeliveryMethods().subscribe({
      next: (res) => (this.deliveryOptions = res),
    });

    this.basketService.basketTotal$
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: (value) => (this.basketTotal = value),
        error: (err) => console.log(err),
      });
  }

  onSubmit(){
    const order:PlaceOrder = {
      deliveryMethodID:this.deliveryOptionId,
      basketID:localStorage.getItem('basketID')
    }
    this.orderService.placeOrder(order).subscribe({
      next:(res)=>{
        this.toastrService.success('Done','Done')
        this.router.navigateByUrl('/shopping')
      },
      error:(err) => {
        this.toastrService.error(err.error.message,'error')
      }
    })
  }

  onDeliveryChange(selectedItem: DeliveryMethod | undefined): void {
    
    if (!selectedItem) {
      return;
    }
    this.deliveryOptionId = selectedItem.deliveryMethodId;
    this.basketTotal.shipping = selectedItem.price;
    this.basketTotal.Total+= selectedItem.price;
  }
}

