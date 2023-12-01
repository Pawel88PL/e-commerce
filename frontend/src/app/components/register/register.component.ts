import { AuthService } from 'src/app/services/auth.service';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
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
      name: ['', [Validators.required, Validators.maxLength(30)]],
      surname: ['', [Validators.required, Validators.maxLength(30)]],
      password: ['', [Validators.required, Validators.minLength(6)]]
    })
  }

  onSubmit() {
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