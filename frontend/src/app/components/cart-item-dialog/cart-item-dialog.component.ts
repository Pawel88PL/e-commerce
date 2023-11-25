import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-cart-item-dialog',
  templateUrl: './cart-item-dialog.component.html',
  styleUrls: ['./cart-item-dialog.component.css']
})
export class CartItemDialogComponent {
  constructor(
    public dialogRef: MatDialogRef<CartItemDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {}

  onCheckout(): void {
    this.dialogRef.close();
  }

  onViewCart(): void {
    this.dialogRef.close();
  }
}
