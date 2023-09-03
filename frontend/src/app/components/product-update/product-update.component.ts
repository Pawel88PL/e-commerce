import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ProductService } from 'src/app/services/product.service';
import { Product } from 'src/app/models/product.model';

@Component({
  selector: 'app-product-update',
  templateUrl: './product-update.component.html',
  styleUrls: ['./product-update.component.css']
})

export class ProductUpdateComponent implements OnInit {
  product: Product = new Product();
  productForm!: FormGroup;

  constructor(
    private fb: FormBuilder,
    private productService: ProductService,
    private route: ActivatedRoute,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.getproduct();
  }

  getproduct(): void {
    const idParm: string | null = this.route.snapshot.paramMap.get('id');
    if (idParm) {
      const id = +idParm;
      this.productService.getProductById(id).subscribe(
        product => {
          this.product = product;
          this.initializeForm();
        }, error => console.log('Błąd pobierania danych produktu:', error)
      );
    }
    else {
      this.router.navigate(['/']);
    }
  }

  initializeForm(): void {
    this.productForm = this.fb.group({
      categoryId: [this.product.categoryId, Validators.required],
      name: [this.product.name, Validators.required],
      price: [this.product.price, [Validators.required, Validators.pattern("^[0-9]*$"), Validators.min(0)]],
      weight: [this.product.weight, [Validators.required, Validators.pattern("^[0-9]*$"), Validators.min(0)]],
      amountAvailable: [this.product.amountAvailable, [Validators.required, Validators.pattern("^[0-9]*$"), Validators.min(0)]],
      priority: [this.product.priority, [Validators.required, Validators.pattern("^[0-9]*$"), Validators.min(0)]],
      description: [this.product.description, Validators.required],
    })
  }

  onSubmit(): void {
    if (this.productForm.valid) {
      this.productService.updateProduct(this.product.productId, this.productForm.value).subscribe(
        (product) => {
          console.log('Pomyślnie zmieniono dane produktu!', product);
          this.router.navigate(['/product', product.productId]);
        },
        (error) => {
          console.error('Wystąpił błąd podczas aktualizacji produktu', error)
        }
      )
    }
  }
}
