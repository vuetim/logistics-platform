import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateAddressModalComponent } from './create-address-modal.component';

describe('CreateAddressModalComponent', () => {
  let component: CreateAddressModalComponent;
  let fixture: ComponentFixture<CreateAddressModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CreateAddressModalComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(CreateAddressModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
