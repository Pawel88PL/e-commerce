import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AdminOrder, Order } from 'src/app/models/order.model';
import { OrderService } from 'src/app/services/order.service';

@Component({
  selector: 'app-admin-panel',
  templateUrl: './admin-panel.component.html',
  styleUrls: ['./admin-panel.component.css']
})
export class AdminPanelComponent implements OnInit {
  isLoading: boolean = false;
  orders: AdminOrder[] = [];

  constructor(private orderService: OrderService, private router: Router) { }

  ngOnInit(): void {
    this.loadOrdersHistory();
  }

  loadOrdersHistory() {
    this.isLoading = true;
    this.orderService.getAllOrders().subscribe({
      next: (orders) => {
        this.orders = orders.map(order => ({
          ...order,
          shortOrderId: order.orderId.slice(0, 8)
        }));
        this.isLoading = false;
        if (orders.length === 0) {
          console.log('Nie znaleziono historii zamówień.');
        }
      },
      error: (error) => {
        console.error('Wystąpił błąd podczas pobierania historii zamówień.', error);
        this.isLoading = false;
      }
    });
  }

  viewOrderDetails(orderId: string) {
    this.router.navigate(['/order-details', orderId]);
  }
}