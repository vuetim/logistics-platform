import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateCustomerWizardComponent } from './create-customer-wizard.component';

describe('CreateCustomerWizardComponent', () => {
  let component: CreateCustomerWizardComponent;
  let fixture: ComponentFixture<CreateCustomerWizardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CreateCustomerWizardComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(CreateCustomerWizardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
