import { Injectable } from "@angular/core";
import { API_ENDPOINTS } from "../../core/config/endpoints";
import { HttpClient } from "@angular/common/http";
import { CreateOrderDto } from "../../core/models/orders/create-order.dt";
import { OrderDetailsDto } from "../../core/models/orders/order-details.dto";
import { PagedResult } from "../../core/models/pagination/paged-result.model";
import { OrderListItem } from "../../core/models/orders/order-list-item.model";
import { OrdersQueryParameters } from "../../core/models/orders/orders-query-parameters.dto";
import { UpdateOrderDto } from "../../core/models/orders/update-order.dto";

@Injectable({ providedIn: 'root' })
export class OrdersApi {
    private readonly baseUrl = API_ENDPOINTS.orders;

    constructor(private http: HttpClient) { }


    getPaged(params: OrdersQueryParameters) {
        return this.http.get<PagedResult<OrderListItem>>(this.baseUrl, {
            params: params as any
        });
    }



    getDetails(id: string) {
        return this.http.get<OrderDetailsDto>(`${this.baseUrl}/${id}`);
    }

    create(dto: CreateOrderDto) {
        return this.http.post(this.baseUrl, dto, { observe: 'response' });
    }

    update(id: string, dto: UpdateOrderDto) {
        return this.http.put<void>(`${this.baseUrl}/${id}`, dto);
    }

    submit(id: string) {
        return this.http.post(`${this.baseUrl}/${id}/submit`, {});
    }

    cancel(id: string) {
        return this.http.post(`${this.baseUrl}/${id}/cancel`, {});
    }

    createLoadFromOrder(orderId: string) {
        return this.http.post<{ loadId: string }>(API_ENDPOINTS.loadsFromOrder, {
            orderId,
            carrierId: null,
            plannedPickupDate: null,
            plannedDeliveryDate: null,
            carrierRate: null,
            rateConfirmationNumber: null,
            splitOrder: false
        });
    }
}
