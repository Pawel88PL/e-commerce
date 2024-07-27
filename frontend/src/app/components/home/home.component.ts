import { Component, OnInit } from '@angular/core';
import { gsap } from 'gsap';
import { OrderService } from 'src/app/services/order.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})

export class HomeComponent implements OnInit {

  constructor(private orderService: OrderService) { }

  ngOnInit() {
    //this.testRedirect();

    gsap.from('.about-us', {
      duration: 1,
      x: '-100%',
      opacity: 0,
      scale: 0.5,
      delay: 0.5,
      ease: "power1.out"
    });
  }

  testRedirect() {
    this.orderService.testRedirect().subscribe({
      next: (response) => {
        // Logowanie odpowiedzi
        console.log('RedirectUrl:', response.redirectUrl);

        // Przekierowanie na URL płatności
        if (response.redirectUrl) {
          window.location.href = response.redirectUrl;
        } else {
          console.error('RedirectUrl is undefined');
        }
      },
      error: (error) => {
        console.error(error);
      }
    });
  }
}