import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { API_ENDPOINTS } from "../../../core/config/endpoints";
import { OrderCostDto } from "../../../core/models/orders/order-costs/order-cost.model";
import { UpdateOrderCostDto } from "../../../core/models/orders/order-costs/update-order-cost.dto";

@Injectable({ providedIn: 'root' })
export class OrderCostsApi {
    private readonly baseUrl = API_ENDPOINTS.orderCosts;

    constructor(private http: HttpClient) { }

    get(orderId: string) {
        return this.http.get<OrderCostDto>(`${this.baseUrl}/${orderId}/costs`);
    }

    update(orderId: string, dto: UpdateOrderCostDto) {
        return this.http.put<void>(`${this.baseUrl}/${orderId}/costs`, dto);
    }
}
