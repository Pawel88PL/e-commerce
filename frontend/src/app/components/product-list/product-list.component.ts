import { Component, HostListener, OnInit } from '@angular/core';
import { ProductService } from 'src/app/services/product.service';
import { Product } from 'src/app/models/product.model';
import { PaginatedResult } from 'src/app/models/paginated-result.model';
import { DEFAULT_ITEMS_PER_PAGE } from 'src/app/config/config';
import gsap from 'gsap';

@Component({
  selector: 'app-product-list',
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.css']
})

export class ProductListComponent implements OnInit {
  currentPage = 1;
  isLoading = false;
  itemsPerPage = DEFAULT_ITEMS_PER_PAGE;
  products: Product[] = [];
  totalPages = 1;
  totalProducts = 0;
  selectedCategory: number | null = null;
  selectedSorting: string | null = null;
  scrollDebounceTimer: any = null;

  constructor(private productService: ProductService) { }

  ngOnInit(): void {
    gsap.from('.product-list', {
      duration: 1,
      x: '100%',
      opacity: 0,
      scale: 0.5,
      delay: 0.5,
      ease: "power1.out"
    });

    gsap.from('.logo', {
      duration: 1,
      x: '-100%',
      opacity: 0,
      scale: 0.5,
      delay: 1.5,
      ease: "power1.out"
    });

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
}