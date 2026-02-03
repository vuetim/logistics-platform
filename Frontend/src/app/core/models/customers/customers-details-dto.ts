import { CustomerAddressDto } from "./addresses/customer-address.dto";
import { CustomerContactDto } from "./contacts/customer-contact.dto";
import { CustomerNoteDto } from "./notes/customer-note.dto";

export interface CustomerDetailsDto {
    id: string;

    name: string;
    email?: string | null;
    phone?: string | null;

    paymentTermsDays: number;
    isActive: boolean;

    addresses?: CustomerAddressDto[];
    contacts?: CustomerContactDto[];
    notes?: CustomerNoteDto[];
}