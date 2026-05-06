import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { API_ENDPOINTS } from "../../../core/config/endpoints";
import { OrderDocumentDto } from "../../../core/models/orders/order-documents/order-document.model";

@Injectable({ providedIn: 'root' })
export class OrderDocumentsApi {
    private readonly baseUrl = API_ENDPOINTS.orderDocuments;

    constructor(private http: HttpClient) { }

    getByOrder(orderId: string) {
        return this.http.get<OrderDocumentDto[]>(`${this.baseUrl}/${orderId}/documents`);
    }

    upload(orderId: string, data: FormData) {
        return this.http.post<OrderDocumentDto>(`${this.baseUrl}/${orderId}/documents/upload`, data);
    }

    delete(orderId: string, id: string) {
        return this.http.delete<void>(`${this.baseUrl}/${orderId}/documents/${id}`);
    }
}
