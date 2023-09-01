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
    this.initializeForm();
  }

  getproduct(): void {
    const idParm: string | null = this.route.snapshot.paramMap.get('id');
    if (idParm) {
      const id = +idParm;
      this.productService.getProductById(id).subscribe(product => { this.product = product }, error => console.log('Error fetching product:', error));
    }
    else {
      this.router.navigate(['/']);
    }
  }

  initializeForm(): void {
    this.productForm = this.fb.group({
      categoryId: ['', Validators.required],
      name: ['', Validators.required],
      price: ['', [Validators.required, Validators.pattern("^[0-9]*$"), Validators.min(0)]],
      weight: ['', [Validators.required, Validators.pattern("^[0-9]*$"), Validators.min(0)]],
      amountAvailable: ['', [Validators.required, Validators.pattern("^[0-9]*$"), Validators.min(0)]],
      priority: ['', [Validators.required, Validators.pattern("^[0-9]*$"), Validators.min(0)]],
      description: ['', Validators.required],
    })
  }

  onSubmit(): void {
    if (this.productForm.valid) {
      this.productService.createProduct(this.productForm.value).subscribe(
        (product) => {
          console.log('Produkt został dodany!', product);
          this.router.navigate(['/product', product.productId]);
        },
        (error) => {
          console.error('Wystąpił błąd podczas dodawania produktu', error)
        }
      )
    }
  }
}
