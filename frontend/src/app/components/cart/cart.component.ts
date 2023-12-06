import { AuthService } from 'src/app/services/auth.service';
import { CartItem } from 'src/app/models/cart.model';
import { CartService } from 'src/app/services/cart.service';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.css']
})
export class CartComponent implements OnInit {
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
      (response: any) => { // Użyj odpowiedniego typu zamiast 'any', jeśli jest dostępny
        this.items = response.cartItems; // Zmienione przypisanie
        this.calculateCosts();
      },
      error => {
        console.error('Błąd podczas ładowania danych koszyka!', error);
        // Można tutaj dodać wyświetlanie błędu w UI
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

  removeFromCart(productId: number) {
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

  clearCart() {
    this.cartService.clearCart().subscribe(
      () => {
        this.items = [];
        this.totalCost = 0;
        // Informacja zwrotna dla użytkownika
        alert('Koszyk został opróżniony.');
      },
      error => console.error('Błąd podczas opróżniania koszyka!', error)
    );
  }

  maxQuantity(amountAvailable: number): number[] {
    const max = Math.min(10, amountAvailable);
    return Array.from({ length: max }, (_, i) => i + 1);
  }


  proceedToCheckout() {
    if (this.authService.isLoggedIn()) {
      this.router.navigate(['/order']);
    } else {
      this.router.navigate(['/checkout']);
    }
  }
}
