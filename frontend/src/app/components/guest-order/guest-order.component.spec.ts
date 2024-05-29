import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GuestOrderComponent } from './guest-order.component';

describe('GuestOrderComponent', () => {
  let component: GuestOrderComponent;
  let fixture: ComponentFixture<GuestOrderComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [GuestOrderComponent]
    });
    fixture = TestBed.createComponent(GuestOrderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
