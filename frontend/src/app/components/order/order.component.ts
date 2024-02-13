import { AuthService } from 'src/app/services/auth.service';
import { CartItem } from 'src/app/models/cart.model';
import { CartService } from 'src/app/services/cart.service';
import { Component, OnInit } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Router } from '@angular/router';
import { SHIPPING_COST } from 'src/app/config/config';
import { Customer } from 'src/app/models/customer.model';
import { CustomerService } from 'src/app/services/customer.service';

@Component({
  selector: 'app-order',
  templateUrl: './order.component.html',
  styleUrls: ['./order.component.css']
})
export class OrderComponent implements OnInit {
  apiBaseUrl: string = environment.apiUrl;
  customer: Customer = {};
  items: CartItem[] = [];
  productCost: number = 0;
  shippingCost: number = SHIPPING_COST;
  totalCost: number = 0;

  constructor(
    private authService: AuthService,
    private cartService: CartService,
    private customerService: CustomerService,
    private router: Router
  ) { }

  ngOnInit() {
    this.loadCartItems();
    this.loadCustomerData();
  }

  loadCartItems() {
    this.cartService.getItems().subscribe(
      (response: any) => {
        this.items = response.cartItems;
        this.calculateCosts();
      },
      error => {
        console.error('Błąd podczas ładowania danych koszyka!', error);
      }
    );
  }

  loadCustomerData() {
    const userId = localStorage.getItem('userId');
    if (userId) {
      this.customerService.getCustomer(userId).subscribe(
        (customer: Customer) => {
          this.customer = customer;
        },
        (error) => {
          console.error('Wystąpił błąd podczas pobierania danych klienta', error);
        }
      );
    }
  }

  calculateCosts() {
    if (Array.isArray(this.items)) {
      this.productCost = this.items.reduce((acc, item) => acc + item.price * item.quantity, 0);
      this.totalCost = this.productCost + this.shippingCost;
    } else {
      console.error('this.items nie jest tablicą');
    }
  }

  placeOrder(): void {
    // Logika wysłłaniaia zamówienia
  }
}