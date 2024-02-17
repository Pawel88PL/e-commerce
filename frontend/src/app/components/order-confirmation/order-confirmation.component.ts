import { ActivatedRoute } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { OrderService } from 'src/app/services/order.service';
import { Order } from 'src/app/models/order.model';
import gsap from 'gsap';
import { SHIPPING_COST } from 'src/app/config/config';

@Component({
  selector: 'app-order-confirmation',
  templateUrl: './order-confirmation.component.html',
  styleUrls: ['./order-confirmation.component.css']
})
export class OrderConfirmationComponent implements OnInit {
  orderId: string | null = null;
  orderDetails: Order | null = null;
  shippingCost: number = SHIPPING_COST;
  
  constructor(private route: ActivatedRoute, private orderService: OrderService) { }

  ngOnInit(): void {
    this.orderId = this.route.snapshot.queryParamMap.get('orderId');

    if (this.orderId) {
      this.orderService.getOrderDetails(this.orderId).subscribe({
        next: (details) => {
          this.orderDetails = details;
        },
        error: (error) => {
          console.error('Wystąpił błąd podczas pobierania szczegółów zamówienia.', error);
        }
      });
        
    }

    gsap.from('.order-confirm', {
      duration: 1,
      x: '-100%',
      opacity: 0,
      scale: 0.5,
      delay: 0.5,
      ease: "power1.out"
    });

    gsap.from('.order-details', {
      duration: 1,
      y: '100%',
      opacity: 0,
      scale: 0.5,
      delay: 1,
      ease: "power1.out"
    });

    gsap.from('.logo', {
      duration: 1,
      x: '100%',
      opacity: 0,
      scale: 0.5,
      delay: 1.5,
      ease: "power1.out"
    });
  }
}
