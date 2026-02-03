import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CreateCustomerAddressDto } from '../../../../../../../core/models/customers/addresses/create-customer-address.dto';
import { FormsModule } from '@angular/forms';
import { NgFor, NgIf } from '@angular/common';
import { CUSTOMER_ADDRESS_TYPES } from '../../../../../../../core/models/customers/addresses/customer-address-type';

@Component({
  selector: 'app-step-addresses',
  standalone: true,
  imports: [FormsModule, NgFor, NgIf],
  templateUrl: './step-addresses.component.html',
  styleUrls: ['../wizard.styles.css', './step-addresses.component.css']

})
export class StepAddressesComponent {

  @Input({ required: true })
  addresses!: CreateCustomerAddressDto[];

  @Output() next = new EventEmitter<void>();
  @Output() back = new EventEmitter<void>();

  addressTypes = Object.values(CUSTOMER_ADDRESS_TYPES);

  draft: CreateCustomerAddressDto = this.empty();

  empty(): CreateCustomerAddressDto {
    return {
      addressLine1: '',
      addressLine2: '',
      city: '',
      state: '',
      country: '',
      postalCode: '',
      type: CUSTOMER_ADDRESS_TYPES.Billing,
      isPrimary: false,
      isActive: true
    };
  }

  isValid(): boolean {
    return !!(
      this.draft.addressLine1 &&
      this.draft.addressLine2 &&
      this.draft.city &&
      this.draft.state &&
      this.draft.country &&
      this.draft.postalCode &&
      this.draft.type
    );
  }

  add() {
    if (!this.isValid()) return;

    const address = { ...this.draft };

    if (!this.addresses.length) {
      address.isPrimary = true;
    }

    this.addresses.push(address);
    this.draft = this.empty();
  }

  remove(i: number) {
    this.addresses.splice(i, 1);

    if (!this.addresses.some(a => a.isPrimary) && this.addresses.length) {
      this.addresses[0].isPrimary = true;
    }
  }
}