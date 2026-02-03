import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StepContactsComponent } from './step-contacts.component';

describe('StepContactsComponent', () => {
  let component: StepContactsComponent;
  let fixture: ComponentFixture<StepContactsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [StepContactsComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(StepContactsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
