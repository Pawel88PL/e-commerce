import { Component, OnInit } from '@angular/core';
import { Product } from 'src/app/models/product.model';
import { ProductService } from 'src/app/services/product.service';

@Component({
  selector: 'app-warehouse',
  templateUrl: './warehouse.component.html',
  styleUrls: ['./warehouse.component.css']
})

export class WarehouseComponent implements OnInit {
  products: Product[] = [];  // Zakładam, że masz model Product gdzieś zdefiniowany

  constructor( private productService: ProductService) { }

  ngOnInit(): void {
    this.productService.getProducts().subscribe(
      data => {
        this.products = data;
        console.log(data);
      },
      error => {
        console.error("Błąd podczas pobierania produktów:", error);
      });  }

  confirmDeleteAlert(productId: number): void {
    if (window.confirm('Czy na pewno chcesz usunąć ten produkt?')) {
      // Usuwanie produktu, np. poprzez serwis HTTP
    }
  }
}
