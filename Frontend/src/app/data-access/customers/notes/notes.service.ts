import { Injectable } from "@angular/core";
import { CustomerNotesApi } from "./notes.api";
import { CreateCustomerNoteDto } from "../../../core/models/customers/notes/create-customer-note.dto";
import { UpdateCustomerNoteDto } from "../../../core/models/customers/notes/update-customer-note.dto";

@Injectable({ providedIn: 'root' })
export class CustomerNotesService {
    constructor(private api: CustomerNotesApi) { }

    getByCustomer(customerId: string) {
        return this.api.getByCustomer(customerId)
    }

    create(dto: CreateCustomerNoteDto) {
        return this.api.create(dto)
    }

    update(id: string, dto: UpdateCustomerNoteDto) {
        return this.api.update(id, dto)
    }
    delete(id: string) {
        return this.api.delete(id)
    }

}