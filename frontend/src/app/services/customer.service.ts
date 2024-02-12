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

  changePassword(customerId:string, passwordChangeRequest: any): Observable<any> {
    return this.http.post(`${this.apiBaseUrl}/customer/${customerId}/change-password`, passwordChangeRequest);
  }

  getCustomer(customerId: string): Observable<Customer> {
    return this.http.get<Customer>(`${this.apiBaseUrl}/customer/${customerId}`);
  }

  updateCustomer(customerId: string, customerData: Customer): Observable<Customer> {
    return this.http.post<Customer>(`${this.apiBaseUrl}/customer/${customerId}`, customerData);
  }

}
