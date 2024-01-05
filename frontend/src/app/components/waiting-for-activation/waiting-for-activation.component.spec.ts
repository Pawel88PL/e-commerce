import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WaitingForActivationComponent } from './waiting-for-activation.component';

describe('WaitingForActivationComponent', () => {
  let component: WaitingForActivationComponent;
  let fixture: ComponentFixture<WaitingForActivationComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [WaitingForActivationComponent]
    });
    fixture = TestBed.createComponent(WaitingForActivationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
