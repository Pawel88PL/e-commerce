import { AuthService } from 'src/app/services/auth.service';
import { Component, OnInit, ViewChild, ElementRef, AfterViewInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ValidationErrors, AbstractControl } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit, AfterViewInit {

  @ViewChild('autoFocusInput') autoFocusInput!: ElementRef;
  @ViewChild('postalCodeInput') postalCodeInput!: ElementRef;
  @ViewChild('phoneNumberInput') phoneNumberInput!: ElementRef;
  @ViewChild('nameInput') nameInput!: ElementRef;
  @ViewChild('surnameInput') surnameInput!: ElementRef;
  @ViewChild('cityInput') cityInput!: ElementRef;
  @ViewChild('streetInput') streetInput!: ElementRef;

  constructor(
    private authService: AuthService,
    private formBuilder: FormBuilder,
    private router: Router,
    private snackBar: MatSnackBar
  ) { }

  loading: boolean = false;
  registerForm!: FormGroup;

  ngOnInit(): void {
    this.registerForm = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email]],
      name: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(50), this.lettersOnly]],
      surname: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(50), this.lettersOnly]],
      city: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(50), this.lettersOnly]],
      postalCode: ['', [Validators.required, Validators.pattern(/^\d{2}-\d{3}$/)]],
      street: ['', [Validators.required, Validators.maxLength(50)]],
      address: ['', [Validators.required, Validators.maxLength(50)]],
      phoneNumber: ['', [Validators.required, Validators.pattern(/^\d{3}[-\s]?\d{3}[-\s]?\d{3}$/)]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', Validators.required],
      termsAccepted: [false, Validators.requiredTrue]
    }, { validator: this.passwordMatchValidator });
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

  onSubmit() {
    if (this.registerForm.valid) {
      this.loading = true;
      this.authService.register(this.registerForm.value).subscribe(
        success => {
          this.loading = true;
          this.snackBar.open('Trwa przetwarzanie Twojej rejestracji...', 'OK', { duration: 2000 });
          this.router.navigate(['/waitingForActivation']);
        },
        error => {
          this.loading = false;
          let errorMsg = 'Wystąpił błąd podczas próby rejestracji!';
          if (error.error && typeof error.error === 'string') {
            errorMsg = error.error;
          }
          this.snackBar.open(errorMsg, 'Zamknij', { duration: 3000 });
        }
      );
    } else {
      this.snackBar.open('Uzupełnij dane formularza', 'Zamknij', { duration: 3000 });
    }
  }
}