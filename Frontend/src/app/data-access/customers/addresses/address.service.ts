import { Injectable } from "@angular/core";
import { CustomerAddressesApi } from "./address.api";
import { CreateCustomerAddressDto } from "../../../core/models/customers/addresses/create-customer-address.dto";
import { UpdateCustomerAddressDto } from "../../../core/models/customers/addresses/update-address.dto";

@Injectable({ providedIn: 'root' })
export class CustomerAddressesService {

    constructor(private api: CustomerAddressesApi) { }

    getByCustomer(customerId: string) {
        return this.api.getByCustomer(customerId)
    }

    create(dto: CreateCustomerAddressDto) {
        return this.api.create(dto)
    }
    update(id: string, dto: UpdateCustomerAddressDto) {
        return this.api.update(id, dto)
    }

    delete(id: string) {
        return this.api.delete(id)
    }



}