import { AuthService } from 'src/app/services/auth.service';
import { CartItem } from 'src/app/models/cart.model';
import { CartService } from 'src/app/services/cart.service';
import { Component, OnInit } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Router } from '@angular/router';
import { SHIPPING_COST } from 'src/app/config/config';
import gsap from 'gsap';

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.css']
})
export class CartComponent implements OnInit {
  apiBaseUrl: string = environment.apiUrl;
  items: CartItem[] = [];
  productCost: number = 0;
  shippingCost: number = SHIPPING_COST;
  totalCost: number = 0;
  cardId: string = localStorage.getItem('cartId') || '';
  userId: string = localStorage.getItem('userId') || '';

  constructor(private cartService: CartService, private authService: AuthService, private router: Router) { }

  ngOnInit() {
    this.loadCartItems();

    gsap.from('.container', {
      duration: 1,
      x: '-100%',
      opacity: 0,
      scale: 0.5,
      delay: 0.5,
      ease: "power1.out"
    });
  }

  loadCartItems() {
    if (this.cardId) {
      this.cartService.getItems().subscribe(
        (response: any) => {
          this.items = response.cartItems;
          this.calculateCosts();
        },
        error => {
          console.error('Wystąpił błąd podczas próby załadowania danych z koszyka', error);
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

  updateQuantity(productId: number, event: Event) {
    const selectElement = event.target as HTMLSelectElement;
    const newQuantity = parseInt(selectElement.value, 10);
    if (this.cartService.cartId) {
      this.cartService.updateItemQuantity(this.cartService.cartId, productId, newQuantity).subscribe(
        () => {
          this.loadCartItems();
        },
        error => console.error('Błąd aktualizacji ilości!', error)
      );
    } else {
      console.error('Brak identyfikatora koszyka!');
    }
  }

  removeItemFromCart(productId: number) {
    if (this.cartService.cartId) {
      this.cartService.removeItem(this.cartService.cartId, productId).subscribe(
        () => {
          this.loadCartItems();
        },
        error => console.error('Błąd podczas usuwania produktu!', error)
      );
    } else {
      console.error('Angular: Brak identyfikatora koszyka');
    }
  }

  maxQuantity(amountAvailable: number): number[] {
    const max = Math.min(10, amountAvailable);
    return Array.from({ length: max }, (_, i) => i + 1);
  }

  proceedToCheckout() {
    this.authService.setInCheckoutProcess();
    if (this.authService.isLoggedIn()) {
      this.cartService.assignCartToUser(this.userId).subscribe();
      this.router.navigate(['/order']);
    } else {
      this.router.navigate(['/checkout']);
    }
  }
}
