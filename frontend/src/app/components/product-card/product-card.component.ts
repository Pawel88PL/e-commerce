import { Component, Input } from '@angular/core';
import { CartService } from 'src/app/services/cart.service';
import { CartItemDialogComponent } from '../cart-item-dialog/cart-item-dialog.component';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-product-card',
  templateUrl: './product-card.component.html',
  styleUrls: ['./product-card.component.css']
})
export class ProductCardComponent {
  
  @Input() product: any;

  constructor(private cartService: CartService, private dialog: MatDialog) {}

  onAddToCart() {
    this.cartService.addToCart(this.product).subscribe((result) => {
      const firstImage = this.product.productImages?.[0]?.imagePath;
      this.dialog.open(CartItemDialogComponent, {
        maxWidth: '500px',
        data: {
          image: firstImage ? 'https://localhost:5047/' + firstImage : null,
          name: this.product.name,
          price: this.product.price
        }
      });
    });
  }
}
