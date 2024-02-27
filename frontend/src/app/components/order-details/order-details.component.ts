import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Order } from 'src/app/models/order.model';
import { OrderService } from 'src/app/services/order.service';
import { SHIPPING_COST } from 'src/app/config/config';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-order-details',
  templateUrl: './order-details.component.html',
  styleUrls: ['./order-details.component.css']
})
export class OrderDetailsComponent {
  isLoading: boolean = false;
  orderId: string | null = null;
  order: Order = new Order();
  shippingCost = SHIPPING_COST;
  orderDeliveryMethod: string = '';

  constructor(private route: ActivatedRoute, public authService: AuthService, private orderService: OrderService) { }

  ngOnInit() {
    this.orderId = this.route.snapshot.paramMap.get('orderId');
    if (this.orderId) {
      this.loadOrderDetails();
    }
  }

  loadOrderDetails() {
    if (this.orderId) {
      this.isLoading = true;
      this.orderService.getOrderDetails(this.orderId).subscribe({
        next: (details) => {
          if (details.isPickupInStore) {
            this.shippingCost = 0;
            this.orderDeliveryMethod = 'Odbiór osobisty';
          } else {
            this.orderDeliveryMethod = 'Kurier';
          }
          this.order = details;
          this.isLoading = false;
        },
        error: (error) => {
          console.error('Wystąpił błąd', error);
          this.isLoading = false;
        }
      });
    } else {
      console.error('Brak orderId');
    }
  }
}