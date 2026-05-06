import { OrderCostLineItemDto } from "./order-cost-line-item.model";

export interface OrderCostDto {
    billTo?: string | null;
    notes?: string | null;
    taxRate: number;
    baseFreight: number;
    accessorials: number;
    quotedTotal: number;
    subTotal: number;
    totalTax: number;
    totalAmount: number;
    totalBillable: number;
    totalNonBillable: number;
    lineItems: OrderCostLineItemDto[];
}
