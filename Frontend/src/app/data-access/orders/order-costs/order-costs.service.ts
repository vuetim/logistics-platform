import { Injectable } from "@angular/core";
import { UpdateOrderCostDto } from "../../../core/models/orders/order-costs/update-order-cost.dto";
import { OrderCostsApi } from "./order-costs.api";

@Injectable({ providedIn: 'root' })
export class OrderCostsService {
    constructor(private api: OrderCostsApi) { }

    get(orderId: string) {
        return this.api.get(orderId);
    }

    update(orderId: string, dto: UpdateOrderCostDto) {
        return this.api.update(orderId, dto);
    }
}
