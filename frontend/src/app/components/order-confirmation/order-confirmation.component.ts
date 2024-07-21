import { ActivatedRoute } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';
import { Component, OnInit } from '@angular/core';
import { OrderService } from 'src/app/services/order.service';
import gsap from 'gsap';

@Component({
  selector: 'app-order-confirmation',
  templateUrl: './order-confirmation.component.html',
  styleUrls: ['./order-confirmation.component.css']
})
export class OrderConfirmationComponent implements OnInit {

  constructor(
    private route: ActivatedRoute,
    public authService: AuthService,
    private orderService: OrderService) { }

  ngOnInit(): void {

    gsap.from('.order-confirm', {
      duration: 1,
      x: '-100%',
      opacity: 0,
      scale: 0.5,
      delay: 0.5,
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
