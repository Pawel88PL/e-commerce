import { Component, OnInit } from '@angular/core';
import { ProductService } from 'src/app/services/product.service';
import { Product } from 'src/app/models/product.model';

@Component({
  selector: 'app-product-list',
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.css']
})

export class ProductListComponent implements OnInit {
  selectedCategory: number | null = null;
  products: Product[] = [];
  
  constructor(private productService: ProductService) { }
  
  ngOnInit(): void {
    this.productService.getProducts().subscribe(data => {
      this.products = data;
    });
  }
  
  get filteredProducts(): Product[] {
    const filtered = !this.selectedCategory ? this.products : this.products.filter(p => p.categoryId === this.selectedCategory);
    console.log(filtered);
    return filtered;
  }

  getTitle(): string {
    switch (this.selectedCategory) {
      case 1: return 'Miody naturalne:';
      case 2: return 'Miody smakowe:';
      case 3: return 'Zestawy miodów:';
      case 4: return 'Produkty pszczele:';
      default: return 'Nasze produkty:';
    }
  }
}