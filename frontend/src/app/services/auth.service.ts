import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, observeOn } from 'rxjs';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private username: string | null = null;

  constructor(private http: HttpClient, private router: Router) { }

  login(username: string, password: string): Observable<any> {
    return new Observable(observer => {
      this.http.post<any>('https://localhost:5047/login', {username, password}).subscribe(
        response => {
          localStorage.setItem('token', response.token);
          this.setUsername(username);
          observer.next(response)
          observer.complete();
        },
        error => {
          observer.error(error);
        }
      )
    })
  }


  register(user: any): Observable<any> {
    return this.http.post('https://localhost:5047/register', user);
  }


  isLoggedIn(): boolean {
    return !!localStorage.getItem('token');
  }

  logout(): void {
    this.clearUsername();
    localStorage.removeItem('token');
    this.router.navigate(['/login']);
  }

  setUsername(username: string) {
    this.username = username;
  }

  getUsername(): string | null {
    return this.username;
  }

  private clearUsername() {
    this.username = null;
  }
}
