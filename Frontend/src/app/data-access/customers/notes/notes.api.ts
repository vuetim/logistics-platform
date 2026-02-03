import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";

import { API_ENDPOINTS } from "../../../core/config/endpoints";
import { CustomerNoteDto } from "../../../core/models/customers/notes/customer-note.dto";
import { CreateCustomerNoteDto } from "../../../core/models/customers/notes/create-customer-note.dto";
import { UpdateCustomerNoteDto } from "../../../core/models/customers/notes/update-customer-note.dto";



@Injectable({ providedIn: 'root' })
export class CustomerNotesApi {

    private baseUrl = API_ENDPOINTS.customerNotes;

    constructor(private http: HttpClient) { }

    getByCustomer(customerId: string) {
        return this.http.get<CustomerNoteDto[]>(
            `${this.baseUrl}/customer/${customerId}`
        );
    }

    create(dto: CreateCustomerNoteDto) {
        return this.http.post<void>(this.baseUrl, dto);
    }

    update(id: string, dto: UpdateCustomerNoteDto) {
        return this.http.put<void>(`${this.baseUrl}/${id}`, dto);
    }

    delete(id: string) {
        return this.http.delete<void>(`${this.baseUrl}/${id}`);
    }
}
