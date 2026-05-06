import { Injectable } from "@angular/core";
import { CreateOrderRouteDto } from "../../../core/models/orders/order-routes/create-order-route.dto";
import { UpdateOrderRouteDto } from "../../../core/models/orders/order-routes/update-order-route.dto";
import { OrderRoutesApi } from "./order-routes.api";

@Injectable({ providedIn: 'root' })
export class OrderRoutesService {

    constructor(private api: OrderRoutesApi) { }

    getByOrder(orderId: string) {
        return this.api.getByOrder(orderId);
    }

    create(orderId: string, dto: CreateOrderRouteDto) {
        return this.api.create(orderId, dto);
    }

    update(orderId: string, routeId: string, dto: UpdateOrderRouteDto) {
        return this.api.update(orderId, routeId, dto);
    }

    delete(orderId: string, routeId: string) {
        return this.api.delete(orderId, routeId);
    }
}
