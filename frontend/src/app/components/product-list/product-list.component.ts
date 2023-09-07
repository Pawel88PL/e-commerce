import { Component, OnInit } from '@angular/core';
import { ProductService } from 'src/app/services/product.service';
import { Product } from 'src/app/models/product.model';

@Component({
  selector: 'app-product-list',
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.css']
})

export class ProductListComponent implements OnInit {
  products: Product[] = [];

  constructor(private productService: ProductService) { }

  ngOnInit(): void {
    this.productService.getProducts().subscribe(
      data => {
        this.products = data;
      },
      error => {
        console.error("Błąd podczas pobierania produktów:", error);
      });
  }

  selectedCategory: number | null = null;
  selectedSorting: string | null = null;

  get filteredProducts(): Product[] {
    let filtered = !this.selectedCategory ? this.products : this.products.filter(p => p.categoryId === this.selectedCategory);

    filtered = filtered.sort((a, b) => {
      switch (this.selectedSorting) {
        case 'category': return (a.categoryId || 0) - (b.categoryId || 0);
        case 'name_asc': return a.name?.localeCompare(b.name || '') || 0;
        case 'price_asc': return a.price - b.price;
        case 'price_desc': return b.price - a.price;
        case 'available-desc': return b.amountAvailable - a.amountAvailable;
        default: return 0;
      }
    });
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