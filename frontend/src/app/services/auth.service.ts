import { CartService } from './cart.service';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, observeOn, tap, catchError, throwError } from 'rxjs';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})

export class AuthService {

  constructor(private cartService: CartService, private http: HttpClient, private router: Router, private snackBar: MatSnackBar) { }

  login(email: string, password: string): Observable<any> {
    return this.http.post<any>('https://localhost:5047/login', { email, password }).pipe(
      tap(res => {
        localStorage.setItem('token', res.token);
        localStorage.setItem('roles', JSON.stringify(res.roles));
        if (res.name) {
          this.setName(res.name);
        };
        if (this.cartService.cartId) {
          this.cartService.assignCartToUser(res.userId).subscribe();
        }
      }),
      catchError(error => {
        let message = 'Wystąpił błąd podczas logowania';
        if (error.status === 401) {
          message = error.error;
        }
        return throwError(() => new Error(message));
      })
    );
  }

  isAdmin(): boolean {
    const roles = this.getRoles();
    return roles.includes('Admin');
  }

  isClient(): boolean {
    const roles = this.getRoles();
    return roles.includes('Client');
  }

  getRoles(): string[] {
    const roles = localStorage.getItem('roles');
    return roles ? JSON.parse(roles) : [];
  }

  register(user: any): Observable<any> {
    return this.http.post('https://localhost:5047/register', user);
  }

  isLoggedIn(): boolean {
    return !!localStorage.getItem('token');
  }

  logout(): void {
    localStorage.removeItem('token');
    localStorage.removeItem('email');
    this.router.navigate(['/login']);
  }

  setName(name: string) {
    localStorage.setItem('name', name);
  }

  getName(): string | null {
    return localStorage.getItem('name');
  }
}
