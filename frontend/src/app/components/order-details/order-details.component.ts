import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Order } from 'src/app/models/order.model';
import { OrderService } from 'src/app/services/order.service';
import { SHIPPING_COST } from 'src/app/config/config';
import { AuthService } from 'src/app/services/auth.service';
import { finalize } from 'rxjs';

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
  selectedStatus: string = '';
  orderDeliveryMethod: string = '';
  orderStatuses = [
    { value: 'Oczekuje na płatność', viewValue: 'Oczekuje na płatność' },
    { value: 'Opłacone', viewValue: 'Opłacone' },
    { value: 'W trakcie realizacji', viewValue: 'W trakcie realizacji' },
    { value: 'Wysłane', viewValue: 'Wysłane' },
    { value: 'Zrealizowane', viewValue: 'Zrealizowane' },
    { value: 'Anulowane', viewValue: 'Anulowane' }
  ];


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

  changeOrderStatus(orderId: string, newStatus: string): void {
    if (orderId) {
      this.isLoading = true;
      this.orderService.updateOrderStatus(orderId, newStatus).pipe(
        finalize(() => this.isLoading = false)
      ).subscribe({
        next: () => {
          console.log('Status zamówienia został zaktualizowany');
          this.loadOrderDetails();
        },
        error: (error) => {
          console.error('Wystąpił błąd podczas aktualizacji statusu zamówienia', error);
        }
      });
    } else {
      console.error('Brak numeru ID zamówienia');
    }
  }
}