import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OrderProcessingDialogComponent } from './order-processing-dialog.component';

describe('OrderProcessingDialogComponent', () => {
  let component: OrderProcessingDialogComponent;
  let fixture: ComponentFixture<OrderProcessingDialogComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [OrderProcessingDialogComponent]
    });
    fixture = TestBed.createComponent(OrderProcessingDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
