import { Injectable } from "@angular/core";
import { API_ENDPOINTS } from "../../../core/config/endpoints";
import { HttpClient } from "@angular/common/http";
import { CreateOrderItemDto } from "../../../core/models/orders/order-items/create-order-item.dto";
import { OrderItemDto } from "../../../core/models/orders/order-items/order-item.model";
import { UpdateOrderItemDto } from "../../../core/models/orders/order-items/update-order-items.dto";

@Injectable({ providedIn: 'root' })
export class OrderItemsApi {
    private readonly baseUrl = API_ENDPOINTS.orderItems;

    constructor(private http: HttpClient) { }


    getByOrder(orderId: string) {
        return this.http.get<OrderItemDto[]>(`${this.baseUrl}/${orderId}/items`);
    }

    create(orderId: string, dto: CreateOrderItemDto) {
        return this.http.post<void>(`${this.baseUrl}/${orderId}/items`, dto);
    }

    update(orderId: string, itemId: string, dto: UpdateOrderItemDto) {
        return this.http.put<void>(`${this.baseUrl}/${orderId}/items/${itemId}`, dto);
    }

    delete(orderId: string, itemId: string) {
        return this.http.delete<void>(`${this.baseUrl}/${orderId}/items/${itemId}`);
    }
}
