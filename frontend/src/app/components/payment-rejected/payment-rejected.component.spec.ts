import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PaymentRejectedComponent } from './payment-rejected.component';

describe('PaymentRejectedComponent', () => {
  let component: PaymentRejectedComponent;
  let fixture: ComponentFixture<PaymentRejectedComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PaymentRejectedComponent]
    });
    fixture = TestBed.createComponent(PaymentRejectedComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
