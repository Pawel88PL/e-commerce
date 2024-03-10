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

import { AboutUsComponent } from './components/about-us/about-us.component';
import { AppComponent } from './app.component';
import { CartComponent } from './components/cart/cart.component';
import { CartItemDialogComponent } from './components/cart-item-dialog/cart-item-dialog.component';
import { CookieConsentComponent } from './components/cookie-consent/cookie-consent.component';
import { CheckoutComponent } from './components/checkout/checkout.component';
import { ConfirmDialogComponent } from './components/confirm-dialog/confirm-dialog.component';
import { CustomerPanelComponent } from './components/customer-panel/customer-panel.component';
import { FooterComponent } from './components/footer/footer.component';
import { HomeComponent } from './components/home/home.component';
import { LoginComponent } from './components/login/login.component';
import { NavbarComponent } from './components/navbar/navbar.component';
import { OrderComponent } from './components/order/order.component';
import { OrderConfirmationComponent } from './components/order-confirmation/order-confirmation.component';
import { ProductAddComponent } from './components/product-add/product-add.component';
import { ProductCardComponent } from './components/product-card/product-card.component';
import { ProductDetailsComponent } from './components/product-details/product-details.component';
import { ProductListComponent } from './components/product-list/product-list.component';
import { ProductUpdateComponent } from './components/product-update/product-update.component';
import { RegisterComponent } from './components/register/register.component';
import { WarehouseComponent } from './components/warehouse/warehouse.component';
import { WaitingForActivationComponent } from './components/waiting-for-activation/waiting-for-activation.component';

import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatMenuModule } from '@angular/material/menu';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSelectModule } from '@angular/material/select';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatToolbarModule } from '@angular/material/toolbar';
import { OrderProcessingDialogComponent } from './components/order-processing-dialog/order-processing-dialog.component';
import { OrderDetailsComponent } from './components/order-details/order-details.component';
import { AdminPanelComponent } from './components/admin-panel/admin-panel.component';
import { TermsAndConditionsComponent } from './components/terms-and-conditions/terms-and-conditions.component';
import { PrivacyPolicyComponent } from './components/privacy-policy/privacy-policy.component'

registerLocaleData(localePl);

@NgModule({
  declarations: [
    AboutUsComponent,
    AppComponent,
    CartComponent,
    CartItemDialogComponent,
    CheckoutComponent,
    ConfirmDialogComponent,
    CookieConsentComponent,
    CustomerPanelComponent,
    FooterComponent,
    HomeComponent,
    LoginComponent,
    NavbarComponent,
    OrderComponent,
    OrderConfirmationComponent,
    ProductAddComponent,
    ProductListComponent,
    ProductCardComponent,
    ProductDetailsComponent,
    ProductUpdateComponent,
    RegisterComponent,
    WarehouseComponent,
    WaitingForActivationComponent,
    OrderProcessingDialogComponent,
    OrderDetailsComponent,
    AdminPanelComponent,
    TermsAndConditionsComponent,
    PrivacyPolicyComponent,
  ],
  imports: [
    AppRoutingModule,
    BrowserAnimationsModule,
    BrowserModule,
    FormsModule,
    HttpClientModule,
    MatButtonModule,
    MatCardModule,
    MatCheckboxModule,
    MatDialogModule,
    MatFormFieldModule,
    MatIconModule,
    MatInputModule,
    MatProgressSpinnerModule,
    MatMenuModule,
    MatToolbarModule,
    MatSelectModule,
    MatSidenavModule,
    MatSnackBarModule,
    ReactiveFormsModule,
  ],
  providers: [
    { provide: LOCALE_ID, useValue: 'pl' }
  ],
  bootstrap: [AppComponent]
})

export class AppModule { }
