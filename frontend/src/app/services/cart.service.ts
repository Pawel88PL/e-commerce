import { v4 as uuidv4 } from 'uuid';
import { CartItem } from '../models/cart.model';
import { firstValueFrom } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Product } from '../models/product.model';

@Injectable({
  providedIn: 'root'
})

export class CartService {
  private cartId: string | null = null;
  private items: CartItem[] = [];

  private cartUrl = 'https://localhost:5047/api/Cart';
  private cartItemUrl = 'https://localhost:5047/api/Cart/items';

  constructor(private http: HttpClient) {
    this.cartId = localStorage.getItem('cartId');
  }

  async addToCart(product: Product): Promise<any> {
    if (!this.cartId) {
      this.cartId = this.generateUUID();
      localStorage.setItem('cartId', this.cartId);
      await this.createCart(this.cartId)
    }

    const data = {
      cartId: this.cartId,
      productId: product.productId,
      quantity: 1,
    };
    
    try {
      return await firstValueFrom(this.http.post(this.cartItemUrl, data));
    } catch (error) {
      return Promise.reject(error);
    }
  }

  private async createCart(cartId: string): Promise<void> {
    const data = {
      shopingCartId: cartId
    };

    await firstValueFrom(this.http.post(this.cartUrl, data));
  }

  clearCart() {
    this.items = [];
  }

  private async fetchUUIDFromServer(): Promise<string> {
    const response: any = await firstValueFrom(this.http.get(this.cartUrl));
    return response.cartId;
  }

  private generateUUID(): string {
    return uuidv4();
  }

  getItems(): CartItem[] {
    return this.items;
  }
}