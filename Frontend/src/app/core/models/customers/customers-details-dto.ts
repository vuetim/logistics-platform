import { CustomerAddressDto } from "./addresses/customer-address.dto";
import { CustomerContactDto } from "./contacts/customer-contact.dto";
import { CustomerBillingDto } from "./customer-billing.dto";
import { CustomerNoteDto } from "./notes/customer-note.dto";

export interface CustomerDetailsDto {
    id: string;

    name: string;
    email?: string | null;
    phone?: string | null;

    billing: CustomerBillingDto;
    isActive: boolean;

    addresses?: CustomerAddressDto[];
    contacts?: CustomerContactDto[];
    notes?: CustomerNoteDto[];
}