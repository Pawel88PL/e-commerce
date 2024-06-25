import { AuthService } from 'src/app/services/auth.service';
import { CartService } from 'src/app/services/cart.service';
import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Router } from '@angular/router';

@Component({
  selector: 'app-cart-item-dialog',
  templateUrl: './cart-item-dialog.component.html',
  styleUrls: ['./cart-item-dialog.component.css']
})
export class CartItemDialogComponent {

  userId: string = this.authService.getUserId() || '';
  
  constructor(
    public dialogRef: MatDialogRef<CartItemDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private authService: AuthService,
    private cartService: CartService,
    private router: Router
  ) { }

  onCheckout(): void {
    this.authService.setInCheckoutProcess();
    if (this.authService.isLoggedIn()) {
      this.cartService.assignCartToUser(this.userId).subscribe();
      this.router.navigate(['/order']);
    } else {
      this.router.navigate(['/checkout']);
    }
    this.dialogRef.close();
  }

  onViewCart(): void {
    this.router.navigate(['/cart']);
    this.dialogRef.close();
  }
}
