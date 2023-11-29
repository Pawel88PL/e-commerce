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
      username: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]]
    })
  }

  onSubmit() {
    if (this.registerForm.valid) {
      this.authService.register(this.registerForm.value).subscribe(
        success => {
          this.authService.login(this.registerForm.value.username, this.registerForm.value.password).subscribe(
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
          this.snackBar.open('Wystąpił błąd podczas próby rejestracji!', 'Zamknij', { duration: 3000 });
        }
      );
    }
  }
}