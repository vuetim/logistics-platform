import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { API_ENDPOINTS } from "../../../core/config/endpoints";
import { CreateOrderExternalIdDto } from "../../../core/models/orders/order-external-ids/create-order-external-id.dto";
import { OrderExternalIdDto } from "../../../core/models/orders/order-external-ids/order-external-id.model";
import { UpdateOrderExternalIdDto } from "../../../core/models/orders/order-external-ids/update-order-external-id.dto";

@Injectable({ providedIn: 'root' })
export class OrderExternalIdsApi {
    private readonly baseUrl = API_ENDPOINTS.orderExternalIds;

    constructor(private http: HttpClient) { }

    getByOrder(orderId: string) {
        return this.http.get<OrderExternalIdDto[]>(`${this.baseUrl}/${orderId}/external-ids`);
    }

    create(orderId: string, dto: CreateOrderExternalIdDto) {
        return this.http.post<OrderExternalIdDto>(`${this.baseUrl}/${orderId}/external-ids`, dto);
    }

    update(orderId: string, id: string, dto: UpdateOrderExternalIdDto) {
        return this.http.put<OrderExternalIdDto>(`${this.baseUrl}/${orderId}/external-ids/${id}`, dto);
    }

    delete(orderId: string, id: string) {
        return this.http.delete<void>(`${this.baseUrl}/${orderId}/external-ids/${id}`);
    }
}
