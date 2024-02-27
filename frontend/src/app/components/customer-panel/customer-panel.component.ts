import { AuthService } from 'src/app/services/auth.service';
import { Component, OnInit } from '@angular/core';
import { Customer } from 'src/app/models/customer.model';
import { CustomerService } from 'src/app/services/customer.service';
import { OrderService } from 'src/app/services/order.service';
import { FormGroup, FormControl, Validators, FormBuilder, AbstractControl, ValidationErrors } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { OrderHistory } from 'src/app/models/order-history.model';
import { Router } from '@angular/router';

@Component({
  selector: 'app-customer-panel',
  templateUrl: './customer-panel.component.html',
  styleUrls: ['./customer-panel.component.css']
})
export class CustomerPanelComponent implements OnInit {
  activeSection: 'accountInfo' | 'addresses' | 'changePassword' | 'orderHistory' = 'orderHistory';
  customer: Customer = {};
  changePasswordForm!: FormGroup;
  customerDataForm!: FormGroup;
  customerFirstName: string = '';
  isLoading = true;
  orders: OrderHistory[] = [];

  constructor(
    public authService: AuthService,
    private customerService: CustomerService,
    private fb: FormBuilder,
    private orderService: OrderService,
    private router: Router,
    private snackBar: MatSnackBar) { }

  ngOnInit(): void {
    this.initializeCustomerDataForm();
    this.loadCustomerData();
    this.loadOrdersHistory();
    this.initializeChangePasswordForm();

    if (this.authService.isAdmin()) {
      this.activeSection = 'accountInfo';
    }
  }

  initializeCustomerDataForm() {
    this.customerDataForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      name: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(50), this.lettersOnly]],
      surname: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(50), this.lettersOnly]],
      city: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(50), this.lettersOnly]],
      postalCode: ['', [Validators.required, Validators.pattern(/^\d{2}-\d{3}$/)]],
      street: ['', [Validators.required, Validators.maxLength(50)]],
      address: ['', [Validators.required, Validators.maxLength(50)]],
      phoneNumber: ['', [Validators.required, Validators.pattern(/^\d{3}[-\s]?\d{3}[-\s]?\d{3}$/)]],
    },);
  }

  initializeChangePasswordForm() {
    this.changePasswordForm = this.fb.group({
      oldPassword: ['', [Validators.required, Validators.minLength(6)]],
      newPassword: ['', [Validators.required, Validators.minLength(6)]],
      confirmNewPassword: ['', [Validators.required, Validators.minLength(6)]]
    }, {
      validators: this.mustMatch('newPassword', 'confirmNewPassword')
    })
  }

  changePassword() {
    if (this.changePasswordForm.valid) {
      const userId = localStorage.getItem('userId');
      const passwordChangeRequest = this.changePasswordForm.value;
      if (userId) {
        this.customerService.changePassword(userId, passwordChangeRequest).subscribe({
          next: () => {
            console.log('Hasło zostało zmienione.');
            this.snackBar.open('Hasło zostało pomyślnie zmienione.', 'Zamknij', {
              duration: 3000,
            });
          },
          error: (error) => {
            let errorMessage = 'Nie udało się zmienić hasła. Spróbuj ponownie.';
            if (error.error && typeof error.error.message === 'string') {
              errorMessage = error.error.message;
            }
            this.snackBar.open(errorMessage, 'Zamknij', {
              duration: 5000,
            });
          }
        });
      }
    }
  }

  lettersOnly(control: AbstractControl): ValidationErrors | null {
    const letters = /^[A-Za-ząćęłńóśźżĄĆĘŁŃÓŚŹŻ]+$/;
    return letters.test(control.value) ? null : { 'lettersOnly': true };
  }

  loadCustomerData() {
    const userId = localStorage.getItem('userId');
    if (userId) {
      this.customerService.getCustomer(userId).subscribe(
        customer => {
          this.customerDataForm.patchValue(customer);
          this.customerFirstName = customer.name || '';
        },
        (error) => {
          console.log('Wystąpił błąd podczas pobierania danych klienta', error);
        }
      );
    }
  }

  loadOrdersHistory() {
    const userId = localStorage.getItem('userId');
    if (userId) {
      this.isLoading = true;
      this.orderService.getOrdersHistory(userId).subscribe({
        next: (orders) => {
          this.orders = orders.map(order => ({
            ...order,
            shortOrderId: order.orderId.slice(0, 8)
          }));
          this.isLoading = false;
          if (orders.length === 0) {
            console.log('Nie znaleziono historii zamówień.');
          }
        },
        error: (error) => {
          console.error('Wystąpił błąd podczas pobierania historii zamówień.', error);
          this.isLoading = false;
        }
      });
    }
  }

  mustMatch(controlName: string, matchingControlName: string) {
    return (formGroup: FormGroup) => {
      const control = formGroup.controls[controlName];
      const matchingControl = formGroup.controls[matchingControlName];

      if (matchingControl.errors && !matchingControl.errors['mustMatch']) {
        return;
      }

      if (control.value !== matchingControl.value) {
        matchingControl.setErrors({ mustMatch: true });
      } else {
        matchingControl.setErrors(null);
      }
    };
  }

  setActiveSection(section: 'accountInfo' | 'addresses' | 'changePassword' | 'orderHistory') {
    this.activeSection = section;
  }

  updateCustomerData() {
    if (this.customerDataForm.valid) {
      const userId = localStorage.getItem('userId');
      if (userId) {
        this.customerService.updateCustomer(userId, this.customerDataForm.value).subscribe({
          next: () => {
            console.log('Dane zostały pomyślnie zaktualizowane.');
            this.snackBar.open('Dane zostały pomyślnie zaktualizowane.', 'Zamknij', {
              duration: 3000,
            });
          },
          error: (error) => {
            let errorMessage = 'Nie udało się zaktualizować danych. Spróbuj ponownie.';
            if (error.error && typeof error.error === 'string') {
              errorMessage = error.error;
            }
            this.snackBar.open(errorMessage, 'Zamknij', {
              duration: 5000,
            });
          }
        });
      }
    } else {
      this.snackBar.open('Formularz zawiera błędy. Proszę je poprawić.', 'Zamknij', {
        duration: 5000,
      });
    }
  }

  viewOrderDetails(orderId: string) {
    this.router.navigate(['/order-details', orderId]);
  }
}