export interface OrderListItem {
    id: string;

    orderNumber: string;
    customerName: string;
    preferredCarrierName?: string | null;

    status: number;
    phase: number;
    direction: number;

    startDate: string;
    endDate: string;

    plannedPickupDate?: string | null;
    plannedDeliveryDate?: string | null;
    origin?: string | null;
    destination?: string | null;
    quotedTotal?: number | null;
    baseFreight?: number | null;
    accessorials?: number | null;
    commodity?: string | null;
    primaryPONumber?: string | null;
    primaryBolNumber?: string | null;
    primaryProNumber?: string | null;
    totalWeight?: number | null;
    totalPallets?: number | null;
    totalVolume?: number | null;
    hasActiveLoad?: boolean;
    activeLoadId?: string | null;
    activeLoadNumber?: string | null;

    createdAt: string;
    updatedAt?: string | null;
}
