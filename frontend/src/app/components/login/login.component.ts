import { AuthService } from 'src/app/services/auth.service';
import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  loginData = {
    username: '',
    password: ''
  };

  constructor(private authService: AuthService, private router: Router) {}

  onSubmit() {
    this.authService.login(this.loginData.username, this.loginData.password).subscribe(
      success => {
        this.router.navigate(['/warehouse']);
      },
      error => {
        console.error(error);
        console.log('Nie udało zalogować');
      }
    )
  }

  onRegister() {
    // logika rejestracji
  }
  
}
