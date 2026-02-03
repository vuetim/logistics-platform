import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CreateCustomerContactDto } from '../../../../../../../core/models/customers/contacts/create-customer-contact.dto';
import { FormsModule } from '@angular/forms';
import { NgFor, NgIf } from '@angular/common';

@Component({
  selector: 'app-step-contacts',
  standalone: true,
  imports: [NgFor, FormsModule, NgIf],
  templateUrl: './step-contacts.component.html',
  styleUrls: ['../wizard.styles.css', './step-contacts.component.css']
})
export class StepContactsComponent {

  @Input({ required: true })
  contacts!: CreateCustomerContactDto[];

  @Output() next = new EventEmitter<void>();
  @Output() back = new EventEmitter<void>();


  draft: CreateCustomerContactDto = this.empty();

  empty(): CreateCustomerContactDto {
    return {
      fullName: '',
      email: '',
      phone: '',
      position: '',
      isPrimary: false,
      isActive: true
    };
  }

  isValid(): boolean {
    return !!(
      this.draft.fullName &&
      this.draft.email &&
      this.draft.phone

    );
  }

  add() {
    if (!this.isValid()) return;

    const contact = { ...this.draft };

    if (!this.contacts.length) {
      contact.isPrimary = true;
    }

    this.contacts.push(contact);
    this.draft = this.empty();
  }

  remove(i: number) {
    this.contacts.splice(i, 1);

    if (!this.contacts.some(a => a.isPrimary) && this.contacts.length) {
      this.contacts[0].isPrimary = true;
    }
  }
}