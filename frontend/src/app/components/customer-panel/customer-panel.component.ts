import { Component, OnInit } from '@angular/core';
import { Customer } from 'src/app/models/customer.model';
import { CustomerService } from 'src/app/services/customer.service';

@Component({
  selector: 'app-customer-panel',
  templateUrl: './customer-panel.component.html',
  styleUrls: ['./customer-panel.component.css']
})
export class CustomerPanelComponent implements OnInit {
  activeSection: 'accountInfo' | 'addresses' | 'changePassword' | 'orderHistory' = 'accountInfo';
  customer: Customer = {};
  orders: any[] = [];

  constructor(private customerService: CustomerService) { }

  ngOnInit(): void {
    this.loadCustomerData();
  }

  setActiveSection(section: 'accountInfo' | 'addresses' | 'changePassword' | 'orderHistory') {
    this.activeSection = section;
  }

  loadCustomerData() {
    const userId = localStorage.getItem('userId');
    if (userId){
      this.customerService.getCustomer(userId).subscribe(
        (customer: Customer) => {
          this.customer = customer;
        },
        (error) => {
          console.log(error);
        }
      );
    }
  }
}
