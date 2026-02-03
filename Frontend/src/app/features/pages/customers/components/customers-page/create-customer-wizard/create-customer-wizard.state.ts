import { CreateCustomerAddressDto } from "../../../../../../core/models/customers/addresses/create-customer-address.dto";
import { CreateCustomerContactDto } from "../../../../../../core/models/customers/contacts/create-customer-contact.dto";
import { CreateCustomerNoteDto } from "../../../../../../core/models/customers/notes/create-customer-note.dto";

export interface CreateCustomerWizardState {
    customer: {
        name: string;
        email?: string;
        phone?: string;
        paymentTermsDays: number;
        isActive: boolean;
    };

    addresses: CreateCustomerAddressDto[];
    contacts: CreateCustomerContactDto[];
    notes: CreateCustomerNoteDto[];
}
