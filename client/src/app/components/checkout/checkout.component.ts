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
import { Basket, IBasket } from '../../shared/models/basket.model';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { BasketTotal } from '../../shared/models/basket-total.model';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { PaymentService } from '../../services/payment.service';
declare var Stripe: any;
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
  paymentForm = new FormGroup({
    nameOnCard: new FormControl('', {
      validators: [
        Validators.required,
        Validators.minLength(3),
        Validators.maxLength(100),
      ],
    }),
  });

  private orderService = inject(OrderService);
  private basketService = inject(BasketService);
  private paymentService = inject(PaymentService);
  private destroyRef = inject(DestroyRef);
  private toastrService = inject(ToastrService);
  private router = inject(Router);
  private stripe: any;
  basketTotal: BasketTotal;
  deliveryOptions: DeliveryMethod[] = [];
  deliveryOptionId: number = 0;
  step = 1;
  loader = false;
  cardNumber: any;
  cardExpiry: any;
  cardCvc: any;
  cardErrors: any;
  orderId: number;
  cardHandler = this.onChange.bind(this);

  onChange({ error }) {
    if (error) {
      this.cardErrors = error.message;
    } else {
      this.cardErrors = null;
    }
  }

  ngAfterViewInit(): void {
    this.stripe = Stripe(
      'pk_test_51NQCA3D80BLjniarWdUpT1b2oGB2AvuK8p5bJgUARq7VI9r711MjBPMwi2cnpz3oxtZGMXBy02uy6TkY5aSXZ8Vg008DNOb9hd',
    );
    const element = this.stripe.elements();

    this.cardNumber = element.create('cardNumber');
    this.cardNumber.addEventListener('change', this.cardHandler);

    this.cardExpiry = element.create('cardExpiry');
    this.cardExpiry.addEventListener('change', this.cardHandler);

    this.cardCvc = element.create('cardCvc');
    this.cardCvc.addEventListener('change', this.cardHandler);
  }
  ngOnDestroy(): void {
    this.cardCvc.destroy();
    this.cardNumber.destroy();
    this.cardExpiry.destroy();
  }

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

  createPayment() {
    this.paymentService.createOrUpdatePayment(this.deliveryOptionId).subscribe({
      next: (res) =>
        this.toastrService.success('payment intent created', 'Success'),
      error: (err) => this.toastrService.error(err.error.message, 'Error'),
    });
  }

  onContinue() {
    this.createPayment();
    const order: PlaceOrder = {
      deliveryMethodID: this.deliveryOptionId,
      basketID: this.basketService.currentValue.basketID,
    };
    this.orderService.placeOrder(order).subscribe({
      next: (res) => {
        this.toastrService.success('Done', 'Done');
        localStorage.setItem('basketID', null);
        this.router.navigateByUrl('/shopping');
      },
      error: (err) => {
        this.toastrService.error(err.error.message, 'error');
      },
    });
    this.step = 2;
  }

  onDeliveryChange(selectedItem: DeliveryMethod | undefined): void {
    if (!selectedItem) {
      return;
    }
    this.deliveryOptionId = selectedItem.deliveryMethodId;
    this.basketTotal.shipping = selectedItem.price;
    this.basketTotal.Total += selectedItem.price;
  }

  async submitOrder() {
    this.paymentForm.markAllAsTouched();

    if (this.paymentForm.invalid) {
      return;
    }

    if (this.cardErrors) {
      return;
    }

    this.loader = true;

    const paymentDetials = await this.confirmPaymentWithStripe(
      this.basketService.currentValue,
    );

    if (paymentDetials.paymentIntent) {
      this.loader = false;
      this.toastrService.success('Order Created Successfuly', 'SUCCESS');
      this.router.navigate(['/checkout/success'], {
        queryParams: { orderId: this.orderId },
      });
      localStorage.removeItem('basketID');
    } else {
      this.loader = false;
      this.toastrService.error(paymentDetials.error.message, 'ERROR');
    }
  }

  async confirmPaymentWithStripe(basket: IBasket) {
    return this.stripe.confirmCardPayment(basket.clientSecret, {
      payment_method: {
        card: this.cardNumber,
        billing_details: {
          name: this.paymentForm.get('nameOnCard').value,
        },
      },
    });
  }

  goBack(): void {
    this.step = 1;
  }
}
