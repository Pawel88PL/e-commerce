import { ActivatedRoute, Router } from '@angular/router';
import { CartService } from 'src/app/services/cart.service';
import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Product } from 'src/app/models/product.model';
import { ProductService } from 'src/app/services/product.service';
import { CartItemDialogComponent } from '../cart-item-dialog/cart-item-dialog.component';

@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.css']
})

export class ProductDetailsComponent implements OnInit {
  product: Product = new Product();
  products: Product[] = [];

  constructor(
    private cartService: CartService,
    private route: ActivatedRoute,
    private router: Router,
    private productService: ProductService,
    public dialog: MatDialog,
  ) { }

  onAddToCart() {
    this.cartService.addToCart(this.product).subscribe((result) => {
      const firstImage = this.product.productImages?.[0]?.imagePath;
      this.dialog.open(CartItemDialogComponent, {
        data: {
          image: firstImage ? 'https://localhost:5047/' + firstImage : null,
          name: this.product.name,
          price: this.product.price
        }
      });
    });
  }

  ngOnInit() {
    const idParm: string | null = this.route.snapshot.paramMap.get('id');

    if (idParm) {
      const id = +idParm;
      this.productService.getProductById(id).subscribe(product => { this.product = product }, error => console.log('Error fetching product:', error));
    }
    else {
      this.router.navigate(['/']);
    }

  }
}