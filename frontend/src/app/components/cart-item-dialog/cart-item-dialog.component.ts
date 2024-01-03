import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Router } from '@angular/router';

@Component({
  selector: 'app-cart-item-dialog',
  templateUrl: './cart-item-dialog.component.html',
  styleUrls: ['./cart-item-dialog.component.css']
})
export class CartItemDialogComponent {
  constructor(
    public dialogRef: MatDialogRef<CartItemDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private router: Router
  ) {}

  onCheckout(): void {
    this.dialogRef.close();
  }

  onViewCart(): void {
    this.router.navigate(['/cart']);
    this.dialogRef.close();
  }
}
