import { NgModule, inject } from '@angular/core';
import { Router, RouterModule, Routes } from '@angular/router';

import { AboutUsComponent } from './components/about-us/about-us.component';
import { AdminPanelComponent } from './components/admin-panel/admin-panel.component';
import { CartComponent } from './components/cart/cart.component';
import { CheckoutComponent } from './components/checkout/checkout.component';
import { CustomerPanelComponent } from './components/customer-panel/customer-panel.component';
import { HomeComponent } from './components/home/home.component';
import { LoginComponent } from './components/login/login.component';
import { OrderComponent } from './components/order/order.component';
import { OrderConfirmationComponent } from './components/order-confirmation/order-confirmation.component';
import { OrderDetailsComponent } from './components/order-details/order-details.component';
import { PrivacyPolicyComponent } from './components/privacy-policy/privacy-policy.component';
import { ProductAddComponent } from './components/product-add/product-add.component';
import { ProductDetailsComponent } from './components/product-details/product-details.component';
import { ProductListComponent } from './components/product-list/product-list.component';
import { ProductUpdateComponent } from './components/product-update/product-update.component';
import { RegisterComponent } from './components/register/register.component';
import { TermsAndConditionsComponent } from './components/terms-and-conditions/terms-and-conditions.component';
import { WaitingForActivationComponent } from './components/waiting-for-activation/waiting-for-activation.component';
import { WarehouseComponent } from './components/warehouse/warehouse.component';


import { adminGuard } from './guards/admin.guard';
import { authGuard } from './guards/auth.guard';

const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'aboutus', component: AboutUsComponent },
  { path: 'addproduct', component: ProductAddComponent, canActivate: [adminGuard] },
  { path: 'adminPanel', component: AdminPanelComponent, canActivate: [adminGuard] },
  { path: 'cart', component: CartComponent },
  { path: 'checkout', component: CheckoutComponent },
  { path: 'customerPanel', component: CustomerPanelComponent, canActivate: [authGuard] },
  { path: 'home', component: HomeComponent },
  { path: 'login', component: LoginComponent },
  { path: 'order', component: OrderComponent, canActivate: [authGuard]},
  { path: 'orderConfirmation', component: OrderConfirmationComponent, canActivate: [authGuard] },
  { path: 'order-details/:orderId', component: OrderDetailsComponent, canActivate: [authGuard] },
  { path: 'privacyPolicy', component: PrivacyPolicyComponent },
  { path: 'products', component: ProductListComponent },
  { path: 'product/:id', component: ProductDetailsComponent },
  { path: 'update/:id', component: ProductUpdateComponent, canActivate: [adminGuard] },
  { path: 'register', component: RegisterComponent },
  { path: 'termsAndConditions', component: TermsAndConditionsComponent },
  { path: 'waitingForActivation', component: WaitingForActivationComponent },
  { path: 'warehouse', component: WarehouseComponent, canActivate: [adminGuard] }
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { scrollPositionRestoration: 'enabled' })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
