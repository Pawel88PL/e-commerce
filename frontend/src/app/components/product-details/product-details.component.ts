import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ProductService } from 'src/app/services/product.service';
import { Product } from 'src/app/models/product.model';

@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.css']
})

export class ProductDetailsComponent implements OnInit {
  product: Product = new Product();
  products: Product[] = [];

  constructor(
    private route: ActivatedRoute,
    private productService: ProductService,
    private router: Router
  ) { }

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
    "slidesToShow": 3,  // Ilość widocznych produktów jednocześnie
    "slidesToScroll": 1,
    "dots": true,
    "infinite": true,
    "arrows": true
  };
}
