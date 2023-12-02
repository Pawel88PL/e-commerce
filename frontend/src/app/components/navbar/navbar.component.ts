import { AuthService } from 'src/app/services/auth.service';
import { Component } from '@angular/core';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})

export class NavbarComponent {
  
  constructor(public authService: AuthService) {}

  logout() {
    this.authService.logout();
  }

  get name(): string {
    return this.authService.getName() ?? '';
  }

  closeNavbar() {
    const navbarToggler = document.querySelector('.navbar-toggler');
    const navbarMenu = document.querySelector('#navbarNav');

    if (navbarToggler && !navbarToggler.classList.contains('collapsed')) {
      navbarToggler.classList.add('collapsed');
      if (navbarMenu) {
        navbarMenu.classList.remove('show');
      }
    }
  }
}
