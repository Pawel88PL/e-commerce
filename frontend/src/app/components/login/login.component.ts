import { AuthService } from 'src/app/services/auth.service';
import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { GenericDialogComponent } from '../generic-dialog/generic-dialog.component';

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

  constructor(private authService: AuthService, public dialog: MatDialog,private router: Router) {}

  onSubmit() {
    this.authService.login(this.loginData.username, this.loginData.password).subscribe(
      success => {
        localStorage.setItem('token', success.token);
        this.router.navigate(['/warehouse']);
      },
      error => {
        console.error(error);
      }
    )
  }

  onRegister() {
    this.openDialog();
  }

  openDialog() {
    this.dialog.open(GenericDialogComponent, {
      data: {
        title: 'Rejestracja użytkownika',
        message: 'Funkcja będzie dosępna wkrótce.',
        showCancelButton: false
      }
    });
  }
}