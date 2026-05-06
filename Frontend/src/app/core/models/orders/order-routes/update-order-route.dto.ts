export interface UpdateOrderRouteDto {
    sequence?: number;
    stopType?: number;
    locationName?: string | null;
    addressLine1?: string | null;
    addressLine2?: string | null;
    city?: string | null;
    state?: string | null;
    postalCode?: string | null;
    country?: string | null;
    plannedArrivalFrom?: string | null;
    plannedArrivalTo?: string | null;
    stopReference?: string | null;
    appointmentNumber?: string | null;
    hasTime?: boolean;
    copyToLoad?: boolean;
    notes?: string | null;
}
