import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-checkout',
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.css']
})

export class CheckoutComponent { 
  constructor( private authService: AuthService, private router: Router) { }

  navigateToGuestOrder() {
    this.authService.setInCheckoutProcessAsGuest();
    
    this.router.navigate(['/guestOrder']);
  }

  navigateToLogin() {
    this.router.navigate(['/login']);
  }
}
