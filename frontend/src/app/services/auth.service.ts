import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap, catchError, throwError } from 'rxjs';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment';
import { CartService } from './cart.service';
import { JwtService } from './jwt.service';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private Url = environment.apiUrl;
  private apiBaseUrl = `${this.Url}/account`;

  constructor(
    private cartService: CartService,
    private http: HttpClient,
    private router: Router,
    private snackBar: MatSnackBar,
    private jwtService: JwtService
  ) { }

  login(email: string, password: string): Observable<any> {
    return this.http.post<any>(`${this.apiBaseUrl}/login`, { email, password }).pipe(
      tap(res => {
        this.setToken(res.token);
        const decodedToken = this.jwtService.decodeToken(res.token);
        if (this.cartService.cartId) {
          this.cartService.assignCartToUser(decodedToken.nameid).subscribe();
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


  register(user: any): Observable<any> {
    return this.http.post<any>(`${this.apiBaseUrl}/register`, user).pipe(
      tap(res => {
        this.setToken(res.token);
        const decodedToken = this.jwtService.decodeToken(res.token);
        if (this.cartService.cartId) {
          this.cartService.assignCartToUser(decodedToken.nameid).subscribe();
        }
      }),
      catchError(error => {
        let message = 'Wystąpił błąd podczas rejestracji';
        if (error.status === 400) {
          message = error.error;
        }
        return throwError(() => new Error(message));
      })
    );
  }

  checkUserExists(email: string): Observable<any> {
    return this.http.post(`${this.apiBaseUrl}/checkUserExists`, { email: email });
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }

  getName(): string | null {
    const token = this.getToken();
    if (!token) return null;
    const decodedToken = this.jwtService.decodeToken(token);
    return decodedToken.unique_name;
  }

  getRoles(): string[] {
    const token = this.getToken();
    if (!token) return [];
    const decodedToken = this.jwtService.decodeToken(token);
    return Array.isArray(decodedToken.role) ? decodedToken.role : [decodedToken.role];
  }

  getUserId(): string | null {
    const token = this.getToken();
    if (!token) return null;
    const decodedToken = this.jwtService.decodeToken(token);
    return decodedToken.nameid;
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
    const token = this.getToken();
    return !!token && !this.jwtService.isTokenExpired(token);
  }

  logout(): void {
    localStorage.removeItem('token');
    this.router.navigate(['/login']);
  }

  setToken(token: string): void {
    localStorage.setItem('token', token);
  }

  setInCheckoutProcess(): void {
    localStorage.setItem('inCheckoutProcess', 'true');
  }

  setInCheckoutProcessAsGuest(): void {
    localStorage.setItem('inCheckoutProcessAsGuest', 'true');
  }

  getInCheckoutProcess(): boolean {
    const inCheckoutProcess = localStorage.getItem('inCheckoutProcess');
    return inCheckoutProcess ? true : false;
  }

  removeInCheckoutProcess(): void {
    localStorage.removeItem('inCheckoutProcess');
    localStorage.removeItem('inCheckoutProcessAsGuest');
  }
}
