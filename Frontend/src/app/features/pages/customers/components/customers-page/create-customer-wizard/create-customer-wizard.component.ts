import { Component, EventEmitter, Output } from '@angular/core';
import { StepAddressesComponent } from './step-addresses/step-addresses.component';
import { NgIf } from '@angular/common';
import { CustomersService } from '../../../../../../data-access/customers/customers.service';
import { CreateCustomerWizardState } from './create-customer-wizard.state';
import { StepContactsComponent } from './step-contacts/step-contacts.component';
import { StepNotesComponent } from './step-notes/step-notes.component';
import { StepCustomerComponent } from './step-customer/step-customer.component';

@Component({
  selector: 'app-create-customer-wizard',
  standalone: true,
  imports: [StepAddressesComponent, NgIf, StepContactsComponent, StepNotesComponent, StepCustomerComponent],
  templateUrl: './create-customer-wizard.component.html',
  styleUrl: './create-customer-wizard.component.css'
})
export class CreateCustomerWizardComponent {
  @Output() close = new EventEmitter<boolean>()
  step = 1;
  state: CreateCustomerWizardState = {
    customer: {
      name: '',
      email: '',
      phone: '',
      paymentTermsDays: 30,
      isActive: true
    },
    addresses: [],
    contacts: [],
    notes: []
  };

  constructor(private customersService: CustomersService) { }

  submit() {
    console.log('CREATE CUSTOMER PAYLOAD', this.state);

    this.customersService.createFull(this.state).subscribe({
      next: () => this.close.emit(true),
      error: err => {
        console.error('CREATE CUSTOMER ERROR', err);
      }
    });
  }

  cancel() {
    this.close.emit(false);
  }
}
