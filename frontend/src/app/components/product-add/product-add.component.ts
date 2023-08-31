import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ProductService } from 'src/app/services/product.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-product-add',
  templateUrl: './product-add.component.html',
  styleUrls: ['./product-add.component.css']
})
export class ProductAddComponent implements OnInit {
  productForm!: FormGroup;

  constructor(
    private fb: FormBuilder,
    private productService: ProductService,
    private router: Router
  ) {}

  ngOnInit(): void {
      this.initializeForm();
  }

  initializeForm(): void {
    this.productForm = this.fb.group({
      categoryId: ['', Validators.required],
      name: ['', Validators.required],
      price: ['', [Validators.required, Validators.pattern("^[0-9]*$"), Validators.min(0)]],
      weight: ['', [Validators.required, Validators.pattern("^[0-9]*$"), Validators.min(0)]],
      amountAvailable: ['', [Validators.required, Validators.pattern("^[0-9]*$"), Validators.min(0)]],
      priority: ['', [Validators.required, Validators.pattern("^[0-9]*$"), Validators.min(0)]],
      description: [''],
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