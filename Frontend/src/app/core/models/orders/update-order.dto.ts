export interface UpdateOrderDto {
    orderType?: string | null;
    direction?: string | null;
    startDate?: { date: string; timezone?: string | null; hasTime: boolean } | null;
    endDate?: { date: string; timezone?: string | null; hasTime: boolean } | null;
    startDateType?: { key?: string | null; value?: string | null } | null;
    endDateType?: { key?: string | null; value?: string | null } | null;

    plannedPickup?: { date: string; timezone?: string | null; hasTime: boolean } | null;
    plannedDelivery?: { date: string; timezone?: string | null; hasTime: boolean } | null;

    dispatchNotes?: string | null;
    deliveryNotes?: string | null;

    preferredCarrierId?: string | null;

    primaryPONumber?: string | null;
    primaryBolNumber?: string | null;
    primaryProNumber?: string | null;
    commodity?: string | null;
    totalWeight?: number | null;
    totalPallets?: number | null;
    totalVolume?: number | null;

    customerRate?: number | null;
}
