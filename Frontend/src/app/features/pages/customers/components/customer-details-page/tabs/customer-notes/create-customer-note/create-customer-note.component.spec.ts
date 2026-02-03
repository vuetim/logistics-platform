import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateCustomerNoteComponent } from './create-customer-note.component';

describe('CreateCustomerNoteComponent', () => {
  let component: CreateCustomerNoteComponent;
  let fixture: ComponentFixture<CreateCustomerNoteComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CreateCustomerNoteComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(CreateCustomerNoteComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
