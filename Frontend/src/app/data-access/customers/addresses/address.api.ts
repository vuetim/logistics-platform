import { Injectable } from "@angular/core";
import { API_ENDPOINTS } from "../../../core/config/endpoints";
import { HttpClient } from "@angular/common/http";
import { CreateCustomerAddressDto } from "../../../core/models/customers/addresses/create-customer-address.dto";
import { CustomerAddressDto } from "../../../core/models/customers/addresses/customer-address.dto";
import { UpdateCustomerAddressDto } from "../../../core/models/customers/addresses/update-address.dto";

@Injectable({ providedIn: 'root' })
export class CustomerAddressesApi {
    private readonly baseUrl = API_ENDPOINTS.customerAddresses
    constructor(private http: HttpClient) { }

    getByCustomer(customerId: string) {
        return this.http.get<CustomerAddressDto[]>
            (`${this.baseUrl}/customer/${customerId}`)
    }
    create(dto: CreateCustomerAddressDto) {
        return this.http.post<CustomerAddressDto>(this.baseUrl, dto)

    }

    update(id: string, dto: UpdateCustomerAddressDto) {
        return this.http.put<UpdateCustomerAddressDto>(`${this.baseUrl}/${id}`, dto)
    }


    delete(id: string) {
        return this.http.delete<void>(`${this.baseUrl}/${id}`)
    }
}