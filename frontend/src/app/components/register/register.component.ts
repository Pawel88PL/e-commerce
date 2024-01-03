import { AuthService } from 'src/app/services/auth.service';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ValidationErrors } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  constructor(
    private authService: AuthService,
    private formBuilder: FormBuilder,
    private router: Router,
    private snackBar: MatSnackBar
  ) { }

  registerForm!: FormGroup;

  ngOnInit(): void {
    this.registerForm = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email]],
      name: ['', [Validators.required, Validators.maxLength(50)]],
      surname: ['', [Validators.required, Validators.maxLength(50)]],
      city: ['', [Validators.required, Validators.maxLength(50)]],
      postalCode: ['', [Validators.required, Validators.pattern(/^\d{2}-\d{3}$/)]],
      street: ['', [Validators.required, Validators.maxLength(50)]],
      address: ['', [Validators.required, Validators.maxLength(50)]],
      phoneNumber: ['', [Validators.required, Validators.pattern(/^\d{3}[-\s]?\d{3}[-\s]?\d{3}$/)]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', Validators.required]
    }, { validator: this.passwordMatchValidator })
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
    console.log(this.registerForm.valid);
    console.log(this.registerForm.errors);
    if (this.registerForm.valid) {
      this.authService.register(this.registerForm.value).subscribe(
        success => {
          this.authService.login(this.registerForm.value.email, this.registerForm.value.password).subscribe(
            loginSuccess => {
              localStorage.setItem('token', loginSuccess.token);
              this.router.navigate(['/products']);
            },
            loginError => {
              console.error(loginError);
            }
          );
        },
        error => {
          console.error(error);
          let errorMsg = 'Wystąpił błąd podczas próby rejestracji!';
          if (error.error && typeof error.error === 'string') {
            errorMsg = error.error;
          }
          this.snackBar.open(errorMsg, 'Zamknij', { duration: 3000 });
        }
      );
    }
  }
}