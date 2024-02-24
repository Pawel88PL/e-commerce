import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { OrderDetails } from 'src/app/models/order-details.model';
import { Order } from 'src/app/models/order.model';
import { OrderService } from 'src/app/services/order.service';
import { SHIPPING_COST } from 'src/app/config/config';

@Component({
  selector: 'app-order-details',
  templateUrl: './order-details.component.html',
  styleUrls: ['./order-details.component.css']
})
export class OrderDetailsComponent {
  orderId: string | null = null;
  order: Order = new Order();
  shippingCost = SHIPPING_COST;

  constructor(private route: ActivatedRoute, private orderService: OrderService) { }

  ngOnInit() {
    this.orderId = this.route.snapshot.paramMap.get('orderId');
    if (this.orderId) {
      this.loadOrderDetails();
    }
  }

  loadOrderDetails() {
    if (this.orderId) {
      this.orderService.getOrderDetails(this.orderId).subscribe({
        next: (details) => {
          this.order = details;
        },
        error: (error) => {
          console.error('Wystąpił błąd', error);
        }
      });
    } else {
      console.error('Brak orderId');
    }
  }
}