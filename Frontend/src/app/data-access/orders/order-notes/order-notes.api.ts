import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { API_ENDPOINTS } from "../../../core/config/endpoints";
import { CreateOrderNoteDto } from "../../../core/models/orders/order-notes/create-order-note.dto";
import { OrderNoteDto } from "../../../core/models/orders/order-notes/order-note.model";
import { UpdateOrderNoteDto } from "../../../core/models/orders/order-notes/update-order-note.dto";

@Injectable({ providedIn: 'root' })
export class OrderNotesApi {
    private readonly baseUrl = API_ENDPOINTS.orderNotes;

    constructor(private http: HttpClient) { }

    getByOrder(orderId: string) {
        return this.http.get<OrderNoteDto[]>(`${this.baseUrl}/${orderId}/notes`);
    }

    create(orderId: string, dto: CreateOrderNoteDto) {
        return this.http.post<OrderNoteDto>(`${this.baseUrl}/${orderId}/notes`, dto);
    }

    update(orderId: string, id: string, dto: UpdateOrderNoteDto) {
        return this.http.put<OrderNoteDto>(`${this.baseUrl}/${orderId}/notes/${id}`, dto);
    }

    delete(orderId: string, id: string) {
        return this.http.delete<void>(`${this.baseUrl}/${orderId}/notes/${id}`);
    }
}
