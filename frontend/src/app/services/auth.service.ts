import { CartService } from './cart.service';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, observeOn, tap, catchError, throwError } from 'rxjs';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})

export class AuthService {

  private apiBaseUrl = environment.apiUrl;

  constructor(private cartService: CartService, private http: HttpClient, private router: Router, private snackBar: MatSnackBar) { }

  login(email: string, password: string): Observable<any> {
    return this.http.post<any>(`${this.apiBaseUrl}/login`, { email, password }).pipe(
      tap(res => {
        this.setToken(res.token);
        this.setRoles(res.roles);
        if (res.name) {
          this.setName(res.name);
        };
        localStorage.setItem('userId', res.userId);
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

  getInCheckoutProcess(): boolean {
    const inCheckoutProcess = localStorage.getItem('inCheckoutProcess');
    return inCheckoutProcess ? true : false;
  }

  getName(): string | null {
    return localStorage.getItem('name');
  }

  getRoles(): string[] {
    const roles = localStorage.getItem('roles');
    return roles ? JSON.parse(roles) : [];
  }

  isAdmin(): boolean {
    const roles = this.getRoles();
    return roles.includes('Admin');
  }

  isClient(): boolean {
    const roles = this.getRoles();
    return roles.includes('Client');
  }

  isLoggedIn(): boolean {
    return !!localStorage.getItem('token');
  }

  logout(): void {
    localStorage.removeItem('token');
    localStorage.removeItem('email');
    localStorage.removeItem('userId');
    this.router.navigate(['/login']);
  }

  register(user: any): Observable<any> {
    return this.http.post(`${this.apiBaseUrl}/register`, user);
  }

  removeInCheckoutProcess() {
    localStorage.removeItem('inCheckoutProcess');
  }

  setName(name: string) {
    localStorage.setItem('name', name);
  }

  setRoles(roles: string[]) {
    localStorage.setItem('roles', JSON.stringify(roles));
  }

  setToken(token: string) {
    localStorage.setItem('token', token);
  }

  setInCheckoutProcess() {
    localStorage.setItem('inCheckoutProcess', 'true');
  }
}
