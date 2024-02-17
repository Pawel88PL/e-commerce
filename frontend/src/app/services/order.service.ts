import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Order } from '../models/order.model';

@Injectable({
  providedIn: 'root'
})
export class OrderService {
  apiBaseUrl: string = environment.apiUrl;
  private orderUrl = `${this.apiBaseUrl}/order`;

  constructor(private http: HttpClient) { }

  createOrder(cartId: string, userId: string): Observable<any> {
    const payload = { cartId: cartId, userId: userId };
    return this.http.post(`${this.orderUrl}`, payload)
      .pipe(catchError(this.handleError));
  }

  getOrderDetails(orderId: string): Observable<Order> {
    return this.http.get<Order>(`${this.orderUrl}/${orderId}`)
      .pipe(catchError(this.handleError));
  }

  private handleError(error: any) {
    console.error('Wystąpił błąd!', error);
    return throwError(() => new Error('Błąd serwera'));
  }

  // Tutaj możesz dodać więcej metod związanych z zamówieniami, np. anulowanie zamówienia itd.
}
