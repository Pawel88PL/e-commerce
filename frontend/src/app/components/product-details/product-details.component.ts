import { ActivatedRoute, Router } from '@angular/router';
import { CartService } from 'src/app/services/cart.service';
import { Component, OnInit } from '@angular/core';
import { Product } from 'src/app/models/product.model';
import { ProductService } from 'src/app/services/product.service';

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
    private productService: ProductService
  ) { }

  onAddToCart() {
    this.cartService.addToCart(this.product)
    .then(() => {
      alert(`'Produkt ${this.product.name} został dodany do koszyka.'`);
    })
    .catch(error => {
      alert(`Błąd: ${error.message || 'Nie udało się dodać produktu do koszyka.'}`);
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

    this.productService.getProducts().subscribe(data => { this.products = data; });
  }

  slideConfig = {
    "slidesToShow": 3,
    "slidesToScroll": 1,
    "dots": true,
    "infinite": true,
    "arrows": true
  };
}
