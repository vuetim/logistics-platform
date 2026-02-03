import { Component, EventEmitter, Output } from '@angular/core';
import { CreateCustomerWizardComponent } from '../create-customer-wizard/create-customer-wizard.component';

@Component({
  selector: 'app-create-customer-modal',
  standalone: true,
  imports: [CreateCustomerWizardComponent],
  templateUrl: './create-customer-modal.component.html',
  styleUrl: './create-customer-modal.component.css'
})
export class CreateCustomerModalComponent {
  @Output() close = new EventEmitter<boolean>()
  step = 1


}
