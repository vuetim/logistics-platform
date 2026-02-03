import { CreateCustomerDto } from './create-customer.dto';
import { CreateCustomerAddressDto } from './addresses/create-customer-address.dto';
import { CreateCustomerContactDto } from './contacts/create-customer-contact.dto';
import { CreateCustomerNoteDto } from './notes/create-customer-note.dto';

export interface CreateCustomerFullDto {
    customer: CreateCustomerDto;
    addresses: CreateCustomerAddressDto[];
    contacts: CreateCustomerContactDto[];
    notes: CreateCustomerNoteDto[];
}
