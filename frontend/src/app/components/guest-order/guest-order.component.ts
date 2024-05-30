import { AuthService } from 'src/app/services/auth.service';
import { CartService } from 'src/app/services/cart.service';
import { OrderService } from 'src/app/services/order.service';
import { Component, OnInit, ViewChild, ElementRef, AfterViewInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ValidationErrors, AbstractControl } from '@angular/forms';
import { ConfirmDialogComponent } from '../confirm-dialog/confirm-dialog.component';
import { OrderProcessingDialogComponent } from '../order-processing-dialog/order-processing-dialog.component';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { Customer } from 'src/app/models/customer.model';
import { environment } from 'src/environments/environment';
import { CartItem } from 'src/app/models/cart.model';
import { SHIPPING_COST } from 'src/app/config/config';
import { gsap } from 'gsap';

@Component({
  selector: 'app-guest-order',
  templateUrl: './guest-order.component.html',
  styleUrls: ['./guest-order.component.css']
})

export class GuestOrderComponent implements OnInit, AfterViewInit {

  apiBaseUrl: string = environment.apiUrl;
  cartId: string = localStorage.getItem('cartId') || '';
  userId: string = '';
  customer: Customer = {};
  items: CartItem[] = [];
  isLoading = false;
  isPickupInStore: boolean = false;
  productCost: number = 0;
  shippingCost: number = SHIPPING_COST;
  totalCost: number = 0;


  @ViewChild('autoFocusInput') autoFocusInput!: ElementRef;
  @ViewChild('postalCodeInput') postalCodeInput!: ElementRef;
  @ViewChild('phoneNumberInput') phoneNumberInput!: ElementRef;
  @ViewChild('nameInput') nameInput!: ElementRef;
  @ViewChild('surnameInput') surnameInput!: ElementRef;
  @ViewChild('cityInput') cityInput!: ElementRef;
  @ViewChild('streetInput') streetInput!: ElementRef;

  constructor(
    private authService: AuthService,
    private cartService: CartService,
    private orderService: OrderService,
    public dialog: MatDialog,
    private formBuilder: FormBuilder,
    private router: Router,
    private snackBar: MatSnackBar
  ) { }

  loading: boolean = false;
  registerForm!: FormGroup;

  ngOnInit(): void {

    this.loadCartItems();

    gsap.from('.personal-data', {
      duration: 1,
      x: '-100%',
      opacity: 0,
      scale: 0.5,
      delay: 1,
      ease: "power1.out"
    });

    gsap.from('.cart', {
      duration: 1,
      x: '100%',
      opacity: 0,
      scale: 0.5,
      delay: 0.5,
      ease: "power1.out"
    });

    gsap.from('.logo', {
      duration: 1,
      y: '100%',
      opacity: 0,
      scale: 0.5,
      delay: 2,
      ease: "power1.out"
    });

    this.registerForm = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email]],
      name: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(50), this.lettersOnly]],
      surname: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(50), this.lettersOnly]],
      city: [{ value: '', disabled: this.isPickupInStore }, [Validators.required, Validators.minLength(2), Validators.maxLength(50), this.lettersOnly]],
      postalCode: ['', [Validators.required, Validators.pattern(/^\d{2}-\d{3}$/)]],
      street: ['', [Validators.required, Validators.maxLength(50)]],
      address: ['', [Validators.required, Validators.maxLength(50)]],
      phoneNumber: ['', [Validators.required, Validators.pattern(/^\d{3}[-\s]?\d{3}[-\s]?\d{3}$/)]],
      password: ['temporaryPassword'],
      termsAccepted: [false, Validators.requiredTrue],
      isGuestClient: [true]
    });
  }

  ngAfterViewInit() {
    setTimeout(() => {
      this.autoFocusInput.nativeElement.focus();
    });

    this.postalCodeInput.nativeElement.addEventListener('input', this.onPostalCodeInputChange.bind(this));
    this.phoneNumberInput.nativeElement.addEventListener('input', this.onPhoneNumberInputChange.bind(this));
    this.nameInput.nativeElement.addEventListener('input', (event: Event) => this.changeFirstLettertoUpperInInputs(event, 'name'));
    this.surnameInput.nativeElement.addEventListener('input', (event: Event) => this.changeFirstLettertoUpperInInputs(event, 'surname'));
    this.cityInput.nativeElement.addEventListener('input', (event: Event) => this.changeFirstLettertoUpperInInputs(event, 'city'));
    this.streetInput.nativeElement.addEventListener('input', (event: Event) => this.changeFirstLettertoUpperInInputs(event, 'street'));
  }

  onPostalCodeInputChange(event: Event): void {
    const input = event.target as HTMLInputElement;
    let value = input.value.replace(/\D/g, '');

    if (value.length > 2) {
      value = value.substring(0, 2) + '-' + value.substring(2, 5);
    }

    input.value = value;
    this.registerForm.controls['postalCode'].setValue(value, { emitEvent: false });
  }

  onPhoneNumberInputChange(event: Event): void {
    const input = event.target as HTMLInputElement;
    let value = input.value.replace(/\D/g, '');

    if (value.length > 6) {
      value = value.substring(0, 3) + '-' + value.substring(3, 6) + '-' + value.substring(6, 9);
    } else if (value.length > 3) {
      value = value.substring(0, 3) + '-' + value.substring(3, 6);
    }

    input.value = value;
    this.registerForm.controls['phoneNumber'].setValue(value, { emitEvent: false });
  }

  changeFirstLettertoUpperInInputs(event: Event, controlName: string): void {
    const input = event.target as HTMLInputElement;
    input.value = input.value.charAt(0).toUpperCase() + input.value.slice(1).toLowerCase();
    this.registerForm.controls[controlName].setValue(input.value, { emitEvent: false });
  }

  lettersOnly(control: AbstractControl): ValidationErrors | null {
    const letters = /^[A-Za-ząćęłńóśźżĄĆĘŁŃÓŚŹŻ]+$/;
    return letters.test(control.value) ? null : { 'lettersOnly': true };
  }

  toggleAddressFields(): void {
    if (this.isPickupInStore) {
      this.registerForm.get('city')?.disable();
      this.registerForm.get('postalCode')?.disable();
      this.registerForm.get('street')?.disable();
      this.registerForm.get('address')?.disable();
    } else {
      this.registerForm.get('city')?.enable();
      this.registerForm.get('postalCode')?.enable();
      this.registerForm.get('street')?.enable();
      this.registerForm.get('address')?.enable();
    }
  }

  loadCartItems() {
    if (this.cartId) {
      this.cartService.getItems().subscribe(
        (response: any) => {
          this.items = response.cartItems;
          this.calculateCosts();
        },
        error => {
          console.error('Błąd podczas ładowania danych koszyka!', error);
        }
      );
    }
  }

  calculateCosts() {
    if (Array.isArray(this.items)) {
      this.productCost = this.items.reduce((acc, item) => acc + item.price * item.quantity, 0);
      if (this.isPickupInStore) {
        this.shippingCost = 0;
        this.totalCost = this.productCost;
      } else {
        this.shippingCost = SHIPPING_COST;
        this.totalCost = this.productCost + this.shippingCost;
      }
    } else {
      console.error('this.items nie jest tablicą');
    }
  }

  placeOrder() {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      data: { totalCost: this.totalCost }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        const processingDialogRef = this.dialog.open(OrderProcessingDialogComponent, {
          disableClose: true
        });

        this.authService.register(this.registerForm.value).subscribe(
          response => {
            const userId = response.userId;
            this.cartService.assignCartToUser(userId);
            this.orderService.createOrder(this.cartId, userId, this.isPickupInStore).subscribe({
              next: (order) => {
                console.log('Zamówienie zostało złożone', order);
                localStorage.removeItem('cartId');
                this.authService.removeInCheckoutProcess();
                this.router.navigate(['/orderConfirmation'], { queryParams: { orderId: order } });
                processingDialogRef.close();
              },
              error: (error) => {
                console.error('Wystąpił błąd przy tworzeniu zamówienia', error);
                processingDialogRef.close();
              }
            });
          },
          error => {
            console.error('Error registering user:', error);
            processingDialogRef.close();
          }
        );
      } else {
        console.log('Order cancelled.');
      }
    });
  }
}