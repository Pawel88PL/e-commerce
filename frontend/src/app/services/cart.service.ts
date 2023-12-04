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
  public cartId: string | null = localStorage.getItem('cartId')
  private cartUrl = 'https://localhost:5047/api/Cart';
  private cartItemUrl = 'https://localhost:5047/api/Cart/items';

  constructor(private http: HttpClient) { }

  addToCart(product: Product): Observable<any> {
    if (!this.cartId) {
      this.cartId = uuidv4();
      localStorage.setItem('cartId', this.cartId);
    }
    return this.addItemToCart(product);
  }

  private addItemToCart(product: Product): Observable<any> {
    return this.http.post(this.cartItemUrl, {
      cartId: this.cartId,
      productId: product.productId,
      quantity: 1
    }).pipe(
      catchError(this.handleError)
    );
  }

  getItems(): Observable<CartItem[]> {
    if (!this.cartId) {
      return throwError(() => new Error('Brak identyfikatora koszyka'));
    }

    return this.http.get<CartItem[]>(`${this.cartUrl}/${this.cartId}/items`).pipe(
      catchError(this.handleError)
    );
  }

  updateItemQuantity(cartId: string, productId: number, quantity: number): Observable<any> {
    const url = `https://localhost:5047/api/Cart/${cartId}/items/${productId}`;
    return this.http.put(url, { quantity }).pipe(
      catchError(this.handleError)
    );
  }

  removeItem(productId: number): Observable<any> {
    return this.http.delete(`${this.cartItemUrl}/${productId}?cartId=${this.cartId}`).pipe(
      catchError(this.handleError)
    );
  }

  clearCart(): Observable<any> {
    if (!this.cartId) {
      return throwError(() => new Error('Brak identyfikatora koszyka'));
    }

    return this.http.delete(`${this.cartUrl}/${this.cartId}`).pipe(
      catchError(this.handleError)
    );
  }

  private handleError(error: any) {
    console.error('Wystąpił błąd!', error);
    return throwError(() => new Error('Błąd serwera'));
  }
}