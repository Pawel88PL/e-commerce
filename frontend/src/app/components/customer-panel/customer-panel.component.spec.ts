import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CustomerPanelComponent } from './customer-panel.component';

describe('CustomerPanelComponent', () => {
  let component: CustomerPanelComponent;
  let fixture: ComponentFixture<CustomerPanelComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CustomerPanelComponent]
    });
    fixture = TestBed.createComponent(CustomerPanelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
