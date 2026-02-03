import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { CustomerContactDto } from "../../../core/models/customers/contacts/customer-contact.dto";
import { CreateCustomerContactDto } from "../../../core/models/customers/contacts/create-customer-contact.dto";
import { UpdateCustomerContactDto } from "../../../core/models/customers/contacts/update-customer-contact.dto";
import { API_ENDPOINTS } from "../../../core/config/endpoints";



@Injectable({ providedIn: 'root' })
export class CustomerContactsApi {

    private baseUrl = API_ENDPOINTS.customerContacts;

    constructor(private http: HttpClient) { }

    getByCustomer(customerId: string) {
        return this.http.get<CustomerContactDto[]>(
            `${this.baseUrl}/customer/${customerId}`
        );
    }

    create(dto: CreateCustomerContactDto) {
        return this.http.post<void>(this.baseUrl, dto);
    }

    update(id: string, dto: UpdateCustomerContactDto) {
        return this.http.put<void>(`${this.baseUrl}/${id}`, dto);
    }

    delete(id: string) {
        return this.http.delete<void>(`${this.baseUrl}/${id}`);
    }
}
