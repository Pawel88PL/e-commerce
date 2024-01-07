import { v4 as uuidv4 } from 'uuid';
import { CartItem } from '../models/cart.model';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, throwError } from 'rxjs';
import { Product } from '../models/product.model';

@Injectable({
  providedIn: 'root'
})

export class CartService {
  apiBaseUrl: string = environment.apiUrl;
  public cartId: string | null = localStorage.getItem('cartId')
  private cartUrl = this.apiBaseUrl + '/api/Cart';
  private cartItemUrl = this.apiBaseUrl + '/api/Cart/items';

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

  assignCartToUser(userId: string): Observable<any> {
    return this.http.post(`${this.cartUrl}/assign/${this.cartId}`, { userId }).pipe(
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
    const url = `${this.cartUrl}/${cartId}/items/${productId}`;
    return this.http.put(url, { quantity }).pipe(
      catchError(this.handleError)
    );
  }

  removeItem(cartId: string ,productId: number): Observable<any> {
    const url = `${this.cartUrl}/${cartId}/items/${productId}`;
    return this.http.delete(url).pipe(
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