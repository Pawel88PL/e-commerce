import { environment } from 'src/environments/environment';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Customer } from '../models/customer.model';

@Injectable({
  providedIn: 'root'
})
export class CustomerService {

  private apiBaseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getCustomerData(): Observable<Customer> {
    return this.http.get<Customer>(`${this.apiBaseUrl}/customer`);
  }
  
}
