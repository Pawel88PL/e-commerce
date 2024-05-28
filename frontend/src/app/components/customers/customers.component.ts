import { Component, OnInit } from '@angular/core';
import { Customer } from 'src/app/models/customer.model';
import { CustomerService } from 'src/app/services/customer.service';

@Component({
  selector: 'app-customers',
  templateUrl: './customers.component.html',
  styleUrls: ['./customers.component.css']
})
export class CustomersComponent implements OnInit {
  customers:  Customer[] = [];
  
  constructor(private customerService: CustomerService) {}

  ngOnInit(): void {
      this.loadCustomers();
  }

  loadCustomers() {
    this.customerService.getAllCustomers().subscribe(
      (data: Customer[]) => {
      this.customers = data;
    },
    error => {
      console.log("Wystąpił błąd podczas pobierania użytkowników", error);
    });
  }
}
