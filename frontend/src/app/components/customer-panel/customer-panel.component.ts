import { Component, OnInit } from '@angular/core';
import { Customer } from 'src/app/models/customer.model';
import { CustomerService } from 'src/app/services/customer.service';
import { FormGroup, FormControl, Validators, FormBuilder, AbstractControl, ValidationErrors } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-customer-panel',
  templateUrl: './customer-panel.component.html',
  styleUrls: ['./customer-panel.component.css']
})
export class CustomerPanelComponent implements OnInit {
  activeSection: 'accountInfo' | 'addresses' | 'changePassword' | 'orderHistory' = 'accountInfo';
  customer: Customer = {};
  customerForm!: FormGroup;
  orders: any[] = [];

  constructor(private customerService: CustomerService, private fb: FormBuilder, private snackBar: MatSnackBar) { }

  ngOnInit(): void {
    this.initializeForm();
    this.loadCustomerData();
  }

  initializeForm() {
    this.customerForm = this.fb.group({
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

  lettersOnly(control: AbstractControl): ValidationErrors | null {
    const letters = /^[A-Za-ząćęłńóśźżĄĆĘŁŃÓŚŹŻ]+$/;
    return letters.test(control.value) ? null : { 'lettersOnly': true };
  }

  loadCustomerData() {
    const userId = localStorage.getItem('userId');
    if (userId) {
      this.customerService.getCustomer(userId).subscribe(
        customer => {
          this.customerForm.patchValue(customer);
        },
        (error) => {
          console.log('Wystąpił błąd podczas pobierania danych klienta', error);
        }
      );
    }
  }

  passwordMatchValidator(form: FormGroup): ValidationErrors | null {
    const password = form.get('password')?.value;
    const confirmPassword = form.get('confirmPassword')?.value;
    if (password && confirmPassword && password !== confirmPassword) {
      form.get('confirmPassword')?.setErrors({ mismatch: true });
      return { mismatch: true };
    } else if (password && !confirmPassword) {
      form.get('confirmPassword')?.setErrors({ mismatch: true });
      return { mismatch: true };
    } else {
      form.get('confirmPassword')?.setErrors(null);
      return null;
    }
  }

  setActiveSection(section: 'accountInfo' | 'addresses' | 'changePassword' | 'orderHistory') {
    this.activeSection = section;
  }

  updateCustomerData() {
    if (this.customerForm.valid) {
      const userId = localStorage.getItem('userId');
      if (userId) {
        this.customerService.updateCustomer(userId, this.customerForm.value).subscribe({
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
}