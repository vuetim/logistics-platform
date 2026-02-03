import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CustomerNotesComponent } from './customer-notes.component';

describe('CustomerNotesComponent', () => {
  let component: CustomerNotesComponent;
  let fixture: ComponentFixture<CustomerNotesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CustomerNotesComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(CustomerNotesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
