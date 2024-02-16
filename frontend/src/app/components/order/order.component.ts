import { AuthService } from 'src/app/services/auth.service';
import { CartItem } from 'src/app/models/cart.model';
import { CartService } from 'src/app/services/cart.service';
import { Component, OnInit } from '@angular/core';
import { ConfirmDialogComponent } from '../confirm-dialog/confirm-dialog.component';
import { Customer } from 'src/app/models/customer.model';
import { CustomerService } from 'src/app/services/customer.service';
import { environment } from 'src/environments/environment';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { SHIPPING_COST } from 'src/app/config/config';
import gsap from 'gsap';
import { OrderService } from 'src/app/services/order.service';

@Component({
  selector: 'app-order',
  templateUrl: './order.component.html',
  styleUrls: ['./order.component.css']
})
export class OrderComponent implements OnInit {
  apiBaseUrl: string = environment.apiUrl;
  cartId: string = localStorage.getItem('cartId') || '';
  userId: string = localStorage.getItem('userId') || '';
  customer: Customer = {};
  items: CartItem[] = [];
  productCost: number = 0;
  shippingCost: number = SHIPPING_COST;
  totalCost: number = 0;

  constructor(
    private authService: AuthService,
    private cartService: CartService,
    private customerService: CustomerService,
    public dialog: MatDialog,
    private orderService: OrderService,
    private router: Router
  ) { }

  ngOnInit() {
    this.loadCartItems();
    this.loadCustomerData();

    gsap.from('.personal-data', {
      duration: 1,
      x: '-100%',
      opacity: 0,
      scale: 0.5,
      delay: 0.5,
      ease: "power1.out"
    });

    gsap.from('.cart', {
      duration: 1,
      x: '100%',
      opacity: 0,
      scale: 0.5,
      delay: 0.5,
      ease: "power1.out"
    });
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

  placeOrder() {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      data: { totalCost: this.totalCost }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.orderService.createOrder(this.cartId, this.userId).subscribe({
          next: (order) => {
            console.log('Zamówienie zostało złożone', order);
            localStorage.removeItem('cartId');
          },
          error: (error) => {
            console.error('Wystąpił błąd przy tworzeniu zamówienia', error);
          }
        });
      } else {
        console.log('Zamówienie zostało anulowane.');
      }
    });
  }
}