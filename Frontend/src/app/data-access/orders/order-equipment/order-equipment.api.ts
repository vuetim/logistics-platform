import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { API_ENDPOINTS } from "../../../core/config/endpoints";
import { CreateOrderEquipmentRequirementDto } from "../../../core/models/orders/order-equipment/create-order-equipment-requirement.dto";
import { OrderEquipmentRequirementDto } from "../../../core/models/orders/order-equipment/order-equipment-requirement.model";
import { UpdateOrderEquipmentRequirementDto } from "../../../core/models/orders/order-equipment/update-order-equipment-requirement.dto";

@Injectable({ providedIn: 'root' })
export class OrderEquipmentApi {
    private readonly baseUrl = API_ENDPOINTS.orderEquipmentRequirements;

    constructor(private http: HttpClient) { }

    getByOrder(orderId: string) {
        return this.http.get<OrderEquipmentRequirementDto[]>(`${this.baseUrl}/${orderId}/equipment`);
    }

    create(orderId: string, dto: CreateOrderEquipmentRequirementDto) {
        return this.http.post<OrderEquipmentRequirementDto>(`${this.baseUrl}/${orderId}/equipment`, dto);
    }

    update(orderId: string, id: string, dto: UpdateOrderEquipmentRequirementDto) {
        return this.http.put<void>(`${this.baseUrl}/${orderId}/equipment/${id}`, dto);
    }

    delete(orderId: string, id: string) {
        return this.http.delete<void>(`${this.baseUrl}/${orderId}/equipment/${id}`);
    }
}
