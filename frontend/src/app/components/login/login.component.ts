import { AuthService } from 'src/app/services/auth.service';
import { Component, OnInit, ViewChild, ElementRef, AfterViewInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})

export class LoginComponent implements OnInit, AfterViewInit {
  loginForm!: FormGroup;
  @ViewChild('autoFocusInput') autoFocusInput!: ElementRef;

  constructor(
    private authService: AuthService,
    private formBuilder: FormBuilder,
    private router: Router,
    private snackBar: MatSnackBar
  ) { }

  ngOnInit(): void {
    this.loginForm = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]]
    })
  }

  ngAfterViewInit() {
    setTimeout(() => {
      this.autoFocusInput.nativeElement.focus();
    });
  }

  onSubmit() {
    if (this.loginForm.valid) {
      const loginData = this.loginForm.value;

      this.authService.login(loginData.email, loginData.password).subscribe(
        success => {
          if (localStorage.getItem('inCheckoutProcess')) {
            this.router.navigate(['/order']);
            localStorage.removeItem('inCheckoutProcess');
          } else {
            const roles = this.authService.getRoles();
            if (roles.includes('Admin')) {
              this.router.navigate(['/warehouse']);
            } else {
              this.router.navigate(['/products']);
            }
          }
        },
        error => {
          this.snackBar.open(error.message, 'Zamknij', {
            duration: 5000,
          });
        }
      );
    }
  }
  
}