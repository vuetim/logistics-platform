import { Injectable } from "@angular/core";
import { CreateOrderItemDto } from "../../../core/models/orders/order-items/create-order-item.dto";
import { UpdateOrderItemDto } from "../../../core/models/orders/order-items/update-order-items.dto";
import { OrderItemsApi } from "./order-items.api";

@Injectable({ providedIn: 'root' })
export class OrderItemsService {

    constructor(private api: OrderItemsApi) { }

    getByOrder(orderId: string) {
        return this.api.getByOrder(orderId);
    }

    create(orderId: string, dto: CreateOrderItemDto) {
        return this.api.create(orderId, dto);
    }

    update(orderId: string, itemId: string, dto: UpdateOrderItemDto) {
        return this.api.update(orderId, itemId, dto);
    }

    delete(orderId: string, itemId: string) {
        return this.api.delete(orderId, itemId);
    }
}
