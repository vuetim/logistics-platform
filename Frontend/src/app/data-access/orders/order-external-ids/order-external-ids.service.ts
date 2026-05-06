import { Injectable } from "@angular/core";
import { CreateOrderExternalIdDto } from "../../../core/models/orders/order-external-ids/create-order-external-id.dto";
import { UpdateOrderExternalIdDto } from "../../../core/models/orders/order-external-ids/update-order-external-id.dto";
import { OrderExternalIdsApi } from "./order-external-ids.api";

@Injectable({ providedIn: 'root' })
export class OrderExternalIdsService {
    constructor(private api: OrderExternalIdsApi) { }

    getByOrder(orderId: string) {
        return this.api.getByOrder(orderId);
    }

    create(orderId: string, dto: CreateOrderExternalIdDto) {
        return this.api.create(orderId, dto);
    }

    update(orderId: string, id: string, dto: UpdateOrderExternalIdDto) {
        return this.api.update(orderId, id, dto);
    }

    delete(orderId: string, id: string) {
        return this.api.delete(orderId, id);
    }
}
