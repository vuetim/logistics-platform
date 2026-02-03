import { Component, Input, Output, EventEmitter } from "@angular/core";
import { CreateCustomerAddressDto } from "../../../../../../../../core/models/customers/addresses/create-customer-address.dto";
import { CustomerAddressDto } from "../../../../../../../../core/models/customers/addresses/customer-address.dto";
import { UpdateCustomerAddressDto } from "../../../../../../../../core/models/customers/addresses/update-address.dto";
import { CustomerAddressesService } from "../../../../../../../../data-access/customers/addresses/address.service";
import { FormsModule } from "@angular/forms";

@Component({
  selector: 'app-create-address-modal',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './create-address-modal.component.html',
  styleUrl: './create-address-modal.component.css'
})
export class CreateAddressModalComponent {

  @Input({ required: true }) customerId!: string;
  @Input() editing?: CustomerAddressDto;

  @Output() close = new EventEmitter<boolean>();

  loading = false;

  model: CreateCustomerAddressDto = {
    customerId: '',
    addressLine1: '',
    addressLine2: '',
    city: '',
    state: '',
    country: '',
    postalCode: '',
    type: 'Billing',
    isPrimary: false,
    isActive: true
  };

  constructor(private service: CustomerAddressesService) { }

  ngOnInit() {
    this.model.customerId = this.customerId;

    if (this.editing) {
      this.model = {
        customerId: this.customerId,
        addressLine1: this.editing.addressLine1,
        addressLine2: this.editing.addressLine2,
        city: this.editing.city,
        state: this.editing.state,
        country: this.editing.country,
        postalCode: this.editing.postalCode,
        type: this.editing.type,
        isPrimary: this.editing.isPrimary,
        isActive: this.editing.isActive
      };
    }
  }

  save() {
    if (!this.model.addressLine1 || !this.model.city || !this.model.country)
      return;

    this.loading = true;

    if (this.editing) {
      const dto: UpdateCustomerAddressDto = { ...this.model };
      this.service.update(this.editing.id, dto).subscribe({
        next: () => this.close.emit(true),
        error: () => this.loading = false
      });
    } else {
      this.service.create(this.model).subscribe({
        next: () => this.close.emit(true),
        error: () => this.loading = false
      });
    }
  }

  cancel() {
    this.close.emit(false);
  }
}
