import { Injectable } from "@angular/core";
import { CustomerContactsApi } from "./contact.api";

import { CreateCustomerContactDto } from "../../../core/models/customers/contacts/create-customer-contact.dto";
import { UpdateCustomerContactDto } from "../../../core/models/customers/contacts/update-customer-contact.dto";

@Injectable({ providedIn: 'root' })
export class CustomerContactsService {

    constructor(private api: CustomerContactsApi) { }

    getByCustomer(customerId: string) {
        return this.api.getByCustomer(customerId);
    }

    create(dto: CreateCustomerContactDto) {
        return this.api.create(dto);
    }

    update(id: string, dto: UpdateCustomerContactDto) {
        return this.api.update(id, dto);
    }

    delete(id: string) {
        return this.api.delete(id);
    }
}
