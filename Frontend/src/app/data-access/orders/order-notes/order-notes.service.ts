import { Injectable } from "@angular/core";
import { CreateOrderNoteDto } from "../../../core/models/orders/order-notes/create-order-note.dto";
import { UpdateOrderNoteDto } from "../../../core/models/orders/order-notes/update-order-note.dto";
import { OrderNotesApi } from "./order-notes.api";

@Injectable({ providedIn: 'root' })
export class OrderNotesService {
    constructor(private api: OrderNotesApi) { }

    getByOrder(orderId: string) {
        return this.api.getByOrder(orderId);
    }

    create(orderId: string, dto: CreateOrderNoteDto) {
        return this.api.create(orderId, dto);
    }

    update(orderId: string, id: string, dto: UpdateOrderNoteDto) {
        return this.api.update(orderId, id, dto);
    }

    delete(orderId: string, id: string) {
        return this.api.delete(orderId, id);
    }
}
