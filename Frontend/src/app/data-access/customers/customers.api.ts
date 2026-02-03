import { HttpClient } from "@angular/common/http";
import { CustomersQueryParameters } from "../../core/models/customers/customers-query-parameters.model";
import { PagedResult } from "../../core/models/pagination/paged-result.model";
import { Injectable } from "@angular/core";
import { CustomerListItem } from "../../core/models/customers/customer-list-item.model";
import { CustomerDto } from "../../core/models/customers/customer.dto";
import { API_ENDPOINTS } from "../../core/config/endpoints";
import { CreateCustomerFullDto } from "../../core/models/customers/create-customer-full.dto";
import { CreateCustomerWizardState } from "../../features/pages/customers/components/customers-page/create-customer-wizard/create-customer-wizard.state";
import { CreateCustomerDto } from "../../core/models/customers/create-customer.dto";
import { UpdateCustomerDto } from "../../core/models/customers/update-customer.dto";
import { CustomerDetailsDto } from "../../core/models/customers/customers-details-dto";

@Injectable({ providedIn: 'root' })
export class CustomersApi {
    private readonly baseUrl = API_ENDPOINTS.customers;

    constructor(private http: HttpClient) { }

    getPaged(params: CustomersQueryParameters) {
        return this.http.get<any>(`${this.baseUrl}/paged`, {
            params: params as any
        });
    }

    getById(id: string) {
        return this.http.get<CustomerDto>(`${this.baseUrl}/${id}`);
    }
    getDetails(id: string) {
        return this.http.get<CustomerDetailsDto>(`${this.baseUrl}/${id}/details`)
    }

    createFull(dto: CreateCustomerFullDto) {
        return this.http.post<CustomerDto>(
            `${this.baseUrl}/full`,
            dto
        );
    }
    create(dto: CreateCustomerDto) {
        return this.http.post<CustomerDto>(this.baseUrl, dto);
    }

    update(id: string, dto: UpdateCustomerDto) {
        return this.http.put<CustomerDto>(`${this.baseUrl}/${id}`, dto);
    }

    delete(id: string) {
        return this.http.delete<void>(`${this.baseUrl}/${id}`);
    }
}
