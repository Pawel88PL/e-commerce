import { environment } from 'src/environments/environment';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, catchError, throwError } from 'rxjs';
import { Customer } from '../models/customer.model';

@Injectable({
  providedIn: 'root'
})
export class CustomerService {

  private apiBaseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getCustomer(customerId: string): Observable<Customer> {
    return this.http.get<Customer>(`${this.apiBaseUrl}/customer/${customerId}`).pipe(
      catchError(error => {
        console.error('Wystąpił błąd podczas pobierania danych klienta', error);
        return throwError(() => new Error('Wystąpił problem z połączeniem do serwera.'));
      })
    );
  }
}
