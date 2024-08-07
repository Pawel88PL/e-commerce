import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Order } from '../models/order.model';
import { OrderHistory } from '../models/order-history.model';

@Injectable({
  providedIn: 'root'
})

export class OrderService {
  apiBaseUrl: string = environment.apiUrl;
  private orderUrl = `${this.apiBaseUrl}/order`;

  constructor(private http: HttpClient) { }

  createOrder(cartId: string, userId: string, isPickupInStore: boolean): Observable<string> {
    const payload = { cartId: cartId, userId: userId, isPickupInStore: isPickupInStore };
    return this.http.post(`${this.orderUrl}`, payload, { responseType: 'text' })
      .pipe(catchError(this.handleError));
  }

  getAllOrders(): Observable<Order[]> {
    const token = localStorage.getItem('token');
    if (!token) {
      return throwError('Token not found');
    }

    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    });
    
    return this.http.get<Order[]>(`${this.orderUrl}/allOrders`, { headers })
      .pipe(catchError(this.handleError));
  }

  getOrderDetails(orderId: string): Observable<Order> {
    return this.http.get<Order>(`${this.orderUrl}/${orderId}`)
      .pipe(catchError(this.handleError));
  }

  getOrdersHistory(userId: string): Observable<OrderHistory[]> {
    return this.http.get<OrderHistory[]>(`${this.orderUrl}/history/${userId}`)
      .pipe(catchError(this.handleError));
  }

  updateOrderStatus(orderId: string, status: string): Observable<Order> {
    const payload = { status: status };
    return this.http.post<Order>(`${this.orderUrl}/updateOrderStatus/${orderId}`, payload)
      .pipe(catchError(this.handleError));
  }

  private handleError(error: any) {
    console.error('Wystąpił błąd!', error);
    return throwError(() => new Error('Błąd serwera'));
  }


  testRedirect(): Observable<{ redirectUrl: string }> {
    return this.http.get<{ redirectUrl: string }>(`${this.apiBaseUrl}/orderConfirmation`)
      .pipe(catchError(this.handleError));
  }

}