import { AuthService } from 'src/app/services/auth.service';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})

export class LoginComponent implements OnInit {
  loginForm!: FormGroup;

  constructor(
    private authService: AuthService,
    private formBuilder: FormBuilder,
    private router: Router,
    private snackBar: MatSnackBar
  ) { }

  ngOnInit(): void {
    this.loginForm = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required]
    })
  }

  onSubmit() {
    if (this.loginForm.valid) {
      const loginData = this.loginForm.value;

      this.authService.login(loginData.email, loginData.password).subscribe(
        success => {
          const roles = this.authService.getRoles();
          if (roles.includes('Admin')) {
            this.router.navigate(['/warehouse']);
          } else {
            this.router.navigate(['/products']);
          }
        },
        error => {
          console.error(error);
          let message = 'Błąd logowania: sprawdź swój email i hasło.';
          if (error.error && typeof error.error === 'string') {
            message = error.error;
          }
          this.snackBar.open(message, 'Zamknij', {
            duration: 3000,
          });
        }
      );
    }
  }
}