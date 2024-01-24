import { Component, HostListener, OnInit } from '@angular/core';
import { ProductService } from 'src/app/services/product.service';
import { Product } from 'src/app/models/product.model';
import { PaginatedResult } from 'src/app/models/paginated-result.model';

@Component({
  selector: 'app-product-list',
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.css']
})

export class ProductListComponent implements OnInit {
  currentPage = 1;
  isLoading = false;
  itemsPerPage = 10;
  products: Product[] = [];
  totalPages = 1;
  totalProducts = 0;
  selectedCategory: number | null = null;
  selectedSorting: string | null = null;
  scrollDebounceTimer: any = null;

  constructor(private productService: ProductService) { }

  ngOnInit(): void {
    this.loadProducts();
  }

  loadProducts(): void {
    if (this.isLoading || this.currentPage > this.totalPages) {
      return;
    }

    this.isLoading = true;
    this.productService.getProductsByPage(this.currentPage, this.itemsPerPage).subscribe(
      (data: PaginatedResult<Product[]>) => {
        const newProducts = data.items.filter(np => !this.products.some(p => p.productId === np.productId));
        this.products = [...this.products, ...newProducts];
        this.totalProducts = data.totalItems;
        this.totalPages = data.totalPages;
        this.isLoading = false;
        if (this.currentPage < this.totalPages) {
          this.currentPage++;
        }
      },
      error => {
        console.error("Błąd podczas pobierania produktów:", error);
        this.isLoading = false;
      }
    );
  }

  @HostListener('window:scroll', ['$event'])
  onScroll(): void {
    if (this.scrollDebounceTimer) clearTimeout(this.scrollDebounceTimer);

    this.scrollDebounceTimer = setTimeout(() => {
      const atBottomOfPage = (window.innerHeight + window.scrollY) >= document.body.offsetHeight - 200;
      if (atBottomOfPage && !this.isLoading && this.currentPage <= this.totalPages) {
        this.loadProducts();
        this.isLoading = false;
      }
    }, 100);
  }

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