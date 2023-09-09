import { Injectable } from '@angular/core';
import { CartItem } from '../models/cart.model';
import { Product } from '../models/product.model';

@Injectable({
  providedIn: 'root'
})
export class CartService {
  private items: CartItem[] = [];

  constructor() { }

  addToCart(product: Product) {
    let item = this.items.find(i => i.product?.productId === product.productId)
    if (item) {
      item.quantity += 1;
    } else {
      this.items.push({ product: product, quantity: 1 })
    }
  }

  getItems(): CartItem[] {
    return this.items;
  }

  clearCart() {
    this.items = [];
  }
}
