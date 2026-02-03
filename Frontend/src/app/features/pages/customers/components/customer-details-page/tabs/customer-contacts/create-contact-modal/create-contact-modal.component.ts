import { Component, EventEmitter, Input, Output } from '@angular/core';

import { FormsModule } from '@angular/forms';
import { CustomerContactDto } from '../../../../../../../../core/models/customers/contacts/customer-contact.dto';
import { UpdateCustomerContactDto } from '../../../../../../../../core/models/customers/contacts/update-customer-contact.dto';
import { CustomerContactsService } from '../../../../../../../../data-access/customers/contacts/contact.service';
import { CreateCustomerContactDto } from '../../../../../../../../core/models/customers/contacts/create-customer-contact.dto';

@Component({
  selector: 'app-create-contact-modal',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './create-contact-modal.component.html',
  styleUrls: ['./create-contact-modal.component.css', '../../customer-addresses/create-address-modal/create-address-modal.component.css']
})
export class CreateContactModalComponent {

  @Input({ required: true }) customerId!: string;
  @Input() editing?: CustomerContactDto;

  @Output() close = new EventEmitter<boolean>();

  loading = false;

  model: CreateCustomerContactDto = {
    customerId: '',
    fullName: '',
    email: '',
    phone: '',
    position: '',
    isPrimary: false,
    isActive: true






  };

  constructor(private service: CustomerContactsService) { }

  ngOnInit() {
    this.model.customerId = this.customerId;

    if (this.editing) {
      this.model = {
        customerId: this.customerId,
        fullName: this.editing.fullName ?? '',
        email: this.editing.email ?? '',
        phone: this.editing.phone ?? '',
        position: this.editing.position ?? '',
        isPrimary: this.editing.isPrimary ?? false,
        isActive: this.editing.isActive ?? true

      };
    }
  }

  save() {
    if (!this.model.fullName || !this.model.email)
      return;

    this.loading = true;

    if (this.editing) {
      const dto: UpdateCustomerContactDto = { ...this.model };
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
