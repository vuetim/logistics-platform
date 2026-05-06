import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UiTextareaComponent } from './ui-textarea.component';

describe('UiTextareaComponent', () => {
  let component: UiTextareaComponent;
  let fixture: ComponentFixture<UiTextareaComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UiTextareaComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(UiTextareaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
