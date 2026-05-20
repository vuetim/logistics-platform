export interface CreateOrderRouteDto {
    sequence: number;
    stopType: number;
    locationName: string;
    addressLine1: string;
    addressLine2?: string | null;
    city: string;
    state: string;
    postalCode: string;
    country: string;
    latitude?: number | null;
    longitude?: number | null;
    plannedArrivalFrom?: string | null;
    plannedArrivalTo?: string | null;
    appointmentType?: number;
    flexMinutes?: number | null;
    timeZone?: string | null;
    hasTime: boolean;
    copyToLoad: boolean;
    stopReference?: string | null;
    appointmentNumber?: string | null;
    poNumbers?: string | null;
    notes?: string | null;
}
