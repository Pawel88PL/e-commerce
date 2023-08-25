import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ProductService } from 'src/app/services/product.service';
import { Product } from 'src/app/models/product.model';
import { Router } from '@angular/router';

@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.css']
})

export class ProductDetailsComponent implements OnInit {
  product: Product = new Product();

  constructor(private route: ActivatedRoute, private productService: ProductService, private router: Router) { }

  ngOnInit() {
    const idParm = this.route.snapshot.paramMap.get('id');
    if (idParm) {
      const id = +idParm;
      this.productService.getProductById(id).subscribe(product => { this.product = product });
    }
    else {
      this.router.navigate(['/']);
    }
  }
}
