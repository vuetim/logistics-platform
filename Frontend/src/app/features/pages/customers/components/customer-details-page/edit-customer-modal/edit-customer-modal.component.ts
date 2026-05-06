import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CustomerDetailsDto } from '../../../../../../core/models/customers/customers-details-dto';
import { CustomersService } from '../../../../../../data-access/customers/customers.service';
import { UpdateCustomerDto } from '../../../../../../core/models/customers/update-customer.dto';
import { CustomerPaymentTerms } from '../../../../../../core/enums/customers/customer-payment-terms.enum';
import { CustomerPaymentMethod } from '../../../../../../core/enums/customers/customer-payment-method.enum';
import { enumToOptions } from '../../../../../../core/utils/enum-options';
import { CommonModule } from '@angular/common';




@Component({
  selector: 'app-edit-customer-modal',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './edit-customer-modal.component.html',
  styleUrl: './edit-customer-modal.component.css'
})
export class EditCustomerModalComponent {
  @Input({ required: true }) customer!: CustomerDetailsDto;
  @Output() close = new EventEmitter<boolean>();

  loading = false;
  paymentMethods = enumToOptions(CustomerPaymentMethod);
  paymentTerms = enumToOptions(CustomerPaymentTerms);

  model: UpdateCustomerDto = {
    name: '',
    email: '',
    phone: '',
    billing: {
      terms: CustomerPaymentTerms.Net30,
      method: CustomerPaymentMethod.ACH,
      creditLimit: 0,
      autoInvoice: false
    },
    IsActive: true
  };

  constructor(private service: CustomersService) { }

  ngOnInit() {
    this.model = {
      name: this.customer.name ?? '',
      email: this.customer.email ?? '',
      phone: this.customer.phone ?? '',

      IsActive: this.customer.isActive,
      billing: { ...this.customer.billing }
    };
  }

  save() {
    if (!this.model.name) return;

    this.loading = true;
    this.service.updateCustomer(this.customer.id, this.model).subscribe({
      next: () => this.close.emit(true),
      error: () => (this.loading = false)
    });
  }

  cancel() {
    this.close.emit(false);
  }
}
