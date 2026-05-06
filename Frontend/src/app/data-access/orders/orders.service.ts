import { Injectable } from "@angular/core";
import { CreateOrderDto } from "../../core/models/orders/create-order.dt";
import { OrdersApi } from "./orders.api";
import { map } from "rxjs";
import { OrdersQueryParameters } from "../../core/models/orders/orders-query-parameters.dto";
import { UpdateOrderDto } from "../../core/models/orders/update-order.dto";

@Injectable({ providedIn: 'root' })
export class OrdersService {

    private orderId?: string;

    constructor(private api: OrdersApi) { }

    get currentOrderId() {
        return this.orderId!;
    }

    create(dto: CreateOrderDto) {
        return this.api.create(dto).pipe(
            map(res => {
                const location = res.headers.get('Location');
                if (location) {
                    this.orderId = location.split('/').pop()!;
                }
                return this.orderId;
            })
        );
    }
    getPaged(params: OrdersQueryParameters) {
        return this.api.getPaged(params);
    }



    getDetails(id: string) {
        return this.api.getDetails(id);
    }

    update(id: string, dto: UpdateOrderDto) {
        return this.api.update(id, dto);
    }


    submit(id: string) {
        return this.api.submit(id);
    }

    cancel(id: string) {
        return this.api.cancel(id);
    }

    createLoadFromOrder(orderId: string) {
        return this.api.createLoadFromOrder(orderId);
    }

    reset() {
        this.orderId = undefined;
    }
}
