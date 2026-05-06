export interface CreateOrderDto {
    customerId: string;
    preferredCarrierId?: string | null;

    orderType: string;
    direction: string;

    startDate: { date: string; timezone?: string | null; hasTime: boolean };
    endDate: { date: string; timezone?: string | null; hasTime: boolean };
    startDateType?: { key?: string | null; value?: string | null } | null;
    endDateType?: { key?: string | null; value?: string | null } | null;

    plannedPickup?: { date: string; timezone?: string | null; hasTime: boolean } | null;
    plannedDelivery?: { date: string; timezone?: string | null; hasTime: boolean } | null;

    // Business fields (optional)
    primaryPONumber?: string | null;
    primaryBolNumber?: string | null;
    primaryProNumber?: string | null;

    commodity?: string | null;
    totalWeight?: number | null;
    totalPallets?: number | null;
    totalVolume?: number | null;

    dispatchNotes?: string | null;
    deliveryNotes?: string | null;
    customerRate?: number | null;
}
