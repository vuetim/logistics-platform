import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StepAddressesComponent } from './step-addresses.component';

describe('StepAddressesComponent', () => {
  let component: StepAddressesComponent;
  let fixture: ComponentFixture<StepAddressesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [StepAddressesComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(StepAddressesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
