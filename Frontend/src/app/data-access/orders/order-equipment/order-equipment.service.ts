import { Injectable } from "@angular/core";
import { CreateOrderEquipmentRequirementDto } from "../../../core/models/orders/order-equipment/create-order-equipment-requirement.dto";
import { UpdateOrderEquipmentRequirementDto } from "../../../core/models/orders/order-equipment/update-order-equipment-requirement.dto";
import { OrderEquipmentApi } from "./order-equipment.api";

@Injectable({ providedIn: 'root' })
export class OrderEquipmentService {

    constructor(private api: OrderEquipmentApi) { }

    getByOrder(orderId: string) {
        return this.api.getByOrder(orderId);
    }

    create(orderId: string, dto: CreateOrderEquipmentRequirementDto) {
        return this.api.create(orderId, dto);
    }

    update(orderId: string, id: string, dto: UpdateOrderEquipmentRequirementDto) {
        return this.api.update(orderId, id, dto);
    }

    delete(orderId: string, id: string) {
        return this.api.delete(orderId, id);
    }
}
