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
    latitude?: number | null;
    longitude?: number | null;
    plannedArrivalFrom?: string | null;
    plannedArrivalTo?: string | null;
    appointmentType?: number | null;
    flexMinutes?: number | null;
    timeZone?: string | null;
    stopReference?: string | null;
    appointmentNumber?: string | null;
    poNumbers?: string | null;
    hasTime?: boolean;
    copyToLoad?: boolean;
    notes?: string | null;
}
