import { ActivatedRoute, Router } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { environment } from 'src/environments/environment';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ProductService } from 'src/app/services/product.service';
import { Product } from 'src/app/models/product.model';

@Component({
  selector: 'app-product-update',
  templateUrl: './product-update.component.html',
  styleUrls: ['./product-update.component.css']
})

export class ProductUpdateComponent implements OnInit {
  apiBaseUrl: string = environment.apiUrl;
  imagePreviews: string[] = [];
  product: Product = new Product();
  productForm!: FormGroup;
  selectedFiles: FileList | null = null;

  constructor(
    private fb: FormBuilder,
    private productService: ProductService,
    private route: ActivatedRoute,
    private router: Router
  ) { }

  deleteImage(imageId: number): void {
    this.productService.deleteImage(imageId).subscribe(
      () => {
        console.log('Zdjęcie zostało pomyślnie usunięte.');
        this.product.productImages = this.product?.productImages?.filter(image => image.imageId !== imageId);
      },
      error => {
        console.error('Wystąpił błąd podczas usuwania zdjęcia:', error);
      }
    );
  }

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
      if (this.selectedFiles && this.selectedFiles.length > 0) {
        const formData = new FormData();
        for (let i = 0; i < this.selectedFiles.length; i++) {
          formData.append("images", this.selectedFiles[i], this.selectedFiles[i].name);
        }
        this.productService.uploadProductImages(formData).subscribe(
          (response: any) => {
            console.log('Zdjęcia zostały zaktualizowane!', response);

            const ImagePaths = response.files;

            this.productService.updateProduct(this.product.productId, this.productForm.value, ImagePaths).subscribe(
              (product) => {
                console.log('Pomyślnie zmieniono dane produktu!', product);
                this.router.navigate(['/product', product.productId]);
              },
              (error) => {
                console.error('Wystąpił błąd podczas aktualizacji produktu', error)
              }
            )
          },
          (error) => {
            console.error('Wystąpił błąd podczas przesyłania zdjęć', error);
          }
        );
      } else {
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
}