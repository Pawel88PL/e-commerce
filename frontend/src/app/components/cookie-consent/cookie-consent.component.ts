import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-cookie-consent',
  templateUrl: './cookie-consent.component.html',
  styleUrls: ['./cookie-consent.component.css']
})
export class CookieConsentComponent implements OnInit {
  showBanner: boolean = false;

  ngOnInit() {
    this.showBanner = !localStorage.getItem('cookieConsent');
  }

  accept() {
    localStorage.setItem('cookieConsent', 'true');
    this.showBanner = false;
  }

  decline() {
    this.showBanner = false;
  }

}