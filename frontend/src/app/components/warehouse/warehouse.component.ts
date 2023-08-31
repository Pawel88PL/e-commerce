import { Component, OnInit } from '@angular/core';
import { Product } from 'src/app/models/product.model';
import { ProductService } from 'src/app/services/product.service';

@Component({
  selector: 'app-warehouse',
  templateUrl: './warehouse.component.html',
  styleUrls: ['./warehouse.component.css']
})

export class WarehouseComponent implements OnInit {
  products: Product[] = [];

  constructor(private productService: ProductService) { }

  ngOnInit(): void {
    this.loadProducts();
  }

  loadProducts(): void {
    this.productService.getProducts().subscribe(
      data => {
        this.products = data;
      },
      error => {
        console.error('Wystąpił błąd podczas pobierania produktów:', error);
      }
    );
  }

  confirmDeleteAlert(productId: number): void {
    if (window.confirm('Czy na pewno chcesz usunąć ten produkt?')) {
      this.productService.deleteProduct(productId).subscribe(
        () => {
          this.products = this.products.filter(product => product.productId !== productId);
          alert('Produkt został usunięty z bazy danych!');
        },
        error => {
          console.error('Wystąpił błąd podczas usuwania produktu:', error);
          alert('Wystąpił błąd podczas usuwania produktu:. Spróbuj ponownie.');
        }
      );
    }
  }
}
