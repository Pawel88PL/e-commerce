import { Component, OnInit } from '@angular/core';
import { CartItem } from 'src/app/models/cart.model';
import { CartService } from 'src/app/services/cart.service';

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

  constructor(private cartService: CartService) { }

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
    const element = event.target as HTMLSelectElement
    const newQuantity = parseInt(element.value, 10);
    this.cartService.updateItemQuantity(productId, newQuantity).subscribe(
      () => {
        this.loadCartItems();
      },
      error => console.error('Błąd aktualizacji ilości!', error)
    );
  }

  removeFromCart(productId: number) {
    // Logika usuwania produktu z koszyka
    this.cartService.removeItem(productId).subscribe(
      () => {
        this.loadCartItems(); // Ponowne załadowanie koszyka po usunięciu produktu
      },
      error => console.error('Błąd podczas usuwania produktu!', error)
    );
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
    // Implementacja przekierowania do kasy
  }
}
