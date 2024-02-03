import { Component, OnInit } from '@angular/core';
import { gsap } from 'gsap';

@Component({
  selector: 'app-waiting-for-activation',
  templateUrl: './waiting-for-activation.component.html',
  styleUrls: ['./waiting-for-activation.component.css']
})
export class WaitingForActivationComponent implements OnInit {

  ngOnInit(): void {
    gsap.from('.container', {
      duration: 1,
      x: '100%',
      opacity: 0,
      scale: 0.5,
      delay: 0.5,
      ease: "power1.out"
    });
  }
}