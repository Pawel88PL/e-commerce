import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CartItemDialogComponent } from './cart-item-dialog.component';

describe('CartItemDialogComponent', () => {
  let component: CartItemDialogComponent;
  let fixture: ComponentFixture<CartItemDialogComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CartItemDialogComponent]
    });
    fixture = TestBed.createComponent(CartItemDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
