import { CreateCustomerAddressDto } from "../../../../../../core/models/customers/addresses/create-customer-address.dto";
import { CreateCustomerContactDto } from "../../../../../../core/models/customers/contacts/create-customer-contact.dto";
import { CustomerBillingDto } from "../../../../../../core/models/customers/customer-billing.dto";
import { CreateCustomerNoteDto } from "../../../../../../core/models/customers/notes/create-customer-note.dto";

export interface CreateCustomerWizardState {
    customer: {
        name: string;
        email?: string;
        phone?: string;
        isActive: boolean;
        billing: CustomerBillingDto;
    };

    addresses: CreateCustomerAddressDto[];
    contacts: CreateCustomerContactDto[];
    notes: CreateCustomerNoteDto[];
}
