import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CreateCustomerWizardState } from '../create-customer-wizard.state';
import { FormsModule, NgModel } from '@angular/forms';
import { CommonModule, NgFor, NgForOf, NgIf } from '@angular/common';
import { enumToOptions } from '../../../../../../../core/utils/enum-options';
import { CustomerPaymentMethod } from '../../../../../../../core/enums/customers/customer-payment-method.enum';
import { CustomerPaymentTerms } from '../../../../../../../core/enums/customers/customer-payment-terms.enum';
import { UiInputComponent } from '../../../../../../../shared/UI/ui-input/ui-input.component';
import { UiSelectComponent } from '../../../../../../../shared/UI/ui-select/ui-select.component';
import { UiCheckboxComponent } from '../../../../../../../shared/UI/ui-checkbox/ui-checkbox.component';
import { UiButtonComponent } from '../../../../../../../shared/UI/ui-button/ui-button.component';

@Component({
  selector: 'app-step-customer',
  standalone: true,
  imports: [FormsModule, CommonModule, UiInputComponent, UiSelectComponent, UiCheckboxComponent, UiButtonComponent],
  templateUrl: './step-customer.component.html',
  styleUrls: ['../wizard.styles.css', './step-customer.component.css']
})
export class StepCustomerComponent {
  @Input({ required: true })
  data!: CreateCustomerWizardState['customer']
  @Output()
  next = new EventEmitter<void>()
  paymentMethods = enumToOptions(CustomerPaymentMethod);
  paymentTerms = enumToOptions(CustomerPaymentTerms);


  isValid(): boolean {
    return !!(
      this.data.name &&
      this.data.name.trim().length >= 2 &&
      this.data.billing.terms !== undefined &&
      this.data.billing.method !== undefined
    );
  }

  submit() {
    if (!this.isValid()) return;
    this.next.emit();
  }
}
