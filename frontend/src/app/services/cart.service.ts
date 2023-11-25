import { v4 as uuidv4 } from 'uuid';
import { CartItem } from '../models/cart.model';
import { Observable, catchError, firstValueFrom, switchMap, throwError } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Product } from '../models/product.model';

@Injectable({
  providedIn: 'root'
})

export class CartService {
  private cartId: string | null = null;
  private items: CartItem[] = [];
  private product: Product = new Product();

  private cartUrl = 'https://localhost:5047/api/Cart';
  private cartItemUrl = 'https://localhost:5047/api/Cart/items';

  constructor(private http: HttpClient) {
    this.cartId = localStorage.getItem('cartId');
  }

  addToCart(product: Product): Observable<any> {
    if (!this.cartId) {
      this.cartId = this.generateUUID();
      localStorage.setItem('cartId', this.cartId);
      return this.createCart(this.cartId).pipe(
        switchMap(() => this.addItemToCart(product))
      );
    }

    return this.addItemToCart(product);
  }

  private addItemToCart(product: Product): Observable<any> {
    const data = {
      cartId: this.cartId,
      productId: product.productId,
      quantity: 1,
    };

    return this.http.post(this.cartItemUrl, data);
  }

  private createCart(cartId: string): Observable<void> {
    const data = {
      shopingCartId: cartId
    };

    return this.http.post<void>(this.cartUrl, data).pipe(
      catchError(error => {
        console.error('Wystąpił błąd podczas tworzenia nowego koszyka!', error);
        return throwError(() => error);
      })
    );
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