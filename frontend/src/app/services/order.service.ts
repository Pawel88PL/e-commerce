import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class OrderService {
  apiBaseUrl: string = environment.apiUrl;
  private orderUrl = `${this.apiBaseUrl}/order`;

  constructor(private http: HttpClient) { }

  createOrder(cartId: string, userId: string): Observable<any> {
    const payload = { cartId: cartId, userId: userId };
    return this.http.post(`${this.orderUrl}`, payload);
  }

  // Tutaj możesz dodać więcej metod związanych z zamówieniami, np. pobieranie szczegółów zamówienia, anulowanie zamówienia itd.

}
