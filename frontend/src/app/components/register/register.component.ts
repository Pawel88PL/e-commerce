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