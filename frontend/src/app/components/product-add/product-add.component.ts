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
  selectedFiles: FileList | null = null;
  imagePreviews: string[] = [];

  constructor(
    private fb: FormBuilder,
    private productService: ProductService,
    private router: Router
  ) { }

  onFilesSelected(event: any): void {
    const files: FileList = event.target.files;
    this.imagePreviews = [];
    for (let i = 0; i < files.length; i++) {
      const file = files[i];
      const reader = new FileReader();

      reader.onload = (e: any) => {
        this.imagePreviews.push(e.target.result);
      };

      reader.readAsDataURL(file);
    }
    const input = event.target as HTMLInputElement;
    if (input.files) {
      this.selectedFiles = input.files;
    }
  }

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
      description: ['', Validators.required],
    })
  }

  onSubmit(): void {
    if (this.productForm.valid) {
      if (this.selectedFiles && this.selectedFiles.length > 0) {
        const formData = new FormData();
        for (let i = 0; i < this.selectedFiles.length; i++) {
          formData.append("images", this.selectedFiles[i], this.selectedFiles[i].name);
        }
        this.productService.uploadProductImages(formData).subscribe(
          (response: any) => {
            console.log('Zdjęcia zostały przesłane pomyślnie!', response);

            const ImagePaths = response.files;
            
            this.productService.createProduct(this.productForm.value, ImagePaths).subscribe(
              (product) => {
                console.log('Produkt został dodany!', product);
                this.router.navigate(['/product', product.productId]);
              },
              (error) => {
                console.error('Wystąpił błąd podczas dodawania produktu', error)
              }
            )
          },
          (error) => {
            console.error('Wystąpił błąd podczas przesyłania zdjęć', error);
          }
        );
      } else {
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
}