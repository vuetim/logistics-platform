import { Injectable } from "@angular/core";
import { OrderDocumentsApi } from "./order-documents.api";

@Injectable({ providedIn: 'root' })
export class OrderDocumentsService {
    constructor(private api: OrderDocumentsApi) { }

    getByOrder(orderId: string) {
        return this.api.getByOrder(orderId);
    }

    upload(orderId: string, data: FormData) {
        return this.api.upload(orderId, data);
    }

    delete(orderId: string, id: string) {
        return this.api.delete(orderId, id);
    }
}
