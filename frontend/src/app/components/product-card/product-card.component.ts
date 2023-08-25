import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-product-card',
  templateUrl: './product-card.component.html',
  styleUrls: ['./product-card.component.css']
})
export class ProductCardComponent {
  
  @Input() product: any;

  constructor() {}

  addToCart() {
    console.log('Produkt ${ this.product.name } dodany do koszyka')
  }
}
