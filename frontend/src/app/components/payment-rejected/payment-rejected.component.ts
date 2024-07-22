import { Component, OnInit } from '@angular/core';
import gsap from 'gsap';

@Component({
  selector: 'app-payment-rejected',
  templateUrl: './payment-rejected.component.html',
  styleUrls: ['./payment-rejected.component.css']
})
export class PaymentRejectedComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
    gsap.from('.error', {
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
