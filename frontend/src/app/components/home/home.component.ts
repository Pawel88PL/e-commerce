import { Component } from '@angular/core';
import { gsap } from 'gsap';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})

export class HomeComponent {
  ngOnInit() {
    gsap.from('.about-us', {
      duration: 1,
      x: '-100%',
      opacity: 0,
      scale: 0.5,
      delay: 0.5,
      ease: "power1.out" 
    });
  }
}