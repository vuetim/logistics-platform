import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CreateCustomerWizardState } from '../create-customer-wizard.state';
import { FormsModule, NgModel } from '@angular/forms';
import { CommonModule, NgFor, NgForOf, NgIf } from '@angular/common';

@Component({
  selector: 'app-step-customer',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './step-customer.component.html',
  styleUrls: ['../wizard.styles.css', './step-customer.component.css']
})
export class StepCustomerComponent {
  @Input({ required: true })
  data!: CreateCustomerWizardState['customer']
  @Output()
  next = new EventEmitter<void>()



  isValid(): boolean {
    return !!(
      this.data.name &&
      this.data.name.trim().length >= 2 &&
      this.data.paymentTermsDays > 0
    );
  }

  submit() {
    if (!this.isValid()) return;
    this.next.emit();
  }
}
