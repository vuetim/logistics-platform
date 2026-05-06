import { OrderItemDto } from "./order-items/order-item.model";


export interface OrderDetailsDto {
    id: string;
    orderNumber: string;

    customerId: string;
    customerName: string;
    preferredCarrierId?: string | null;
    preferredCarrierName?: string | null;

    orderType: number;
    direction: number;

    status: number;
    phase: number;

    startDate: { date: string; timezone?: string | null; hasTime: boolean };
    endDate: { date: string; timezone?: string | null; hasTime: boolean };
    startDateType?: { key?: string | null; value?: string | null } | null;
    endDateType?: { key?: string | null; value?: string | null } | null;

    plannedPickup?: { date: string; timezone?: string | null; hasTime: boolean } | null;
    plannedDelivery?: { date: string; timezone?: string | null; hasTime: boolean } | null;
    origin: string;
    destination: string;

    dispatchNotes?: string | null;
    deliveryNotes?: string | null;

    customerRate?: number | null;
    baseFreight?: number | null;
    accessorials?: number | null;
    quotedTotal?: number | null;
    primaryPONumber?: string | null;
    primaryBolNumber?: string | null;
    primaryProNumber?: string | null;
    commodity?: string | null;
    totalWeight?: number | null;
    totalPallets?: number | null;
    totalVolume?: number | null;
    hasActiveLoad?: boolean;
    activeLoadId?: string | null;
    activeLoadNumber?: string | null;

    items: OrderItemDto[];

    createdAt: string;
}
