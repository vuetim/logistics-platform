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
    plannedArrivalFrom?: string | null;
    plannedArrivalTo?: string | null;
    hasTime: boolean;
    copyToLoad: boolean;
    stopReference?: string | null;
    appointmentNumber?: string | null;
    notes?: string | null;
}
