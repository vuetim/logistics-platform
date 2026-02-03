import { Injectable } from "@angular/core";
import { CustomersQueryParameters } from "../../core/models/customers/customers-query-parameters.model";
import { CustomersApi } from "./customers.api";
import { CreateCustomerDto } from "../../core/models/customers/create-customer.dto";
import { CreateCustomerFullDto } from "../../core/models/customers/create-customer-full.dto";
import { CreateCustomerWizardState } from "../../features/pages/customers/components/customers-page/create-customer-wizard/create-customer-wizard.state";

@Injectable({ providedIn: 'root' })
export class CustomersService {
    constructor(private api: CustomersApi) { }

    getPaged(params: CustomersQueryParameters) {
        return this.api.getPaged(params);
    }

    getCustomer(id: string) {
        return this.api.getById(id);
    }
    getCustomerDetails(id: string) {
        return this.api.getDetails(id)
    }

    createFull(state: CreateCustomerWizardState) {
        const dto: CreateCustomerFullDto = {
            customer: state.customer,
            addresses: state.addresses,
            contacts: state.contacts,
            notes: state.notes
        };

        return this.api.createFull(dto);
    }

    //   updateCustomer(id: string, dto: UpdateCustomerDto) {
    //     return this.api.update(id, dto);
    //   }

    deleteCustomer(id: string) {
        return this.api.delete(id);
    }
}
