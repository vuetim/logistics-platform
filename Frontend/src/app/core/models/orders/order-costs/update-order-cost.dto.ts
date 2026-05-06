import { OrderCostLineItemDto } from "./order-cost-line-item.model";

export interface UpdateOrderCostDto {
    billTo?: string | null;
    notes?: string | null;
    taxRate: number;
    lineItems: OrderCostLineItemDto[];
}
