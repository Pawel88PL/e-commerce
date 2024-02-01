import { AuthService } from 'src/app/services/auth.service';
import { CartItem } from 'src/app/models/cart.model';
import { CartService } from 'src/app/services/cart.service';
import { Component, OnInit } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Router } from '@angular/router';

@Component({
  selector: 'app-order',
  templateUrl: './order.component.html',
  styleUrls: ['./order.component.css']
})
export class OrderComponent implements OnInit {
  apiBaseUrl: string = environment.apiUrl;
  items: CartItem[] = [];
  productCost: number = 0;
  shippingCost: number = 0;
  totalCost: number = 0;

  constructor(private cartService: CartService, private authService: AuthService, private router: Router) { }

  ngOnInit() {
    this.loadCartItems();
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