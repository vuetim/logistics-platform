import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { API_ENDPOINTS } from "../../../core/config/endpoints";
import { CreateOrderRouteDto } from "../../../core/models/orders/order-routes/create-order-route.dto";
import { OrderRouteDto } from "../../../core/models/orders/order-routes/order-route.model";
import { UpdateOrderRouteDto } from "../../../core/models/orders/order-routes/update-order-route.dto";

@Injectable({ providedIn: 'root' })
export class OrderRoutesApi {
    private readonly baseUrl = API_ENDPOINTS.orderRoutes;

    constructor(private http: HttpClient) { }

    getByOrder(orderId: string) {
        return this.http.get<OrderRouteDto[]>(`${this.baseUrl}/${orderId}/routes`);
    }

    create(orderId: string, dto: CreateOrderRouteDto) {
        return this.http.post<void>(`${this.baseUrl}/${orderId}/routes`, dto);
    }

    update(orderId: string, routeId: string, dto: UpdateOrderRouteDto) {
        return this.http.put<void>(`${this.baseUrl}/${orderId}/routes/${routeId}`, dto);
    }

    delete(orderId: string, routeId: string) {
        return this.http.delete<void>(`${this.baseUrl}/${orderId}/routes/${routeId}`);
    }
}
