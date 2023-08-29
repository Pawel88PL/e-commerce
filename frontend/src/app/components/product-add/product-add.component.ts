import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ProductService } from 'src/app/services/product.service';

@Component({
  selector: 'app-product-add',
  templateUrl: './product-add.component.html',
  styleUrls: ['./product-add.component.css']
})
export class ProductAddComponent implements OnInit {
  productForm!: FormGroup;

  constructor(
    private formBuilder: FormBuilder
  ) {}

  ngOnInit(): void {
      this.initializeForm();
  }

  initializeForm(): void {
    this.productForm = this.formBuilder.group({
      category: ['', Validators.required],
      name: ['', Validators.required],
      price: ['', [Validators.required, Validators.pattern("^[0-9]*$"), Validators.min(0)]],
      weight: ['', [Validators.required, Validators.pattern("^[0-9]*$"), Validators.min(0)]],
      amountAvailable: ['', [Validators.required, Validators.pattern("^[0-9]*$"), Validators.min(0)]],
      displayOrder: ['', [Validators.required, Validators.pattern("^[0-9]*$"), Validators.min(0)]],
      description: [''],
      images: [''] // Będziesz musiał obsłużyć przesyłanie plików w bardziej skomplikowany sposób
    })
  }

  onSubmit(): void {
    if (this.productForm.valid) {
      const newProduct = this.productForm.value;
      // Tu możesz użyć serwisu productService, aby wysłać nowy produkt do backendu:
      // this.productService.addProduct(newProduct).subscribe(response => {
      //   // Obsłuż odpowiedź z backendu, np. wyświetl powiadomienie o sukcesie
      // }, error => {
      //   // Obsłuż błąd
      // });
    }
  }  
}
