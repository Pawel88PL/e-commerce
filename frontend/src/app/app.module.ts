import { AdminGuard } from './guards/admin.guard';
import { registerLocaleData } from '@angular/common';
import localePl from '@angular/common/locales/pl'
import { LOCALE_ID } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { ReactiveFormsModule } from '@angular/forms';
import { SlickCarouselModule } from 'ngx-slick-carousel';

import { AppComponent } from './app.component';
import { CartComponent } from './components/cart/cart.component';
import { CartItemDialogComponent } from './components/cart-item-dialog/cart-item-dialog.component';
import { CookieConsentComponent } from './components/cookie-consent/cookie-consent.component';
import { FooterComponent } from './components/footer/footer.component';
import { GenericDialogComponent } from './components/generic-dialog/generic-dialog.component';
import { HomeComponent } from './components/home/home.component';
import { LoginComponent } from './components/login/login.component';
import { NavbarComponent } from './components/navbar/navbar.component';
import { ProductAddComponent } from './components/product-add/product-add.component';
import { ProductCardComponent } from './components/product-card/product-card.component';
import { ProductDetailsComponent } from './components/product-details/product-details.component';
import { ProductListComponent } from './components/product-list/product-list.component';
import { ProductUpdateComponent } from './components/product-update/product-update.component';
import { RegisterComponent } from './components/register/register.component';
import { WarehouseComponent } from './components/warehouse/warehouse.component';

import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatMenuModule } from '@angular/material/menu';
import { MatToolbarModule } from '@angular/material/toolbar'
import { MatSelectModule } from '@angular/material/select';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { CheckoutComponent } from './components/checkout/checkout.component';
import { OrderComponent } from './components/order/order.component';
import { WaitingForActivationComponent } from './components/waiting-for-activation/waiting-for-activation.component';

registerLocaleData(localePl);

@NgModule({
  declarations: [
    AppComponent,
    CartComponent,
    CartItemDialogComponent,
    CookieConsentComponent,
    FooterComponent,
    GenericDialogComponent,
    HomeComponent,
    LoginComponent,
    NavbarComponent,
    ProductAddComponent,
    ProductListComponent,
    ProductCardComponent,
    ProductDetailsComponent,
    ProductUpdateComponent,
    RegisterComponent,
    WarehouseComponent,
    CheckoutComponent,
    OrderComponent,
    WaitingForActivationComponent,
  ],
  imports: [
    AppRoutingModule,
    BrowserAnimationsModule,
    BrowserModule,
    FormsModule,
    HttpClientModule,
    MatButtonModule,
    MatDialogModule,
    MatFormFieldModule,
    MatIconModule,
    MatInputModule,
    MatMenuModule,
    MatToolbarModule,
    MatSelectModule,
    MatSidenavModule,
    MatSnackBarModule,
    ReactiveFormsModule,
    SlickCarouselModule
  ],
  providers: [
    AdminGuard,
    { provide: LOCALE_ID, useValue: 'pl' }
  ],
  bootstrap: [AppComponent]
})

export class AppModule { }
