import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';
import { CartService } from 'src/app/services/cart.service';
import { Component, OnInit } from '@angular/core';
import { environment } from 'src/environments/environment';
import { MatDialog } from '@angular/material/dialog';
import { Product } from 'src/app/models/product.model';
import { ProductService } from 'src/app/services/product.service';
import { CartItemDialogComponent } from '../cart-item-dialog/cart-item-dialog.component';
import { gsap } from 'gsap';

@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.css']
})

export class ProductDetailsComponent implements OnInit {
  apiBaseUrl: string = environment.apiUrl;
  product: Product = new Product();
  products: Product[] = [];
  formattedPrice: string = '';

  constructor(
    public authService: AuthService,
    private cartService: CartService,
    public dialog: MatDialog,
    private productService: ProductService,
    private route: ActivatedRoute,
    private router: Router,
  ) { }

  onAddToCart() {
    this.cartService.addToCart(this.product).subscribe((result) => {
      const firstImage = this.product.productImages?.[0]?.imagePath;
      this.dialog.open(CartItemDialogComponent, {
        maxWidth: '500px',
        data: {
          image: firstImage ? this.apiBaseUrl + firstImage : null,
          name: this.product.name,
          price: this.product.price
        }
      });
    });
  }

  ngOnInit() {
    const idParm: string | null = this.route.snapshot.paramMap.get('id');

    gsap.from('.product-details', {
      duration: 1,
      y: '-100%',
      opacity: 0,
      scale: 0.5,
      delay: 0.5,
      ease: "power1.out"
    });


    if (idParm) {
      const id = +idParm;
      this.productService.getProductById(id).subscribe(
        product => {
          this.product = product
          this.formattedPrice = this.product.price.toFixed(2) + ' zł';
        },
        error => console.log('Wystąpił problem ze znalezieniem:', error));
    }
    else {
      this.router.navigate(['/']);
    }
  }
}