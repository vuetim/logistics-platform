export interface OrderCostLineItemDto {
    id?: string | null;
    type: number;
    qty: number;
    price: number;
    amount: number;
    isCustomer: boolean;
    isCarrier: boolean;
    notes?: string | null;
}
