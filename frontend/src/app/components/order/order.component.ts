import { AuthService } from 'src/app/services/auth.service';
import { CartItem } from 'src/app/models/cart.model';
import { CartService } from 'src/app/services/cart.service';
import { Component, OnInit } from '@angular/core';
import { ConfirmDialogComponent } from '../confirm-dialog/confirm-dialog.component';
import { OrderProcessingDialogComponent } from '../order-processing-dialog/order-processing-dialog.component';
import { Customer } from 'src/app/models/customer.model';
import { CustomerService } from 'src/app/services/customer.service';
import { environment } from 'src/environments/environment';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { SHIPPING_COST } from 'src/app/config/config';
import gsap from 'gsap';
import { OrderService } from 'src/app/services/order.service';
import { query } from '@angular/animations';

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
  isLoading = false;
  isPickupInStore: boolean = false;
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
      delay: 1,
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

    gsap.from('.logo', {
      duration: 1,
      y: '100%',
      opacity: 0,
      scale: 0.5,
      delay: 2,
      ease: "power1.out"
    });
  }

  loadCartItems() {
    if (this.cartId) {
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
      if (this.isPickupInStore) {
        this.shippingCost = 0;
        this.totalCost = this.productCost;
      } else {
        this.shippingCost = SHIPPING_COST;
        this.totalCost = this.productCost + this.shippingCost;
      }
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
        const processingDialogRef = this.dialog.open(OrderProcessingDialogComponent, {
          disableClose: true
        });

        this.orderService.createOrder(this.cartId, this.userId, this.isPickupInStore).subscribe({
          next: (order) => {
            console.log('Zamówienie zostało złożone', order);
            localStorage.removeItem('cartId');
            this.router.navigate(['/orderConfirmation'], { queryParams: { orderId: order } });
            processingDialogRef.close();
          },
          error: (error) => {
            console.error('Wystąpił błąd przy tworzeniu zamówienia', error);
            processingDialogRef.close();
          }
        });
      } else {
        console.log('Zamówienie zostało anulowane.');
      }
    });
  }
}